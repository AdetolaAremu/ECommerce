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

    public bool DeleteImages(List<string> paths)
    {
      try
      {
        int count = 0;

        foreach (var item in paths)
        {
          var getPath = File.Exists(item);

          if (getPath) count++;
        }

        if (count == paths.Count()) {
          foreach (var pathsToDelete in paths)
          {
            File.Delete(pathsToDelete);
          }

          return true;
        } else {
          Console.WriteLine("One or more files not found");
          return false;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }

    public bool DeleteSingleImage(string path)
    {
      try
      {
        if (File.Exists(path)) {
          File.Delete(path);

          return true;
        } else {
          Console.WriteLine("Cannot delete empty path");

          return false;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        return false;
      }
    }
  }
}