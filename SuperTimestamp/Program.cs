using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


            // Strip info from path
            string fExt = Path.GetExtension(origPath);
            string fName = Path.GetFileNameWithoutExtension(origPath);
            string dirName = Path.GetDirectoryName(origPath);

            // For Debugging
            Console.WriteLine("Extension:" + fExt);
            Console.WriteLine("Name:" + fName);
            Console.WriteLine("DirectoryName:" + dirName);

            // Strip Info from Name
            // we are searching for characters from right to left so it's easiest to do it piecewise
            string rootName = "";
            string timestamp = "";
            string timestampIncrement = "";

            // Search for presence of timestamp and increment
            Regex regexTimestampAndIncrement = new Regex(@"(.*)_(\d{8})_(\d*)"); // Regex for comparison
            Match matchTimestampAndIncrement = regexTimestampAndIncrement.Match(fName); // Match object to store matches
            if (matchTimestampAndIncrement.Success) // If normal timestampand increment are present
            {
                rootName = matchTimestampAndIncrement.Groups[1].ToString();
                timestamp = matchTimestampAndIncrement.Groups[2].ToString();
                timestampIncrement = matchTimestampAndIncrement.Groups[3].ToString();

            }
            else
            {
                // See if there's a date but no increment
                Regex regexTimestamp = new Regex(@"(.*)_(\d{8})");
                Match matchTimestamp = regexTimestamp.Match(fName);
                if (matchTimestamp.Success)
                {
                    rootName = matchTimestamp.Groups[1].ToString();
                    timestamp = matchTimestamp.Groups[2].ToString();
                }
                else
                {
                    rootName = fName;
                }
            }

            //// For Debugging
            //Console.WriteLine("Root Name:" + rootName);
            //Console.WriteLine("Timestamp:" + timestamp);
            //Console.WriteLine("Increment:" + timestampIncrement);






            //// Get date
            //DateTime localDate = DateTime.Now;
            //string strLocalDate = localDate.ToString("yyyyMMdd");




            //// Make new path
            //string newPath = origPath + "_" + strLocalDate;

            ////// For debug
            ////Console.Write("New Path is: \t" + newPath + '\t');
            ////foreach (char c in newPath)
            ////{
            ////    Console.Write(Convert.ToInt32(c) + " ");
            ////}
            ////Console.WriteLine();




            //// https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
            //// get the file attributes for file or directory
            //FileAttributes attr = File.GetAttributes(origPath);

            ////detect whether its a directory or file
            //if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            //    //MessageBox.Show("Its a directory");
            //    System.IO.Directory.Move(origPath, newPath);
            //else
            //    //MessageBox.Show("Its a file");
            //    System.IO.File.Move(origPath, newPath);



            Console.ReadKey();
        }
    }
}
