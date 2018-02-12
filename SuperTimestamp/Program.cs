using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperTimestamp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Verify argument(s) (file path) were supplied
            if(args.Length < 1)
            {
                //Console.WriteLine("No path supplied.  Exiting.");
                //Console.ReadKey();
                return;
            }


            // Get original path
            string origPath = args[0];
            for(int argidx = 1; argidx < args.Length; argidx++)
            {
                origPath += " " + args[argidx];
            }

            //// For Debug
            //Console.Write("Original path is: \t" + origPath + '\t');
            //foreach (char c in origPath)
            //{
            //    Console.Write(Convert.ToInt32(c) + " ");
            //}
            //Console.WriteLine();





            // Get date
            DateTime localDate = DateTime.Now;
            string strLocalDate = localDate.ToString("yyyyMMdd");




            // Make new path
            string newPath = origPath + "_" + strLocalDate;

            //// For debug
            //Console.Write("New Path is: \t" + newPath + '\t');
            //foreach (char c in newPath)
            //{
            //    Console.Write(Convert.ToInt32(c) + " ");
            //}
            //Console.WriteLine();




            // https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(origPath);

            //detect whether its a directory or file
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
                //MessageBox.Show("Its a directory");
                System.IO.Directory.Move(origPath, newPath);
            else
                //MessageBox.Show("Its a file");
                System.IO.File.Move(origPath, newPath);



            //Console.ReadKey();
        }
    }
}
