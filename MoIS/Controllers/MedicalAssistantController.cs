using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoIS.Models;
using System.Data.Entity.Validation;

namespace MoIS.Controllers
{
    using System.Data;
    using System.Web.Security;
    using MoIS.Models;
    public class MedicalAssistantController : Controller
    {

        //GET: MedicalAssistant
        public ActionResult Index()
        {
            if ((string)SessionRole() != "MO")
            {
                return RedirectToAction("Index", "Home");
            }
            DBModels dBModel = new DBModels();

            List<medicalassistant> medicalassistantList = dBModel.medicalassistants.ToList();

            medicalassistant medicalassistantVM = new medicalassistant();

            List<medicalassistant> medicalassistantVMList = medicalassistantList.Select(x => new medicalassistant
            {
                MaID = x.MaID,
                MaIcNo = x.MaIcNo,
                Role = x.Role,
                Username = x.Username,
                MaName = x.MaName
            }).ToList();

            return View(medicalassistantList);

        }

        private object SessionRole()
        {
            return Session["role"];
        }

        [HttpGet]
        public ActionResult Create()
        {
            if ((string)SessionRole() != "MO")
            {
                return RedirectToAction("Index", "Home");
            }
            ViewBag.Role = new List<SelectListItem>()
            {
                new SelectListItem(){ Value="MA", Text = "MA", Selected = true},
                new SelectListItem(){ Value="MO", Text = "MO"},
            };
            return View();
        }


        [HttpPost]
        public ActionResult Create(medicalassistant maModel)
        {
            DBModels dBModel = new DBModels();
            medicalassistant newma = new medicalassistant();

            newma.MaName = maModel.MaName;
            newma.MaIcNo = maModel.MaIcNo;
            newma.Username = maModel.Username;
            newma.Password = maModel.Password;
            newma.ConfirmPassword = maModel.ConfirmPassword;
            newma.Role = maModel.Role;
            newma.RegisteredDate = @DateTime.Now;
            newma.Status = 1;
            dBModel.medicalassistants.Add(newma);
            dBModel.SaveChanges();

            int latestempId = newma.MaID;
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";
            return RedirectToAction("Index");


        }

        [HttpGet]
        public ActionResult ResetPassword(int id, medicalassistant medicalassistantModel)
        {
            DBModels dBModel = new DBModels();

            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update medicalassistant set password = {0} where MaID = {1}",
                "123456", id);

            Session["success"] = "Succesfully Reset Password to 123456.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            DBModels dBModel = new DBModels();
            medicalassistant medicalassistantModel = dBModel.medicalassistants.Find(id);
            
            string[] Role = { "MA", "MO" };
            ViewData["Role"] = Role;
            return View(medicalassistantModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, medicalassistant medicalassistantModel)
        {
            DBModels dBModel = new DBModels();
            medicalassistantModel = dBModel.medicalassistants.Find(id);

            medicalassistantModel.MaName = Request.Form["MaName"];
            medicalassistantModel.MaIcNo = Request.Form["MaIcNo"];
            medicalassistantModel.Role = Request.Form["Role"];

            dBModel.Entry(medicalassistantModel).State = EntityState.Modified;
            dBModel.SaveChanges();

            Session["success"] = "Succesfully Update Profile.";
            return View("Edit", new { id = id });
        }
    }
}