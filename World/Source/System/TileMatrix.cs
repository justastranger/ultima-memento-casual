/***************************************************************************
 *                               TileMatrix.cs
 *                            -------------------
 *   begin                : May 1, 2002
 *   copyright            : (C) The RunUO Software Team
 *   email                : info@runuo.com
 *
 *   $Id$
 *
 ***************************************************************************/

/***************************************************************************
 *
 *   This program is free software; you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation; either version 2 of the License, or
 *   (at your option) any later version.
 *
 ***************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Server
{
    public class TileMatrix
    {
        private StaticTile[][][][][] m_StaticTiles;
        private LandTile[][][] m_LandTiles;

        private LandTile[] m_InvalidLandBlock;
        private StaticTile[][][] m_EmptyStaticBlock;

        private FileStream m_Map;

        private FileStream m_Index;
        private BinaryReader m_IndexReader;

        private FileStream m_Statics;

        private int m_FileIndex;
        private int m_BlockWidth, m_BlockHeight;
        private int m_Width, m_Height;

        private Map m_Owner;

        private TileMatrixPatch m_Patch;
        private int[][] m_StaticPatches;
        private int[][] m_LandPatches;

        public Map Owner
        {
            get
            {
                return m_Owner;
            }
        }

        public TileMatrixPatch Patch
        {
            get
            {
                return m_Patch;
            }
        }

        public int BlockWidth
        {
            get
            {
                return m_BlockWidth;
            }
        }

        public int BlockHeight
        {
            get
            {
                return m_BlockHeight;
            }
        }

        public int Width
        {
            get
            {
                return m_Width;
            }
        }

        public int Height
        {
            get
            {
                return m_Height;
            }
        }

        public FileStream MapStream
        {
            get { return m_Map; }
            set { m_Map = value; }
        }

        public FileStream IndexStream
        {
            get { return m_Index; }
            set { m_Index = value; }
        }

        public FileStream DataStream
        {
            get { return m_Statics; }
            set { m_Statics = value; }
        }

        public BinaryReader IndexReader
        {
            get { return m_IndexReader; }
            set { m_IndexReader = value; }
        }

        public bool Exists
        {
            get { return (m_Map != null && m_Index != null && m_Statics != null); }
        }

        private static List<TileMatrix> m_Instances = new List<TileMatrix>();
        private List<TileMatrix> m_FileShare = new List<TileMatrix>();

        public TileMatrix(Map owner, int fileIndex, int mapID, int width, int height)
        {
            for (int i = 0; i < m_Instances.Count; ++i)
            {
                TileMatrix tm = m_Instances[i];

                if (tm.m_FileIndex == fileIndex)
                {
                    tm.m_FileShare.Add(this);
                    m_FileShare.Add(tm);
                }
            }

            m_Instances.Add(this);
            m_FileIndex = fileIndex;
            m_Width = width;
            m_Height = height;
            m_BlockWidth = width >> 3;
            m_BlockHeight = height >> 3;

            m_Owner = owner;

            if (fileIndex != 0x7F)
            {
                string mapPath = Core.FindDataFile("map{0}.mul", fileIndex);

                if (File.Exists(mapPath))
                    m_Map = new FileStream(mapPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);

                string indexPath = Core.FindDataFile("staidx{0}.mul", fileIndex);

                if (File.Exists(indexPath))
                {
                    m_Index = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    m_IndexReader = new BinaryReader(m_Index);
                }

                string staticsPath = Core.FindDataFile("statics{0}.mul", fileIndex);

                if (File.Exists(staticsPath))
                    m_Statics = new FileStream(staticsPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }

            m_EmptyStaticBlock = new StaticTile[8][][];

            for (int i = 0; i < 8; ++i)
            {
                m_EmptyStaticBlock[i] = new StaticTile[8][];

                for (int j = 0; j < 8; ++j)
                    m_EmptyStaticBlock[i][j] = new StaticTile[0];
            }

            m_InvalidLandBlock = new LandTile[196];

            m_LandTiles = new LandTile[m_BlockWidth][][];
            m_StaticTiles = new StaticTile[m_BlockWidth][][][][];
            m_StaticPatches = new int[m_BlockWidth][];
            m_LandPatches = new int[m_BlockWidth][];

            m_Patch = new TileMatrixPatch(this, mapID);
        }

        public StaticTile[][][] EmptyStaticBlock
        {
            get
            {
                return m_EmptyStaticBlock;
            }
        }

        public void SetStaticBlock(int x, int y, StaticTile[][][] value)
        {
            if (x < 0 || y < 0 || x >= m_BlockWidth || y >= m_BlockHeight)
                return;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new StaticTile[m_BlockHeight][][][];

            m_StaticTiles[x][y] = value;

            if (m_StaticPatches[x] == null)
                m_StaticPatches[x] = new int[(m_BlockHeight + 31) >> 5];

            m_StaticPatches[x][y >> 5] |= 1 << (y & 0x1F);
        }

        public StaticTile[][][] GetStaticBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= m_BlockWidth || y >= m_BlockHeight || m_Statics == null || m_Index == null)
                return m_EmptyStaticBlock;

            if (m_StaticTiles[x] == null)
                m_StaticTiles[x] = new StaticTile[m_BlockHeight][][][];

            StaticTile[][][] tiles = m_StaticTiles[x][y];

            if (tiles == null)
            {
                for (int i = 0; tiles == null && i < m_FileShare.Count; ++i)
                {
                    TileMatrix shared = m_FileShare[i];

                    if (x >= 0 && x < shared.m_BlockWidth && y >= 0 && y < shared.m_BlockHeight)
                    {
                        StaticTile[][][][] theirTiles = shared.m_StaticTiles[x];

                        if (theirTiles != null)
                            tiles = theirTiles[y];

                        if (tiles != null)
                        {
                            int[] theirBits = shared.m_StaticPatches[x];

                            if (theirBits != null && (theirBits[y >> 5] & (1 << (y & 0x1F))) != 0)
                                tiles = null;
                        }
                    }
                }

                if (tiles == null)
                    tiles = ReadStaticBlock(x, y);

                m_StaticTiles[x][y] = tiles;
            }

            return tiles;
        }

        public StaticTile[] GetStaticTiles(int x, int y)
        {
            StaticTile[][][] tiles = GetStaticBlock(x >> 3, y >> 3);

            return tiles[x & 0x7][y & 0x7];
        }

        private static TileList m_TilesList = new TileList();

        public StaticTile[] GetStaticTiles(int x, int y, bool multis)
        {
            StaticTile[][][] tiles = GetStaticBlock(x >> 3, y >> 3);

            if (multis)
            {
                IPooledEnumerable eable = m_Owner.GetMultiTilesAt(x, y);

                if (eable == Map.NullEnumerable.Instance)
                    return tiles[x & 0x7][y & 0x7];

                bool any = false;

                foreach (StaticTile[] multiTiles in eable)
                {
                    if (!any)
                        any = true;

                    m_TilesList.AddRange(multiTiles);
                }

                eable.Free();

                if (!any)
                    return tiles[x & 0x7][y & 0x7];

                m_TilesList.AddRange(tiles[x & 0x7][y & 0x7]);

                return m_TilesList.ToArray();
            }
            else
            {
                return tiles[x & 0x7][y & 0x7];
            }
        }

        public void SetLandBlock(int x, int y, LandTile[] value)
        {
            if (x < 0 || y < 0 || x >= m_BlockWidth || y >= m_BlockHeight)
                return;

            if (m_LandTiles[x] == null)
                m_LandTiles[x] = new LandTile[m_BlockHeight][];

            m_LandTiles[x][y] = value;

            if (m_LandPatches[x] == null)
                m_LandPatches[x] = new int[(m_BlockHeight + 31) >> 5];

            m_LandPatches[x][y >> 5] |= 1 << (y & 0x1F);
        }

        public LandTile[] GetLandBlock(int x, int y)
        {
            if (x < 0 || y < 0 || x >= m_BlockWidth || y >= m_BlockHeight || m_Map == null)
                return m_InvalidLandBlock;

            if (m_LandTiles[x] == null)
                m_LandTiles[x] = new LandTile[m_BlockHeight][];

            LandTile[] tiles = m_LandTiles[x][y];

            if (tiles == null)
            {
                for (int i = 0; tiles == null && i < m_FileShare.Count; ++i)
                {
                    TileMatrix shared = m_FileShare[i];

                    if (x >= 0 && x < shared.m_BlockWidth && y >= 0 && y < shared.m_BlockHeight)
                    {
                        LandTile[][] theirTiles = shared.m_LandTiles[x];

                        if (theirTiles != null)
                            tiles = theirTiles[y];

                        if (tiles != null)
                        {
                            int[] theirBits = shared.m_LandPatches[x];

                            if (theirBits != null && (theirBits[y >> 5] & (1 << (y & 0x1F))) != 0)
                                tiles = null;
                        }
                    }
                }

                if (tiles == null)
                    tiles = ReadLandBlock(x, y);

                m_LandTiles[x][y] = tiles;
            }

            return tiles;
        }

        public LandTile GetLandTile(int x, int y)
        {
            LandTile[] tiles = GetLandBlock(x >> 3, y >> 3);

            return tiles[((y & 0x7) << 3) + (x & 0x7)];
        }

        private static TileList[][] m_Lists;

        private static StaticTile[] m_TileBuffer = new StaticTile[128];

        private unsafe StaticTile[][][] ReadStaticBlock(int x, int y)
        {
            try
            {
                m_IndexReader.BaseStream.Seek(((x * m_BlockHeight) + y) * 12, SeekOrigin.Begin);

                int lookup = m_IndexReader.ReadInt32();
                int length = m_IndexReader.ReadInt32();

                if (lookup < 0 || length <= 0)
                {
                    return m_EmptyStaticBlock;
                }
                else
                {
                    int count = length / 7;

                    m_Statics.Seek(lookup, SeekOrigin.Begin);

                    if (m_TileBuffer.Length < count)
                        m_TileBuffer = new StaticTile[count];

                    StaticTile[] staTiles = m_TileBuffer;//new StaticTile[tileCount];

                    fixed (StaticTile* pTiles = staTiles)
                    {
#if !MONO
                        NativeReader.Read(m_Statics.SafeFileHandle.DangerousGetHandle(), pTiles, length);
#else
						NativeReader.Read( m_Statics.Handle, pTiles, length );
#endif
                        if (m_Lists == null)
                        {
                            m_Lists = new TileList[8][];

                            for (int i = 0; i < 8; ++i)
                            {
                                m_Lists[i] = new TileList[8];

                                for (int j = 0; j < 8; ++j)
                                    m_Lists[i][j] = new TileList();
                            }
                        }

                        TileList[][] lists = m_Lists;

                        StaticTile* pCur = pTiles, pEnd = pTiles + count;

                        while (pCur < pEnd)
                        {
                            lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add(pCur->m_ID, pCur->m_Z);
                            pCur = pCur + 1;
                        }

                        StaticTile[][][] tiles = new StaticTile[8][][];

                        for (int i = 0; i < 8; ++i)
                        {
                            tiles[i] = new StaticTile[8][];

                            for (int j = 0; j < 8; ++j)
                                tiles[i][j] = lists[i][j].ToArray();
                        }

                        return tiles;
                    }
                }
            }
            catch (EndOfStreamException)
            {
                if (DateTime.Now >= m_NextStaticWarning)
                {
                    Console.WriteLine("Warning: Static EOS for {0} ({1}, {2})", m_Owner, x, y);
                    m_NextStaticWarning = DateTime.Now + TimeSpan.FromMinutes(1.0);
                }

                return m_EmptyStaticBlock;
            }
        }

        private DateTime m_NextStaticWarning;
        private DateTime m_NextLandWarning;

        public void Force()
        {
            if (ScriptCompiler.Assemblies == null || ScriptCompiler.Assemblies.Length == 0)
                throw new Exception();
        }

        private unsafe LandTile[] ReadLandBlock(int x, int y)
        {
            try
            {
                m_Map.Seek(((x * m_BlockHeight) + y) * 196 + 4, SeekOrigin.Begin);

                LandTile[] tiles = new LandTile[64];

                fixed (LandTile* pTiles = tiles)
                {
#if !MONO
                    NativeReader.Read(m_Map.SafeFileHandle.DangerousGetHandle(), pTiles, 192);
#else
					NativeReader.Read( m_Map.Handle, pTiles, 192 );
#endif
                }

                return tiles;
            }
            catch
            {
                if (DateTime.Now >= m_NextLandWarning)
                {
                    Console.WriteLine("Warning: Land EOS for {0} ({1}, {2})", m_Owner, x, y);
                    m_NextLandWarning = DateTime.Now + TimeSpan.FromMinutes(1.0);
                }

                return m_InvalidLandBlock;
            }
        }

        public void Dispose()
        {
            if (m_Map != null)
                m_Map.Close();

            if (m_Statics != null)
                m_Statics.Close();

            if (m_IndexReader != null)
                m_IndexReader.Close();
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct LandTile
    {
        internal short m_ID;
        internal sbyte m_Z;

        public int ID
        {
            get { return m_ID; }
        }

        public int Z
        {
            get { return m_Z; }
            set { m_Z = (sbyte)value; }
        }

        public int Height
        {
            get { return 0; }

        }

        public bool Ignored
        {
            get { return (m_ID == 2 || m_ID == 0x1DB || (m_ID >= 0x1AE && m_ID <= 0x1B5)); }
        }

        public LandTile(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(short id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }
    }

    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]
    public struct StaticTile
    {
        internal ushort m_ID;
        internal byte m_X;
        internal byte m_Y;
        internal sbyte m_Z;
        internal short m_Hue;

        public int ID
        {
            get { return m_ID; }
        }

        public int X
        {
            get { return m_X; }
            set { m_X = (byte)value; }
        }

        public int Y
        {
            get { return m_Y; }
            set { m_Y = (byte)value; }
        }

        public int Z
        {
            get { return m_Z; }
            set { m_Z = (sbyte)value; }
        }

        public int Hue
        {
            get { return m_Hue; }
            set { m_Hue = (short)value; }
        }

        public int Height
        {
            get { return TileData.ItemTable[m_ID & TileData.MaxItemValue].Height; }
        }

        public StaticTile(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;

            m_X = 0;
            m_Y = 0;
            m_Hue = 0;
        }

        public StaticTile(ushort id, byte x, byte y, sbyte z, short hue)
        {
            m_ID = id;
            m_X = x;
            m_Y = y;
            m_Z = z;
            m_Hue = hue;
        }

        public void Set(ushort id, sbyte z)
        {
            m_ID = id;
            m_Z = z;
        }

        public void Set(ushort id, byte x, byte y, sbyte z, short hue)
        {
            m_ID = id;
            m_X = x;
            m_Y = y;
            m_Z = z;
            m_Hue = hue;
        }
    }
}
