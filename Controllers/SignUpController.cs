using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Core;
using Microsoft.AspNetCore.Mvc.Cors;


namespace Core.Controllers
{
   
    public class SignUpController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult Default()
        {
            return View();
        }
        public IActionResult sub(int id)
        {
            string filename = String.Concat("c:\\users\\andrew\\source\\repos\\core\\views\\signup\\sub\\",id,".cshtml");
           FileResult mfile =  File("DownloadName",filename, "application/octet-stream");
        //     VirtualFileResult mfile =  File("DownloadName",filename);
        //     mfile.FileDownloadName = "testigfile";
        //     mfile.\
             return mfile;
        //    return 
        
        }
        public   FileResult File(string fileDownloadName, string filePath, string contentType = "application/octet-stream")  
        {     
           // return new VirtualFileResult(fileDownloadName, contentType);
           return new  FileResult(fileDownloadName, filePath, contentType);
        }
    }
}