using CSharpAssignment2.Interface;
using CSharpAssignment2.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CSharpAssignment2.Tests.Models
{
    class InMemoryProductRepository : IProductRepository
    {
        private readonly List<tbl_Product> db = new List<tbl_Product>();

        public Exception ExceptionToThrow { get; set; }

        public IEnumerable<tbl_Product> ProductList()
        {
            return db.ToList();
        }

        public tbl_Product GetProductByID(int id)
        {
            return db.FirstOrDefault(d => d.id == id);
        }

        public void CreateProduct(tbl_Product productToCreate)
        {
            if (ExceptionToThrow != null)
                throw ExceptionToThrow;

            db.Add(productToCreate);
        }
        public void SaveChanges(tbl_Product productToUpdate)
        {
            foreach (tbl_Product product in db)
            {
                if (product.id == productToUpdate.id)
                {
                    db.Remove(product);
                    db.Add(productToUpdate);
                    break;
                }
            }
        }

        public void Add(tbl_Product productToAdd)
        {
            db.Add(productToAdd);
        }

        public int SaveChanges()
        {
            return 1;
        }

        public void DeleteProduct(int id)
        {
            db.Remove(GetProductByID(id));
        }
    }
}
