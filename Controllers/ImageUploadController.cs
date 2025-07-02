using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration; // appsettings.json settings
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Net.Mime;

namespace MVCFlowerShop.Controllers
{
    public class ImageUploadController : Controller
    {
        private const string s3BucketName = "mvcflowershop-tp072943";

        // function 1: connection string to the aws account
        private List<string> getValues()
        {
            List<string> values = new List<string>();

            // link to appsettings.json and get back the values
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            IConfigurationRoot configure = builder.Build();  // build the json file 

            // read the info from json using configura instance 
            values.Add(configure["connectionToS3:Key1"]);
            values.Add(configure["connectionToS3:Key2"]);
            values.Add(configure["connectionToS3:Key3"]);

            Console.WriteLine("Connection to S3 bucket established successfully.");
            return values;
        }
        public IActionResult Index()
        {
            return View();
        }

        // function 2: upload the image to the S3 bucket and generate the url and store to db
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessUploadImage(List<IFormFile> imagefile)
        {
            // 1. add credentials to the AWS S3 client for action
            List<string> values = getValues();
            if (values.Count < 3)
            {
                Console.WriteLine("AWS S3 credentials are not properly configured.");
                return BadRequest("AWS S3 credentials are not properly configured.");
            }

            var awsS3Client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            // 2. read image files and upload to S3 bucket
            foreach (var image in imagefile)
            {
                Console.WriteLine($"Processing file: {image.FileName}");
                if (image.Length <= 0)
                {
                    Console.WriteLine("No files selected for upload.");
                    return BadRequest("No files selected for upload.");
                }
                else if (image.Length > 1048576) // 1 MB limit
                {
                    Console.WriteLine("File size exceeds the limit of 1 MB.");
                    return BadRequest("File size exceeds the limit of 1 MB.");
                }
                else if (image.ContentType.ToLower() != "image/png" &&
                           image.ContentType.ToLower() != "image/gif" &&
                           image.ContentType.ToLower() != "image/jpeg" &&
                           image.ContentType.ToLower() != "image/jpg")
                {
                    Console.WriteLine("Invalid file type. Only PNG, GIF, JPEG, and JPG are allowed.");
                    return BadRequest("Invalid file type. Only PNG, GIF, JPEG, and JPG are allowed.");
                }

                // 3. update images to S3 bucket and generate the URL
                try
                {
                    // upload to  S3 bucket
                    PutObjectRequest uploadRequest = new PutObjectRequest
                    {
                        InputStream = image.OpenReadStream(),
                        BucketName = s3BucketName,
                        Key = "images/" + image.FileName,
                        CannedACL = S3CannedACL.PublicRead
                    };

                    await awsS3Client.PutObjectAsync(uploadRequest);
                    Console.WriteLine($"Image {image.FileName} uploaded successfully to S3 bucket {s3BucketName}.");
                    Console.WriteLine($"Image URL: https://{s3BucketName}.s3.amazonaws.com/images/{image.FileName}");
                }
                catch (AmazonS3Exception s3Ex)
                {
                    Console.WriteLine($"S3 error: {s3Ex.Message}");
                    return BadRequest($"S3 error: {s3Ex.Message}");
                }
                catch (Exception ex)
                {

                }
            }
            return RedirectToAction("Index", "ImageUpload");
        }
        //function 3: Display image from S3 as gallery
        public async Task<IActionResult> DisplayImageFromS3()
        {
            //1. add credential for action
            List<string> values = getValues();
            var awsS3client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            List<S3Object> s3Objects = new List<S3Object>();
            // 2. get images from S3 bucket
            try
            {
                // s3 token - telling whether still image in the s3 bucket or not
                string token = null;
                do
                {
                    ListObjectsRequest request = new ListObjectsRequest
                    {
                        BucketName = s3BucketName,
                    };

                    // getting response image back from S3 bucket
                    ListObjectsResponse response = await awsS3client.ListObjectsAsync(request).ConfigureAwait(false);
                    s3Objects.AddRange(response.S3Objects);
                    token = response.NextMarker; // get the next marker for pagination

                } while (token != null);
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }

            return View(s3Objects);
        }

        //function 4: Delete image from S3
        public async Task<IActionResult> DeleteImage(string ImageName)
        {
            //1. add credential for action
            List<string> values = getValues();
            var awsS3client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);
            try
            {
                //create a delete request 
                DeleteObjectRequest deleteRequest = new DeleteObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = ImageName
                };
                await awsS3client.DeleteObjectAsync(deleteRequest);
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest(ex.Message);
            }
            return RedirectToAction("DisplayImageFromS3", "ImageUpload");
        }

        public async Task<IActionResult> DownloadImage(string ImageName)
        {
            //1. add credential for action
            List<string> values = getValues();
            var awsS3client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);
            Stream imageStream;

            //2.get image from S3 and transfer to network
            try
            {
                GetObjectRequest request = new GetObjectRequest
                {
                    BucketName = s3BucketName,
                    Key = ImageName
                };
                var getObjectResponse = await awsS3client.GetObjectAsync(request);
                using (var responseStream = getObjectResponse.ResponseStream)
                {
                    imageStream = new MemoryStream();
                    await responseStream.CopyToAsync(imageStream);
                    imageStream.Position = 0;
                }
            }
            catch (AmazonS3Exception exception)
            {
                throw new Exception("Read object operation failed.", exception);
            }
            catch (Exception exception)
            {
                throw new Exception("Read object operation failed.", exception);
            }

            // download / view the image through network
            string imagefile = Path.GetFileName(ImageName);
            Response.Headers.Add("Content-Disposition", new ContentDisposition
            {
                FileName = imagefile,
                Inline = true // false = prompt the user for downloading; true = browser to try to show the file inline
            }.ToString());
            return File(imageStream, "image/jpeg");
        }

        public async Task<IActionResult> GetPresignedURLImage(string ImageName)
        {
            //1. add credential for action
            List<string> values = getValues();
            var awsS3client = new AmazonS3Client(values[0], values[1], values[2], RegionEndpoint.USEast1);

            ViewBag.presignedUrl = "";
            //2. generate presigned URL for the image
            try
            {
                GetPreSignedUrlRequest presignedRequest = new GetPreSignedUrlRequest
                {
                    BucketName = s3BucketName,
                    Key = ImageName,
                    Expires = DateTime.UtcNow.AddMinutes(5) // URL valid for 5 minutes
                };
                ViewBag.presignedUrl = awsS3client.GetPreSignedURL(presignedRequest);
            }
            catch (AmazonS3Exception ex)
            {
                return BadRequest("Error: " + ex.Message);
            }
            Console.WriteLine("Presigned URL: " + ViewBag.presignedUrl);

            return View(ViewBag);
        }
    }
}
