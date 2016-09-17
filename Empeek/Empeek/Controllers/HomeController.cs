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
        TemporaryRepository Repository = TemporaryRepository.Current;

        public ViewResult Index(string path)
        {
            if (Repository.dataCount.CurrentPath == null)
                Repository.GetAllDirs("D:\\Music");
            else Repository.GetAllDirs(path);

            if(Repository.dataDisk.Count == 0)
                ViewBag.DataDisk = Repository.GetAllDisks();

            ViewBag.ExceptionMessage = Repository.exceptionMessage;
            ViewBag.Exception = Repository.exception;

            // back - forward set

            ViewBag.DataDisk = Repository.dataDisk;
            ViewBag.DataDir = Repository.dataDir;
            ViewBag.DataFile = Repository.dataFile;
            ViewBag.CountFiles = Repository.dataCount;
            return View();
        }




        /*
        public ViewResult Index()
        {
            return View(Repository.GetAllDisks());
        }

        public ActionResult SeeFileContents(string path)
        {
            Repository.SeeDirContents(path);
            ViewBag.DataDir = Repository.dataDir;
            ViewBag.DataFile = Repository.dataFile;
            return View("Index");
        }

        public ActionResult CountFiles()
        {
            ViewBag.Repository = Repository.dataCount;
            return View("Index");
        }
    */
    }
}