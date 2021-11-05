using HealthyTummy.Data;
using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public class ProductService: IProductService
    {
        private readonly ApplicationDbContext _db;

        public ProductService(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddProductToDatabase(Product product)
        {
            _db.Products.Add(product);
            _db.SaveChanges();
        }

        public Product EditProductInDB(Product product)
        {
            Product productToEdit = _db.Products.Find(product.Id);
            productToEdit.Name = product.Name;
            productToEdit.UnitType = product.UnitType;
            productToEdit.CaloriesPerUnit = product.CaloriesPerUnit;
            _db.SaveChanges();

            return productToEdit;
        }

        public Product RemoveProductFromDB(Product product)
        {
            _db.Products.Remove(product);
            _db.SaveChanges();
            return product;
        }
    }
}
