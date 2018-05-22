using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
    namespace MoIS.Models
{
    public class DeceasedButiranViewModel
    {
        public deceased deceased { get; set; }
        public butiranmcdpmc butiranmcdpmc { get; set; }
        public int DeceasedID { get; set; }
        [DisplayName("Name Simati")]
        public string DeceasedName { get; set; }
        public string Address { get; set; }
        public Nullable<int> Postcode { get; set; }
        [DisplayName("No. Kad Pengenalan / Pasport")]
        public string NoIC { get; set; }
        public string Race { get; set; }
        public string Religion { get; set; }
        [DisplayName("Tarikh dan Masa Masuk Wad")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfWad { get; set; }
        [DisplayName("Tarikh dan Masa Kematian")]
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> DateOfDeath { get; set; }
        [DisplayName("Tarikh dan Masa Pendaftaran")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfRegistration { get; set; }
        public int CreatedBy { get; set; }
        [DisplayName("Date of Last Modified")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfLastModified { get; set; }
        public int ModifiedBy { get; set; }
        [DisplayName("Kes")]
        public int TypeOfCase { get; set; }
        [DisplayName("Tarikh dan Masa Menerima")]
        [DataType(DataType.Date)]
        public System.DateTime DateOfReceived { get; set; }
        public string MaName { get; set; }
        public string MaIcNo { get; set; }
        public string Role { get; set; }
        [DisplayName("Wad Unit")]
        public string WadUnit { get; set; }
        [DisplayName("No. Pendaftaran Hospital")]
        public string NoOfHospitalRegister { get; set; }
        [DisplayName("Butiran ID")]
        public int ButiranID { get; set; }
        [DisplayName("No. MCD")]
        public string NoMCD { get; set; }
        [DisplayName("No. PMC")]
        public string NoPMC { get; set; }
        [DisplayName("Sebab Kematian")]
        public string SebabKematian { get; set; }
        [DataType(DataType.Date)]
        public System.DateTime CreatedDate { get; set; }
        public Nullable<int> Editedby { get; set; }
        [DataType(DataType.Date)]
        public Nullable<System.DateTime> EditedDate { get; set; }
        public Nullable<short> Status { get; set; }
        [DisplayName("Jantina")]
        public string Gender { get; set; }
        [DisplayName("Umur")]
        public int Age { get; set; }

    }
}