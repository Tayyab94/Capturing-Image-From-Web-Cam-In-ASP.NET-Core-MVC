using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DemoWebCam.Controllers
{
    public class CamraController : Controller
    {
        private readonly IHostingEnvironment _environment;

        public CamraController(IHostingEnvironment hostingEnvironment)
        {
            this._environment=hostingEnvironment;
        }

        [HttpGet]
        public IActionResult Capture()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Capture(string name)
        {
            var files = HttpContext.Request.Form.Files;

            if(files!=null)
            {
                foreach (var file in files)
                {
                    if(file.Length>0)
                    {
                        // Getting the File name
                        var fileName = file.FileName;

                        // Unique file name Guid...
                        var myUniqueFileName = Convert.ToString(Guid.NewGuid());

                        //Getting the Extesion..
                        var fileExtension = Path.GetExtension(fileName);

                        //Connecting Filename + fileExtension (Guid Unique Name)
                        var newFileName = string.Concat(myUniqueFileName, fileExtension);

                        //Generating the Path to store the photo..
                        var filePath = Path.Combine(_environment.WebRootPath, "CamraPhoto") + $@"\{newFileName}";

                        if (!string.IsNullOrEmpty(filePath))
                        {
                            // Storing Image in Folder  
                            StoreInFolder(file, filePath);
                        }

                        var imageBytes = System.IO.File.ReadAllBytes(filePath);
                        //if (imageBytes != null)
                        //{
                        //    // Storing Image in Folder  
                        //    StoreInDatabase(imageBytes);
                        //}
                    }

                }
                return Json(true);
            }
               else
            {
                return Json(false);
            }

        }


        private void StoreInFolder(IFormFile file, string fileName)
        {
            using (FileStream fs = System.IO.File.Create(fileName))
            {
                file.CopyTo(fs);

                fs.Flush();
            }
        }



        /// <summary>  
        /// Saving captured image into database.  
        /// </summary>  
        /// <param name="imageBytes"></param>  
        //private void StoreInDatabase(byte[] imageBytes)
        //{
        //    try
        //    {
        //        if (imageBytes != null)
        //        {
        //            string base64String = Convert.ToBase64String(imageBytes, 0, imageBytes.Length);
        //            string imageUrl = string.Concat("data:image/jpg;base64,", base64String);
        //            ImageStore imageStore = new ImageStore()
        //            {
        //                CreateDate = DateTime.Now,
        //                ImageBase64String = imageUrl,
        //                ImageId = 0
        //            };
        //            _context.ImageStore.Add(imageStore);
        //            _context.SaveChanges();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

    }
}
