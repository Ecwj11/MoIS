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
    using MoIS.Helper;

    public class MedicalAssistantController : CommonController
    {
        private object SessionMaID()
        {
            
            return Session["maID"];
        }

        //GET: MedicalAssistant
        public ActionResult Index()
        {
            return RedirectToAction("Statistics");
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
            foreach (var item in medicalassistantList)
            {
                ViewData["ID" + item.MaID] = Encrypt(item.MaID.ToString());
            }
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

            var maDetailsUsername = dBModel.medicalassistants.Where(x => x.Username == maModel.Username).FirstOrDefault();
            var maDetailsMaIcNo = dBModel.medicalassistants.Where(x => x.MaIcNo == maModel.MaIcNo).FirstOrDefault();
            if (maDetailsUsername != null)
            {
                Session["error"] = "Username has been taken.";
                return RedirectToAction("Create");
            }
            if (maDetailsMaIcNo != null)
            {
                Session["error"] = "User with IC No (" + maModel.MaIcNo + ") has been taken.";
                return RedirectToAction("Create");
            }
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
            int lastInsertId = newma.MaID;

            //update password to hashed password
            dBModel.Database.ExecuteSqlCommand("Update medicalassistant set Password = {0} where MaID = {1}", Helper.Hashing.HashPassword(maModel.Password), lastInsertId);
            int latestempId = newma.MaID;
            ModelState.Clear();
            Session["success"] = "Registration Successful.";// ViewBag.SuccessMessage = "Registration Successful.";
            return RedirectToAction("Statistics");
        }

        [HttpGet]
        public ActionResult Details()
        {
            if ((string)SessionRole() != "MO")
            {
                return RedirectToAction("Index", "Home");
            }
            int maID = (int)SessionMaID();
            DBModels dBModel = new DBModels();
            medicalassistant medicalassistant = dBModel.medicalassistants.Where(x => x.MaID == maID).FirstOrDefault();
            return View(medicalassistant);
        }

        [HttpGet]
        public ActionResult ResetPassword(string eId, medicalassistant medicalassistantModel)
        {
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();

            string myPassword = "123456";
            string myHash = Helper.Hashing.HashPassword(myPassword);

            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update medicalassistant set password = {0} where MaID = {1}",
                myHash, id);

            Session["success"] = "Succesfully Reset Password to 123456.";
            return RedirectToAction("Statistics");
        }

        [HttpGet]
        public ActionResult Edit(string eId)
        {
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();
            medicalassistant medicalassistantModel = dBModel.medicalassistants.Find(id);
            
            string[] Role = { "MA", "MO" };
            ViewData["Role"] = Role;
            ViewData["maID"] = eId;
            return View(medicalassistantModel);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult EditPost()
        {
            string eId = Request.Form["maID"];
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();

            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update medicalassistant set MaName = {0}, MaIcNo = {1}, Role = {2} where MaID = {3}",
                Request.Form["MaName"], Request.Form["MaIcNo"], Request.Form["Role"], id);
            Session["success"] = "Succesfully Update.";
            return RedirectToAction("Edit", new { eId = eId });
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult ChangePasswordPost()
        {
            string currentPassword = Request.Form["currentPassword"];
            string newPassword = Request.Form["newPassword"];
            string confirmNewPassword = Request.Form["confirmNewPassword"];

            int maID = Convert.ToInt32(SessionMaID());

            DBModels dBModel = new DBModels();
            medicalassistant medicalassistant = dBModel.medicalassistants.Where(x => x.MaID == maID).FirstOrDefault();

            if(currentPassword == null || currentPassword == "")
            {
                Session["error"] = "Please enter current password.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            bool hash = Helper.Hashing.ValidatePassword(currentPassword, medicalassistant.Password);
            if (hash == false)
            {
                Session["error"] = "Current password does not match.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            if (newPassword == null || newPassword == "")
            {
                Session["error"] = "Please enter new password.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            if (confirmNewPassword == null || confirmNewPassword == "")
            {
                Session["error"] = "Please enter confirm new password.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            if (newPassword != confirmNewPassword)
            {
                Session["error"] = "Confirm new password and new password does not match.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            if (newPassword.Length < 6)
            {
                Session["error"] = "New password must be at least 6 characters.";
                return RedirectToAction("ChangePassword", "MedicalAssistant");
            }
            string myHash = Helper.Hashing.HashPassword(newPassword);
            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update medicalassistant set password = {0} where MaID = {1}",
                myHash, maID);

            Session["success"] = "Successfully change password.";
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult ChangeStatus(string eId)
        {
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();
            medicalassistant medicalassistant = dBModel.medicalassistants.Where(x => x.MaID == id).FirstOrDefault();
            string username = medicalassistant.Username;

            int updateStatus = 1;
            if (medicalassistant.Status == 1)
            {
                updateStatus = 2;
                Session["warning"] = "Succesfully Deactivate User - " + username + ".";
            } else
            {
                updateStatus = 1;
                Session["success"] = "Succesfully Activate User - " + username + ".";
            }
            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update medicalassistant set status = {0} where MaID = {1}",
                updateStatus, id);

            
            return RedirectToAction("Statistics");
        }

        public ActionResult Statistics()
        {
            if (SessionMaID() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int maID = Convert.ToInt32(SessionMaID());
            DBModels dBModel = new DBModels();

            int deceasedTotal = dBModel.Database.SqlQuery<int>("Select count(DeceasedID) as total from deceased where Status = 1").FirstOrDefault<int>();
            int maleTotal = dBModel.Database.SqlQuery<int>("Select count(DeceasedID) as total from deceased where Gender = 'Male' and Status = 1").FirstOrDefault<int>();
            int femaleTotal = dBModel.Database.SqlQuery<int>("Select count(DeceasedID) as total from deceased where Gender = 'Female' and Status = 1").FirstOrDefault<int>();
            decimal genderTotal = maleTotal + femaleTotal;
            decimal malePercent = maleTotal / genderTotal * 100;
            decimal femalePercent = femaleTotal / genderTotal * 100;

            int monthlyDeceased = dBModel.Database.SqlQuery<int>("select count(DeceasedID) / count(distinct(month(DateOfRegistration))) as avgMonthly from deceased where status = 1").FirstOrDefault<int>();
            int weeklyDeceased = dBModel.Database.SqlQuery<int>("select count(DeceasedID) / (datediff(DateOfRegistration, (select DateOfRegistration from deceased where status = 1 order by DeceasedID Limit 1)) + 1) as avgWeekly from deceased where status = 1").FirstOrDefault<int>();

            IEnumerable<StatisticsViewModel> weeklyDeceasedAll = dBModel.Database.SqlQuery<StatisticsViewModel>("select Gender, count(DeceasedID) as number, month(DateOfRegistration) as month from deceased where Status = 1 and Gender is not null group by Gender, month(DateOfRegistration)").ToList();
            IEnumerable<StatisticsViewModel> monthlyDeceasedAll = dBModel.Database.SqlQuery<StatisticsViewModel>("select Gender, count(DeceasedID) as number, dayname(DateOfRegistration) as day from deceased where Status = 1 and Gender is not null group by Gender, dayname(DateOfRegistration)").ToList();

            int maleSun = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Sunday'").FirstOrDefault<int>();
            int maleMon = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Monday'").FirstOrDefault<int>();
            int maleTues = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Tuesday'").FirstOrDefault<int>();
            int maleWed = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Wednesday'").FirstOrDefault<int>();
            int maleThurs = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Thursday'").FirstOrDefault<int>();
            int maleFri = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Friday'").FirstOrDefault<int>();
            int maleSat = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Male' and dayname(d.DateOfRegistration) = 'Saturday'").FirstOrDefault<int>();

            int femaleSun = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Sunday'").FirstOrDefault<int>();
            int femaleMon = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Monday'").FirstOrDefault<int>();
            int femaleTues = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Tuesday'").FirstOrDefault<int>();
            int femaleWed = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Wednesday'").FirstOrDefault<int>();
            int femaleThurs = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Thursday'").FirstOrDefault<int>();
            int femaleFri = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Friday'").FirstOrDefault<int>();
            int femaleSat = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from weekday w left join deceased d on w.day = dayname(d.DateOfRegistration) where Gender = 'Female' and dayname(d.DateOfRegistration) = 'Saturday'").FirstOrDefault<int>();

            int maleJan = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'January'").FirstOrDefault<int>();
            int maleFeb = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'February'").FirstOrDefault<int>();
            int maleMar = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'March'").FirstOrDefault<int>();
            int maleApr = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'April'").FirstOrDefault<int>();
            int maleMay = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'May'").FirstOrDefault<int>();
            int maleJun = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'June'").FirstOrDefault<int>();
            int maleJul = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'July'").FirstOrDefault<int>();
            int maleAug = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'August'").FirstOrDefault<int>();
            int maleSep = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'September'").FirstOrDefault<int>();
            int maleOct = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'October'").FirstOrDefault<int>();
            int maleNov = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'November'").FirstOrDefault<int>();
            int maleDec = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Male' and monthname(d.DateOfRegistration) = 'December'").FirstOrDefault<int>();

            int femaleJan = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'January'").FirstOrDefault<int>();
            int femaleFeb = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'February'").FirstOrDefault<int>();
            int femaleMar = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'March'").FirstOrDefault<int>();
            int femaleApr = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'April'").FirstOrDefault<int>();
            int femaleMay = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'May'").FirstOrDefault<int>();
            int femaleJun = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'June'").FirstOrDefault<int>();
            int femaleJul = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'July'").FirstOrDefault<int>();
            int femaleAug = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'August'").FirstOrDefault<int>();
            int femaleSep = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'September'").FirstOrDefault<int>();
            int femaleOct = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'October'").FirstOrDefault<int>();
            int femaleNov = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'November'").FirstOrDefault<int>();
            int femaleDec = dBModel.Database.SqlQuery<int>("select coalesce(count(DeceasedID), 0) as count from month m left join deceased d on m.month = monthname(d.DateOfRegistration) where Gender = 'Female' and monthname(d.DateOfRegistration) = 'December'").FirstOrDefault<int>();

            int normalCount = dBModel.Database.SqlQuery<int>("Select count(DeceasedID) as total from deceased where Status = 1 and TypeOfCase = 2").FirstOrDefault<int>();
            int policeCount = dBModel.Database.SqlQuery<int>("Select count(DeceasedID) as total from deceased where Status = 1 and TypeOfCase = 1").FirstOrDefault<int>();
            int bbsCount = dBModel.Database.SqlQuery<int>("Select count(BsID) as total from butiranbedahsiasat where Status = 1").FirstOrDefault<int>();

            List<medicalassistant> medicalassistantList = dBModel.medicalassistants.ToList();
            foreach (var item in medicalassistantList)
            {
                ViewData["ID" + item.MaID] = Encrypt(item.MaID.ToString());
            }
            ViewData["EditLink"] = "http://localhost:65100/MedicalAssistant/Edit";
            ViewData["DetailLink"] = "http://localhost:65100/MedicalAssistant/Details";
            var model = new StatisticsViewModel()
            {
                medicalassistantList = medicalassistantList,
                deceasedTotal = deceasedTotal,
                maleTotal = maleTotal,
                femaleTotal = femaleTotal,
                malePercent = malePercent,
                femalePercent = femalePercent,
                weeklyDeceased = weeklyDeceased,
                monthlyDeceased = monthlyDeceased,
                maleSun = maleSun,
                maleMon = maleMon,
                maleTues = maleTues,
                maleWed = maleWed,
                maleThurs = maleThurs,
                maleFri = maleFri,
                maleSat = maleSat,
                femaleSun = femaleSun,
                femaleMon = femaleMon,
                femaleTues = femaleTues,
                femaleWed = femaleWed,
                femaleThurs = femaleThurs,
                femaleFri = femaleFri,
                femaleSat = femaleSat,
                maleJan = maleJan,
                maleFeb = maleFeb,
                maleMar = maleMar,
                maleApr = maleApr,
                maleMay = maleMay,
                maleJun = maleJun,
                maleJul = maleJul,
                maleAug = maleAug,
                maleSep = maleSep,
                maleOct = maleOct,
                maleNov = maleNov,
                maleDec = maleDec,
                femaleJan = femaleJan,
                femaleFeb = femaleFeb,
                femaleMar = femaleMar,
                femaleApr = femaleApr,
                femaleMay = femaleMay,
                femaleJun = femaleJun,
                femaleJul = femaleJul,
                femaleAug = femaleAug,
                femaleSep = femaleSep,
                femaleOct = femaleOct,
                femaleNov = femaleNov,
                femaleDec = femaleDec,
                normalCount = normalCount,
                policeCount = policeCount,
                bbsCount = bbsCount

            };
            return View(model);
        }
    }
}