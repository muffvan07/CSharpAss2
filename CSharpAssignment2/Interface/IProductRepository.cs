using CSharpAssignment2.Models;
using System.Collections.Generic;

namespace CSharpAssignment2.Interface
{
    public interface IProductRepository
    {
        IEnumerable<tbl_Product> ProductList();
        tbl_Product GetProductByID(int id);
        void CreateProduct(tbl_Product product);
        int SaveChanges();
        void DeleteProduct(int id);
    }
}