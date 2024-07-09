using Slugify;

namespace ecommerce.Helpers
{
  public interface ISlugService
  {
    public string GenerateSlug(string phrase);
  }

  public class SlugService : ISlugService
  {
    private readonly SlugHelper _slugHelper;

    public SlugService(SlugHelper slugHelper)
    {
      _slugHelper = slugHelper;
    }

    public string GenerateSlug(string phrase)
    {
      return _slugHelper.GenerateSlug(phrase);
    }
  }
}