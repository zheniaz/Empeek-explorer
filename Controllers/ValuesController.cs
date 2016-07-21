using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;

namespace Empeek.Controllers
{
    public class ValuesController : ApiController
    {
        public short less10Mb = 0;
        public short more10Less50Mb = 0;
        public short more100Mb = 0;
        List<string> listOfPath = new List<string>();
        public string currentPath = "";

        // GET api/values
        public IEnumerable<string> Get()
        {
            if (listOfPath == null)
            {
                return Start("C:\\");
            }
            return listOfPath;
        }

        // GET api/values/5
        public ushort[,,] Get([FromUri]string strPath)
        {
            if (!Directory.Exists(strPath))
                return new ushort[less10Mb, more10Less50Mb, more100Mb];
            currentPath = strPath;
            
            DirectoryInfo dir = new DirectoryInfo(strPath);
            GetCountAllFilesInCurentDirectoriesAndSubdirectories(dir);
            Start(strPath);
            return new ushort[less10Mb, more10Less50Mb, more100Mb];
        }

        void GetCountAllFilesInCurentDirectoriesAndSubdirectories(DirectoryInfo dir)
        {
            ResetValues();

            foreach (FileInfo fi in dir.GetFiles())
            {

                if (fi.Length <= 10485760)
                    less10Mb++;
                if (10485760 <= fi.Length && fi.Length <= 52428800)
                    more10Less50Mb++;
                if (104857600 <= fi.Length)
                    more100Mb++;
            }

            foreach (DirectoryInfo d in dir.GetDirectories())
                GetCountAllFilesInCurentDirectoriesAndSubdirectories(d);
        }

        void ResetValues()
        {
            less10Mb = 0;
            more10Less50Mb = 0;
            more100Mb = 0;
            listOfPath.Clear();
        }


        private List<string> Start(string path)
        {
            // Find all subdirectories and files
            if (Directory.Exists(path))
            {
                listOfPath.AddRange(Directory.GetDirectories(path).ToList());
                listOfPath.AddRange(Directory.GetFiles(path).ToList());
            }
            return listOfPath;
        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
