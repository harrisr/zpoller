using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlb_bd1_poller_app
{
    class ProgramBB
    {
        public static void MainZZB(string[] args)
        {
            BinaryWriter bw;
            BinaryReader br;
            string user = string.Empty;
            string date = string.Empty;
            string filename = string.Empty;
            char chr;


            byte[] bytes = File.ReadAllBytes("mydata.db1");
            int count = bytes.Length;

            for (int i = 0; i < count; i++)
            {
                if (i >= 17 && i < 25)
                {
                    chr = Convert.ToChar(bytes[i]);
                    user += chr;
                }
                if (i >= 34 && i < 43)
                {
                    chr = Convert.ToChar(bytes[i]);
                    date += chr;
                }
                if (i >= 52 && i < 64)
                {
                    chr = Convert.ToChar(bytes[i]);
                    filename += chr;
                }
                Console.Write(bytes[i]);
            }
            System.Console.WriteLine("\n\n");

            System.Console.WriteLine(user);
            System.Console.WriteLine(date);
            System.Console.WriteLine(filename);


            System.Console.WriteLine("\n\nPress <ENTER> to finish...");
            System.Console.ReadLine();
        }

    }
}

