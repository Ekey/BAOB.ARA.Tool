using System;

namespace BAOB.Unpacker
{
    class AraEntry
    {
        public UInt32 dwNameHash { get; set; }
        public UInt32 dwOffset { get; set; }
        public UInt16 sUnknown1 { get; set; } // ??
        public UInt16 sUnknown2 { get; set; } // ??
        public Int32 dwCompressedSize { get; set; }
        public Int32 dwDecompressedSize { get; set; }
        public Int32 dwCompressedFlag { get; set; }
    }
}
