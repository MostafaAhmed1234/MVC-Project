using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Presentation.Layer.Helpers
{
    public static class DocumentSettings
    {
        //1.uploadFile 
        public static async Task<string> uploadFile(IFormFile formFile, string folderName)
        {
            //1.Get Located FolderPath that we will store file in it "wwwroot"
            //- to by daynamic because we will deploy in server, we won't do this
            //string folderPath = "D:\\.net route\\Vedios\\Eng Ahmed Nasr\\07 ASP.NET Core MVC\\Session 02\\Demos\\All.Solution\\Presentation.Layer\\wwwroot\\Files\\";
            //string folderPath = Directory.GetCurrentDirectory()+ "\\wwwroot\\Files\\"+ folderName;
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName);
            //================================
            //2.Get File Name and make it uinque
            string fileName = $"{Guid.NewGuid()}{formFile.FileName}";
            //================================
            //3. Get File Path
            string filePath = Path.Combine(folderPath, fileName);
            //=================
            //4.save file as streams[Data per time] + dispose file connection
            using var fileStream = new FileStream(filePath,FileMode.Create);
            //================
            //5.copy my file to fileStream
            await formFile.CopyToAsync(fileStream);
            //==========
            //6. return fileName to save it in DataBase
            return fileName;
        }

        //========================================
        //2.DeleteFile 
        public static void DeleteFile(string fileName, string folderName)
        {
            //1.Get FilePath for this File
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", folderName, fileName);
            //=============
            //2.check file is exists using class File, methed Exists
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
    }
}
