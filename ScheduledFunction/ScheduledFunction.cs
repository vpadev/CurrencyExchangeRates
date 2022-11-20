using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ScheduledFunction
{
    public class ScheduledFunction
    {
        [FunctionName("Function1")]
        // Cron experssion to run every day for once - 0 0 0 * * *
        public void Run([TimerTrigger("*/5 * * * * *")]TimerInfo myTimer, ILogger log)
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string fileName = Environment.GetEnvironmentVariable("FileName");
            process.StartInfo.FileName = fileName;
            process.StartInfo.Arguments = "save USD INR,EUR";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            log.LogInformation($"{output}: {DateTime.Now}");
            string err = process.StandardError.ReadToEnd();
            if(err.Length > 0)
            {
                log.LogInformation($"errors are \n\t{output}");
            }
            process.WaitForExit();

            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }
    }
}
