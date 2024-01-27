using Newtonsoft.Json;
using WebApi.Models;

namespace WebApi.GraphAndFileHandler
{
    public class GraphFolderFinder
    {
        private string tenantId;
        private string baseEndpointUrl;

        public GraphFolderFinder()
        {
            this.tenantId = "b!dn0Km7g4ikqggvk5o2wPbzurM7uEjzxJoV64tz6Tp_XZ0hgfBfDjQJDSql9d2P_9";
            this.baseEndpointUrl = "https://graph.microsoft.com/v1.0/drives/" + tenantId + "/root/children";
        }

        public async Task<bool> getRootFolderTest()
        {
            bool hasAccess = false;
            try
            {
                //get token for graph api
                GraphAuthentificator graphAuthentificator = new GraphAuthentificator();
                string token = await graphAuthentificator.GetQualtechApiToken();

                Console.WriteLine(token);
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    var response = await client.GetAsync(this.baseEndpointUrl);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            Console.WriteLine("Has access");
                            hasAccess = true;
                        }
                        else
                        {
                            Console.WriteLine("Does not have access " + response.StatusCode);
                            hasAccess = false;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Response is null");
                        hasAccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur survenu dans GraphFolderFinder.getRootFolderTest() : " + ex.ToString());
            }
            return hasAccess;
        }

        public async Task<string> getFolderProperties(string pathToTest, string token)
        {
            string basePath = "https://graph.microsoft.com/v1.0/drives/b!dn0Km7g4ikqggvk5o2wPbzurM7uEjzxJoV64tz6Tp_XZ0hgfBfDjQJDSql9d2P_9/root:/";
            string idFolder = "";
            // Encode pathToTest correctement
            string encodedPathToTest = Uri.EscapeDataString(pathToTest);

            string path = basePath + encodedPathToTest;



            try
            {

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(path);

                    if (response != null)
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            Dictionary<string, object> driveItem = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(response.Content.ReadAsStringAsync().Result);
                            Console.WriteLine("Accès au dossier " + driveItem["id"]);
                            idFolder = driveItem["id"].ToString();
                        }
                        else
                        {
                            Console.WriteLine("Erreur d'accès: " + response.StatusCode);
                        }
                    }
                    else
                    {
                        Console.WriteLine("Response is null");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur survenue dans GraphFolderFinder.isValidPath() : " + ex.ToString());
            }
            return idFolder;
        }

        public async Task<ResponseGraph> getChildrenFiles(string pathToTest)
        {

            ResponseGraph reponseG = new ResponseGraph();


            List<GraphFile> filesToCopy = new List<GraphFile>();

            GraphAuthentificator graphAuthentificator = new GraphAuthentificator();
            string token = await graphAuthentificator.GetQualtechApiToken();

            string idFolder = await getFolderProperties(pathToTest, token);

            string url = "https://graph.microsoft.com/v1.0/drives/b!dn0Km7g4ikqggvk5o2wPbzurM7uEjzxJoV64tz6Tp_XZ0hgfBfDjQJDSql9d2P_9/items/" + idFolder + "/children";
            try
            {



                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Access aux fichiers de  " + idFolder);
                        var jsonString = await response.Content.ReadAsStringAsync();
                        var driveItemResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString);

                        response = await client.GetAsync(url);

                        reponseG.folderExists = true;


                        if (driveItemResponse.ContainsKey("value"))
                        {
                            List<Dictionary<string, object>> dictList = (List<Dictionary<string, object>>)JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(driveItemResponse["value"].ToString());

                            if (dictList is List<Dictionary<string, object>>)
                            {

                                //limitation de folders
                                foreach (var item in dictList.Take(4))
                                {
                                    if (item is Dictionary<string, object> driveItem)
                                    {
                                        // Console.WriteLine($"Drive Item Name: {driveItem["name"]}");
                                        // Console.WriteLine($"Download URL: {driveItem["@microsoft.graph.downloadUrl"]}");

                                        if (driveItem.ContainsKey("file"))
                                        {
                                            string downloadUrl = driveItem["@microsoft.graph.downloadUrl"].ToString();
                                            string filename = driveItem["name"].ToString();
                                            string format = "";

                                            using (HttpClient downloadClient = new HttpClient())
                                            {
                                                HttpResponseMessage downloadResponse = await downloadClient.GetAsync(downloadUrl);

                                                if (downloadResponse.IsSuccessStatusCode)
                                                {
                                                    // Read le fichier dans un byte array
                                                    byte[] fileData = await downloadResponse.Content.ReadAsByteArrayAsync();

                                                    // 
                                                    filesToCopy.Add(new GraphFile(fileData,filename,format));
                                                }
                                                else
                                                {
                                                    Console.WriteLine($"Error downloading file: {downloadResponse.StatusCode}");
                                                }
                                            }
                                        }

                                    }
                                }
                                Console.WriteLine($"Number of files : {filesToCopy.Count}");
                                reponseG.data = filesToCopy;

                            }
                            else
                            {
                                Console.WriteLine("Erreur peut pas typer le json correctement");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Erreur");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Erreur access au drive " + response.StatusCode);
                        reponseG.folderExists = true;
                    }
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur survenu dans GraphFolderFinder.getFolderContent() : " + ex.ToString());
            }

            return reponseG;
        }
    }

    

    public struct ResponseGraph
    {
        public bool folderExists;
        public List<GraphFile> data;

        public ResponseGraph()
        {
            this.folderExists = false;
            this.data = new List<GraphFile>();
        }
    }
}
