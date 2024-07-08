using ecommerce.DTO;
using ecommerce.Models;

namespace ecommerce.Services.Interfaces
{
  public interface IProductRepository
  {
    // get all products, be able to search, sort and paginate
    IEnumerable<Product> GetAllProducts(string searchTerm, string sortBy, int itemSize);

    // get one product
    Product GetOneProduct(int productId);

    // create product
    bool CreateProduct(CreateProductDTO createProductDTO);

    // update product
    bool UpdateProduct(CreateProductDTO createProductDTO, int productId);

    // delete product
    bool DeleteProduct(int productId);

    // get products by category
    IEnumerable<Product> GetProductsByCategory(int categoryId);

    // saveProduct
    bool SaveTransaction();
  }
}