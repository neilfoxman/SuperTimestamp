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

        public static bool DEBUGGING = true;
        public enum FTYPE { DIR, FILE};

        static void Main(string[] args)
        {
            // Verify argument(s) (file path) were supplied
            if (args.Length < 1)
            {
                //Console.WriteLine("No path supplied.  Exiting.");
                //Console.ReadKey();
                return;
            }




            // Get original path
            string targetPathPrev = args[0];
            for (int argidx = 1; argidx < args.Length; argidx++)
            {
                targetPathPrev += " " + args[argidx];
            }
            DebugOutput("targetPathPrev:" + targetPathPrev);








            // https://stackoverflow.com/questions/1395205/better-way-to-check-if-a-path-is-a-file-or-a-directory
            // get the file attributes for file or directory
            FileAttributes attr = File.GetAttributes(targetPathPrev);
            //detect whether its a directory or file
            FTYPE ftype;
            if ((attr & FileAttributes.Directory) == FileAttributes.Directory)
            {
                //Its a directory
                ftype = FTYPE.DIR;
            }
            else
            {
                ftype = FTYPE.FILE;
            }






            // Strip info from path
            string targetDirName = Path.GetDirectoryName(targetPathPrev);
            string targetNamePrev = Path.GetFileNameWithoutExtension(targetPathPrev);
            string targetExt = Path.GetExtension(targetPathPrev);

            // Strip backslash off drive letter paths - only does for drive letter paths for some reason
            if (targetDirName[targetDirName.Length - 1] == '\\')
            {
                DebugOutput("Unnecessary backslash found in directory.");
                DebugOutput("targetDirName Was:" + targetDirName);
                targetDirName = targetDirName.Substring(0, targetDirName.Length - 1);
            }

            // For Debugging
            DebugOutput("targetDirName:" + targetDirName);
            DebugOutput("targetNamePrev:" + targetNamePrev);
            DebugOutput("targetExt:" + targetExt);





            // Strip Info from Name
            // we are searching for characters from right to left so it's easiest to do it piecewise
            string targetNameRoot = "";
            string targetNameTimestampPrev = "";
            string targetNameIncrementPrev = "";

            // Search for presence of targetNameTimestampPrev and increment
            Regex regexTimestampIncrement = new Regex(@"(.*)_(\d{8})_(\d*)"); // Regex for comparison
            Match matchTimestampIncrement = regexTimestampIncrement.Match(targetNamePrev); // Match object to store matches
            if (matchTimestampIncrement.Success) // If normal timestampand increment are present
            {
                targetNameRoot = matchTimestampIncrement.Groups[1].ToString();
                targetNameTimestampPrev = matchTimestampIncrement.Groups[2].ToString();
                targetNameIncrementPrev = matchTimestampIncrement.Groups[3].ToString();

            }
            else
            {
                // See if there's a date but no increment
                Regex regexTimestamp = new Regex(@"(.*)_(\d{8})");
                Match matchTimestamp = regexTimestamp.Match(targetNamePrev);
                if (matchTimestamp.Success)
                {
                    targetNameRoot = matchTimestamp.Groups[1].ToString();
                    targetNameTimestampPrev = matchTimestamp.Groups[2].ToString();
                }
                else
                {
                    targetNameRoot = targetNamePrev;
                }
            }
            DebugOutput("targetNameRoot:" + targetNameRoot);
            DebugOutput("targetNameTimestampPrev:" + targetNameTimestampPrev);
            DebugOutput("targetNameIncrementPrev:" + targetNameIncrementPrev);






            // Get new date
            DateTime dtNewDate = DateTime.Now;
            string targetNameTimestampNew = dtNewDate.ToString("yyyyMMdd");



            // Set the correct increment number depending on the other targetNameRoots in this directory
            // Search directory for same name and timestamp
            // unused approach but here for ref: https://stackoverflow.com/questions/8443524/using-directory-getfiles-with-a-regex-in-c
            string targetNameRootAndTimestampNew = targetNameRoot + '_' + targetNameTimestampNew;
            DebugOutput("targetNameRootAndTimestampNew:" + targetNameRootAndTimestampNew);
            string[] matchPathsFound;
            if (ftype == FTYPE.DIR)
            {
                matchPathsFound = Directory.GetDirectories(targetDirName, targetNameRootAndTimestampNew + '*', SearchOption.TopDirectoryOnly);
            }
            else
            {
                matchPathsFound = Directory.GetFiles(targetDirName, targetNameRootAndTimestampNew + '*', SearchOption.TopDirectoryOnly);
            }

            int foundIncrementMax = 0; // Start out with assumed max increment number.  This is the stored record
            if (matchPathsFound.Length > 0) // If found matching names in this directory with same timestamp
            {
                DebugOutput("Found matches in directory:");
                foreach (string f in matchPathsFound)
                {
                    DebugOutput(f);
                }

                // Enhance search by scanning for increment number if present
                Regex regexSameNameInDirectory = new Regex(targetNameRootAndTimestampNew + @"_(\d+)");
                
                // Inspect each match to find highest increment number
                foreach(string matchPathFound in matchPathsFound)
                {
                    Match matchSameNameInDirectory = regexSameNameInDirectory.Match(matchPathFound);
                    int foundIncrement;
                    if (matchSameNameInDirectory.Success)
                    {
                        foundIncrement = Convert.ToInt32(matchSameNameInDirectory.Groups[1].ToString());
                        DebugOutput("Found Incremented Name w num: " + foundIncrement.ToString());
                    }
                    else
                    {
                        foundIncrement = 1; //  Any unincremented files are treated as having an increment number of 1
                        DebugOutput("Found Unincremented Names:" + matchPathFound);
                    }

                    // If the increment number beats the record
                    if(foundIncrement > foundIncrementMax)
                    {
                        // Store the new record
                        foundIncrementMax = foundIncrement;
                    }

                }
                DebugOutput("Max Increment Found:" + foundIncrementMax);
                
            }
            else
            {
                DebugOutput("No matches found in directory");
            }

            // Determine this increment number
            string targetNameIncrementNew = Convert.ToString(foundIncrementMax + 1);
            DebugOutput("targetNameIncrementNew:" + targetNameIncrementNew);




            // Add Increment number to targetNameNew
            string targetNameNew = targetNameRoot + '_' + targetNameTimestampNew + '_' + targetNameIncrementNew;
            DebugOutput("targetNameNew:" + targetNameNew);


            // Construct Entire new path
            // add backslash if needed
            string targetPathNew = targetDirName + '\\' + targetNameNew + targetExt;
            DebugOutput("targetPathNew:" + targetPathNew);






            // Rename file using Move() method
            if (ftype == FTYPE.DIR)
            {
                System.IO.Directory.Move(targetPathPrev, targetPathNew);
            }
            else
            {
                System.IO.File.Move(targetPathPrev, targetPathNew);
            }




            Console.ReadKey();
        }

        public static void DebugOutput(string s)
        {
            if (DEBUGGING)
            {
                Console.WriteLine(s);
            }
        }
    }
}
