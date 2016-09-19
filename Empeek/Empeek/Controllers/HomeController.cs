using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Empeek.Models;

namespace Empeek.Controllers
{
    public class HomeController : Controller
    {
        private static TemporaryRepository Repository = TemporaryRepository.Current;
        private string backStr = Repository.dataCount.CurrentPath;

        public ViewResult Index(string path)
        {
            Repository.GetAllDirs(path);

            if (Repository.dataDisk.Count == 0)
                ViewBag.DataDisk = Repository.GetAllDisks();

            ViewBag.ExceptionMessage = Repository.exceptionMessage;
            ViewBag.Exception = Repository.exception;

            if (Repository.dataCount.CurrentPath != null && Repository.dataCount.CurrentPath.Length >= 4)
                ViewBag.Back = BackPath(Repository.dataCount.CurrentPath);

            ViewBag.DataDisk = Repository.dataDisk;
            ViewBag.DataDir = Repository.dataDir;
            ViewBag.DataFile = Repository.dataFile;
            ViewBag.CountFiles = Repository.dataCount;
            return View();
        }

        private string BackPath(string str)
        {
            backStr = Repository.dataCount.CurrentPath.Remove(Repository.dataCount.CurrentPath.LastIndexOf('\\'));
            if (backStr.Length == 2)
                backStr = backStr + @"\";
            return backStr;
        }
    }
}