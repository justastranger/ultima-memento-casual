/***************************************************************************
 *                            TileMatrixPatch.cs
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
using System.IO;
using System.Runtime.InteropServices;

namespace Server
{
    public class TileMatrixPatch
    {
        private int m_LandBlocks, m_StaticBlocks;

        private static bool m_Enabled = true;

        public static bool Enabled
        {
            get
            {
                return m_Enabled;
            }
            set
            {
                m_Enabled = value;
            }
        }

        public int LandBlocks
        {
            get
            {
                return m_LandBlocks;
            }
        }

        public int StaticBlocks
        {
            get
            {
                return m_StaticBlocks;
            }
        }

        public TileMatrixPatch(TileMatrix matrix, int index)
        {
            if (!m_Enabled)
                return;

            string mapDataPath = Core.FindDataFile("mapdif{0}.mul", index);
            string mapIndexPath = Core.FindDataFile("mapdifl{0}.mul", index);

            if (File.Exists(mapDataPath) && File.Exists(mapIndexPath))
                m_LandBlocks = PatchLand(matrix, mapDataPath, mapIndexPath);

            string staDataPath = Core.FindDataFile("stadif{0}.mul", index);
            string staIndexPath = Core.FindDataFile("stadifl{0}.mul", index);
            string staLookupPath = Core.FindDataFile("stadifi{0}.mul", index);

            if (File.Exists(staDataPath) && File.Exists(staIndexPath) && File.Exists(staLookupPath))
                m_StaticBlocks = PatchStatics(matrix, staDataPath, staIndexPath, staLookupPath);
        }

        private unsafe int PatchLand(TileMatrix matrix, string dataPath, string indexPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    BinaryReader indexReader = new BinaryReader(fsIndex);

                    int count = (int)(indexReader.BaseStream.Length / 4);

                    for (int i = 0; i < count; ++i)
                    {
                        int blockID = indexReader.ReadInt32();
                        int x = blockID / matrix.BlockHeight;
                        int y = blockID % matrix.BlockHeight;

                        fsData.Seek(4, SeekOrigin.Current);

                        LandTile[] tiles = new LandTile[64];

                        fixed (LandTile* pTiles = tiles)
                        {
#if !MONO
                            NativeReader.Read(fsData.SafeFileHandle.DangerousGetHandle(), pTiles, 192);
#else
							NativeReader.Read( fsData.Handle, pTiles, 192 );
#endif
                        }

                        matrix.SetLandBlock(x, y, tiles);
                    }

                    indexReader.Close();

                    return count;
                }
            }
        }

        private static StaticTile[] m_TileBuffer = new StaticTile[128];

        private unsafe int PatchStatics(TileMatrix matrix, string dataPath, string indexPath, string lookupPath)
        {
            using (FileStream fsData = new FileStream(dataPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                using (FileStream fsIndex = new FileStream(indexPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (FileStream fsLookup = new FileStream(lookupPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        BinaryReader indexReader = new BinaryReader(fsIndex);
                        BinaryReader lookupReader = new BinaryReader(fsLookup);

                        int count = (int)(indexReader.BaseStream.Length / 4);

                        TileList[][] lists = new TileList[8][];

                        for (int x = 0; x < 8; ++x)
                        {
                            lists[x] = new TileList[8];

                            for (int y = 0; y < 8; ++y)
                                lists[x][y] = new TileList();
                        }

                        for (int i = 0; i < count; ++i)
                        {
                            int blockID = indexReader.ReadInt32();
                            int blockX = blockID / matrix.BlockHeight;
                            int blockY = blockID % matrix.BlockHeight;

                            int offset = lookupReader.ReadInt32();
                            int length = lookupReader.ReadInt32();
                            lookupReader.ReadInt32(); // Extra

                            if (offset < 0 || length <= 0)
                            {
                                matrix.SetStaticBlock(blockX, blockY, matrix.EmptyStaticBlock);
                                continue;
                            }

                            fsData.Seek(offset, SeekOrigin.Begin);

                            int tileCount = length / 7;

                            if (m_TileBuffer.Length < tileCount)
                                m_TileBuffer = new StaticTile[tileCount];

                            StaticTile[] staTiles = m_TileBuffer;

                            fixed (StaticTile* pTiles = staTiles)
                            {
#if !MONO
                                NativeReader.Read(fsData.SafeFileHandle.DangerousGetHandle(), pTiles, length);
#else
								NativeReader.Read( fsData.Handle, pTiles, length );
#endif
                                StaticTile* pCur = pTiles, pEnd = pTiles + tileCount;

                                while (pCur < pEnd)
                                {
                                    lists[pCur->m_X & 0x7][pCur->m_Y & 0x7].Add((ushort)pCur->m_ID, pCur->m_Z);
                                    pCur = pCur + 1;
                                }

                                StaticTile[][][] tiles = new StaticTile[8][][];

                                for (int x = 0; x < 8; ++x)
                                {
                                    tiles[x] = new StaticTile[8][];

                                    for (int y = 0; y < 8; ++y)
                                        tiles[x][y] = lists[x][y].ToArray();
                                }

                                matrix.SetStaticBlock(blockX, blockY, tiles);
                            }
                        }

                        indexReader.Close();
                        lookupReader.Close();

                        return count;
                    }
                }
            }
        }
    }
}