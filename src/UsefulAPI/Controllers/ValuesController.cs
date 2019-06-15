// <copyright file="ValuesController.cs" company="APH Software">
// Copyright (c) Andrew Hawkins. All rights reserved.
// </copyright>

namespace APIDocker.Controllers
{
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IList<string> _values = new List<string>() { "qwerty", "asdfgh" };

        public ValuesController()
        {
            // values = new List<string>() { "value1", "value2" };
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get() => _values;

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            if (_values.Count > id
                && id >= 0)
            {
                return $"value {id} = {_values[id]}";
            }

            return "No value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
            _values.Add(value);
        }

        /*
        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        */

        /*
        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        */
    }
}