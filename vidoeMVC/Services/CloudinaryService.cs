﻿

namespace vidoeMVC.Services
{
  
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.IO;
using System.Threading.Tasks;

public class CloudinaryService
    {
        private readonly Cloudinary _cloudinary;

        public CloudinaryService(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        public async Task<VideoUploadResult> UploadVideoAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new VideoUploadParams()
            {
                File = new FileDescription(fileName, fileStream)
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }

        public async Task<ImageUploadResult> UploadThumbnailAsync(Stream fileStream, string fileName)
        {
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(fileName, fileStream)
            };

            return await _cloudinary.UploadAsync(uploadParams);
        }
    }

}
