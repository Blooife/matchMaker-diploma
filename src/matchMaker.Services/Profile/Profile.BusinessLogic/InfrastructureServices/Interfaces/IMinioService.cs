using Microsoft.AspNetCore.Http;

namespace Profile.BusinessLogic.InfrastructureServices.Interfaces;

public interface IMinioService
{
    public string BucketName { get; set; }
    public string Endpoint { get; set; }
    Task UploadFileAsync(string objectName, IFormFile file);
    Task DeleteFileAsync(string objectName);
}