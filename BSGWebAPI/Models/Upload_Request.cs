using System;

namespace BSGWebAPI.Models
{
    public class Upload_Request
    {
        public string KODECABANG { get; set; }
        public int JNSKREDIT { get; set; }
        public string CIF { get; set; }
        public string NIK { get; set; }
        public string INSURED { get; set; }
        public string DOB { get; set; }
        public string GENDER { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string KDKREDIT { get; set; }
        public string NOREF { get; set; }
        public string STARTDATE { get; set; }
        public decimal PLAFOND { get; set; }
        public int DURATION { get; set; }
        public string KDOCCUPATION { get; set; }
        public string DETAILOCCUP { get; set; }
        public string ACCTNO { get; set; }
        public decimal PREMI { get; set; }
    }
}
