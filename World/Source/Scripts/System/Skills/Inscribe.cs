using System;
using System.Collections;
using Server;
using Server.Targeting;
using Server.Items;

namespace Server.SkillHandlers
{
    public class Inscribe
    {
        public static void Initialize()
        {
            SkillInfo.Table[(int)SkillName.Inscribe].Callback = new SkillUseCallback(OnUse);
        }

        public static TimeSpan OnUse(Mobile m)
        {
            Target target = new InternalTargetSrc();
            m.Target = target;
            m.SendLocalizedMessage(1046295); // Target the book you wish to copy.
            target.BeginTimeout(m, TimeSpan.FromMinutes(1.0));

            return TimeSpan.FromSeconds(1.0);
        }

        private static Hashtable m_UseTable = new Hashtable();

        private static void SetUser(BaseBook book, Mobile mob)
        {
            m_UseTable[book] = mob;
        }

        private static void CancelUser(BaseBook book)
        {
            m_UseTable.Remove(book);
        }

        public static Mobile GetUser(BaseBook book)
        {
            return (Mobile)m_UseTable[book];
        }

        public static bool IsEmpty(BaseBook book)
        {
            foreach (BookPageInfo page in book.Pages)
            {
                foreach (string line in page.Lines)
                {
                    if (line.Trim().Length != 0)
                        return false;
                }
            }
            return true;
        }

        public static void Copy(BaseBook bookSrc, BaseBook bookDst)
        {
            bookDst.Title = bookSrc.Title;
            bookDst.Author = bookSrc.Author;

            BookPageInfo[] pagesSrc = bookSrc.Pages;
            BookPageInfo[] pagesDst = bookDst.Pages;
            for (int i = 0; i < pagesSrc.Length && i < pagesDst.Length; i++)
            {
                BookPageInfo pageSrc = pagesSrc[i];
                BookPageInfo pageDst = pagesDst[i];

                int length = pageSrc.Lines.Length;
                pageDst.Lines = new string[length];

                for (int j = 0; j < length; j++)
                    pageDst.Lines[j] = pageSrc.Lines[j];
            }
        }

        private class InternalTargetSrc : Target
        {
            public InternalTargetSrc() : base(3, false, TargetFlags.None)
            {
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                BaseBook book = targeted as BaseBook;
                if (book == null)
                    from.SendLocalizedMessage(1046296); // That is not a book
                else if (Inscribe.IsEmpty(book))
                    from.SendLocalizedMessage(501611); // Can't copy an empty book.
                else if (Inscribe.GetUser(book) != null)
                    from.SendLocalizedMessage(501621); // Someone else is inscribing that item.
                else
                {
                    Target target = new InternalTargetDst(book);
                    from.Target = target;
                    from.SendLocalizedMessage(501612); // Select a book to copy this to.
                    target.BeginTimeout(from, TimeSpan.FromMinutes(1.0));
                    Inscribe.SetUser(book, from);
                }
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Timeout)
                    from.SendLocalizedMessage(501619); // You have waited too long to make your inscribe selection, your inscription attempt has timed out.
            }
        }

        private class InternalTargetDst : Target
        {
            private BaseBook m_BookSrc;

            public InternalTargetDst(BaseBook bookSrc) : base(3, false, TargetFlags.None)
            {
                m_BookSrc = bookSrc;
            }

            protected override void OnTarget(Mobile from, object targeted)
            {
                if (m_BookSrc.Deleted)
                    return;

                BaseBook bookDst = targeted as BaseBook;

                if (bookDst == null)
                    from.SendLocalizedMessage(1046296); // That is not a book
                else if (Inscribe.IsEmpty(m_BookSrc))
                    from.SendLocalizedMessage(501611); // Can't copy an empty book.
                else if (bookDst == m_BookSrc)
                    from.SendLocalizedMessage(501616); // Cannot copy a book onto itself.
                else if (!bookDst.Writable)
                    from.SendLocalizedMessage(501614); // Cannot write into that book.
                else if (Inscribe.GetUser(bookDst) != null)
                    from.SendLocalizedMessage(501621); // Someone else is inscribing that item.
                else
                {
                    if (from.CheckTargetSkill(SkillName.Inscribe, bookDst, 0, 50))
                    {
                        Inscribe.Copy(m_BookSrc, bookDst);

                        from.SendLocalizedMessage(501618); // You make a copy of the book.
                        from.PlaySound(0x249);
                    }
                    else
                    {
                        from.SendLocalizedMessage(501617); // You fail to make a copy of the book.
                    }
                }
            }

            protected override void OnTargetCancel(Mobile from, TargetCancelType cancelType)
            {
                if (cancelType == TargetCancelType.Timeout)
                    from.SendLocalizedMessage(501619); // You have waited too long to make your inscribe selection, your inscription attempt has timed out.
            }

            protected override void OnTargetFinish(Mobile from)
            {
                Inscribe.CancelUser(m_BookSrc);
            }
        }
    }
}