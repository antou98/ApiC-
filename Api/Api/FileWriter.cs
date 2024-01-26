using Api.Models;

namespace Api
{
    public class FileWriter
    {
        public string baseDrivePath { get; set; }

        public FileWriter()
        {
            this.baseDrivePath = "C:\\testc#\\";
        }

        
        public string CreateNewFolder(string newFolderName)
        {
            
            string newpath = "";
          
            if (CheckIfFolderExists(newFolderName))
            {
                Console.WriteLine($"{newFolderName} exists.");

                newpath = Path.Combine(baseDrivePath, newFolderName);
            }
            else
            {
                Console.WriteLine($"{newFolderName} does not exist. Creating...");

                CreateFolder(newFolderName);

                Console.WriteLine($"{newFolderName} created.");

                newpath = Path.Combine(baseDrivePath, newFolderName);
            }
            return newpath;
        }

        public  bool CheckIfFolderExists(string folderName)
        {
        
            string folderPath = Path.Combine(baseDrivePath, folderName);
            return Directory.Exists(folderPath);
        }

        public  void CreateFolder(string folderName)
        {
            
            string folderPath = Path.Combine(baseDrivePath, folderName);
            Directory.CreateDirectory(folderPath);
        }

        public void writeFile(List<byte[]> bytes, string folderPath)
        {
            try
            {
                Console.WriteLine("s");
                int index = 0;
                if (Directory.Exists(folderPath))
                {
                    foreach (byte[] file in bytes)
                    {
                        File.WriteAllBytes(folderPath+$"copie{index}.pdf", file);
                        index++;
                    }
                }
                else
                {
                    Console.WriteLine("Does not exist FileWriter");
                }
            }
            catch(Exception e)
            {

            }
            
        }
    }
}
