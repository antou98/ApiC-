using WebApi.Models;

namespace WebApi.GraphAndFileHandler
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

        public bool CheckIfFolderExists(string folderName)
        {

            string folderPath = Path.Combine(baseDrivePath, folderName);
            return Directory.Exists(folderPath);
        }

        public void CreateFolder(string folderName)
        {

            string folderPath = Path.Combine(baseDrivePath, folderName);
            Directory.CreateDirectory(folderPath);
        }

        public void writeFile(List<GraphFile> bytes, string folderPath)
        {
            try
            {
                if (Directory.Exists(folderPath))
                {
                    foreach (GraphFile file in bytes)
                    {
                        File.WriteAllBytes(folderPath+ file.Name, file.data);
                    }
                }
                else
                {
                    Console.WriteLine("Does not exist FileWriter");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}
