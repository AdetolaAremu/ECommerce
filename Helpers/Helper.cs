namespace ecommerce.Helpers
{
  public class Helper
  {
    // public string HashPassword(string password)
    // {
    //   return BCrypt.Net.BCrypt.HashPassword(password);
    // }
    public string SingleImageUpload(IFormFile image, string path)
    {
      Directory.CreateDirectory(path);

      string imagePath = null;

      if (image.Length > 0)
      {
        var filePath = Path.Combine(path, image.FileName);

        using (var stream = new FileStream(filePath, FileMode.Create)) image.CopyToAsync(stream);

        imagePath = filePath;
      }

      return imagePath;
    }

    public async Task<string[]> BulkImageUpload(IFormFile[] images, string path)
    {
      Directory.CreateDirectory(path);
      
      var bulkImages = new List<string>();

      if (images.Any())
      {
        foreach (var image in images)
        {
          var imagePath = Path.Combine(path, image.FileName);

          using (var stream = new FileStream(imagePath, FileMode.Create)) await image.CopyToAsync(stream);

          bulkImages.Add(imagePath);
        }
      }

      return bulkImages.ToArray();
    }
  }
}