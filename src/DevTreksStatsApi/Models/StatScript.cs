using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevTreks.DevTreksStatsApi.Models
{
    /// <summary>
    ///Purpose:		POCO Model for running statistical scripts
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA Algorithm 2, subalgo 2 (R webapi) and subalgo 3 (python webap)
    ///Notes:       More sophisticated webapi apps may want to use standard DevTreks.ContentURI composite model
    ///</summary>
    public class StatScript
    {
        public StatScript()
        {
            this.Key = string.Empty;
            this.Name = string.Empty;
            this.DataURL = string.Empty;
            this.ScriptURL = string.Empty;
            this.OutputURL = string.Empty;
            this.StatType = string.Empty;
            this.RExecutablePath = string.Empty;
            this.PyExecutablePath = string.Empty;
            this.DefaultRootFullFilePath = string.Empty;
            this.DefaultRootWebStoragePath = string.Empty;
            this.IsComplete = false;
            this.IsDevelopment = false;
            this.ErrorMessage = string.Empty;
        }
        public StatScript(StatScript statScript)
        {
            this.Key = statScript.Key;
            this.Name = statScript.Name;
            this.DataURL = statScript.DataURL;
            this.ScriptURL = statScript.ScriptURL;
            this.OutputURL = statScript.OutputURL;
            this.StatType = statScript.StatType;
            this.RExecutablePath = statScript.RExecutablePath;
            this.PyExecutablePath = statScript.PyExecutablePath;
            this.DefaultRootFullFilePath = statScript.DefaultRootFullFilePath;
            this.DefaultRootWebStoragePath = statScript.DefaultRootWebStoragePath;
            this.IsComplete = statScript.IsComplete;
            this.IsDevelopment = statScript.IsDevelopment;
            this.ErrorMessage = statScript.ErrorMessage;
        }
        public enum STAT_TYPE
        {
            none = 0,
            r       = 1,
            py      = 2,
            aml     = 3
        }
        //first 1 prop set by api
        public string Key { get; set; }
        //these 4 properties are set by client and sent as POCO object
        public string Name { get; set; }
        public string DataURL { get; set; }
        public string ScriptURL { get; set; }
        public string OutputURL { get; set; }
        //the client sends this to host
        public string StatType { get; set; }
        //the host sets these 4 properties using di from appsettings
        public string RExecutablePath { get; set; }
        public string PyExecutablePath { get; set; }
        public string DefaultRootFullFilePath { get; set; }
        public string DefaultRootWebStoragePath { get; set; }
        //set by api
        public bool IsComplete { get; set; }
        public bool IsDevelopment { get; set; }
        public string ErrorMessage { get; set; }
        public static STAT_TYPE GetStatType(string executablepath)
        {
            STAT_TYPE eStatType = STAT_TYPE.none;
            if (executablepath.Contains("python"))
            {
                eStatType = STAT_TYPE.py;
            }
            else
            {
                eStatType = STAT_TYPE.r;
            }
            //aml addressed when subalgo 4 is debugged
            return eStatType;
        }
}
}
