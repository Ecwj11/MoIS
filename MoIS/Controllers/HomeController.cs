using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MoIS.Models;
using System.Data.Entity.Validation;
using Rotativa;

namespace MoIS.Controllers
{
    public class HomeController : Controller
    {
        private object SessionMaID()
        {
            if(Session["maID"] == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return Session["maID"];
        }
        // GET: Home
        public ActionResult Index()
        {
            DBModels dBModel = new DBModels();

            List<deceased> deceasedList = dBModel.deceaseds.ToList();

            deceased deceasedVM = new deceased();

            List<deceased> deceasedVMList = deceasedList.Select(x => new deceased
            {
                DeceasedID = x.DeceasedID,
                NoIC = x.NoIC,
                TypeOfCase = x.TypeOfCase,
                DateOfRegistration = x.DateOfRegistration,
                DeceasedName = x.DeceasedName,
            }).OrderByDescending(m => m.DeceasedID).ToList();

            return View(deceasedVMList);

        }

        // GET: Deceased/Create
        public ActionResult Create()
        {
            ViewBag.TypeOfCase = new List<SelectListItem>()
            {

                new SelectListItem(){ Value="1", Text = "Normal", Selected = true},
                new SelectListItem(){ Value="0", Text = "Police"},

            };

            ViewBag.Race = new List<SelectListItem>()
            {

                new SelectListItem(){ Value="Melayu", Text = "Melayu", Selected = true},
                new SelectListItem(){ Value="Cina", Text = "Cina"},
                new SelectListItem(){ Value="India", Text = "India"},
                new SelectListItem(){ Value="Baba dan Nyonya", Text = "Baba dan Nyonya"},
                new SelectListItem(){ Value="Iban", Text = "Iban"},
            };

            ViewBag.Religion = new List<SelectListItem>()
            {

                new SelectListItem(){ Value="Islam", Text = "Islam", Selected = true},
                new SelectListItem(){ Value="Buddhism", Text = "Buddhism"},
                new SelectListItem(){ Value="Christianity", Text = "Christianity"},
                new SelectListItem(){ Value="Hinduism", Text = "Hinduism"},

            };

            return View();
        }

        // POST: Deceased/Create
        [HttpPost]
        public ActionResult Create(DeceasedButiranViewModel deceasedModel)
        {
            DBModels dBModel = new DBModels();
            deceased newdeceased = new deceased();
            butiranmcdpmc newrecord = new butiranmcdpmc();
            butiranwari newwari = new butiranwari();
            butiranpengurusmayat newpengurusmayat = new butiranpengurusmayat();
            pengecamansemula newpengecamansemula = new pengecamansemula();

            int maId = (int)SessionMaID();

            newdeceased.DeceasedName = deceasedModel.DeceasedName;
            newdeceased.NoIC = deceasedModel.NoIC;
            newdeceased.Gender = deceasedModel.Gender;// Request.Form["Gender"];
            newdeceased.Age = deceasedModel.Age;// int.Parse(Request.Form["Age"]);
            newdeceased.Race = deceasedModel.Race;
            newdeceased.Religion = deceasedModel.Religion;
            newdeceased.Address = deceasedModel.Address;
            newdeceased.Postcode = deceasedModel.Postcode;
            newdeceased.DateOfWad = DateTime.Parse(Request.Form["DateOfWad"]);
            newdeceased.DateOfDeath = DateTime.Parse(Request.Form["DateOfDeath"]);
            newdeceased.DateOfRegistration = @DateTime.Now;
            newdeceased.CreatedBy = maId;
            newdeceased.DateOfLastModified = @DateTime.Now;
            newdeceased.ModifiedBy = maId;
            newdeceased.TypeOfCase = int.Parse(Request.Form["TypeOfCase"]);
            newdeceased.WadUnit = deceasedModel.WadUnit;
            newdeceased.DateOfReceived = deceasedModel.DateOfReceived;
            newdeceased.NoOfHospitalRegister = deceasedModel.NoOfHospitalRegister;
            newdeceased.Status = 1;
            postcode postcodeData = dBModel.postcodes.Where(x => x.Postcode1 == deceasedModel.Postcode).FirstOrDefault();
            
            if(postcodeData != null)
            {
                newdeceased.Nationality = "Malaysian";
            } else
            {
                newdeceased.Nationality = "No";
                newdeceased.Postcode = null;
            }
            dBModel.deceaseds.Add(newdeceased);

            newrecord.NoMCD = deceasedModel.NoMCD;
            newrecord.NoPMC = deceasedModel.NoPMC;
            newrecord.SebabKematian = deceasedModel.SebabKematian;
            newrecord.DeceasedID = deceasedModel.DeceasedID;
            newrecord.CreatedBy = maId;
            newrecord.CreatedDate = @DateTime.Now;
            newrecord.EditedBy = maId;
            newrecord.EditedDate = @DateTime.Now;
            newrecord.Status = 1;
            dBModel.butiranmcdpmcs.Add(newrecord);


            newwari.DeceasedID = deceasedModel.DeceasedID;
            newwari.Editedby = maId;
            newwari.EditedDate = @DateTime.Now;
            newwari.Status = 1;
            dBModel.butiranwaris.Add(newwari);

            newpengurusmayat.DeceasedID = deceasedModel.DeceasedID;
            newpengurusmayat.EditedBy = maId;
            newpengurusmayat.EditedDate = @DateTime.Now;
            newpengurusmayat.Status = 1;
            dBModel.butiranpengurusmayats.Add(newpengurusmayat);

            newpengecamansemula.DeceasedID = deceasedModel.DeceasedID;
            newpengecamansemula.EditedBy = maId;
            newpengecamansemula.EditedDate = @DateTime.Now;
            newpengecamansemula.Status = 1;
            dBModel.pengecamansemulas.Add(newpengecamansemula);

            dBModel.SaveChanges();
            Session["success"] = "Created Successfully";
            return RedirectToAction("Index");
        }
        // GET: Deceased/Details/5
        public ActionResult Details(int id)
        {
            DBModels dBModel = new DBModels();
            deceased deceased = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);

