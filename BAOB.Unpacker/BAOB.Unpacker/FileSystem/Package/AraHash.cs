using System;

namespace BAOB.Unpacker
{
    class AraHash
    {
        public static UInt32 iGetHash(String m_String)
        {
            UInt32 dwHash = 0x811C9DC5;

            for (Int32 i = 0; i < m_String.Length; i++)
            {
                dwHash = 0x01000193 * ((Byte)m_String[i] ^ dwHash);
            }

            return dwHash;
        }
    }
}
