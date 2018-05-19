using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoIS.Models;

namespace MoIS.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {            
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(MoIS.Models.medicalassistant maModel)
        {
            using (DBModels dBModel = new DBModels())
            {
                var maDetails = dBModel.medicalassistants.Where(x => x.Username == maModel.Username && x.Password == maModel.Password).FirstOrDefault();
                if (maDetails == null)
                {
                    maModel.LoginErrorMessage = "Wrong username or password.";
                    return View("Index", maModel);
                }
                else
                {

                    //  var first = dBModel.medicalassistants.Where(s => s.MaID == maDetails.MaID).First();
                    //  first.LastLogin = @DateTime.Now;
                    // dBModel.SaveChanges();

                    Session["maID"] = maDetails.MaID;
                    Session["username"] = maDetails.Username;
                    Session["role"] = maDetails.Role;
                    if(maDetails.Role == "MO")
                    {
                        return RedirectToAction("Index", "MedicalAssistant");
                    } else
                    {
                        return RedirectToAction("Index", "Home");
                    }
                }


            }
        }

        public ActionResult LogOut()
        {
            int maId = (int)Session["maID"];
            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }

    }
}