            deceased deceasedVM = new deceased();

            deceasedVM.DeceasedID = deceased.DeceasedID;
            deceasedVM.DeceasedName = deceased.DeceasedName;
            deceasedVM.NoIC = deceased.NoIC;
            deceasedVM.DateOfRegistration = deceased.DateOfRegistration;
            deceasedVM.DateOfWad = deceased.DateOfWad;
            deceasedVM.DateOfDeath = deceased.DateOfDeath;
            deceasedVM.Address = deceased.Address;
            deceasedVM.Postcode = deceased.Postcode;
            // deceasedVM.CityName = deceased.city.CityName;
            // deceasedVM.StateName = deceased.state.StateName;

            return View(deceasedVM);
        }

        // GET: Deceased/McdPmcDetails/5
        public ActionResult McdPmcDetails(int id)
        {
            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranmcdpmc butiranmcdpmcData = dBModel.butiranmcdpmcs.SingleOrDefault(x => x.DeceasedID == id);
            butiranwari butiranwariData = dBModel.butiranwaris.SingleOrDefault(x => x.DeceasedID == id);
           
            var medicalassistantPenerima = dBModel.medicalassistants.Where(x => x.MaID == deceasedData.CreatedBy).ToList();
            var medicalassistantPegawaiperubatan = dBModel.medicalassistants.Where(x => x.MaID == butiranmcdpmcData.CreatedBy).ToList();
            var medicalassistantPegawaimenyerahkan = dBModel.medicalassistants.Where(x => x.MaID == butiranwariData.ReturnedBy).ToList();

            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranmcdpmc = dBModel.butiranmcdpmcs.Where(x => x.DeceasedID == id).ToList();
            var butiranbedahsiasat = dBModel.butiranbedahsiasats.Where(x => x.DeceasedID == id).ToList();
            var butiranwari = dBModel.butiranwaris.Where(x => x.DeceasedID == id).ToList();
            var butiranpengurusmayat = dBModel.butiranpengurusmayats.Where(x => x.DeceasedID == id).ToList();

            if (butiranbedahsiasat == null)
            {
                butiranbedahsiasat = null;
            }
            var model = new DetailsViewModel()
            {
                medicalassistantPenerimaList = medicalassistantPenerima,
                medicalassistantPegawaiperubatanList = medicalassistantPegawaiperubatan,
                medicalassistantPegawaimenyerahkanList = medicalassistantPegawaimenyerahkan,
                deceasedList = deceased,
                butiranmcdpmcList = butiranmcdpmc,
                butiranbedahsiasatList = butiranbedahsiasat,
                butiranwariList = butiranwari,
                butiranpengurusmayatList = butiranpengurusmayat
            };
            return View(model);
        }

