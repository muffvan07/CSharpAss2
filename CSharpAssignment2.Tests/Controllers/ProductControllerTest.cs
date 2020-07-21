using CSharpAssignment2.Controllers;
using CSharpAssignment2.Interface;
using CSharpAssignment2.Models;
using CSharpAssignment2.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace CSharpAssignment2.Tests.Controllers
{
    [TestClass]
    public class ProductControllerTest
    {
        /// <summary>
        /// This method used for index view
        /// </summary>
        [TestMethod]
        public void IndexView()
        {
            var productController = GetProductController(new InMemoryProductRepository());
            ViewResult result = (ViewResult)productController.ProductList();
            Assert.AreEqual("ProductList", result.ViewName);
        }

        /// <summary>
        /// This method used to get product controller
        /// </summary>
        /// <param name="repository"></param>
        /// <returns></returns>
        private static ProductController GetProductController(IProductRepository productRepository)
        {
            ProductController productController = new ProductController(productRepository);
            productController.ControllerContext = new ControllerContext()
            {
                Controller = productController,
                RequestContext = new RequestContext(new MockHttpContext(), new RouteData())
            };
            return productController;
        }

        /// <summary>
        ///  This method used to get all product listing
        /// </summary>
        [TestMethod]
        public void GetAllProductFromRepository()
        {
            // Arrange
            tbl_Product product1 = GetProductName(52,"Samsung TV");
            tbl_Product product2 = GetProductName(53, "Apple iPhone XS");
            InMemoryProductRepository productRepository = new InMemoryProductRepository();
            productRepository.Add(product1);
            productRepository.Add(product2);
            var controller = GetProductController(productRepository);
            var result = controller.ProductList();
            var datamodel = (IEnumerable<tbl_Product>)result;
            CollectionAssert.Contains(datamodel.ToList(), product1);
            CollectionAssert.Contains(datamodel.ToList(), product2);
        }

        /// <summary>
        /// This method used to get emp name
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        tbl_Product GetProductName(int id, string Name)
        {
            return new tbl_Product
            {
                id = id,
                ProductName = Name,
            };
        }

        /// <summary>
        /// This test method used to post employee
        /// </summary>
        [TestMethod]
        public void CreateProductInRepository()
        {
            InMemoryProductRepository productRepository = new InMemoryProductRepository();
            ProductController productController = GetProductController(productRepository);
            tbl_Product product = GetProductID();
            productController.CreateProduct(product);
            IEnumerable<tbl_Product> products = productRepository.ProductList();
            Assert.IsTrue(products.Contains(product));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        tbl_Product GetProductID()
        {
            return GetProductName(52, "Samsung TV");
        }

        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void CreateRedirectOnSuccess()
        {
            ProductController controller = GetProductController(new InMemoryProductRepository());
            tbl_Product model = GetProductID();
            var result = (RedirectToRouteResult)controller.CreateProduct(model);
            Assert.AreEqual("ProductList", result.RouteValues["action"]);
        }

        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void ViewIsNotValid()
        {
            ProductController productController = GetProductController(new InMemoryProductRepository());
            productController.ModelState.AddModelError("", "mock error message");
            tbl_Product model = GetProductName(52 , "");
            var result = (ViewResult)productController.CreateProduct(model);
            Assert.AreEqual("CreateProduct", result.ViewName);
        }

        /// <summary>
        ///
        /// </summary>
        [TestMethod]
        public void RepositoryThrowsException()
        {
            // Arrange
            InMemoryProductRepository productRepository = new InMemoryProductRepository();
            Exception exception = new Exception();
            productRepository.ExceptionToThrow = exception;
            ProductController controller = GetProductController(productRepository);
            tbl_Product product = GetProductID();
            var result = (ViewResult)controller.CreateProduct(product);
            Assert.AreEqual("CreateProduct", result.ViewName);
            ModelState modelState = result.ViewData.ModelState[""];
            Assert.IsNotNull(modelState);
            Assert.IsTrue(modelState.Errors.Any());
            Assert.AreEqual(exception, modelState.Errors[0].Exception);
        }

        private class MockHttpContext : HttpContextBase
        {
            private readonly IPrincipal _user = new GenericPrincipal(new GenericIdentity("someUser"), null /* roles */);
            public override IPrincipal User
            {
                get
                {
                    return _user;
                }

                set
                {
                    base.User = value;
                }
            }
        }
    }
}
