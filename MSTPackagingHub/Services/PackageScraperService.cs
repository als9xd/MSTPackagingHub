using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MSTPackagingHub.Interfaces;
using System.Xml;
using System.Xml.Serialization;

using System.Web.Script.Serialization;
using System.Configuration;
using System.Data;
using System.Data.SQLite;
using Dapper;

using System.Text.RegularExpressions;

namespace MSTPackagingHub.Services
{
    public class PackageScraperService : IPackageScraper
    {
        private class SqliteDataAccess
        {
            public static List<Package.Script> LoadScripts()
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {
                    var output = cnn.Query<Package.Script>("select * from Script", new DynamicParameters());
                    return output.ToList();
                }
            }

            public static void SaveScript(Package.Script script)
            {
                using (IDbConnection cnn = new SQLiteConnection(LoadConnectionString()))
                {

                    cnn.Execute(@"insert into Script
                                    (PackageId, OSVer, Environment, FileName, Authors, Path, Text, Info, CreationTime, CreationTimeUtc, LastAccessTime, LastAccessTimeUtc, LastWriteTime, LastWriteTimeUtc)
                                values
                                    (@PackageId, @OSVer, @Environment, @FileName, @Authors, @Path, @Text, @Info, @CreationTime, @CreationTimeUtc, @LastAccessTime, @LastAccessTimeUtc, @LastWriteTime, @LastWriteTimeUtc)
                    ", new
                    {
                        PackageID = script.PackageID,
                        OSVer = script.OSVer,
                        Environment = script.Environment,
                        FileName = script.FileName,
                        Authors = script.Authors,
                        Path = script.Path,
                        Text = script.Text,
                        Info = script.Info,
                        CreationTime = script.CreationTime,
                        CreationTimeUtc = script.CreationTimeUtc,
                        LastAccessTime = script.LastAccessTime,
                        LastAccessTimeUtc = script.LastAccessTimeUtc,
                        LastWriteTime = script.LastWriteTime,
                        LastWriteTimeUtc = script.LastWriteTimeUtc,
                    });
                }
            }

            private static string LoadConnectionString(string id = "Default")
            {
                return ConfigurationManager.ConnectionStrings[id].ConnectionString;
            }
        }

        public class Package
        {
            public class Script
            {
                public string PackageID { get; set; }
                public string OSVer { get; set; }
                public string Environment { get; set; }

                public string Authors { get; set; }
                public string FileName { get; set; }
                public string Path { get; set; }
                public long Length { get; set; }
                public string Extension { get; set; }
                public DateTime CreationTime { get; set; }
                public DateTime CreationTimeUtc { get; set; }
                public DateTime LastWriteTime { get; set; }
                public DateTime LastWriteTimeUtc { get; set; }
                public DateTime LastAccessTime { get; set; }
                public DateTime LastAccessTimeUtc { get; set; }

                public string Text { get; set; }
                public string Info { get; set; }

