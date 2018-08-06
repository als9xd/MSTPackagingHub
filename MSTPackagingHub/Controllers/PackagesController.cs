using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MSTPackagingHub.Interfaces;
using MSTPackagingHub.Services;

namespace MSTPackagingHub.Controllers
{

    public class PackagesController : ApiController
    {
        private IPackageScraper _packageScraperService;

        public PackagesController(IPackageScraper packageScraperService)
        {
            _packageScraperService = packageScraperService;
        }

        // GET api/<controller>
        public List<PackageScraperService.Script> Get()
        {
            return _packageScraperService.GetLoadedScripts();
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}