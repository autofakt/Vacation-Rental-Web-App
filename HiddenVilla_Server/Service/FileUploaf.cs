using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HiddenVilla_Server.Service
{
    public class FileUploaf : IFileUpload
    {
        public bool DeleteFile(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task<string> UploadFile(IBrowserFile file)
        {
            throw new NotImplementedException();
        }
    }
}
