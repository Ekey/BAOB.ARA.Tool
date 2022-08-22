using System;
using System.IO;

namespace BAOB.Unpacker
{
    class Program
    {
        private static String m_Title = "Batman™: Arkham Origins Blackgate ARA Unpacker";

        static void Main(String[] args)
        {
            Console.Title = m_Title;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(m_Title);
            Console.WriteLine("(c) 2022 Ekey (h4x0r) / v{0}\n", Utils.iGetApplicationVersion());
            Console.ResetColor();

            if (args.Length != 2)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[Usage]");
                Console.WriteLine("    BAOB.Unpacker <m_File> <m_Directory>\n");
                Console.WriteLine("    m_File - Source of ARA archive file");
                Console.WriteLine("    m_Directory - Destination directory\n");
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("[Examples]");
                Console.WriteLine("    BAOB.Unpacker E:\\Games\\BAOB\\GameData\\ozzyarchive.ara D:\\Unpacked");
                Console.ResetColor();
                return;
            }

            String m_AraFile = args[0];
            String m_Output = Utils.iCheckArgumentsPath(args[1]);

            if (!File.Exists(m_AraFile))
            {
                Utils.iSetError("[ERROR]: Input ARA file -> " + m_AraFile + " <- does not exist");
                return;
            }

            AraUnpack.iDoIt(m_AraFile, m_Output);
        }
    }
}