        // GET: Deceased/NextProcess/1
        public ActionResult NextProcess(int id)
        {
            DBModels dBModel = new DBModels();
            butiranwari waris = dBModel.butiranwaris.Where(x => x.DeceasedID == id).FirstOrDefault();
            if(SessionMaID() == null)
            {
                return RedirectToAction("Index", "Login");
            }
            int maID = (int)SessionMaID();
            
            Session["Perhubungan"] = waris.Perhubungan;
            string[] perhubunganSelect = { "Son", "Daughter", "Father", "Mother", "Brother", "Sister", "Cousin", "GrandSon", "GrandDaughter", "Uncle", "Aunt" };
            ViewData["perhubunganSelect"] = perhubunganSelect;
            ViewData["Perhubungan"] = waris.Perhubungan;

            Session["error"] = null;
            butiranwari butiranwariModel = new butiranwari();
            pengecamansemula newrecord = new pengecamansemula();
            medicalassistant medicalassistantModel = new medicalassistant();
            permohonansimpanmayat permohonansimpanmayatModel = new permohonansimpanmayat();
            deceased deceasedModel = new deceased();

            using (DBModels dBModel2 = new DBModels())
            {
                butiranwariModel = dBModel2.butiranwaris.Where(x => x.DeceasedID == id).FirstOrDefault();
                newrecord = dBModel2.pengecamansemulas.Where(x => x.DeceasedID == id).FirstOrDefault();
                medicalassistantModel = dBModel2.medicalassistants.Where(x => x.MaID == maID).FirstOrDefault();
                permohonansimpanmayatModel = dBModel2.permohonansimpanmayats.Where(x => x.DeceasedID == id).FirstOrDefault();
                deceasedModel = dBModel2.deceaseds.Where(x => x.DeceasedID == id).FirstOrDefault();
            }
            if(butiranwariModel.WarisName == null)
            {
                ViewData["fieldsetDisabled"] = "";
            } else
            {
                ViewData["fieldsetDisabled"] = "disabled";
            }
            ViewData["checked"] = "checked";
            if (newrecord.Lain_lainCara != null) { 
                if (newrecord.Lain_lainCara.IndexOf("Fizikal") != -1)
                {
                    ViewData["tandaFizikal"] = "1";
                }
                if (newrecord.Lain_lainCara.IndexOf("Tatoo") != -1)
                {
                    ViewData["tandaTatoo"] = "1";
                }
                if (newrecord.Lain_lainCara.IndexOf("Jari") != -1)
                {
                    ViewData["capJari"] = "1";
                }
                if (newrecord.Lain_lainCara.IndexOf("Odontologi") != -1)
                {
                    ViewData["odontologi"] = "1";
                }
                if (newrecord.Lain_lainCara.IndexOf("DNA") != -1)
                {
                    ViewData["dna"] = "1";
                }
                string lain_lain_string = newrecord.Lain_lainCara.Replace("Tanda-tanda Fizikal", "");
                lain_lain_string = lain_lain_string.Replace("Tanda Tatoo", "");
                lain_lain_string = lain_lain_string.Replace("Cap Jari", "");
                lain_lain_string = lain_lain_string.Replace("Odontologi", "");
                lain_lain_string = lain_lain_string.Replace("DNA", "");
                lain_lain_string = lain_lain_string.Replace(",", "");
                if (lain_lain_string != "")
                {
                    ViewData["lain_lain"] = "1";
                    ViewData["lain_lain_string"] = lain_lain_string;
                }
            }
            
            if(newrecord.HartaBenda != null && newrecord.HartaBenda != "")
            {
                ViewData["hartaBendaYes"] = "checked";
                ViewData["hartaBendaNo"] = "";
                string[] hartaBenda = newrecord.HartaBenda.Split(',');
                ViewData["hartaBenda"] = hartaBenda;
            } else
            {
                ViewData["hartaBendaYes"] = "";
                ViewData["hartaBendaNo"] = "checked";
            }

            if(permohonansimpanmayatModel.Duration != null && permohonansimpanmayatModel.Duration != "")
            {
                ViewData["pMYes"] = "checked";
                ViewData["pMNo"] = "";
            }
            else
            {
                ViewData["pMYes"] = "";
                ViewData["pMNo"] = "checked";
            }

            if(deceasedModel.Religion == "Islam")
            {
                ViewData["isIslam"] = "checked";
                ViewData["notIslam"] = "";
            } else
            {
                ViewData["isIslam"] = "";
                ViewData["notIslam"] = "checked";
            }
            var dbv = new WarisanPengecamanViewModel { butiranwari = butiranwariModel, pengecamansemula = newrecord, medicalassistant = medicalassistantModel, permohonansimpanmayat = permohonansimpanmayatModel };
            return View(dbv);
        }

