using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace Test1AWSLambda
{
    public class Function
    {
        private readonly string[] _supportedImageTypes = new string[] { ".jpg", ".png", ".jpeg" };
        private readonly AmazonS3Client _s3Client;
        public Function() => _s3Client = new AmazonS3Client();

        public string[] SupportedImageTypes => _supportedImageTypes;

        public string FunctionHandler(S3Event s3Event, ILambdaContext context)
        {
            foreach (var record in s3Event.Records)
            {
                if (SupportedImageTypes.Contains(Path.GetExension()))
                {

                }
            }
        }
    }
}
