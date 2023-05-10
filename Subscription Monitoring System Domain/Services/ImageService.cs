using Microsoft.AspNetCore.Http;
using Subscription_Monitoring_System_Data.ViewModels;
using Subscription_Monitoring_System_Domain.Contracts;
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
                string path = PathConstants.ProfilePicturesPath;
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }


                string ext = Path.GetExtension(imageFile.FileName);

                string uniqueString = Guid.NewGuid().ToString();

                string newFileName = uniqueString + ext;
                string fileWithPath = Path.Combine(path, newFileName);
                FileStream stream = new(fileWithPath, FileMode.Create);
                imageFile.CopyTo(stream);
                stream.Close();
                return newFileName;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteImage(UserViewModel user)
        {
            try
            {

                string path = Path.Combine(PathConstants.ProfilePicturesPath, user.ProfilePictureImageName);
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
