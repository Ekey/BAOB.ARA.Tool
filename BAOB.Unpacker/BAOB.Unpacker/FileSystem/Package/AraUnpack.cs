using System;
using System.IO;
using System.Collections.Generic;

namespace BAOB.Unpacker
{
    class AraUnpack
    {
        static List<AraEntry> m_EntryTable = new List<AraEntry>();

        public static void iDoIt(String m_Archive, String m_DstFolder)
        {
            AraHashList.iLoadProject();

            using (FileStream TAraStream = File.OpenRead(m_Archive))
            {
                var m_Header = new AraHeader();

                m_Header.dwMagic = TAraStream.ReadUInt32();
                m_Header.dwVersion = TAraStream.ReadInt32();
                m_Header.dwTotalFiles = TAraStream.ReadInt32();
                m_Header.dwEntries = TAraStream.ReadInt32();
                m_Header.dwTableOffset = TAraStream.ReadUInt32();
                m_Header.dwBaseOffset = TAraStream.ReadUInt32();
                m_Header.dwReserved = TAraStream.ReadInt32();

                Int64 dwTempOffset = TAraStream.Position;

                if (m_Header.dwMagic != 0x414D5241)
                {
                    throw new Exception("[ERROR]: Invalid magic of ARA archive file!");
                }

                if (m_Header.dwVersion != 2)
                {
                    throw new Exception("[ERROR]: Invalid version of ARA archive file!");
                }

                TAraStream.Seek(m_Header.dwTableOffset, SeekOrigin.Begin);

                m_EntryTable.Clear();
                for (Int32 i = 0; i < m_Header.dwTotalFiles; i++)
                {
                    UInt32 dwOffset = TAraStream.ReadUInt32();
                    UInt16 sUnknown1 = TAraStream.ReadUInt16();
                    UInt16 sUnknown2 = TAraStream.ReadUInt16();
                    Int32 dwCompressedSize = TAraStream.ReadInt32();
                    Int32 dwDecompressedSize = TAraStream.ReadInt32();
                    Int32 dwCompressedFlag = TAraStream.ReadInt32();

                    var TEntry = new AraEntry
                    {
                        dwNameHash = 0,
                        dwOffset = dwOffset,
                        sUnknown1 = sUnknown1,
                        sUnknown2 = sUnknown2,
                        dwCompressedSize = dwCompressedSize,
                        dwDecompressedSize = dwDecompressedSize,
                        dwCompressedFlag = dwCompressedFlag,
                    };

                    m_EntryTable.Add(TEntry);
                }

                TAraStream.Seek(dwTempOffset, SeekOrigin.Begin);

                for (Int32 i = 0; i < m_Header.dwEntries; i++)
                {
                    UInt32 dwEntrieOffset = TAraStream.ReadUInt32();

                    dwTempOffset = TAraStream.Position;

                    if (dwEntrieOffset != 0)
                    {
                        TAraStream.Seek(dwEntrieOffset, SeekOrigin.Begin);
                        Int32 dwSubEntries = TAraStream.ReadInt32();

                        for (Int32 j = 0; j < dwSubEntries; j++)
                        {
                            UInt32 dwNameHash = TAraStream.ReadUInt32();
                            Int32 dwFileID = TAraStream.ReadInt32();

                            m_EntryTable[dwFileID].dwNameHash = dwNameHash;
                        }
                    }

                    TAraStream.Seek(dwTempOffset, SeekOrigin.Begin);
                }

                foreach (var m_Entry in m_EntryTable)
                {
                    String m_FileName = AraHashList.iGetNameFromHashList(m_Entry.dwNameHash).Replace("/", @"\");
                    String m_FullPath = m_DstFolder + m_FileName;

                    Utils.iSetInfo("[UNPACKING]: " + m_FileName);
                    Utils.iCreateDirectory(m_FullPath);

                    TAraStream.Seek(m_Entry.dwOffset, SeekOrigin.Begin);
                    var lpTemp = TAraStream.ReadBytes(m_Entry.dwCompressedSize);

                    if (m_Entry.dwCompressedFlag == 0)
                    {
                        File.WriteAllBytes(m_FullPath, lpTemp);
                    }
                    else
                    {
                        var lpBuffer = Zlib.iDecompress(lpTemp);
                        File.WriteAllBytes(m_FullPath, lpBuffer);
                    }
                }

                TAraStream.Dispose();
            }
        }
    }
}
