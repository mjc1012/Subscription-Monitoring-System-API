using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Subscription_Monitoring_System_Data.Dtos;
using Subscription_Monitoring_System_Data.Models;
using Subscription_Monitoring_System_Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Subscription_Monitoring_System_Data.Constants;

namespace Subscription_Monitoring_System_Domain.Services
{
    public class ImageService : IImageService
    {

        public ImageService(){}

        public string SaveImage(IFormFile imageFile)
        {
            try
            {
                var path = PathConstants.ProfilePicturesPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                var ext = Path.GetExtension(imageFile.FileName);

                string uniqueString = Guid.NewGuid().ToString();

                var newFileName = uniqueString + ext;
                var fileWithPath = Path.Combine(path, newFileName);
                var stream = new FileStream(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return newFileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteImage(UserDto user)
        {
            try
            {

                var path = Path.Combine(PathConstants.ProfilePicturesPath, user.ProfilePictureImageName);
                if (File.Exists(path))
                {
                    File.Delete(path);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
