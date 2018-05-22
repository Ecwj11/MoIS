using System.Collections.Generic;
namespace MoIS.Models
{
    public class StatisticsViewModel
    {
        public List<medicalassistant> medicalassistantList { get; set; }
        public List<medicalassistant> medicalassistantPenerimaList { get; set; }
        public List<medicalassistant> medicalassistantPegawaiperubatanList { get; set; }
        public List<medicalassistant> medicalassistantPegawaimenyerahkanList { get; set; }
        public List<deceased> deceasedList { get; set; }
        public List<butiranmcdpmc> butiranmcdpmcList { get; set; }
        public List<butiranbedahsiasat> butiranbedahsiasatList { get; set; }
        public List<butiranwari> butiranwariList { get; set; }
        public List<butiranpengurusmayat> butiranpengurusmayatList { get; set; }
        public int deceasedTotal { get; set; }
        public int maleTotal { get; set; }
        public int femaleTotal { get; set; }
        public decimal malePercent { get; set; }
        public decimal femalePercent { get; set; }
        public int weeklyDeceased { get; set; }
        public int monthlyDeceased { get; set; }
        public List<deceased> weeklyDeceasedList { get; set; }
        public List<deceased> monthlyDeceasedList { get; set; }
        public int maleSun { get; set; }
        public int maleMon { get; set; }
        public int maleTues { get; set; }
        public int maleWed { get; set; }
        public int maleThurs { get; set; }
        public int maleFri { get; set; }
        public int maleSat { get; set; }
        public int femaleSun { get; set; }
        public int femaleMon { get; set; }
        public int femaleTues { get; set; }
        public int femaleWed { get; set; }
        public int femaleThurs { get; set; }
        public int femaleFri { get; set; }
        public int femaleSat { get; set; }
        public int maleJan { get; set; }
        public int maleFeb { get; set; }
        public int maleMar { get; set; }
        public int maleApr { get; set; }
        public int maleMay { get; set; }
        public int maleJun { get; set; }
        public int maleJul { get; set; }
        public int maleAug { get; set; }
        public int maleSep { get; set; }
        public int maleOct { get; set; }
        public int maleNov { get; set; }
        public int maleDec { get; set; }
        public int femaleJan { get; set; }
        public int femaleFeb { get; set; }
        public int femaleMar { get; set; }
        public int femaleApr { get; set; }
        public int femaleMay { get; set; }
        public int femaleJun { get; set; }
        public int femaleJul { get; set; }
        public int femaleAug { get; set; }
        public int femaleSep { get; set; }
        public int femaleOct { get; set; }
        public int femaleNov { get; set; }
        public int femaleDec { get; set; }
        public int normalCount { get; set; }
        public int policeCount { get; set; }
        public int bbsCount { get; set; }
    }
}