        // GET: Deceased/BPM/1
        public ActionResult BPM(int id)
        {
            DBModels dBModel = new DBModels();
            butiranpengurusmayat bpmdetail = dBModel.butiranpengurusmayats.Where(x => x.DeceasedID == id).FirstOrDefault();
            butiranpengurusmayat bpm = new butiranpengurusmayat();

            bpm.Syarikat_Individu = bpmdetail.Syarikat_Individu;
            bpm.Pemandu = bpmdetail.Pemandu;
            bpm.PhoneNO = bpmdetail.PhoneNO;
            bpm.NoIC = bpmdetail.NoIC;
            bpm.DateTime = bpmdetail.DateTime;
            bpm.CreatedBy = bpmdetail.CreatedBy;

            return View(bpmdetail);

        }

        // GET: Deceased/BBS/1
        public ActionResult BBS(int id)
        {

        DBModels dBModel = new DBModels();
        butiranbedahsiasat bbsdeatil = dBModel.butiranbedahsiasats.Where(x => x.DeceasedID == id).FirstOrDefault();
        butiranbedahsiasat bbs = new butiranbedahsiasat();
            if (bbsdeatil == null)
            {
                return View();
            }
            else
            {
                bbsdeatil.NoBedahSiasat = bbsdeatil.NoBedahSiasat;
                bbsdeatil.DateTimeBS = bbsdeatil.DateTimeBS;
                bbsdeatil.NoLaporanPolis = bbsdeatil.NoLaporanPolis;
                bbsdeatil.DateTimePOL = bbsdeatil.DateTimePOL;
            }

            return View(bbsdeatil);

        }
        // POST: Deceased/NextProcess/1
        [HttpPost]
        public ActionResult NextProcess(int id, butiranwari butiranwariModel)
        {
            int maID = (int)SessionMaID();
            DBModels dBModel = new DBModels();
            butiranwariModel = dBModel.butiranwaris.Where(x => x.DeceasedID == id).FirstOrDefault();
            permohonansimpanmayat permohonansimpanmayatModel = dBModel.permohonansimpanmayats.Where(x => x.DeceasedID == id).FirstOrDefault();

            var editData = Request.Form["editData"];
            if(editData == null)
            {
                Session["warning"] = "No changes.";
                return RedirectToAction("NextProcess", "Home", new { id = id });
            }
            butiranwariModel.WarisName = Request.Form["butiranwari.WarisName"];
            butiranwariModel.WarisNoIC = Request.Form["butiranwari.WarisNoIC"];
            butiranwariModel.Alamat = Request.Form["butiranwari.Alamat"];
            butiranwariModel.PhoneNo = Request.Form["butiranwari.PhoneNo"];
            butiranwariModel.Perhubungan = Request.Form["Perhubungan"];
            butiranwariModel.DateTimeRequest = DateTime.Parse(Request.Form["butiranwari.DateTimeRequest"]);
            butiranwariModel.DateTimeReturn = DateTime.Parse(Request.Form["butiranwari.DateTimeReturn"]);
            butiranwariModel.ReturnedBy = maID;
            butiranwariModel.Editedby = maID;
            butiranwariModel.EditedDate = @DateTime.Now;
            dBModel.Entry(butiranwariModel).State = EntityState.Modified;
            dBModel.SaveChanges();

            string hartaBenda = "";
            for (int i = 1; i<=12; i++)
            {
                if(Request.Form["hartaBenda_" + i] != null && Request.Form["hartaBenda_" + i] != "")
                {
                    hartaBenda += Request.Form["hartaBenda_" + i] + ",";
                }
            }
            hartaBenda = hartaBenda.TrimEnd(',');
            string Labelmayat = Request.Form["labelMayat"];
            string PengecamanMukaSimati = Request.Form["pengecamanMukaSimati"];
            string Pakaian = Request.Form["pakaian"];
            string tandaFizikal = Request.Form["tandaFizikal"];
            string tandaTatoo = Request.Form["tandaTatoo"];
            string capJari = Request.Form["capJari"];
            string odontologi = Request.Form["odontologi"];
            string dna = Request.Form["dna"];
            string pengecamansemula_Lain_lain = Request.Form["lain-lain"];
            string lain_lain = "";
            if(tandaFizikal != null)
            {
                lain_lain += "," + tandaFizikal;
            } 
            if (tandaTatoo != null)
            {
                lain_lain += "," + tandaTatoo;
            }
            if (capJari != null)
            {
                lain_lain += "," + capJari;
            }            
            if (odontologi != null)
            {
                lain_lain += "," + odontologi;
            }
            if (dna != null)
            {
                lain_lain += "," + dna;
            }
            if (pengecamansemula_Lain_lain != null)
            {
                lain_lain += "," + pengecamansemula_Lain_lain;
            }
            lain_lain = lain_lain.TrimStart(',');
            // Update command
            int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand(
                "Update pengecamansemula set LabelMayat = {0}, PengecamanMukaSimati = {1}, Pakaian = {2}, `Lain-lainCara` = {3}, HartaBenda = {4} where DeceasedID = {5}",
                Labelmayat, PengecamanMukaSimati, Pakaian, lain_lain, hartaBenda, id);

            if(Request.Form["simpanMayatTick"] == "1")
            {
                if (permohonansimpanmayatModel == null)
                {
                    permohonansimpanmayat newpsm = new permohonansimpanmayat();

                    newpsm.DeceasedID = id;
                    newpsm.WarisID = butiranwariModel.WarisID;
                    newpsm.CreatedBy = maID;
                    newpsm.CreatedDate = @DateTime.Now;
                    newpsm.EditedBy = maID;
                    newpsm.EditedDate = @DateTime.Now;
                    newpsm.Duration = Request.Form["duration"];
                    newpsm.Catatan = Request.Form["catatan"];
                    newpsm.PSMRequestDate = @DateTime.Parse(Request.Form["toDate"]);
                    newpsm.Status = 1;
                    dBModel.permohonansimpanmayats.Add(newpsm);

                    dBModel.SaveChanges();
                } else
                {
                    // Update command
                    dBModel.Database.ExecuteSqlCommand(
                        "Update permohonansimpanmayat set Duration = {0}, Catatan = {1}, PSMRequestDate = {2}, `Status` = {3}, EditedBy = {4}, EditedDate = {5} where DeceasedID = {6}",
                        Request.Form["duration"], Request.Form["catatan"], @DateTime.Parse(Request.Form["toDate"]), 1, maID, @DateTime.Now, id);
                }
            }
            
            Session["success"] = "Update Success.";
            return RedirectToAction("NextProcess", "Home", new { id = id });
        }

