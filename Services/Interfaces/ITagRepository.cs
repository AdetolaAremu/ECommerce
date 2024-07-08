using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ITagRepository
  {
    // get all tags
    IEnumerable<Tag> GetAllTags();
    
    // Get A tag
    Tag GetATag(CreateTagDTO createTagDTO);

    // create a tag
    bool CreateTag(CreateTagDTO createTagDTO);

    // update a tag
    bool UpdateTag(int tagId, TagDTO tagDTO);

    // delete tag
    bool DeleteTag(Tag tag);
  }
}