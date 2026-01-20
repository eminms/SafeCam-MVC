namespace SafeCam.Utilities.FileUpload
{
    public static class ImageUpload
    {
        public static async Task<string> SaveImage(this IFormFile file,IWebHostEnvironment _env,string path)
        {

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string filename = Guid.NewGuid() + "_" + file.FileName;
            string fullpath = Path.Combine(_env.WebRootPath, path, filename);

            using(var stream=new FileStream(fullpath,FileMode.Create ))
            {
               await file.CopyToAsync(stream);
            }

            return filename;
        }
    }
}
