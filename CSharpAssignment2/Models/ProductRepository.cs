using CSharpAssignment2.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace CSharpAssignment2.Models
{
    public class ProductRepository : IProductRepository
    {
        /// <summary>
        /// CSharpAssignment2 entity object
        /// </summary>
        private readonly CSharpAssignment2Entities db = new CSharpAssignment2Entities();


        /// <summary>
        /// This method is used to get all Product listing
        /// </summary>
        /// <returns></returns>
        public IEnumerable<tbl_Product> ProductList()
        {
            int Id = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
            var result = db.tbl_Product.Where(s => s.UserId == Id);
            return result.ToList();
        }


        /// <summary>
        /// This method used to filter Product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public tbl_Product GetProductByID(int id)
        {
            return db.tbl_Product.FirstOrDefault(d => d.id == id);
        }


        /// <summary>
        /// This method is used to create new Product
        /// </summary>
        /// <param name="Create"></param>
        public void CreateProduct(tbl_Product product)
        {
            if (product.ProductName != null && product.UploadImage != null)
            {
                product.UserId = Convert.ToInt32(HttpContext.Current.Session["UserID"]);
                string fileName = Path.GetFileNameWithoutExtension(product.UploadImage.FileName);
                string extension = Path.GetExtension(product.UploadImage.FileName);
                fileName += extension;
                product.Image = fileName;
                fileName = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/"), fileName);
                product.UploadImage.SaveAs(fileName);
                db.tbl_Product.Add(product);
                db.SaveChanges();
            }
        }


        /// <summary>
        ///  Save chages functions
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return db.SaveChanges();
        }

        public void DeleteProduct(int id)
        {
            var product = GetProductByID(id);
            string filePath = HttpContext.Current.Server.MapPath("~/Images/" + product.Image);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
            db.tbl_Product.Remove(product);
            db.SaveChanges();
        }
    }
}