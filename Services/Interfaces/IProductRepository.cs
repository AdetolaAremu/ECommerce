using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IProductRepository
  {
    // get all products, be able to search, sort and paginate
    IEnumerable<Product> GetAllProducts(string? searchTerm, string? sortBy, int pageNumber, int itemSize);

    // get one product
    Product GetOneProduct(int productId);

    // check if product exists
    bool CheckIfProductExists(int productId);

    // create product
    bool CreateProduct(CreateProductDTO createProductDTO, string coverImage, List<string> productImages);

    // update product
    bool UpdateProduct(CreateProductDTO productDTO, int productId, string? coverImage, List<string?> productImages);

    // delete product
    bool DeleteProduct(Product product);

    // get products by category
    IEnumerable<Product> GetProductsByCategory(int categoryId, int pageNumber, int pageSize);

    // saveProduct
    bool SaveTransaction();
  }
}