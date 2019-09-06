using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using batman.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace batman.Controllers
{
    [Route("api/[controller]")]
    public class DictionaryLandmarkTypeController : Controller
    {
        DataAccessLayer da = new DataAccessLayer();

        // GET: api/<controller>
        [HttpGet("[action]")]
        public IEnumerable<DictionaryLandmarkType> GetAllLandmarks()
        {
            return da.GetAllLandmarks();
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost("[action]")]
        public bool CreateLandmarkType(DictionaryLandmarkType landmarkType)
        {
            return da.AddDictionaryLandmarkType(landmarkType);
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
