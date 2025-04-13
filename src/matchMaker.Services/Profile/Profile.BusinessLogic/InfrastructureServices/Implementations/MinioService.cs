using Microsoft.AspNetCore.Http;
using Minio;
using Minio.DataModel.Args;
using Profile.BusinessLogic.InfrastructureServices.Interfaces;


namespace Profile.BusinessLogic.InfrastructureServices.Implementations;

public class MinioService : IMinioService
{
    private readonly IMinioClient _minioClient;
    
    public MinioService(string endpoint, string accessKey, string secretKey, string bucketname)
    {
        _minioClient = new MinioClient()
                            .WithEndpoint(endpoint)
                            .WithCredentials(accessKey, secretKey)
                            .WithSSL(false)
                            .Build();
        BucketName = bucketname;
        Endpoint = endpoint;
    }

    public string BucketName { get; set; }
    public string Endpoint { get; set; }
    public async Task UploadFileAsync(string objectName, IFormFile file)
    {
        var bucketExistsArgs = new BucketExistsArgs()
            .WithBucket(BucketName);
            
        bool found = await _minioClient.BucketExistsAsync(bucketExistsArgs).ConfigureAwait(false);
            
        if (!found)
        {
            await _minioClient.MakeBucketAsync(new MakeBucketArgs().WithBucket(BucketName));
        }
            
        using(var fileStream = new MemoryStream())
        {
            await file.CopyToAsync(fileStream);
                
            var fileBytes = fileStream.ToArray();
            var putObjectArgs = new PutObjectArgs()
                .WithBucket(BucketName)
                .WithObject(objectName)
                .WithStreamData(new MemoryStream(fileBytes))
                .WithObjectSize(fileStream.Length)
                .WithContentType(Path.GetExtension(objectName));
            await _minioClient.PutObjectAsync(putObjectArgs).ConfigureAwait(false);
        }
    }

    public async Task<Stream> GetFileAsync(string objectName)
    {
        var memoryStream = new MemoryStream();
        await _minioClient.GetObjectAsync(new GetObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectName)
            .WithCallbackStream(stream =>
            {
                stream.CopyTo(memoryStream);
            }));

        memoryStream.Position = 0;
        
        return memoryStream;
    }

    public async Task DeleteFileAsync(string objectName)
    {
        await _minioClient.RemoveObjectAsync(new RemoveObjectArgs()
            .WithBucket(BucketName)
            .WithObject(objectName));
    }
}