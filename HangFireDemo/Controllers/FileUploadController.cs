using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace HangFireDemo.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class FileUploadController : ControllerBase
	{
		private readonly IBackgroundJobClient _backgroundJobClient;
		private readonly FileProcessingService _fileProcessingService;

		public FileUploadController(IBackgroundJobClient backgroundJobClient, FileProcessingService fileProcessingService)
		{
			_backgroundJobClient = backgroundJobClient;
			_fileProcessingService = fileProcessingService;
		}

		// Endpoint for uploading files
		//[HttpPost("upload")]
		//public IActionResult UploadFiles(List<IFormFile> files)
		//{
		//	// Enqueue the background job to process the files
		//	BackgroundJob.Enqueue(() => _fileProcessingService.ProcessUploadedFiles(files));

		//	return Ok(new { Message = "Files are being processed in the background." });
		//}

		[HttpPost("upload")]
		public IActionResult UploadFiles(IFormFile[] files)
		{
			// Create a folder to store the uploaded files
			string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
			if (!Directory.Exists(uploadFolder))
			{
				Directory.CreateDirectory(uploadFolder);
			}

			// Save each file to the upload folder and enqueue a background job
			foreach (var file in files)
			{
				string filePath = Path.Combine(uploadFolder, file.FileName);
				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					file.CopyTo(stream);
				}

				_backgroundJobClient.Enqueue<FileProcessingService>(job => job.ProcessFile(filePath));
			}

			return Ok("Files uploaded successfully");
		}

		// Endpoint for retrieving all uploaded file names
		[HttpGet("files")]
		public IActionResult GetUploadedFiles()
		{
			var files = _fileProcessingService.GetUploadedFiles();
			return Ok(files);
		}

		// Endpoint for downloading a specific file by its name
		[HttpGet("files/{fileName}")]
		public IActionResult DownloadFile(string fileName)
		{
			try
			{
				var fileContent = _fileProcessingService.GetFileContent(fileName);
				return File(fileContent, "application/octet-stream", fileName);
			}
			catch (FileNotFoundException)
			{
				return NotFound(new { Message = "File not found." });
			}
		}
	}

}
