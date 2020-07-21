using CSharpAssignment2.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace CSharpAssignment2.Controllers
{
    public class LoginRegisterController : Controller
    {
        readonly CSharpAssignment2Entities db = new CSharpAssignment2Entities();

        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Register(tbl_User user)
        {
            user.EmailVerification = false;

            var isExists = IsEmailExists(user.Email);

            if (isExists)
            {
                ModelState.AddModelError("EmailExists", "Email Already Exists");
                return View();
            }

            user.ActivationCode = Guid.NewGuid();
            user.Password = EncryptPassword.Encrypt(user.Password);

            db.tbl_User.Add(user);
            db.SaveChanges();

            SendEmailToUser(user.Email, user.ActivationCode.ToString());
            var Message = "Registration Completed. Please Check your Mail : " + user.Email;
            ViewBag.Message = Message;

            return View("RegistrationConfirm");
        }


        public bool IsEmailExists(string eMail)
        {
            var IsCheck = db.tbl_User.Where(email => email.Email == eMail).FirstOrDefault();
            return IsCheck != null;
        }

        public void SendEmailToUser(string emailId, string activationCode)
        {
            var GenarateUserVerificationLink = "/LoginRegister/UserVerification/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, GenarateUserVerificationLink);

            var fromMail = new MailAddress("youremail@gmail.com", "UserName"); // set your email  
            var fromEmailpassword = "yourpassword"; // Set your password   
            var toEmail = new MailAddress(emailId);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromMail.Address, fromEmailpassword)
            };

            var Message = new MailMessage(fromMail, toEmail)
            {
                Subject = "Confirm Your Account",
                Body = "<br/> Your registration completed Succesfully." +
                           "<br/> Please click on the below link for account verification" +
                           "<br/><br/><a href=" + link + ">" + link + "</a>",
                IsBodyHtml = true
            };
            smtp.Send(Message);
        }

        public ActionResult UserVerification(string id)
        {
            db.Configuration.ValidateOnSaveEnabled = false; // Ignor to password confirmation   
            var IsVerify = db.tbl_User.Where(u => u.ActivationCode == new Guid(id)).FirstOrDefault();

            if (IsVerify != null)
            {
                IsVerify.EmailVerification = true;
                db.SaveChanges();
                ViewBag.Message = "Email Verification completed";
            }
            else
            {
                ViewBag.Message = "Invalid Request...Email not verify";
                ViewBag.Status = false;
            }

            return View();
        }


        public ActionResult Login()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(tbl_User user)
        {
            var EncryptpassWord = EncryptPassword.Encrypt(user.Password);
            var obj = db.tbl_User.Where(a => a.Email.Equals(user.Email) && a.Password.Equals(EncryptpassWord) && a.EmailVerification == true).FirstOrDefault();
            if (obj != null)
            {
                Session["UserID"] = obj.id.ToString();
                Session["UserName"] = obj.FirstName.ToString();
                return RedirectToAction("ProductList", "Product");
            }
            return View(user);
        }


        public ActionResult Logout()
        {
            Session["userName"] = null;
            Session.Abandon();
            return RedirectToAction("Login", "LoginRegister");
        }
    }
}