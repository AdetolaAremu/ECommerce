using ecommerce.DataStore;
using ecommerce.DTO;
using ecommerce.Helpers;
using ecommerce.Models;
using ecommerce.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace ecommerce.Services
{
  public class ProductRepository : IProductRepository
  {
    private ApplicationDBContext _applicationDBContext;
    private readonly ISlugService _slugService;

    public ProductRepository(ApplicationDBContext applicationDBContext, ISlugService slugService)
    {
      _applicationDBContext = applicationDBContext;
      _slugService = slugService;
    }

    public IEnumerable<Product> GetAllProducts(string? searchTerm, string? sortBy, int pageNumber, int itemSize)
    {
      var query = _applicationDBContext.Products.AsQueryable();

      if (!string.IsNullOrEmpty(searchTerm))
      {
        query = query.Where(p => p.Title.ToLower().Contains(searchTerm.ToLower()) || p.Description.ToLower().Contains(searchTerm.ToLower()));
      }

      if (!string.IsNullOrEmpty(sortBy))
      {
        switch (sortBy.ToLower())
        {
          case "title":
            query = query.OrderBy(p => p.Title);
            break;
          case "price":
            query = query.OrderByDescending(p => p.Price);
            break;
          case "quantity":
            query = query.OrderByDescending(p => p.Quantity);
            break;
          case "date":
            query = query.OrderByDescending(p => p.CreatedAt);
            break;
          default:
            query = query.OrderByDescending(p => p.Id);
            break;
        }
      }
      // Console.WriteLine(query);

      return query.Skip((pageNumber - 1) * itemSize).Take(itemSize).ToList();
    }

    public Product GetOneProduct(int productId)
    {
      return _applicationDBContext.Products.Where(p => p.Id == productId).Include(p => p.Reviews).First();
    }

    public bool CreateProduct(CreateProductDTO createProductDTO, string coverImage, List<string> productImages) 
    {
      var createProduct = new Product() {
        Title = createProductDTO.Title,
        Description = createProductDTO.Description,
        Price = createProductDTO.Price,
        SKU = createProductDTO.SKU,
        Quantity = createProductDTO.Quantity,
        CoverImage = coverImage,
        Manufacturer = createProductDTO.Manufacturer,
        Slug = _slugService.GenerateSlug(createProductDTO.Title),
        UserId = createProductDTO.UserId
      };

      _applicationDBContext.Add(createProduct);
      var saveProduct = SaveTransaction();

      if (saveProduct == true) {
          // Console.WriteLine(createProductDTO);

        foreach (var category in createProductDTO.CategoryIds)
        {
          var newCategories = new ProductCategory(){
            CategoryId = category,
            ProductId = createProduct.Id
          };

          _applicationDBContext.Add(newCategories);
        }

        foreach(var image in productImages)
        {
          var newImages = new ProductImages(){
            Image = image,
            ProductId = createProduct.Id
          };

          _applicationDBContext.Add(newImages);
        }

        return SaveTransaction();
      } else {
        return false;
      }
    }

    public bool UpdateProduct(ProductDTO productDTO, int productId)
    {
      var product = _applicationDBContext.Products.Where(p => p.Id == productId).First();

      var updatedProduct = new Product(){
        Title = productDTO.Title.ToLower(),
        Description = productDTO.Description.ToLower(),
        Price = productDTO.Price,
        SKU = productDTO.SKU,
        Quantity = productDTO.Quantity,
        // CoverImage = productDTO.CoverImage,
        Manufacturer = productDTO.Manufacturer.ToLower(),
        Slug = !string.IsNullOrEmpty(productDTO.Title) ? _slugService.GenerateSlug(productDTO.Title) : productDTO.Slug
      };

      // delete the previous categories and create new ones
      if (productDTO.categoryIds.Any())
      {
        var categoriesToDelete = _applicationDBContext.ProductCategories.Where(pc => pc.ProductId == productId).ToList();
        _applicationDBContext.RemoveRange(categoriesToDelete);

        foreach (var category in productDTO.categoryIds)
        {
          var newCategories = new ProductCategory(){
            ProductId = updatedProduct.Id,
            CategoryId = category
          };

          _applicationDBContext.Add(newCategories);
        }
      }
     
      _applicationDBContext.Update(product);
      return SaveTransaction();
    }

    public bool CheckIfProductExists(int productId)
    {
      return _applicationDBContext.Products.Any(p => p.Id == productId);
    }

    public bool DeleteProduct(Product product)
    {
      _applicationDBContext.Remove(product);
      return SaveTransaction();
    }

    public IEnumerable<Product> GetProductsByCategory(int categoryId, int pageNumber, int pageSize)
    {
      var queryProducts = _applicationDBContext.Products.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));

      return queryProducts.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }

    public bool SaveTransaction() 
    {
      var saveChanges = _applicationDBContext.SaveChanges();

      return saveChanges >= 0 ? true : false;
    }
  }
}