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
        public medicalassistant medicalassistant { get; set; }
        public permohonansimpanmayat permohonansimpanmayat { get; set; }
        public deceased deceased { get; set; }
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
        public int MaID { get; set; }
        public string MaName { get; set; }
        public string Username { get; set; }
        public string MaIcNo { get; set; }
        public string Role { get; set; }
        public string Duration { get; set; }
        public string Catatan { get; set; }
        public Nullable<System.DateTime> PSMRequestDate { get; set; }
        public string DeceasedName { get; set; }
        public string Address { get; set; }
        public Nullable<int> Postcode { get; set; }
        public string Nationality { get; set; }
        public string NoIC { get; set; }
        public string Gender { get; set; }
        public Nullable<int> Age { get; set; }
        public string Race { get; set; }
        public string Religion { get; set; }
        public Nullable<System.DateTime> DateOfWad { get; set; }
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        public System.DateTime DateOfRegistration { get; set; }
        public System.DateTime DateOfLastModified { get; set; }
        public int ModifiedBy { get; set; }
        public int TypeOfCase { get; set; }
        public System.DateTime DateOfReceived { get; set; }
        public string WadUnit { get; set; }
        public string NoOfHospitalRegister { get; set; }
        public byte[] qrcode { get; set; }
    }
}