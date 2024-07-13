using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Models;
using ecommerce.Services.Interfaces;

namespace ecommerce.Services
{
  public class ReviewRepository : IReviewRepository
  {
    private ApplicationDBContext _applicationDBContext;

    public ReviewRepository(ApplicationDBContext applicationDBContext)
    {
      _applicationDBContext = applicationDBContext;
    }

    public IEnumerable<Review> GetAllReviews(string searchTerm, int pageNumber, int pageSize)
    {
      var query = _applicationDBContext.Reviews.AsQueryable();

      if (!string.IsNullOrEmpty(searchTerm))
      {
        query = query.Where(r => r.User.FirstName == searchTerm || r.User.LastName == searchTerm);
      }

      return query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public Review GetOneReview(int reviewId)
    {
      return _applicationDBContext.Reviews.Where(r => r.Id == reviewId).First();
    }

    public IEnumerable<Review> GetAllReviewsOfAProduct(int productId, int pageNumber, int pageSize)
    {
      return _applicationDBContext.Reviews.Where(r => r.ProductId == productId).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public Review GetProductOfAReview(int productId)
    {
      return _applicationDBContext.Reviews.Where(p => p.ProductId == productId).First();
    }

    public IEnumerable<Review> GetReviewsOfALoggedInUser(int userId, int pageNumber, int pageSize)
    {
      return _applicationDBContext.Reviews.Where(r => r.UserId == userId).Skip((pageNumber - 1) * pageSize)
        .Take(pageSize).ToList();
    }

    public bool CheckIfReviewExists(int reviewId)
    {
      return _applicationDBContext.Reviews.Any(r => r.Id == reviewId);
    }

    public bool CheckIfUserReviewExistsForAProduct(int userId, int productId)
    {
      return _applicationDBContext.Reviews.Any(r => r.UserId == userId && r.ProductId == productId);
    }

    public bool CreateReview(CreateReviewDTO createReviewDTO)
    {
      var review = new Review(){
        ProductId = createReviewDTO.ProductId,
        Rating = createReviewDTO.Rating,
        UserId = createReviewDTO.UserId
      };

      _applicationDBContext.Add(review);

      return SaveTransaction();
    }

    public bool DeleteReview(Review review)
    {
      _applicationDBContext.Remove(review);

      return SaveTransaction(); 
    }

    public bool SaveTransaction()
    {
      var review = _applicationDBContext.SaveChanges();

      return review >= 0 ? true : false;
    }
  }
}