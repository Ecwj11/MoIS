using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace MoIS.Models
{
    public class WarisanPengecamanViewModel
    {
        public butiranwari butiranwari { get; set; }
        public pengecamansemula pengecamansemula { get; set; }
        public int WarisID { get; set; }
        [DisplayName("Nama Waris")]
        public string WarisName { get; set; }
        [DisplayName("No. IC Waris")]
        public string WarisNoIC { get; set; }
        public string Alamat { get; set; }
        [DisplayName("No. Telefon")]
        public string PhoneNo { get; set; }
        public string Perhubungan { get; set; }
        [DisplayName("Tarikh dan Masa Tuntutan")]
        public Nullable<System.DateTime> DateTimeRequest { get; set; }
        [DisplayName("Tarikh and Masa Menyerahkan")]
        public Nullable<System.DateTime> DateTimeReturn { get; set; }
        public Nullable<int> ReturnedBy { get; set; }
        public Nullable<int> DeceasedID { get; set; }
        public byte[] Signature { get; set; }
        public Nullable<int> Editedby { get; set; }
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<short> Status { get; set; }
        public int CaseID { get; set; }
        [DisplayName("Pendaftaran Mayat")]
        public string LabelMayat { get; set; }
        [DisplayName("Pengecaman Muka Simati")]
        public string PengecamanMukaSimati { get; set; }
        public string Pakaian { get; set; }
        [DisplayName("Lain-lain Cara")]
        public string Lain_lainCara { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> EditedBy { get; set; }
    }
}