using System;
using System.IO;
using Microsoft.Azure;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.FileExtensions;
using Microsoft.Extensions.Configuration.Json;

namespace BlobConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("apsettings.json");

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json",true,true)
                .Build();
            var connectionString = config["connectionString"];

            Console.WriteLine(connectionString);

            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(connectionString);
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            CloudBlobContainer blobContainer = blobClient.GetContainerReference("newcontainer");
            blobContainer.CreateIfNotExists();
            blobContainer.SetPermissions(new BlobContainerPermissions{
                PublicAccess = BlobContainerPublicAccessType.Blob
            });

            CloudBlockBlob blob = blobContainer.GetBlockBlobReference("foto.jpg");

            using (var fileStream = System.IO.File.OpenRead(@"C:\Users\gfajardo.CECOMSACORP\OneDrive - CECOMSA S.A\Imágenes\Wallpapers\20898.jpg"))
            {
                blob.UploadFromStream(fileStream);
            }

            Console.WriteLine("Archivo en la nube");
        }
    }
}
