using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using DevTreks.DevTreksStatsApi.Models;

namespace DevTreks.DevTreksStatsApi.Helpers
{
    /// <summary>
    ///Purpose:		Run webapis using netcore apps
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA Algorithm 2, subalgo 2 (R webapi) and subalgo 3 (python webap)
    ///</summary>
    public class ExecuteScript
    {
        public static async Task<bool> CreateStatScript(IStatScriptRepository StatScriptRep,
            StatScript item)
        {

            bool bIsSuccess = false;
            StatScript.FillInProductionStatScript(StatScriptRep, item);
            if (item.IsDevelopment)
            {
                bIsSuccess = await ExecuteScript.RunScript(item);
            }
            else
            {
                bIsSuccess = await ExecuteScript.RunScript(item);
            }
            return bIsSuccess;
        }
        public static async Task<bool> RunScript(StatScript statScript)
        {
            StringBuilder sb = new StringBuilder();
            statScript.IsComplete = false;

            if (string.IsNullOrEmpty(statScript.DataURL) || (!statScript.DataURL.EndsWith(".csv")))
            {
                statScript.ErrorMessage = "The dataset file URL has not been added to the Data URL. The file must be stored in a Resource and use a csv file extension.";
            }
            if (string.IsNullOrEmpty(statScript.ScriptURL) || (!statScript.ScriptURL.EndsWith(".txt")))
            {
                statScript.ErrorMessage += "The script file URL has not been added to the Joint Data.The file must be stored in a Resource and use a txt file extension.";
            }

            string sScriptExecutable = string.Empty;
            if (statScript.StatType == StatScript.STAT_TYPE.py.ToString())
            {
                sScriptExecutable = statScript.PyExecutablePath;
            }
            else
            {
                sScriptExecutable = statScript.RExecutablePath;
            }
            string sDataURLFilePath = string.Empty;
            string sScriptURLFilePath = string.Empty;
            try
            {
                ProcessStartInfo start = new ProcessStartInfo();
                start.FileName = sScriptExecutable;
                start.RedirectStandardOutput = true;
                start.UseShellExecute = false;

                //task.when.all this
                sDataURLFilePath = await FileStorageIO.SaveURLInTempFile(statScript, statScript.DataURL);
                sScriptURLFilePath = await FileStorageIO.SaveURLInTempFile(statScript,
                    statScript.ScriptURL, sDataURLFilePath);
                //init url where stat results held
                statScript.OutputURL = string.Empty;

                start.Arguments = string.Format("{0} {1}", sScriptURLFilePath, sDataURLFilePath);
                start.CreateNoWindow = true;

                //the scripts are run sync
                using (Process process = Process.Start(start))
                {
                    using (StreamReader reader = process.StandardOutput)
                    {
                        sb.Append(await reader.ReadToEndAsync());
                    }

                    process.WaitForExit();
                }
                //this is how client accesses results 
                //api only returns json statscript and can't access wwwroot except through api
                statScript.StatisticalResult = sb.ToString();
                //result is added to temp file storage and path is converted to url for auditing
                //the url can't be directly accessed but the file path can be found
                statScript.OutputURL
                    = await FileStorageIO.SaveStringInURL(statScript, 
                    statScript.StatisticalResult, sDataURLFilePath);
                if (!string.IsNullOrEmpty(statScript.StatisticalResult))
                {
                    //fill in completed date -used to delete completed scripts on server
                    statScript.DateCompleted
                        = DateTime.Now.Date.ToString("d", CultureInfo.InvariantCulture);
                    statScript.IsComplete = true;
                }
            }
            catch (Exception x)
            {
                statScript.ErrorMessage += x.Message;
            }
            return statScript.IsComplete;
        }
    }
}
