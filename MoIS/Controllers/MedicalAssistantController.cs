using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoIS.Models;

namespace MoIS.Controllers
{
    using System.Web.Security;
    using MoIS.Models;
    public class MedicalAssistantController : Controller
    {

        //GET: MedicalAssistant
        public ActionResult Index()
        {
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

        [HttpGet]
        public ActionResult Create()
        {

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
            dBModel.medicalassistants.Add(newma);
            dBModel.SaveChanges();
            
            int latestempId = newma.MaID;
            ModelState.Clear();
            ViewBag.SuccessMessage = "Registration Successful.";
            return RedirectToAction("Index");

           
        }
    }
}