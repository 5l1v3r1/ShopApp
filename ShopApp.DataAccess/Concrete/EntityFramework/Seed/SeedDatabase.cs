﻿using Microsoft.EntityFrameworkCore;
using ShopApp.DataAccess.Concrete.EntityFramework.Context;
using ShopApp.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;

namespace ShopApp.DataAccess.Concrete.EntityFramework.Seed
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new ShopContext();
            if (context.Database.GetPendingMigrations().Count()==0)
            {
                if (context.Categories.Count()==0)
                {
                    context.Categories.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                {
                    context.Products.AddRange(Products);
                    context.AddRange(ProductCategory);
                }
                context.SaveChanges();
            }
        }

        private static Category[] Categories = {
            new Category() { Name = "Telefon"},
            new Category() { Name = "Bilgisayar"},
            new Category() { Name = "Elektronik"}
        };

        private static Product[] Products = {
            new Product() { Name = "Samsung S2", Price = 2000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S3", Price = 3000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S4", Price = 4000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S5", Price = 5000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S6", Price = 6000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S7", Price = 7000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S8", Price = 8000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S9", Price = 9000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
            new Product() { Name = "Samsung S10", Price = 10000, ImageURL = "1.jpg", Description="<p>Güzel Telefon</p>" },
        };

        private static ProductCategory[] ProductCategory =
        {
            new ProductCategory() { Product = Products[0], Category = Categories[0]},
            new ProductCategory() { Product = Products[0], Category = Categories[2]},
            new ProductCategory() { Product = Products[1], Category = Categories[0]},
            new ProductCategory() { Product = Products[1], Category = Categories[1]},
            new ProductCategory() { Product = Products[2], Category = Categories[0]},
            new ProductCategory() { Product = Products[2], Category = Categories[2]},
            new ProductCategory() { Product = Products[3], Category = Categories[1]}
        };
    }
}
