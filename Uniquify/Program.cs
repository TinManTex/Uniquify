using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Uniquify {
    class Program {
        static void Main(string[] args) {
            if (args.Length!=1) {
                //TODO print some usage
                return;
            }

            string path = args[0];
            bool filterChars = false;//DEBUGNOW
            if (args.Length > 1) {
                filterChars = args[1] == "filter";
            }

            if (File.Exists(path) == false && Directory.Exists(path) == false) {
                Console.WriteLine("Could not find file or path " + path);
                return;
            }

            Uniquify(path, filterChars);
        }

        static List<char> characters = new List<char>{
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z',
            'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
            '0', '1', '2', '3', '4', '5', '6', '7', '8', '9',
            '_', '-',
            '/',
            '\\',
            '`','~','!','@','#','$','%','^','&','*','(',')','+','=','[','{',']','}',';',':','?','>','<',
            '.',
        };


        static public void Uniquify(string path, bool filterChars) {


            HashSet<string> stringHashSet = new HashSet<string>();

            

            string[] files = null; 
            if (File.Exists(path)) {
                files = new string[] { path };
            }
            if (Directory.Exists(path)) {
                files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            }

            foreach (string filePath in files) {
                Console.WriteLine(filePath);
                string line;
                using (StreamReader inputText = new StreamReader(filePath)) {
                    while ((line = inputText.ReadLine()) != null) {
                        if (filterChars)
                        {
                            bool add = true;
                            foreach (char character in line)
                            {
                                if (!characters.Contains(character))
                                {
                                    add = false;
                                }
                            }

                            

                            if (add)
                            {
                                stringHashSet.Add(line);
                            }
                        }
                        else
                        {
                            stringHashSet.Add(line);
                        }
                    }
                }
            }

            string[] stringList = stringHashSet.ToArray<string>();
            stringList = stringList.OrderBy(s => s, StringComparer.InvariantCultureIgnoreCase).ToArray<string>();

            string outPath = Path.GetDirectoryName(path) + "\\";
            string outFileName = Path.GetFileNameWithoutExtension(path) + "_unique.txt";
            if (filterChars)
            {
                outFileName = Path.GetFileNameWithoutExtension(path) + "_unique_filtered.txt";
            }
            outPath += outFileName;
            outPath = outPath.Replace("\\", "/");
            File.WriteAllLines(outPath, stringList);
        }

        public class StrCmpLogicalSort : IComparer<string> {

            [DllImport("shlwapi.dll", CharSet = CharSet.Unicode, ExactSpelling = true)]
            static extern int StrCmpLogicalW(String x, String y);

            public int Compare(string x, string y) {
                return StrCmpLogicalW(x, y);
            }

        }

    }
}
