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
    private Helper _helper;

    public ProductRepository(ApplicationDBContext applicationDBContext, ISlugService slugService, Helper helper)
    {
      _applicationDBContext = applicationDBContext;
      _slugService = slugService;
      _helper = helper;
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

    public bool UpdateProduct(CreateProductDTO productDTO, int productId, string? coverImage, List<string?> productImages)
    {
      var product = _applicationDBContext.Products.Where(p => p.Id == productId).First();

      product.Title = productDTO.Title.ToLower();
      product.Description = productDTO.Description.ToLower();
      product.Price = productDTO.Price;
      product.SKU = productDTO.SKU;
      product.Quantity = productDTO.Quantity;
      product.CoverImage = coverImage ?? product.CoverImage;
      product.Manufacturer = productDTO.Manufacturer.ToLower();
      product.Slug = !string.IsNullOrEmpty(productDTO.Title) ? _slugService.GenerateSlug(productDTO.Title) : product.Slug;

      // what if it contains images
      if (productImages.Any())
      {
        // delete all previous images
        var deleteImages = _helper.DeleteImages(productImages);

        if (deleteImages) {
          foreach (var newImages in productImages)
          {
            var savedImages = new ProductImages(){
              Image = newImages,
              ProductId = productId
            };
            _applicationDBContext.Add(savedImages);
          }
        }
      }

      // delete the previous categories and create new ones
      if (productDTO.CategoryIds.Any())
      {
        var categoriesToDelete = _applicationDBContext.ProductCategories.Where(pc => pc.ProductId == productId).ToList();
        _applicationDBContext.RemoveRange(categoriesToDelete);
        SaveTransaction();

        foreach (var category in productDTO.CategoryIds)
        {
          var newCategories = new ProductCategory(){
            ProductId = productId,
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
      // delete cover image
      _helper.DeleteSingleImage("uploads/product-cover-image/" + product.CoverImage);

      // delete categories
      var getProductCategory = _applicationDBContext.ProductCategories.Where(pc => pc.ProductId == product.Id).ToList();

      foreach (var productCatgory in getProductCategory)
      {        
        _applicationDBContext.Remove(productCatgory);
      }

      // delete product images
      var getProductimages = _applicationDBContext.ProductImages.Where(pi => pi.ProductId == product.Id).ToList();

      foreach (var productImages in getProductimages)
      {
        _helper.DeleteSingleImage("uploads/product-images/" + productImages.Image);

        _applicationDBContext.Remove(productImages);
      }

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