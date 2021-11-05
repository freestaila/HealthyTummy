using HealthyTummy.Models;

namespace HealthyTummy.Services
{
    public interface IProductService
    {
        public void AddProductToDatabase(Product product);
        public Product EditProductInDB(Product product);
        public Product RemoveProductFromDB(Product product);
    }
}