        // POST: Deceased/BPM/1
        [HttpPost]
        public ActionResult BPM(int id, butiranpengurusmayat butiranpengurusmayatModel)
        {
            DBModels dBModel = new DBModels();
            butiranpengurusmayatModel = dBModel.butiranpengurusmayats.Where(x => x.DeceasedID == id).FirstOrDefault();

            butiranpengurusmayatModel.Syarikat_Individu = Request.Form["Syarikat_Individu"];
            butiranpengurusmayatModel.Pemandu = Request.Form["Pemandu"];
            butiranpengurusmayatModel.NoIC = Request.Form["NoIC"];
            butiranpengurusmayatModel.PhoneNO = Request.Form["PhoneNO"];
            butiranpengurusmayatModel.DateTime = Request.Form["DateTime"];
            butiranpengurusmayatModel.CreatedBy = 1;
            butiranpengurusmayatModel.CreatedDate = @DateTime.Now;
            butiranpengurusmayatModel.EditedBy = 1;
            butiranpengurusmayatModel.EditedDate = @DateTime.Now;
            dBModel.Entry(butiranpengurusmayatModel).State = EntityState.Modified;
            dBModel.SaveChanges();

            Session["success"] = "Update Success.";
            return RedirectToAction("BPM", "Home" , new { id = id });
        }

