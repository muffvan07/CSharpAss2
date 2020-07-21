using CSharpAssignment2.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CSharpAssignment2.Controllers
{
    public class UserController : Controller
    {
        readonly CSharpAssignment2Entities db = new CSharpAssignment2Entities();

        // GET: User
        public ActionResult ChangeProfile(int? id)
        {
            tbl_User Profile = db.tbl_User.Find(id);
            if (Profile == null)
            {
                return HttpNotFound();
            }
            return View(Profile);
        }

        [HttpPost]
        public ActionResult ChangeProfile(tbl_User user)
        {
            db.Entry(user).State = EntityState.Modified;
            var result = db.SaveChanges();
            if (result > 0)
            {
                ViewBag.SuccessEditProfile = "Profile has been changed successfully";
                return RedirectToAction("ProductList", "Product");
            }
            return View();
        }
    }
}