// <copyright file="ValuesController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace UsefulAPI.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        ////// GET api/values
        ////[HttpGet]
        ////public static ActionResult<IEnumerable<string>> Get()
        ////{
        ////    return new string[] { "value1", "value2" };
        ////}

        ////// GET api/values/5
        ////[HttpGet("{id}")]
        ////public static ActionResult<string> Get(int id)
        ////{
        ////    return "value";
        ////}

        ////// POST api/values
        ////[HttpPost]
        ////public static void Post([FromBody] string value)
        ////{
        ////}

        ////// PUT api/values/5
        ////[HttpPut("{id}")]
        ////public static void Put(int id, [FromBody] string value)
        ////{
        ////}

        ////// DELETE api/values/5
        ////[HttpDelete("{id}")]
        ////public static void Delete(int id)
        ////{
        ////}
    }
}