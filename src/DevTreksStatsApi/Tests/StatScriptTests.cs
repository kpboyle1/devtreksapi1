using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevTreks.DevTreksStatsApi.Models;
using DevTreks.DevTreksStatsApi.Helpers;
using DevTreks.DevTreksStatsApi.Client;

namespace DevTreks.DevTreksStatsApi.Tests
{
    /// <summary>
    ///Purpose:		Tests of Controller Api actions
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA Algorithm 2, subalgo 2 (R webapi) and subalgo 3 (python webap)
    ///Notes:       
    ///</summary>
    public class StatScriptTests
    {
        public static async Task<StatScript> GetAllTest(IStatScriptRepository StatScriptRep, bool isPyTest)
        {
            //only runs when first stat.IsDevelopment = true;
            StatScript testStat = StatScript.GetTestStatScript(StatScriptRep, isPyTest);

            //also runs createtest
            if (testStat.IsDevelopment && (!string.IsNullOrEmpty(testStat.Key)))
            {
                Uri uri = await ClientProgram.ClientCreate(testStat);
            }
            

            return testStat;
        }
        public static async Task<StatScript> CreateTest(IStatScriptRepository StatScriptRep)
        {
            StatScript testStat = new StatScript();
            int i = 0;
            //repository constructor adds a statscript by default 
            //which includes host scriptexecutable paths and isdevelopment property
            string sRScriptExecutablePath = string.Empty;
            string sPyScriptExecutablePath = string.Empty;
            string sDefaultRootFullFilePath = string.Empty;
            string sDefaultRootWebStoragePath = string.Empty;
            bool bIsDevelopment = false;
            bool bIsSuccess = false;
            foreach (var statscript in StatScriptRep.GetAll())
            {
                if (i == 0)
                {
                    //di in repository 
                    sRScriptExecutablePath = statscript.RExecutablePath;
                    sPyScriptExecutablePath = statscript.PyExecutablePath;
                    sDefaultRootFullFilePath = statscript.DefaultRootFullFilePath;
                    sDefaultRootWebStoragePath = statscript.DefaultRootWebStoragePath;
                    bIsDevelopment = statscript.IsDevelopment;
                    if (!bIsDevelopment)
                    {
                        return testStat;
                    }
                }
                else
                {
                    statscript.RExecutablePath = sRScriptExecutablePath;
                    statscript.PyExecutablePath = sPyScriptExecutablePath;
                    statscript.DefaultRootFullFilePath = sDefaultRootFullFilePath;
                    statscript.DefaultRootWebStoragePath = sDefaultRootWebStoragePath;
                    //statscript.OutputURL is set by host and stores the statresults as a blob
                    statscript.IsDevelopment = bIsDevelopment;
                    if (bIsDevelopment)
                    {
                        bIsSuccess = await ExecuteScript.RunScript(statscript);
                    }
                    else
                    {
                        bIsSuccess = await ExecuteScript.RunScript(statscript);
                    }
                    testStat = new StatScript(statscript);
                    //start with 1 script only
                    break;
                }
                i++;
            }
            return testStat;
        }
        public static async Task<StatScript> UpdateTest(IStatScriptRepository StatScriptRep, 
            StatScript statScript)
        {
            StatScript testStat = new StatScript(statScript);
            if (!statScript.IsDevelopment)
            {
                return testStat;
            }
            Uri uri = await ClientProgram.ClientUpdate(testStat);
            return testStat;
        }
        public static async Task<StatScript> DeleteTest(IStatScriptRepository StatScriptRep, 
            StatScript statScript)
        {
            StatScript testStat = new StatScript(statScript);
            if (!statScript.IsDevelopment)
            {
                return testStat;
            }
            Uri uri = await ClientProgram.ClientDelete(testStat);
            return testStat;
        }
    }
}
