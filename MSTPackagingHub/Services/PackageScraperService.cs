using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using MSTPackagingHub.Interfaces;
using System.Xml;
using System.Xml.Serialization;

using System.Web.Script.Serialization;

namespace MSTPackagingHub.Services
{
    public class PackageScraperService : IPackageScraper
    {
        private void SerializeScripts<T>(T serializableObject, string fileName)
        {
            if (serializableObject == null) { return; }

            XmlDocument xmlDocument = new XmlDocument();
            XmlSerializer serializer = new XmlSerializer(serializableObject.GetType());
            using (MemoryStream stream = new MemoryStream())
            {
                serializer.Serialize(stream, serializableObject);
                stream.Position = 0;
                xmlDocument.Load(stream);
                xmlDocument.Save(fileName);
                stream.Close();
            }
        }

        public T DeSerializeScripts<T>(string fileName)
        {
            if (string.IsNullOrEmpty(fileName)) { return default(T); }

            T objectOut = default(T);

            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(fileName);
            string xmlString = xmlDocument.OuterXml;

            using (StringReader read = new StringReader(xmlString))
            {
                Type outType = typeof(T);

                XmlSerializer serializer = new XmlSerializer(outType);
                using (XmlReader reader = new XmlTextReader(read))
                {
                    objectOut = (T)serializer.Deserialize(reader);
                    reader.Close();
                }

                read.Close();
            }

            return objectOut;
        }


        public class Script
        {
            public string Name { get; set; }
            public string FullName { get; set; }
            public long Length { get; set; }
            public string Extension { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime CreationTimeUtc { get; set; }
            public DateTime LastWriteTime { get; set; }
            public DateTime LastWriteTimeUtc { get; set; }
            public DateTime LastAccessTime { get; set; }
            public DateTime LastAccessTimeUtc { get; set; }

            public string Text { get; set; }
        }

        private List<string> Directories = new List<string>();

        public List<Script> Scripts;

        public bool SetDirectories(string[] dirs)
        {
            foreach (string dir in dirs)
            {
                if (!Directory.Exists(dir))
                {
                    throw new System.IO.DirectoryNotFoundException($"Could not find directory at { dir }");
                }
                else
                {
                    Directories.Add(dir);
                }

            }
            return true;
        }

        public List<Script> GetScripts()
        {
            List<FileInfo> FindFiles(List<FileInfo> results, string dir, int depth)
            {
                try
                {
                    results.AddRange(new DirectoryInfo(dir).GetFiles("*.pl"));
                }
                catch (System.UnauthorizedAccessException) { }

                if (depth > 1)
                {
                    try
                    {
                        foreach (DirectoryInfo d in new DirectoryInfo(dir).GetDirectories())
                        {
                            FindFiles(results, d.FullName, depth - 1);
                        }
                    }
                    catch (System.UnauthorizedAccessException) { }

                }
                return results;
            }

            List<Script> _scripts = new List<Script>();

            foreach (string dir in Directories)
            {
                List<FileInfo> fsis = FindFiles(new List<FileInfo>(), dir, 4);

                foreach (FileInfo fsi in fsis)
                {
                    Script script = new Script
                    {
                        Name = fsi.Name,
                        FullName = fsi.FullName,
                        Length = fsi.Length,
                        Extension = fsi.Extension,
                        CreationTime = fsi.CreationTime,
                        CreationTimeUtc = fsi.CreationTimeUtc,
                        LastWriteTime = fsi.LastWriteTime,
                        LastWriteTimeUtc = fsi.LastWriteTimeUtc,
                        LastAccessTime = fsi.LastAccessTime,
                        LastAccessTimeUtc = fsi.LastAccessTimeUtc,
                        Text = File.ReadAllText(fsi.FullName)
                    };
                    _scripts.Add(script);
                }

            }

            return _scripts;
        }
        public bool LoadScripts()
        {
            string filePath = Path.Combine(HttpContext.Current.Server.MapPath("~"), "PackageScripts.xml");
            if (File.Exists(filePath))
            {
                Scripts = DeSerializeScripts<List<Script>>(filePath);
            }
            else
            {
                Scripts = GetScripts();
                SerializeScripts(Scripts, filePath);
            }

            return true;
        }

        JavaScriptSerializer jsonSerialiser = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

        public string GetLoadedScriptsJSON()
        {
            return jsonSerialiser.Serialize(Scripts);
        }

        public List<Script> GetLoadedScripts()
        {
            return Scripts;
        }

    }
}