using HealthyTummy.Data;
using HealthyTummy.Models;
using HealthyTummy.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace HealthyTummy.Controllers
{
    public class ProductController : NotificationsController
    {
        private readonly ApplicationDbContext _db;
        private readonly IProductService _productService;

        public ProductController(ApplicationDbContext db, IProductService productSerivce)
        {
            _db = db;
            _productService = productSerivce;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> productsList = _db.Products;
            var isAjax = Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            if (isAjax)
            {
                return PartialView("_ProductsTable", productsList);
            }
            return View(productsList);
        }

        #region CreateAction
        [HttpGet]
        public IActionResult Create()
        {
            Product newProduct = new();
            newProduct.ActionType = true;
            return PartialView("_AddEditProduct", newProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                _productService.AddProductToDatabase(newProduct);
                CreateNotification("Product saved!");
            }
            newProduct.ActionType = true;
            return PartialView("_AddEditProduct", newProduct);
        }
        #endregion

        #region EditSection
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var productToEdit = _db.Products.Where(x => x.Id == id).FirstOrDefault();
            productToEdit.ActionType = false;
            if (productToEdit == null)
            {
                return NotFound();
            }

            return PartialView("_AddEditProduct", productToEdit);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Product editedProduct)
        {
            if (ModelState.IsValid)
            {
                _productService.EditProductInDB(editedProduct);
                CreateNotification("Product changed!");
            }
            return PartialView("_AddEditProduct", editedProduct);
        }
        #endregion

        #region DeleteSection
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var productToDelete = _db.Products.Where(x => x.Id == id).FirstOrDefault();
            if (productToDelete == null)
            {
                return NotFound();
            }
            return PartialView("_DeleteProductPartial", productToDelete);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(Product productToDelete)
        {

            _productService.RemoveProductFromDB(productToDelete);
            CreateNotification("Product deleted!"); 
            return PartialView("_DeleteProductPartial",productToDelete);
        }
        #endregion
    }
}