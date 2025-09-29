namespace Company.G02.PL.Helpers
{
    public class DocumentSettings
    {
        // 1. Upload
        public static string UploadFile(IFormFile file, string folderName)
        {
            // 1. Get Folder Location
            //string folderPath = "C:\\Users\\Mai\\Desktop\\C#\\Company.G02\\Company.G02.PL\\wwwroot\\files\\images\\";

            //var folderPath = Directory.GetCurrentDirectory()+ "\\wwwroot\\files\\images\\" + folderName;

            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\files", folderName);

            // 2. File Name (Unique File Name)

            var fileName = $"{Guid.NewGuid()}{file.FileName}";

            // File Path
            var filePath = Path.Combine(folderPath, fileName);

            using var fileStream = new FileStream(filePath, FileMode.Create);
            file.CopyTo(fileStream);

            return fileName;
            
        }

        // 2. Delete

        public static void DeleteFile(string fileName, string folderName) 
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "\\wwwroot\\files", folderName, fileName);
            if(File.Exists(filePath))
            {
                File.Delete(filePath);
            }

        }
    }
} 
