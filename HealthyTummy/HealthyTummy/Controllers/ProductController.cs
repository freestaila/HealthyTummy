using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthyTummy.Data;
using HealthyTummy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HealthyTummy.Controllers
{
    public class ProductController : NotificationsController
    {
        private readonly ApplicationDbContext _db;

        public ProductController(ApplicationDbContext db)
        {
            _db = db;
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
            Product newProduct = new Product();
            return PartialView("_AddEditProduct", newProduct);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                _db.Products.Add(newProduct);
                CreateNotification("Product saved!");
            }
            _db.SaveChanges();
            return PartialView("_AddEditProduct", newProduct);
        }
        #endregion

        #region EditSection
        [HttpGet]
        public IActionResult Edit(int id)
        {
            // int id = int.Parse(this.RouteData.Values["id"].ToString());
            var productToEdit = _db.Products.Where(x => x.Id == id).FirstOrDefault();
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
            Product productToEdit = _db.Products.Find(editedProduct.Id);
            productToEdit.Name = editedProduct.Name;
            productToEdit.UnitType = editedProduct.UnitType;
            productToEdit.CaloriesPerUnit = editedProduct.CaloriesPerUnit;
            _db.SaveChanges();
            CreateNotification("Product changed!");
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

            _db.Products.Remove(productToDelete);
            CreateNotification("Product deleted!");
            _db.SaveChanges();
            return PartialView("_DeleteProductPartial",productToDelete);
        }
        #endregion
    }
}