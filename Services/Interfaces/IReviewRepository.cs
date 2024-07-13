using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IReviewRepository
  {
    // get all reviews
    IEnumerable<Review> GetAllReviews(string searchTerm, int pageNumber, int pageSize);

    // get one review
    Review GetOneReview(int reviewId);

    // Get all reviews of a product
    IEnumerable<Review> GetAllReviewsOfAProduct(int productId, int pageNumber, int pageSize);

    // Get product of a review
    Review GetProductOfAReview(int productId);

    // all reviews of a logged in user
    IEnumerable<Review> GetReviewsOfALoggedInUser(int userId, int pageNumber, int pageSize);

    // bool, product review exists for logged in user
    bool CheckIfUserReviewExistsForAProduct(int userId, int productId);

    // check if review exists
    bool CheckIfReviewExists(int reviewId);

    bool CreateReview(CreateReviewDTO createReviewDTO);
    // bool UpdateReview(ReviewDTO reviewDTO); // you cannot update a review I think
    bool DeleteReview(Review review);
    bool SaveTransaction();
  }
}