using Api.GraphReader;
using Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SimpleController : ControllerBase
    {
        private readonly ILogger<SimpleController> _logger;

        public SimpleController(ILogger<SimpleController> logger)
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

            if(data != null)
            {
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
