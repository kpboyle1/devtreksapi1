using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using DevTreks.DevTreksStatsApi.Models;
using DevTreks.DevTreksStatsApi.Helpers;
using DevTreks.DevTreksStatsApi.Tests;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace DevTreks.DevTreksStatsApi.Controllers
{
    /// <summary>
    ///Purpose:		Controller api for running statistical scripts
    ///Author:		www.devtreks.org
    ///Date:		2016, September
    ///References:	CTA Algorithm 2, subalgo 2 (R webapi) and subalgo 3 (python webap)
    ///             based on: https://docs.asp.net/en/latest/tutorials/first-web-api.html
    ///             API	Description	                    Requestbody	    Responsebody
    ///             GET /api/statscript Get all StatScript items:   None            Array of to-do items
    ///             GET /api/statscript/{id}
    ///             Get an item by ID                   None            StatScript item
    ///             POST /api/statscript Add a new item StatScript item:      StatScript item
    ///             PUT /api/statscript/{id}
    ///             Update an existing item             StatScript item      None
    ///             PATCH /api/statscript/{id}
    ///             Update an existing item             StatScript item      None
    ///             DELETE /api/statscript/{id}
    ///             Delete an item.                     None            None
    ///</summary>
    [Route("api/[controller]")]
    public class StatScriptController : Controller
    {
        public StatScriptController(IStatScriptRepository statScript)
        {
            //statScripts come from dep injection in Startup.cs
            StatScriptRep = statScript;
        }
        public IStatScriptRepository StatScriptRep { get; set; }

        //GET /api/statscript
        [HttpGet]
        public async Task<IEnumerable<StatScript>> GetAll()
        {
            //tests only run if statscript.IsDevelopment = true
            //this also runs create test
            //set this to true to test python, false to test R
            bool bIsPyTest = false;
            StatScript testScript = await StatScriptTests.GetAllTest(StatScriptRep, bIsPyTest);

            //MVC automatically serializes the object to JSON and writes the JSON into the body of the response message. The response code for this method is 200, assuming there are no unhandled exceptions. (Unhandled exceptions are translated into 5xx errors.)
            return StatScriptRep.GetAll();
            //example of results
            //HTTP / 1.1 200 OK
            //Content - Type: application / json; charset = utf - 8
            //Server: Microsoft - IIS / 10.0
            //Date: Thu, 18 Jun 2015 20:51:10 GMT
            //Content - Length: 82

            //  [{ "Key":"4f67d7c5-a2a9-4aae-b030-16003dd829ae","Name":"Item1","IsComplete":false}]
        }

        //GET /api/statscript/{id}
        [HttpGet("{id}", Name = "GetStatScript")]
        public IActionResult GetById(string id)
        {
            //If no item matches the requested ID, the method returns a 404 error.This is done by returning NotFound.
            //Otherwise, the method returns 200 with a JSON response body.This is done by returning an ObjectResult
            var item = StatScriptRep.Find(id);
            if (item == null)
            {
                return NotFound();
            }
            return new ObjectResult(item);
        }
        //during tests this is initialized using the GetAll action
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StatScript initStat)
        {
            //The [FromBody] attribute tells MVC to get the value of the stat script from the body of the HTTP request.
            //initStat must include the scriptURL and dataURL, but nothing else is required
            if (initStat == null)
            {
                return BadRequest();
            }

            //adds a new guid to item.Key and stores in repository dictionary in memory
            StatScriptRep.Add(initStat);

            //runs scripts and adds results to initStat.StatisticalResult, saves in file system
            bool bHasStatResult = await ExecuteScript.RunScript(StatScriptRep, initStat);
            
            
            //devtreks client retrieves the url to the statscript object 
            //and consumes item.OutputURL blob holding the stat results string
            //The CreatedAtRoute method returns a 201 response, which is the standard response 
            //for an HTTP POST method that creates a new resource on the server. 
            //CreateAtRoute also adds a Location header to the response. 
            //The Location header specifies the URI of the newly created to-do item. See 10.2.2 201 Created.
            //the GetById method created the "GetStatScript" named route:
            return CreatedAtRoute("GetStatScript", new { id = initStat.Key }, initStat);
            //use the Location header URI to access the resource just created. 
            //item.OutputDataURL holds the statistical results
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, [FromBody] StatScript item)
        {
            //The response is 204 (No Content). According to the HTTP spec, a PUT request requires the client to send the entire updated entity, not just the deltas. To support partial updates, use HTTP PATCH
            if (item == null || item.Key != id)
            {
                return BadRequest();
            }
            var statscript = StatScriptRep.Find(id);
            if (statscript == null)
            {
                return NotFound();
            }

            //tests are run when statstcript.isdevelopment = true
            StatScript testScript = await StatScriptTests.UpdateTest(StatScriptRep, statscript);

            StatScriptRep.Update(item);
            return new NoContentResult();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromBody] StatScript item, string id)
        {
            //This overload is similar to the previously shown Update, but uses HTTP PATCH. The response is 204 (No Content).
            if (item == null)
            {
                return BadRequest();
            }

            var statscript = StatScriptRep.Find(id);
            if (statscript == null)
            {
                return NotFound();
            }

            //tests are run when statstcript.isdevelopment = true
            StatScript testScript = await StatScriptTests.UpdateTest(StatScriptRep, statscript);

            item.Key = statscript.Key;

            StatScriptRep.Update(item);
            return new NoContentResult();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            //The response is 204 (No Content).
            var statscript = StatScriptRep.Find(id);
            if (statscript == null)
            {
                return NotFound();
            }

            //tests are run when statstcript.isdevelopment = true
            StatScript testScript = await StatScriptTests.DeleteTest(StatScriptRep, statscript);

            //no harm: all scripts must be deleted
            StatScriptRep.Remove(id);
            return new NoContentResult();
        }
    }
}
