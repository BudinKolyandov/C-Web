using Andreys.Data;
using Andreys.Models;
using Andreys.ViewModels.Products;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Andreys.Services
{
    public class ProductsService : IProductService
    {
        private readonly AndreysDbContext db;

        public ProductsService(AndreysDbContext db)
        {
            this.db = db;
        }

        public int Add(ProductAddInputModel model)
        {
            var categoryEnum = Enum.Parse<Category>(model.Category);
            var genderEnum = Enum.Parse<Gender>(model.Gender);

            var product = new Product()
            {
                Name = model.Name,
                Description = model.Description,
                ImageUrl = model.ImageUrl,
                Price = model.Price,
                Category = categoryEnum,
                Gender = genderEnum
            };

            this.db.Products.Add(product);
            this.db.SaveChanges();
            return product.Id;
        }


        public IEnumerable<Product> GetAll()
            => this.db.Products.Select(x => new Product
            {
                Id = x.Id,
                Name = x.Name,
                ImageUrl = x.ImageUrl,
                Price = x.Price
            })
            .ToArray();

        public Product GetById(int id)
            => this.db.Products.FirstOrDefault(x => x.Id == id);

        public void DeleteById(int id)
        {
            var product = this.GetById(id);
            this.db.Products.Remove(product);
            this.db.SaveChanges();
        }
    }
}