        // POST: Deceased/BBS/1
        [HttpPost]
        public ActionResult BBS(int id, butiranbedahsiasat butiranbedahsiasatModel)
        {
            DBModels dBModel = new DBModels();
            butiranbedahsiasatModel = dBModel.butiranbedahsiasats.Where(x => x.DeceasedID == id).FirstOrDefault();
            if (butiranbedahsiasatModel == null)
            {
                butiranbedahsiasat newbutiranbedahsiasat = new butiranbedahsiasat();
                newbutiranbedahsiasat.DeceasedID = id;
                newbutiranbedahsiasat.NoBedahSiasat = Request.Form["NoBedahSiasat"];
                newbutiranbedahsiasat.DateTimeBS = DateTime.Parse(Request.Form["DateTimeBS"]);
                newbutiranbedahsiasat.NoLaporanPolis = Request.Form["NoLaporanPolis"];
                newbutiranbedahsiasat.DateTimePOL = DateTime.Parse(Request.Form["DateTimePOL"]);
                newbutiranbedahsiasat.CreatedBy = 1;
                newbutiranbedahsiasat.CreatedDate = @DateTime.Now;
                newbutiranbedahsiasat.EditedBy = 1;
                newbutiranbedahsiasat.EditedDate = @DateTime.Now;
                dBModel.butiranbedahsiasats.Add(newbutiranbedahsiasat);
                dBModel.SaveChanges();
            }
            else
            {
                butiranbedahsiasatModel.NoBedahSiasat = Request.Form["NoBedahSiasat"];
                butiranbedahsiasatModel.DateTimeBS = DateTime.Parse(Request.Form["DateTimeBS"]);
                butiranbedahsiasatModel.NoLaporanPolis = Request.Form["NoLaporanPolis"];
                butiranbedahsiasatModel.CreatedBy = 1;
                butiranbedahsiasatModel.CreatedDate = @DateTime.Now;
                butiranbedahsiasatModel.EditedBy = 1;
                butiranbedahsiasatModel.EditedDate = @DateTime.Now;
                dBModel.Entry(butiranbedahsiasatModel).State = EntityState.Modified;
                dBModel.SaveChanges();
            }
            Session["success"] = "Update Success.";
            return RedirectToAction("BBS", "Home", new { id = id });
        }

        // GET: Deceased/Edit/5
        public ActionResult Edit(int id)
        {
            Session["error"] = null;
            deceased deceasedModel = new deceased();
            butiranmcdpmc newrecord = new butiranmcdpmc();
            using (DBModels dBModel = new DBModels())
            {
                deceasedModel = dBModel.deceaseds.Find(id);// dBModel.deceaseds.Where(x => x.DeceasedID == id).FirstOrDefault();
                newrecord = dBModel.butiranmcdpmcs.Where(x => x.DeceasedID == id).FirstOrDefault();
            }
            ViewData["checked"] = "checked";
            ViewData["gender"] = deceasedModel.Gender;
            var dbv = new DeceasedButiranViewModel { deceased = deceasedModel, butiranmcdpmc = newrecord };
          return View(dbv);
            
        }

        // POST: Deceased/Edit/5
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Edit(int id, deceased deceasedModel)
        {
            int maId = (int)SessionMaID();
           
            using (DBModels dBModel = new DBModels())
            {
                if (Request.Form["deceased.DateOfDeath"] == null || Request.Form["deceased.DateOfDeath"] == "")
                {
                    Session["error"] = "Date of Death cannot be empty.";
                    return Redirect(Request.UrlReferrer.ToString());
                }
                    deceasedModel = new deceased();
                    deceasedModel = dBModel.deceaseds.Find(id);
                    
                    deceasedModel.DeceasedName = Request.Form["deceased.DeceasedName"];
                    deceasedModel.Address = Request.Form["deceased.Address"];
                    deceasedModel.Postcode = int.Parse(Request.Form["deceased.Postcode"]);
                    deceasedModel.NoIC = Request.Form["deceased.NoIC"];
                    deceasedModel.Gender = Request.Form["Gender"];
                    deceasedModel.Age = int.Parse(Request.Form["deceased.Age"]);
                    deceasedModel.Race = Request.Form["deceased.Race"];
                    deceasedModel.Religion = Request.Form["deceased.Religion"];
                    deceasedModel.DateOfWad = DateTime.Parse(Request.Form["deceased.DateOfWad"]);
                    if (Request.Form["deceased.DateOfDeath"] != null && Request.Form["deceased.DateOfDeath"] != "")
                    {
                        deceasedModel.DateOfDeath = DateTime.Parse(Request.Form["deceased.DateOfDeath"]);
                    }
                    deceasedModel.DateOfRegistration = DateTime.Parse(Request.Form["deceased.DateOfRegistration"]);
                    deceasedModel.TypeOfCase = int.Parse(Request.Form["TypeOfCase"]);
                    deceasedModel.DateOfReceived = DateTime.Parse(Request.Form["deceased.DateOfReceived"]);
                    deceasedModel.WadUnit = Request.Form["deceased.WadUnit"];
                    deceasedModel.NoOfHospitalRegister = Request.Form["deceased.NoOfHospitalRegister"];
                    deceasedModel.ModifiedBy = maId;

                    string NoMCD = Request.Form["butiranmcdpmc.NoMCD"];
                    string NoPMC = Request.Form["butiranmcdpmc.NoPMC"];
                    string SebabKematian = Request.Form["butiranmcdpmc.SebabKematian"];

                    dBModel.Entry(deceasedModel).State = EntityState.Modified;
                    dBModel.SaveChanges();

                    // Update command
                    int noOfRowUpdated = dBModel.Database.ExecuteSqlCommand("Update butiranmcdpmc set NoMCD = {0}, NoPMC = {1}, SebabKematian= {2} where DeceasedID = {3}", NoMCD, NoPMC, SebabKematian, id);

            }
            Session["success"] = "Update Success.";
            return RedirectToAction("Edit", "Home", new { id = id });
        }

