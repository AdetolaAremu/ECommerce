using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IReviewRepository
  {
    // get all reviews
    IEnumerable<Review> GetAllReviews();

    // get one review
    Review GetOneReview(int reviewId);

    // Get all reviews of a product
    IEnumerable<Review> GetAllReviewsOfABook(int productId);

    // Get product of a review
    Review GetBookOfAReview(int productId);

    // bool, product review exists for logged in user
    IEnumerable<Review> GetReviewsOfALoggedInUser();

    bool CreateReview(CreateReviewDTO createReviewDTO);
    bool UpdateReview(ReviewDTO reviewDTO);
    bool DeleteReview(Review review);
    bool SaveTransaction();
  }
}