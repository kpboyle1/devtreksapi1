using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;
using DevTreks.DevTreksStatsApi.Models;

namespace DevTreks.DevTreksStatsApi.Client
{
    /// <summary>
    ///Purpose:		Test running webapi using netcore client apps
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA Algorithm 2, subalgo 2 (R webapi) and subalgo 3 (python webapi)
    ///Note:        DevTreks implements a production client version of code
    ///</summary>
    public class ClientProgram
    {
        //static readonly string _baseAddress = "http://localhost:52958";
        public static async Task<Uri> RunClient(StatScript statScript)
        {
            HttpClient client = new HttpClient();
            
            var json = JsonConvert.SerializeObject(statScript);

            // Post statscript
            Uri address = new Uri(string.Concat(statScript.DefaultRootWebStoragePath, "api/statscript"));


            //create controller actionresult says this only returns a url 
            //to the created statscript referenced in Location Header
            HttpResponseMessage response = 
                await client.PostAsync(address, 
                new StringContent(json, Encoding.UTF8, "application/json"));
            
            // Check that response was successful or throw exception
            response.EnsureSuccessStatusCode();

            // the statistical result of running the statscript : the key to the statscript object created
            //{http://localhost:52958/api/StatScript/2e100e5e-997f-4b84-ac69-91b8add6bad2}
            Uri outputURL = response.Headers.Location;
            return outputURL;
            
            ////alternative is to return a response body with string holding the json stat results
            //string sResponse = await response.Content.ReadAsStringAsync();
            //return sResponse;
        }
        
        //static async void RunClient()
        //{
        //    HttpClient client = new HttpClient();
        //    StatScript ss = new StatScript();
        //    ss.Key = "123";
        //    ss.Name = "Test1";
        //    ss.DataURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1771/Regress1.csv";
        //    ss.ScriptURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1765/R1Web.txt";
        //    ss.OutputURL = "http://localhost:5000/resources/temp/2146500643.out1.csv";
        //    ss.IsComplete = false;
        //    string sGetAddress = string.Concat("http://localhost:5000/api/statscript/");

        //    // Post contact
        //    Uri address = new Uri(string.Concat(_baseAddress, "/api/statscript"));
        //    HttpResponseMessage response = await client.PostAsJsonAsync(address.ToString(), ss);

        //    // Check that response was successful or throw exception
        //    response.EnsureSuccessStatusCode();

        //    // Read result as StatScript
        //    StatScript result = await response.Content.ReadAsAsync<StatScript>();

        //    Console.WriteLine("Result: Name: {0} IsComplete: {1}", result.Name, result.IsComplete);
        //}

        //static async Task RunClient()
        //{
        //    // Create an HttpClient instance
        //    HttpClient client = new HttpClient();

        //    //// Use chunked encoding as otherwise HttpClient would try buffering the content to 
        //    //// figure out the content length.
        //    //client.DefaultRequestHeaders.TransferEncodingChunked = true;

        //    // Create a push content so that we can use XDocument.Save to a stream
        //    XDocument xDoc = XDocument.Load("Sample.xml", LoadOptions.None);
        //    PushStreamContent xDocContent = new PushStreamContent(
        //        (stream, content, context) =>
        //        {
        //            // After save we close the stream to signal that we are done writing.
        //            using (stream)
        //            {
        //                xDoc.Save(stream);
        //            }
        //        },
        //        "application/xml");

        //    // Send POST request to server and wait asynchronously for the response
        //    Uri address = new Uri(_baseAddress + "/api/book");
        //    HttpResponseMessage response = await client.PostAsync(address, xDocContent);

        //    // Ensure we get a successful response.
        //    response.EnsureSuccessStatusCode();

        //    // Read the response using XDocument as well
        //    Stream responseStream = await response.Content.ReadAsStreamAsync();
        //    XDocument xResponseDoc = XDocument.Load(responseStream);
        //    Console.WriteLine("Received response: {0}", xResponseDoc.ToString());
        //}
        //static async void RunClient3()
        //{

        //    StatScript ss = new StatScript();
        //    ss.Key = "123";
        //    ss.Name = "Test1";
        //    ss.DataURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1771/Regress1.csv";
        //    ss.ScriptURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1765/R1Web.txt";
        //    ss.OutputURL = "http://localhost:5000/resources/temp/2146500643.out1.csv";
        //    ss.IsComplete = false;
        //    string sGetAddress = string.Concat("http://localhost:5000/api/", ss);
        //    HttpClient client = new HttpClient();

        //    // Send asynchronous request
        //    HttpResponseMessage response = await client.GetAsync(sGetAddress);

        //    // Check that response was successful or throw exception
        //    response.EnsureSuccessStatusCode();

        //    // Read response asynchronously and save asynchronously to file
        //    using (FileStream fileStream = new FileStream("output.png", FileMode.Create, FileAccess.Write, FileShare.None))
        //    {
        //        await response.Content.CopyToAsync(fileStream);
        //    }