        // GET: Deceased/BringToDeathReport/5
        public ActionResult BringToDeathReport(int id)
        {
            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);

            var medicalassistantPenerima = dBModel.medicalassistants.Where(x => x.MaID == deceasedData.CreatedBy).ToList();
            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();

            var model = new DetailsViewModel()
            {
                medicalassistantPenerimaList = medicalassistantPenerima,
                deceasedList = deceased,
            };
            ViewData["tick"] = "\u221A";
            string[] tableTanda = {  "Tidak bernafas", "Jasad tidak bergerak", "Anak mata kembang dan tidak bergerak", "Kaku / mayat keras", "Lebam mayat", "Decomposed / Reput", "Tulang", "Tanpa kepala / Decapitasi", "Terapung dalam air", "Tergantung",
            "Terbakar / Rentong", "Bunuh / Dijerut / Dibungkus", "Perut Terburai", "Otak Keluar", "Kepala Remuk", "Perdarahan Banyak", "Lain-lain (nyatakan)" };
            ViewData["tableTanda"] = tableTanda;
            return View(model);
        }

        // GET: Deceased/BorangPSML/5
        public ActionResult BorangPSML(int id)
        {
            ViewData["tick"] = "\u221A";
            int maId = (int)System.Web.HttpContext.Current.Session["maID"];

            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranbedahsiasat = dBModel.butiranbedahsiasats.Where(x => x.DeceasedID == id).ToList();

            var model = new DetailsViewModel()
            {
                deceasedList = deceased,
                butiranbedahsiasatList = butiranbedahsiasat,
            };
            ViewData["deceasedId"] = deceasedData.DeceasedID;
            return View(model);
        }

        // GET: Deceased/AutopsyProtocol/5
        public ActionResult AutopsyProtocol(int id)
        {
            ViewData["tick"] = "\u221A";
           
            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranwari butiranwariData = dBModel.butiranwaris.SingleOrDefault(x => x.DeceasedID == id);
            butiranbedahsiasat butiranbedahsiasatData = dBModel.butiranbedahsiasats.SingleOrDefault(x => x.DeceasedID == id);
            if (butiranbedahsiasatData != null)
            {
                medicalassistant medicalassistantBS = dBModel.medicalassistants.SingleOrDefault(x => x.MaID == butiranbedahsiasatData.CreatedBy);
                ViewData["dateBS"] = butiranbedahsiasatData.DateTimeBS;
                ViewData["medicalassistantBS"] = medicalassistantBS.MaName;
            } else
            {
                ViewData["dateBS"] = null;
                ViewData["medicalassistantBS"] = null;
            }

            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranwari = dBModel.butiranwaris.Where(x => x.DeceasedID == id).ToList();

            var model = new DetailsViewModel()
            {
                deceasedList = deceased,
                butiranwariList = butiranwari,
            };

            string[] headNeck = {  "Scalp", "Skull", "Meninges", "Brain", "Cervical Spine", "Face", "Throat", "Thyroid gland" };
            ViewData["headNeck"] = headNeck;

            string[] chest = { "Ribs", "Pleural cavities", "Trachea & Bronchi", "Lungs", "Heart", "Aorta", "Oesophagus" };
            ViewData["chest"] = chest;

            string[] abdomen = { "Peritoneal Cavity", "Stomach", "Small & large bowel", "Liver", "Gall bladder", "Pancreas", "Spleen", "Adrenals", "Kidneys", "Bladder", "Prostate", "Uterus & Ovaries", "Skeleton" };
            ViewData["abdomen"] = abdomen;

            string[] specimensTaken = { "Blood", "Urine", "Histopathology", "Stomach contains", "Finger Nail", "Hair", "Clothing" };
            ViewData["specimensTaken"] = specimensTaken;

            string[] weightOfOrgan = { "BRAIN", "HEART", "RIGHT LUNG", "LEFT LUNG", "LIVER", "SPLEEN", "RIGHT KIDNEY", "LEFT KIDNEY" };
            ViewData["weightOfOrgan"] = weightOfOrgan;
            ViewData["deceasedId"] = deceasedData.DeceasedID;
            return View(model);
        }

