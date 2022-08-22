using System;

namespace BAOB.Unpacker
{
    class AraHeader
    {
        public UInt32 dwMagic { get; set; } // 0x414D5241 (ARMA)
        public Int32 dwVersion { get; set; } // 2
        public Int32 dwTotalFiles { get; set; }
        public Int32 dwEntries { get; set; } // 8192
        public UInt32 dwTableOffset { get; set; }
        public UInt32 dwBaseOffset { get; set; }
        public Int32 dwReserved { get; set; } // 0
    }
}
