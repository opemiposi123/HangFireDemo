namespace HangFireDemo
{
	public class FileProcessingService
	{
		private readonly string _uploadPath;

		public FileProcessingService()
		{
			_uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

			// Ensure the directory exists
			if (!Directory.Exists(_uploadPath))
			{
				Directory.CreateDirectory(_uploadPath);
			}
		}

		// Upload and process files
		//public void ProcessUploadedFiles(List<IFormFile> files)
		//{
		//	foreach (var file in files)
		//	{
		//		var filePath = Path.Combine(_uploadPath, file.FileName);
		//		using (var stream = new FileStream(filePath, FileMode.Create))
		//		{
		//			file.CopyTo(stream);
		//		}
		//	}
		//}

		public void ProcessFile(string filePath)
		{
			try
			{
				// Process the file (e.g. upload to cloud storage, database, etc.)
				Console.WriteLine($"Processing file: {filePath}");

				// Simulate some processing time
				Task.Delay(5000).Wait();

				Console.WriteLine($"File processed successfully: {filePath}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error processing file: {ex.Message}");
			}
		}

		// Retrieve all uploaded file names
		public List<string> GetUploadedFiles()
		{
			if (!Directory.Exists(_uploadPath))
			{
				return new List<string>(); // Return an empty list if the directory does not exist
			}

			var files = Directory.GetFiles(_uploadPath).Select(Path.GetFileName).ToList();
			return files;
		}

		// Retrieve the content of a specific file
		public byte[] GetFileContent(string fileName)
		{
			var filePath = Path.Combine(_uploadPath, fileName);

			if (!File.Exists(filePath))
			{
				throw new FileNotFoundException("File not found", fileName);
			}

			return File.ReadAllBytes(filePath);
		}
	}

}
