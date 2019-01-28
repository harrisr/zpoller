using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rlb_bd1_poller_app
{
    public class ProgramAA
    {
        public static void MainZZA(string[] args)
        {
            BinaryWriter bw;
            BinaryReader br;

            int i = 25;
            double d = 3.14157;
            bool bt = true;
            bool bf = false;
            string s = "75001579.img";
            DateTime dt = DateTime.Parse("01/11/2019 16:08:14.123");
            string user = "john doe";

            //create the file
            try
            {
                bw = new BinaryWriter(new FileStream("mydata.db1", FileMode.Create));
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot create file.");
                return;
            }

            //writing into the file
            try
            {
                bw.Write(i);
                bw.Write(d);
                bw.Write(i);
                bw.Write(user);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(dt.ToShortDateString());
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(s);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
                bw.Write(bt);
                bw.Write(bf);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message + "\n Cannot write to file.");
                return;
            }
            bw.Close();

            //reading from the file
            //try
            //{
            //    br = new BinaryReader(new FileStream("mydata.bd1", FileMode.Open));
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine(e.Message + "\n Cannot open file.");
            //    return;
            //}

            //try
            //{
            //    i = br.ReadInt32();
            //    Console.WriteLine("Integer data: {0}", i);
            //    d = br.ReadDouble();
            //    Console.WriteLine("Double data: {0}", d);
            //    b = br.ReadBoolean();
            //    Console.WriteLine("Boolean data: {0}", b);
            //    s = br.ReadString();
            //    Console.WriteLine("String data: {0}", s);
            //}
            //catch (IOException e)
            //{
            //    Console.WriteLine(e.Message + "\n Cannot read from file.");
            //    return;
            //}
            //br.Close();
            Console.ReadKey();
        }
    }
}
