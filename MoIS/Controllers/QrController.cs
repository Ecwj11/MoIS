using MoIS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;

namespace MoIS.Controllers
{
    public class QrController : CommonController
    {
        // GET: Qr
        public ActionResult Index()
        {
            return View();
        }

        // GET: Qr
        public ActionResult Qr(string eId)
        {
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();
            deceased deceased = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranwari butiranwari = dBModel.butiranwaris.SingleOrDefault(x => x.DeceasedID == id);
            butiranbedahsiasat butiranbedahsiasat = dBModel.butiranbedahsiasats.SingleOrDefault(x => x.DeceasedID == id);

            string c = "UNIT PERUBATAN FORENSIK SARAWAK KAD PENGENALAN MAYAT KES POLIS \n";
            c += "WAD: " + deceased.WadUnit + " \n";
            c += "TEL: " + " \n";
            c += "NAMA: " + deceased.DeceasedName + " \n";
            c += "K/P: " + deceased.NoIC + " \n";
            c += "R/N: " +  " \n";
            c += "UMUR:" + deceased.Age + " \n";
            c += "JANTINA:" + deceased.Gender + " \n";
            c += "KETURUNAN:" + deceased.Race + " \n";
            c += "AGAMMA:" + deceased.Religion + " \n";
            c += "ALAMAT:" + deceased.Address + " \n";
            c += "TARIKH DAN WAKTU DIDAFTAR MASUK:" + deceased.DateOfRegistration + " \n";
            c += "DOKTOR:" + " \n";
            c += "TARIKH DAN WAKTU KEMATIAN:" + deceased.DateOfDeath + " \n";
            c += "JENIS KES POLIS:" + " \n";
            c += "KES B.I.D:" + " \n";
            c += "NAMA WARIS:" + butiranwari.WarisName + " \n";
            c += "ALAMAT:" + butiranwari.Alamat + " \n";
            c += "TEL:" + butiranwari.PhoneNo + " \n";
            c += "POLIS YANG DIHUBUINGI:" + " \n";
            c += "NO:" + " \n";
            c += "BALAI:" + " \n";
            c += "DIHUBUNGI OLEH:" + " \n";
            c += "TARIKH" + " \n";
            c += "WARIS SIMATI TELAH DIHUBUNGI:" + " \n";
            var result = new HomeController().PrintViewToPdf(eId, "DcdPmsDetails");
            var model = new QrModel();
            //ViewData["content"] = content;
            model.Content = c;
            model.DeceasedName = deceased.DeceasedName;
            model.NoIC = deceased.NoIC;
            string noBedahSiasat = "";
            if (butiranbedahsiasat != null && butiranbedahsiasat.NoBedahSiasat != "" && butiranbedahsiasat.NoBedahSiasat != null)
            {
                noBedahSiasat = " (" + butiranbedahsiasat.NoBedahSiasat + ")";
            }
            model.NoRegistration = "FORHUS " + deceased.DeceasedID + "/" + deceased.DateOfRegistration.Year + noBedahSiasat;

            return View(model);
        }

        // GET: Qr/QrDetails/5
        public ActionResult QrDetails(string eId, string type = "normal")
        {
            int id = Convert.ToInt32(Decrypt(eId));
            DBModels dBModel = new DBModels();
            deceased deceasedData = dBModel.deceaseds.SingleOrDefault(x => x.DeceasedID == id);
            butiranbedahsiasat butiranbedahsiasatData = dBModel.butiranbedahsiasats.SingleOrDefault(x => x.DeceasedID == id);

            string noBedahSiasat = "";
            if (butiranbedahsiasatData != null)
            {
                if (butiranbedahsiasatData.NoBedahSiasat != "" && butiranbedahsiasatData.NoBedahSiasat != null)
                {
                    noBedahSiasat = " (" + butiranbedahsiasatData.NoBedahSiasat + ")";
                }
            }

            var deceased = dBModel.deceaseds.Where(x => x.DeceasedID == id).ToList();
            var butiranwari = dBModel.butiranwaris.Where(x => x.DeceasedID == id).ToList();

            var model = new DetailsViewModel()
            {
                deceasedList = deceased,
                butiranwariList = butiranwari,
                NoRegistration = "FORHUS " + deceasedData.DeceasedID + "/" + deceasedData.DateOfRegistration.Year + noBedahSiasat
            };
            string DomainUrl = "http://localhost:65100";
            ViewData["DomainUrl"] = DomainUrl;
            ViewData["eId"] = eId;
            if (type == "print")
            {
                ViewData["print"] = "print";
            }
            return View(model);
        }

        public ActionResult PrintViewToPdf(string eId, string page)
        {
            var report = new ActionAsPdf(page, new { eId = eId, type = "print" });
            return report;
        }
    }
}