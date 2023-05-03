using Microsoft.AspNetCore.Http;
using Subscription_Monitoring_System_Data.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Subscription_Monitoring_System_Domain.Contracts
{
    public interface IImageService
    {
        public string SaveImage(IFormFile imageFile);
        public void DeleteImage(UserDto user);
    }
}
