﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApp.Business.Abstract;
using ShopApp.Entities.Concrete;
using ShopApp.WebUI.Models;

namespace ShopApp.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IProductService _productService;
        private ICategoryService _categoryService;
        public AdminController(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
        }
        public IActionResult ProductList()
        {
            return View(new ProductListModel()
            {
                Products = _productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            if (ModelState.IsValid == true)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Price = model.Price,
                    Description = model.Description,
                    ImageURL = model.ImageURL
                };

                if (_productService.Create(entity))
                {
                    return RedirectToAction("ProductList");
                }
                ViewBag.ErrorMessage = _productService.ErrorMessage;
                return View(model);
            }
            return View(model);
        }

        public IActionResult EditProduct(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var entity = _productService.GetByIdWithCategories((int)id);
            if (entity == null)
            {
                return NotFound();
            }
            var model = new ProductModel()
            {
                Id = entity.Id,
                Name = entity.Name,
                Price = entity.Price,
                Description = entity.Description,
                ImageURL = entity.ImageURL,
                SelectedCategories = entity.ProductCategories.Select(i => i.Category).ToList()
            };
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, int[] categoryIds, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                var entity = _productService.GetById(model.Id);

                if (entity == null)
                {
                    return NotFound();
                }

                entity.Name = model.Name;
                entity.Description = model.Description;
                entity.Price = model.Price;

                if (file != null)
                {
                    entity.ImageURL = file.Name;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.Name);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }
                _productService.Update(entity, categoryIds);
                return RedirectToAction("ProductList");
            }
            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }
        [HttpPost]
        public IActionResult DeleteProduct(int Id)
        {
            var entity = _productService.GetById(Id);
            if (entity != null)
            {
                _productService.Delete(entity);
            }
            return RedirectToAction("ProductList");
        }



        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            });
        }
        public IActionResult CreateCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCategory( CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };
            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }
        public IActionResult EditCategory(int id)
        {
            var entity = _categoryService.GetByIdWithProducts(id);

            return View(new CategoryModel() { 
                Id = entity.Id,
                Name = entity.Name,
                Products = entity.ProductCategories.Select( p => p.Product).ToList()
            });
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Name = model.Name;
            _categoryService.Update(entity);

            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int Id)
        {
            var entity = _categoryService.GetById(Id);
            if (entity != null)
            {
                _categoryService.Delete(entity);
            }
            return RedirectToAction("CategoryList");
        }
        [HttpPost]
        public IActionResult DeleteFromCategory(int Id, int categoryId)
        {
            _categoryService.DeleteFromCategory(Id, categoryId);
            return Redirect("/admin/editcategory/" + categoryId);
        }
    }
}