using MSTPackagingHub.Services;

using System.Collections.Generic;


namespace MSTPackagingHub.Interfaces
{
    public interface IPackageScraper
    {
        List<PackageScraperService.Package.Script> GetScripts();
    }
}