                public static List<Script> FindScripts(string dir,int maxDepth)
                {
                    List<FileInfo> FindFiles(List<FileInfo> results, string curDir, int depth)
                    {
                        try
                        {
                            results.AddRange(new DirectoryInfo(curDir).GetFiles("*.pl"));
                        }
                        catch (System.UnauthorizedAccessException) { }

                        if (depth > 1)
                        {
                            try
                            {
                                foreach (DirectoryInfo d in new DirectoryInfo(curDir).GetDirectories())
                                {
                                    FindFiles(results, d.FullName, depth - 1);
                                }
                            }
                            catch (System.UnauthorizedAccessException) { }

                        }
                        return results;
                    }

                    List<Script> scripts = new List<Script>();

                    List<FileInfo> fsis = FindFiles(new List<FileInfo>(), dir, maxDepth);

                    foreach (FileInfo fsi in fsis)
                    {
                        string text = File.ReadAllText(fsi.FullName);
                        Regex commentBlocksR = new Regex(@"\=pod(?:(?:(?!\=pod))|(?:[^\*]))*\=cut|\/\/[^\n\r]*(?=[\n\r])", RegexOptions.Multiline);
                        MatchCollection commentBlocks = commentBlocksR.Matches(text);
                        Regex commentLinesR = new Regex(@"#(.*)", RegexOptions.Multiline);
                        MatchCollection commentLines = commentLinesR.Matches(text);

                        Dictionary<string, string> authors = new Dictionary<string, string>();

                        using (var reader = new StreamReader(@"C:\Users\t-als9xd\source\repos\MSTPackagingHub\MSTPackagingHub\CSV_Database_of_First_Names.csv"))
                        {
                            List<string> firstNames = new List<string>();
                            while (!reader.EndOfStream)
                            {
                                firstNames.Add(reader.ReadLine().Trim());
                            }

                            foreach (Match m in commentLines)
                            {
                                foreach (string firstName in firstNames)
                                {
                                    Regex firstNameExistsR = new Regex(firstName + @"\s", RegexOptions.Multiline);
                                    var test = m.ToString();
                                    var nMatches = firstNameExistsR.Matches(m.ToString());

                                    if(m.Length != 0)
                                    {
                                        Regex firstNameR = new Regex(firstName + @"\s([A-Z][a-z]*\.?)?\s?([A-Z][a-z]*\,?)?\s?", RegexOptions.Multiline);
                                        MatchCollection fNameMatches = firstNameR.Matches(m.ToString());

                                        foreach (Match fnm in fNameMatches)
                                        {
                                            string fNameResult = fnm.ToString().Replace("\n", "").Replace("\r", "").Trim();
                                            authors[firstName] = fNameResult;
                                        }
                                    }
                                }

                            }

                            string joinedAuthors = string.Join(", ", authors.Select(x => x.Value));

                            List<string> readmeInfo = new List<string>();
                            List<FileInfo> readmeFsis = new List<FileInfo>();
                            readmeFsis.AddRange(new DirectoryInfo(fsi.Directory.ToString()).GetFiles("readme*"));
                            readmeFsis.AddRange(new DirectoryInfo(fsi.Directory.ToString()).Parent.GetFiles("readme*"));
                            if (readmeFsis.Count() != 0)
                            {
                                readmeInfo.Add(HttpUtility.JavaScriptStringEncode(File.ReadAllText(readmeFsis[0].FullName),true));
                            }

                            Script script = new Script
                            {
                                Authors = joinedAuthors,
                                FileName = fsi.Name,
                                Path = fsi.FullName,
                                Length = fsi.Length,
                                Extension = fsi.Extension,
                                CreationTime = fsi.CreationTime,
                                CreationTimeUtc = fsi.CreationTimeUtc,
                                LastWriteTime = fsi.LastWriteTime,
                                LastWriteTimeUtc = fsi.LastWriteTimeUtc,
                                LastAccessTime = fsi.LastAccessTime,
                                LastAccessTimeUtc = fsi.LastAccessTimeUtc,
                                Text = text,
                                Info = $"[{String.Join(",",readmeInfo.ToArray())}]"
                            };
                            string[] scriptPath = script.Path.Split(System.IO.Path.DirectorySeparatorChar);

                            Dictionary<string, int> dirAttrIndexes = Package.FindPackageDirAttrs(scriptPath);

                            Package.Script scriptObj = new Package.Script
                            {
                                PackageID = scriptPath[dirAttrIndexes["PackageID"]],
                                OSVer = scriptPath[dirAttrIndexes["OSVer"]],
                                Environment = dirAttrIndexes.ContainsKey("Environment") ? scriptPath[dirAttrIndexes["Environment"]] : null,
                                FileName = script.FileName,
                                Authors = script.Authors,
                                Path = script.Path,
                                Text = script.Text,
                                Info = script.Info,
                                CreationTime = script.CreationTime,
                                CreationTimeUtc = script.CreationTimeUtc,
                                LastAccessTime = script.LastAccessTime,
                                LastAccessTimeUtc = script.LastAccessTimeUtc,
                                LastWriteTime = script.LastWriteTime,
                                LastWriteTimeUtc = script.LastWriteTimeUtc,
                            };

                            SqliteDataAccess.SaveScript(scriptObj);

                        }
                    }
                
                    return scripts;
                }

            }

            public static Dictionary<string, int> FindPackageDirAttrs(string[] dirs){
                int dirLen = dirs.Count();

                string[] ValidOSver =
                {
                        "winxp",
                        "win7",
                        "win8",
                        "win10"
                };

                Dictionary<string, int> structure = new Dictionary<string, int>();

                // Find OSVer
                for (int i = 0; i < dirLen; i++)
                {
                    if (ValidOSver.Contains(dirs[i]))
                    {
                        structure.Add("OSVer",i);
                        break;
                    }
                }

                // Set PackageId
                if(structure["OSVer"] + 2 < dirLen)
                {
                    structure.Add("PackageID", structure["OSVer"] + 2);
                }

                // Set Environment
                string[] ValidEnv =
                {
                    "prod",
                    "dev"
                };
                if (structure.ContainsKey("PackageID") && (structure["PackageID"] + 1 < dirLen))
                {
                    if (ValidEnv.Contains(dirs[structure["PackageID"] + 1]))
                    {
                        structure.Add("Environment", structure["PackageID"] + 1);
                    }
                }
                else if (structure.ContainsKey("OSVer") && (structure["OSVer"] + 3 < dirLen))
                {
                    if ( ValidEnv.Contains(dirs[structure["OSVer"] + 3]) )
                    { 
                        structure.Add("Environment", structure["OSVer"] + 3);
                    }
                }
                return structure;
            }

            public string SetDirAtrribute(string attr,string[] dirs)
            {
                switch (attr)
                {
                    case "PackageID":


                    case "OSVer":
                        string[] ValidValues =
                        {
                            "winxp",
                            "win7",
                            "win8",
                            "win10"
                        };
                        string osVer = null;
                        if (dirs.Count() > 5)
                        {
                            osVer = dirs[5];
                        }
                        if (!ValidValues.Contains(osVer))
                        {
                            foreach (string dir in dirs)
                            {
                                if (ValidValues.Contains(dir))
                                {
                                    return dir;
                                }
                            }
                            return null;
                        }
                        return osVer;

                    default:
                        return null;
                }
           
            }


        }

        // \\minerfiles.mst.edu\dfs\software\itwindist\win10\appdist\sccm_2012_drivers\20151030T152938_rename_mapping.pl

        public void LoadScripts(string[] dirs)
        {

            foreach (string dir in dirs)
            {
              //List<Package.Script> scripts = Package.Script.FindScripts(dir, 4);
            }
        }

        public List<Package.Script> GetScripts()
        {
            return SqliteDataAccess.LoadScripts();
        }

    }
}