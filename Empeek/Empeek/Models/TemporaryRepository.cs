using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;

namespace Empeek.Models
{
    public class TemporaryRepository
    {
        private static TemporaryRepository repo = new TemporaryRepository();

        public static TemporaryRepository Current { get { return repo; } }

        public string exceptionMessage = "";
        public bool exception = false;
        public List<string> dataDisk = new List<string>();
        public List<string> dataDir = new List<string>();
        public List<string> dataFile = new List<string>();
        public CountFilesAndDirs dataCount = new CountFilesAndDirs();

        public IEnumerable<string> GetAllDisks()
        {
            return GetHardDisks();
        }

        public IEnumerable<string> GetAllDirs(string path)
        {
            ResetValues();
            dataCount.CurrentPath = path;
            SeeDirContents(path);
            return dataDir;
        }

        #region FIRST TASK

        // Get all directories and files in current directory
        private void SeeDirContents(string path)
        {
            // Find all subdirectories and files
            if (Directory.Exists(path))
            {
                try
                {
                    dataDir.AddRange(Directory.GetDirectories(path).ToList());
                }
                catch
                {
                    exception = true;
                    exceptionMessage = "Denied access on the path " + path;
                    return;
                }

                try
                {
                    var tempList = Directory.GetFiles(path).ToList();
                    foreach (var item in tempList)
                    {
                        dataFile.Add(item.Split('\\').LastOrDefault());
                    }
                }
                catch
                {
                    exception = true;
                    exceptionMessage = "Access is denied to the file " + path.Split('\\').LastOrDefault();
                    return;
                }
            }
            else return;

            //BackForwardSet(path);
            
            GetCount(path);
        }

        #endregion

        #region SECOND TASK

        // Get count files
        private void GetCount(string strPath)
        {
            if (!Directory.Exists(strPath))
                return;

            DirectoryInfo dir = new DirectoryInfo(strPath);

            GetCountAllFiles(dir);
        }

        // Get Count All Files In Curent Directories And Subdirectories
        List<FileInfo> dirGetFiles = new List<FileInfo>();
        private void GetCountAllFiles(DirectoryInfo dir)
        {
            dirGetFiles.Clear();
            // Cheking if UnauthorizedAccessException Проверка для случаев, когда отказано в доступе к файлу или директории
            try
            {
                dirGetFiles.AddRange(dir.GetFiles());
            }
            catch
            {
                return;
            }

            foreach (FileInfo fileInfo in dirGetFiles)
            {
                if (fileInfo.Length <= 10485760)
                    dataCount.Less10Mb++;
                else if (10485760 <= fileInfo.Length && fileInfo.Length <= 52428800)
                    dataCount.More10Less50Mb++;
                else if (52428800 <= fileInfo.Length && fileInfo.Length <= 104857600)
                    dataCount.More50Less100Mb++;
                else dataCount.More100Mb++;
            }

            try
            {
                foreach (DirectoryInfo subDirs in dir.GetDirectories())
                {
                    if (subDirs.Attributes != FileAttributes.Directory)
                        continue;
                    GetCountAllFiles(subDirs);
                }
            }
            catch
            {
                return;
            }
        }

        #endregion

        // Get hard disks 
        public List<string> GetHardDisks()
        {
            foreach (var item in DriveInfo.GetDrives())
            {
                dataDisk.Add(item.ToString());
            }
            return dataDisk;
        }


        private void ResetValues()
        {
            exception = false;
            exceptionMessage = "";
            dataCount.Less10Mb = 0;
            dataCount.More10Less50Mb = 0;
            dataCount.More50Less100Mb = 0;
            dataCount.More100Mb = 0;
            dataCount.CurrentPath = "";
            dataDir.Clear();
            dataFile.Clear();
        }
    }
}