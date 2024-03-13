using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.IO;

namespace VideoUpload.Utilidades
{
	public class AmazonManager
	{
		private string AwsAccessKey { get; set; }
		private string AwsSecretAccessKey { get; set; }

		public AmazonManager(string awsAccessKey, string awsSecretAccessKey)
		{
			AwsAccessKey = awsAccessKey;
			AwsSecretAccessKey = awsSecretAccessKey;
		}

		public bool CopyFile(string sourceBucket, string destinationBucket, string sourceFile, string destinationFile)
		{
			try
			{
				using (var client = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretAccessKey))
				{
					var request = new CopyObjectRequest
					{
						SourceBucket = sourceBucket,
						SourceKey = sourceFile,
						DestinationBucket = string.IsNullOrEmpty(destinationBucket) ? sourceBucket : destinationBucket,
						DestinationKey = destinationFile
					};
					CopyObjectResponse response = client.CopyObject(request);
				}

				return true;
			}
			catch (AmazonS3Exception s3Exception)
			{
				throw s3Exception;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool MoveFile(string sourceBucket, string destinationBucket, string sourceFile, string destinationFile)
		{
			try
			{
				using (var client = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretAccessKey))
				{
					var request = new CopyObjectRequest
					{
						SourceBucket = sourceBucket,
						SourceKey = sourceFile,
						DestinationBucket = string.IsNullOrEmpty(destinationBucket) ? sourceBucket : destinationBucket,
						DestinationKey = destinationFile
					};
					CopyObjectResponse response = client.CopyObject(request);

					var requestDel = new DeleteObjectRequest
					{
						BucketName = sourceBucket,
						Key = sourceFile
					};
					var retorno = client.DeleteObject(requestDel);
				}

				return true;
			}
			catch (AmazonS3Exception s3Exception)
			{
				throw s3Exception;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool DeleteFile(string sourceBucket, string sourceFile)
		{
			try
			{
				using (var client = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretAccessKey))
				{
					var requestDel = new DeleteObjectRequest
					{
						BucketName = sourceBucket,
						Key = sourceFile
					};
					var retorno = client.DeleteObject(requestDel);
				}

				return true;
			}
			catch (AmazonS3Exception s3Exception)
			{
				throw s3Exception;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public bool UploadFile(string sourceBucket, byte[] file, string fileName)
		{
			try
			{
				using (var client = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretAccessKey))
				{
					using (var stream = new MemoryStream(file))
					{
						var request = new PutObjectRequest()
							{
								BucketName = sourceBucket,
								CannedACL = S3CannedACL.AuthenticatedRead,								
								Key = fileName,
								InputStream = stream,
                                AutoCloseStream = false,
                                Timeout = 1800000
							};

						var response = client.PutObject(request);
					}
				}

				return true;
			}
			catch (AmazonS3Exception s3Exception)
			{
				throw s3Exception;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		public string GetUrlFile(string sourceBucket, string fileName)
		{
			try
			{
				var client = AWSClientFactory.CreateAmazonS3Client(AwsAccessKey, AwsSecretAccessKey);
				GetPreSignedUrlRequest request = new GetPreSignedUrlRequest();
				request.BucketName = sourceBucket;
				request.Key = fileName;
				request.Expires = DateTime.Now.AddHours(1);
				request.Protocol = Protocol.HTTP;
				var url = client.GetPreSignedURL(request);

				return url;
			}
			catch (AmazonS3Exception s3Exception)
			{
				throw s3Exception;
			}
			catch (Exception ex)
			{
				throw ex;
			}
		}

		/// <summary>
		/// This Generates a signed http url using a canned policy.
		/// To create the PEM file and KeyPairID please visit https://aws-portal.amazon.com/gp/aws/developer/account/index.html?action=access-key
		/// </summary>
		/// <param name="resourceURL">The URL of the distribution item you are signing.</param>
		/// <param name="expiryTime">UTC time to expire the signed URL</param>
		/// <param name="pemFileLocation">The path and name to the PEM file. Can be either Relative or Absolute.</param>
		/// <param name="keypairId">The ID of the private key used to sign the request</param>
		/// <param name="urlEncode">Whether to URL encode the result</param>
		/// <returns>A String that is the signed http request.</returns>
		public static string GetPreSignedURLWithPEMFile(string resourceURL, DateTime expiryTime, string pemFileLocation, string keypairId, bool urlEncode)
		{

			DateTime expires = DateTime.UtcNow.AddSeconds(30);


			expiryTime = DateTime.Now.AddHours(1);

			if (pemFileLocation.StartsWith("~"))
			{
				var baseDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
				pemFileLocation = Path.GetFullPath(baseDirectory + pemFileLocation.Replace("~", string.Empty));
			}

			System.IO.StreamReader myStreamReader = new System.IO.StreamReader(pemFileLocation);
			string pemKey = myStreamReader.ReadToEnd();
			pemKey = pemKey.Replace("\n", "");
			// return GetPreSignedURLWithPEMKey(resourceURL, expiryTime, pemKey, keypairId, urlEncode);


			return "";
		}
	}
}