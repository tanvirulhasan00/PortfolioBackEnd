using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Portfolio.RepositoryConfig.IRepositories.IFileRepo
{
    public interface IFileRepository
    {
        Task<string> FileUpload(IFormFile file);
        void DeleteFile(string fileUrl);

    }
}