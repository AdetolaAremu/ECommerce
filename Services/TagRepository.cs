using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class TagRepository : ITagRepository
  {
    ApplicationDBContext _applicationDBContext;

    public TagRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Tag> GetAllTags(int pageNumber, int pageSize)
    {
      return _applicationDBContext.Tags.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public Tag GetATag(int tagId)
    {
      return _applicationDBContext.Tags.Where(t => t.Id == tagId).First();
    }

    public bool CheckIfTagExists(int tagId)
    {
      return _applicationDBContext.Tags.Any(t => t.Id == tagId);
    }

    public bool CreateTag(CreateTagDTO createTagDTO)
    {
      var tag = new Tag(){
        Name = createTagDTO.Name
      };

      _applicationDBContext.Add(tag);

      return SaveTransaction();
    }

    public bool UpdateTag(int tagId, TagDTO tagDTO)
    {
      var tag = GetATag(tagId);

      tag.Name = tagDTO.Name;

      return SaveTransaction();
    }

    public bool DeleteTag(Tag tag)
    {
      _applicationDBContext.Remove(tag);

      return SaveTransaction();
    }

    public bool SaveTransaction()
    {
      var tag = _applicationDBContext.SaveChanges();

      return tag >= 0 ? true : false;
    }
  }
}