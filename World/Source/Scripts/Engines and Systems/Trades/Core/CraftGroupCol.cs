using System;

namespace Server.Engines.Craft
{
    public class CraftGroupCol : System.Collections.CollectionBase
    {
        public CraftGroupCol()
        {
        }

        public int Add(CraftGroup craftGroup)
        {
            return List.Add(craftGroup);
        }

        public void Remove(int index)
        {
            if (index > Count - 1 || index < 0)
            {
            }
            else
            {
                List.RemoveAt(index);
            }
        }

        public CraftGroup GetAt(int index)
        {
            return (CraftGroup)List[index];
        }

        public int SearchFor(TextDefinition groupName)
        {
            for (int i = 0; i < List.Count; i++)
            {
                CraftGroup craftGroup = (CraftGroup)List[i];

                int nameNumber = craftGroup.NameNumber;
                string nameString = craftGroup.NameString;

                if ((nameNumber != 0 && nameNumber == groupName.Number) || (nameString != null && nameString == groupName.String))
                    return i;
            }

            return -1;
        }
    }
}