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

        // GET api/<controller>/5
        public List<PackageScraperService.Package.Script> Get(string os = null, string author = null)
        {
            List<PackageScraperService.Package.Script> _scripts = _packageScraperService.GetScripts();
            if (os != null)
            {
                _scripts = _scripts.Where(s => s.OSVer == os).ToList();
            }
            if (author != null)
            {
                _scripts = _scripts.Where(s => s.Authors.ToLower().Contains(author.ToLower())).ToList();
            }

            return _scripts;
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