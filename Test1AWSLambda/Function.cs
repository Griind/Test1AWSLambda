using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;
using Amazon.Lambda.S3Events;
using Amazon.S3;
using Amazon.S3.Model;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Test1AWSLambda
{
    public class Function
    {
        private readonly string[] _supportedImageTypes = new string[] { ".jpg", ".png", ".jpeg" };
        private readonly AmazonS3Client _s3Client;

        public Function() => _s3Client = new AmazonS3Client();

        public async Task<string> FunctionHandlerAsync(S3Event s3Event, ILambdaContext context)
        {
            foreach (var record in s3Event.Records)
            {
                if (!_supportedImageTypes.Contains(Path.GetExtension(record.S3.Object.Key).ToLower()))
                {
                    Console.WriteLine(
                    $"Object {record.S3.Bucket.Name}:{record.S3.Object.Key} is not a supported image type");
                    continue;
                }
                Console.WriteLine(
                  $"Determining whether image {record.S3.Bucket.Name}:{record.S3.Object.Key} has been compressed");

                var taggingResponse = await _s3Client.GetObjectTaggingAsync(new GetObjectTaggingRequest
                {
                    BucketName = record.S3.Bucket.Name,
                    Key = record.S3.Object.Key
                });

                if (taggingResponse.Tagging.Any(tag => tag.Key == "Compressed" && tag.Value == "true"))
                {
                    Console.WriteLine(
                        $"Image {record.S3.Bucket.Name}:{record.S3.Object.Key} has already been compressed");
                    continue;
                }

                using(var objectResponse = await _s3Client.GetObjectAsync(record.S3.Bucket.Name, record.S3.Object.Key))
                using(Stream responseStream = objectResponse.ResponseStream)
                {
                    Console.WriteLine($"Compressing image {record.S3.Bucket.Name}:{record.S3.Object.Key}");
                }

                
            }
            return null;
        }
    }
}
