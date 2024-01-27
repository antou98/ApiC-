using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.GraphAndFileHandler;
using WebApi.Models;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasicController : ControllerBase
    {
        private readonly ILogger<BasicController> _logger;

        public BasicController(ILogger<BasicController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Route("test")]
        public async Task<IActionResult> test()
        {
            return Ok("Api OK");
        }

        [HttpPost]
        [Route("copyFiles")]
        public async Task<IActionResult> copyFilesToLocalServer([FromBody] BodySharePointLocation data)
        {
            // ... handle the POST request ...

            if (data != null)
            {
                GraphFolderFinder graphFinder = new GraphFolderFinder();
                //General/Manufacture/0 - General/10 - Quality control/Mill test (E-mail)
                ResponseGraph filesToSave = await graphFinder.getChildrenFiles(data.sharePointPath);

                if (filesToSave.folderExists != true)
                {
                    Console.WriteLine("Folder sharepoint entered does not exist");
                }
                else
                {
                    Console.WriteLine("NB files = " + filesToSave.data.Count);
                }

                FileWriter fw = new FileWriter();

                try
                {
                     

                    string pathOfNewFolder = fw.CreateNewFolder(data.localPath)+"\\";

                    Console.WriteLine($"{pathOfNewFolder}");
                    fw.writeFile(filesToSave.data, pathOfNewFolder);

                   

                    string responseMessage = "Hello, this is a simple API!";

                    return Ok(responseMessage);
                }
                catch (Exception ex)
                {
                    _logger.LogError($"An error occurred: {ex.Message}");
                    return StatusCode(500, "Internal Server Error");
                }

                return Ok("Succes");
            }
            else
            {
                return BadRequest();
            }
        }

        /*//http://localhost:5002/api/Simple/copyFiles
        [HttpPost]
        [Route("copyFiles")]
        public async Task<IActionResult> Get()
        {
            GraphFolderFinder graphFinder= new GraphFolderFinder();
            ResponseGraph filesToSave = await graphFinder.getChildrenFiles("General/Manufacture/0 - General/10 - Quality control/Mill test (E-mail)");

            if (filesToSave.folderExists != true)
            {
                Console.WriteLine("Folder sharepoint entered does not exist");
            }
            else
            {
                Console.WriteLine("NB files = " + filesToSave.data.Count);
            }

            FileWriter fw = new FileWriter();
            try
            {

                string pathOfNewFolder = fw.CreateNewFolder("j");

                
                fw.writeFile(filesToSave.data, pathOfNewFolder);

                //si < 0 nouveau path sinon existe déjà
                if (pathOfNewFolder.Length > 0 )
                {
                    
                    
                   
                }
                else
                {
                    
                }

                string responseMessage = "Hello, this is a simple API!";

                return Ok(responseMessage);
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred: {ex.Message}");
                return StatusCode(500, "Internal Server Error");
            }
        }*/
    }
}

