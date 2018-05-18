using System.Collections.Generic;
namespace MoIS.Models
{
    public class DetailsViewModel
    {
        public List<medicalassistant> medicalassistantPenerimaList { get; set; }
        public List<medicalassistant> medicalassistantPegawaiperubatanList { get; set; }
        public List<medicalassistant> medicalassistantPegawaimenyerahkanList { get; set; }
        public List<deceased> deceasedList { get; set; }
        public List<butiranmcdpmc> butiranmcdpmcList { get; set; }
        public List<butiranbedahsiasat> butiranbedahsiasatList { get; set; }
        public List<butiranwari> butiranwariList { get; set; }
        public List<butiranpengurusmayat> butiranpengurusmayatList { get; set; }
        //public List<butiranbedahsiasat> butiranbedahsiasatList { get; set; }
    }
}