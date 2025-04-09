using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Portfolio.RepositoryConfig.IRepositories.IFileRepo;

namespace Portfolio.RepositoryConfig.Repositories.FileRepo
{
    public class FileRepository : IFileRepository
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public FileRepository(IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public void DeleteFile(string fileUrl)
        {
            // Get the root path of wwwroot
            var rootPath = _env.WebRootPath;
            // Delete old NID picture if it exists
            if (!string.IsNullOrEmpty(fileUrl))
            {
                var filePath = Path.Combine(rootPath, fileUrl.TrimStart('/'));
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
            }
        }

        public async Task<string> FileUpload(IFormFile file)
        {
            // Get the root path of wwwroot
            var rootPath = _env.WebRootPath;

            // Generate unique names for the files
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

            // Combine root path with file names to create file paths
            var filePath = Path.Combine(rootPath, "images", fileName);

            // Ensure the "images" folder exists in wwwroot
            var imagesFolder = Path.Combine(rootPath, "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            // Save the profile picture
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Create URLs for the saved files
            var fileUrl = $"{_httpContextAccessor.HttpContext?.Request.Scheme}://{_httpContextAccessor.HttpContext?.Request.Host}/images/{fileName}";
            return fileUrl;
        }
    }
}