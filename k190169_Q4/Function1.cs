using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace k190169_Q4
{
    public static class Function1
    {
        [FunctionName("Function1")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];



            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;
            string responseMessage = String.Empty;
            if (string.IsNullOrEmpty(name))
            {
                responseMessage = "Please Enter Correct Formated Query!!!";
            }
            else
            {
                String path = AppDomain.CurrentDomain.BaseDirectory + "\\ Output \\"; // path containing all json files 
                string[] filePaths = Directory.GetFiles(path, "*.json");

                for(int i=0;i<filePaths.Length;i++)
                {
                    if(Path.GetFileName(filePaths[i]).Equals(name))
                    {
                        responseMessage = File.ReadAllText(filePaths[i]);
                        break;
                    }
                }

                if(responseMessage.Equals(String.Empty))
                 responseMessage = "Not Found";
            }

            return new OkObjectResult(responseMessage);
        }
    }
}
