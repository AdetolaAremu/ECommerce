using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface ITagRepository
  {
    // get all tags
    IEnumerable<Tag> GetAllTags(int pageNumber, int pageSize);
    
    // Get A tag
    Tag GetATag(int tagId);

    // tag exists
    bool CheckIfTagExists(int tagId);

    // create a tag
    bool CreateTag(CreateTagDTO createTagDTO);

    // update a tag
    bool UpdateTag(int tagId, TagDTO tagDTO);

    // delete tag
    bool DeleteTag(Tag tag);

    // save transaction
    bool SaveTransaction();
  }
}