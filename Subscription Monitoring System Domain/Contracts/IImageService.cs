using Microsoft.AspNetCore.Http;
using Subscription_Monitoring_System_Data.ViewModels;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IImageService
    {
        public string SaveImage(IFormFile imageFile);
        public void DeleteImage(UserViewModel user);
    }
}
