using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniquify {
    class Program {
        static void Main(string[] args) {
            if (args.Length!=1) {
                //TODO print some usage
                return;
            }

            string path = args[0];

            if (File.Exists(path) == false && Directory.Exists(path) == false) {
                Console.WriteLine("Could not find file or path " + path);
                return;
            }

            Uniquify(path);
        }

        static public void Uniquify(string path) {
            HashSet<string> stringHashSet = new HashSet<string>();

            List<string> stringList = stringHashSet.ToList<string>();

            string[] files = null; 
            if (File.Exists(path)) {
                files = new string[] { path };
            }
            if (Directory.Exists(path)) {
                files = Directory.GetFiles(path, "*.txt", SearchOption.AllDirectories);
            }

            foreach (string filePath in files) {
                string line;
                using (StreamReader inputText = new StreamReader(filePath)) {
                    while ((line = inputText.ReadLine()) != null) {
                        stringHashSet.Add(line);
                    }
                }
            }

            stringList = stringHashSet.ToList<string>();
            stringList.Sort();

            string outPath = Path.GetDirectoryName(path) + "\\";
            string outFileName = Path.GetFileNameWithoutExtension(path) + "_unique.txt";
            outPath += outFileName;
            outPath = outPath.Replace("\\", "/");
            File.WriteAllLines(outPath, stringList.ToArray());
        }


    }
}