        // GET: Deceased/BorangPBPFT/5
        public ActionResult BorangPBPFT(int id)
        {
            ViewData["tick"] = "\u221A";

            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranwari butiranwariData = dBModel.butiranwaris.SingleOrDefault(x => x.DeceasedID == id);
            butiranbedahsiasat butiranbedahsiasatData = dBModel.butiranbedahsiasats.SingleOrDefault(x => x.DeceasedID == id);
            if (butiranbedahsiasatData != null)
            {
                medicalassistant medicalassistantBS = dBModel.medicalassistants.SingleOrDefault(x => x.MaID == butiranbedahsiasatData.CreatedBy);
                ViewData["dateBS"] = butiranbedahsiasatData.DateTimeBS;
                ViewData["medicalassistantBS"] = medicalassistantBS.MaName;
                ViewData["noLaporanPolis"] = butiranbedahsiasatData.NoLaporanPolis;
            }
            else
            {
                ViewData["dateBS"] = null;
                ViewData["medicalassistantBS"] = null;
                ViewData["noLaporanPolis"] = null;
            }

            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranwari = dBModel.butiranwaris.Where(x => x.DeceasedID == id).ToList();

            var model = new DetailsViewModel()
            {
                deceasedList = deceased,
                butiranwariList = butiranwari,
            };
            ViewData["deceasedId"] = deceasedData.DeceasedID;
            return View(model);
        }

        public ActionResult PrintViewToPdf(int id, string page)
        {
            var report = new ActionAsPdf(page, new { id = id });
            return report;
        }

        public ActionResult PrintPartialViewToPdf(int id)
        {
            int maId = (int)SessionMaID();

            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranmcdpmc butiranmcdpmcData = dBModel.butiranmcdpmcs.SingleOrDefault(x => x.DeceasedID == id);
            butiranwari butiranwariData = dBModel.butiranwaris.SingleOrDefault(x => x.DeceasedID == id);

            var medicalassistantPenerima = dBModel.medicalassistants.Where(x => x.MaID == deceasedData.CreatedBy).ToList();
            var medicalassistantPegawaiperubatan = dBModel.medicalassistants.Where(x => x.MaID == butiranmcdpmcData.CreatedBy).ToList();
            var medicalassistantPegawaimenyerahkan = dBModel.medicalassistants.Where(x => x.MaID == butiranwariData.ReturnedBy).ToList();

            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranmcdpmc = dBModel.butiranmcdpmcs.Where(x => x.DeceasedID == id).ToList();
            var butiranbedahsiasat = dBModel.butiranbedahsiasats.Where(x => x.DeceasedID == id).ToList();
            var butiranwari = dBModel.butiranwaris.Where(x => x.DeceasedID == id).ToList();
            var butiranpengurusmayat = dBModel.butiranpengurusmayats.Where(x => x.DeceasedID == id).ToList();

            if (butiranbedahsiasat == null)
            {
                butiranbedahsiasat = null;
            }
            var model = new DetailsViewModel()
            {
                medicalassistantPenerimaList = medicalassistantPenerima,
                medicalassistantPegawaiperubatanList = medicalassistantPegawaiperubatan,
                medicalassistantPegawaimenyerahkanList = medicalassistantPegawaimenyerahkan,
                deceasedList = deceased,
                butiranmcdpmcList = butiranmcdpmc,
                butiranbedahsiasatList = butiranbedahsiasat,
                butiranwariList = butiranwari,
                butiranpengurusmayatList = butiranpengurusmayat
            };
            var report = new PartialViewAsPdf("~/Views/Home/McdPmcDetails.cshtml", model);
            return report;
            return View(model);
        }
    }
}