        //    Process process = new Process();
        //    process.StartInfo.FileName = "output.png";
        //    process.Start();
        //}
        //static async void RunClient2()
        //{
        //    try
        //    {
        //        //POST http://localhost:5000/api/statscript HTTP/1.1
        //        //Host: localhost: 5000
        //        //Content - Type: application / json

        //        //{
        //        //                    "title": "my title",
        //        //    "author": "my author"
        //        //}


        //        StatScript ss = new StatScript();
        //        ss.Key = "123";
        //        ss.Name = "Test1";
        //        ss.DataURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1771/Regress1.csv";
        //        ss.ScriptURL = "http://localhost:5000/resources/network_carbon/resourcepack_526/resource_1765/R1Web.txt";
        //        ss.OutputURL = "http://localhost:5000/resources/temp/2146500643.out1.csv";
        //        ss.IsComplete = false;

        //        //string sStatScript = "[{ "Key":"4f67d7c5-a2a9-4aae-b030-16003dd829ae","Name":"Item1","IsComplete":false}]";

        //        //does mvc automatically encode this as json?
        //        string sPostAddress = string.Concat("http://localhost:5000/api/", ss);
        //        HttpClient client = new HttpClient();
        //        // Send asynchronous request
        //        HttpResponseMessage response = await client.PostAsync(sPostAddress, ).ConfigureAwait(false);
        //        //// Create form parameters that are sent in the posted content body.
        //        //Dictionary<string, string> statScript = new Dictionary<string, string>
        //        //{
        //        //    { "key", ss.Key },
        //        //    { "name", ss.Name },
        //        //    { "dataurl", ss.DataURL },
        //        //    { "outputurl", ss.OutputURL },
        //        //    { "scriptURL", ss.ScriptURL },
        //        //    { "iscomplete", ss.IsComplete.ToString() },
        //        //};
        //        //FormUrlEncodedContent authentication = new FormUrlEncodedContent(statScript);
        //        //HttpClient client = new HttpClient();
        //        //// Send asynchronous request
        //        //HttpResponseMessage response = await client.PostAsync(sPostAddress, authentication).ConfigureAwait(false);
        //        //requires webappi.client 5.2.2. which is incompatible with netcore1
        //        //var statScript = new
        //        //{
        //        //    GlobalParameters = new Dictionary<string, string>() {
        //        //            { "dataurl", ss.DataURL },
        //        //            { "outputurl", ss.OutputURL },
        //        //            { "scriptURL", ss.ScriptURL },
        //        //        }
        //        //};
        //        //HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

        //        // Check that response was successful or throw exception
        //        response.EnsureSuccessStatusCode();

        //        //// If client authentication failed then we get a JSON response from Azure Market Place
        //        //if (!response.IsSuccessStatusCode)
        //        //{
        //        //    JToken error = await dataMarketResponse.Content.ReadAsAsync<JToken>();
        //        //    string errorType = error.Value<string>("error");
        //        //    string errorDescription = error.Value<string>("error_description");
        //        //    throw new HttpRequestException(string.Format("Azure market place request failed: {0} {1}", errorType, errorDescription));
        //        //}
        //        // Read response asynchronously and save asynchronously to file
        //        using (FileStream fileStream = new FileStream("out1.csv", FileMode.Create, FileAccess.Write, FileShare.None))
        //        {
        //            await response.Content.CopyToAsync(fileStream);
        //        }

        //        Process process = new Process();
        //        process.StartInfo.FileName = "output.png";
        //        process.Start();
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine("Request caused exception: {0}", e.Message);
        //    }
        //}

        //static void Main(string[] args)
        //{
        //    RunClient();

        //    Console.WriteLine("Hit ENTER to exit...");
        //    Console.ReadLine();
        //}
        //public static void RunScript(string[] args)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    string sScriptExecutable = string.Empty;
        //    if (args.Count() > 0)
        //    {
        //        sScriptExecutable = args[0];
        //    }
        //    string sScriptPath = string.Empty;
        //    if (args.Count() > 1)
        //    {
        //        sScriptPath = args[1];
        //    }
        //    string sInputPath = string.Empty;
        //    if (args.Count() > 2)
        //    {
        //        sInputPath = args[2];
        //    }
        //    ProcessStartInfo start = new ProcessStartInfo();
        //    start.FileName = sScriptExecutable;
        //    start.RedirectStandardOutput = true;
        //    start.UseShellExecute = false;
        //    start.Arguments = string.Format("{0} {1}", sScriptPath, sInputPath);
        //    start.CreateNoWindow = true;
        //    string sLastLine = string.Empty;
        //    using (Process process = Process.Start(start))
        //    {
        //        using (StreamReader reader = process.StandardOutput)
        //        {
        //            while (!reader.EndOfStream)
        //            {
        //                sLastLine = reader.ReadLine();
        //                sb.AppendLine(sLastLine);
        //            }

        //            //sb.Append(reader.ReadToEnd());
        //            //retain for writing to file
        //            //using (StreamWriter sw = new StreamWriter(output3))
        //            //{
        //            //    sb.Append(reader.ReadToEnd());
        //            //    sw.Write(sb.ToString());
        //            //}
        //        }
        //        process.WaitForExit();
        //    }
        //}
    }
}
