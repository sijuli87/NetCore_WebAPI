using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BSGWebAPI.Models
{
    public class Inquiry_Response
    {
        public string error { get; set; }
        public int status { get; set; }
        public Inquiry_Result result { get; set; }
        public string rCode { get; set; }
        public string message { get; set; }
        public string timestamp { get; set; }
    }

    public class Inquiry_Result
    {
        public string KODECABANG { get; set; }
        public string CIF { get; set; }
        public string NIK { get; set; }
        public string INSURED { get; set; }
        public string DOB { get; set; }
        public string GENDER { get; set; }
        public string ADDRESS { get; set; }
        public string PHONE { get; set; }
        public string STARTDATE { get; set; }
        public decimal PLAFOND { get; set; }
        public int DURATION { get; set; }
        public string KDOCCUPATION { get; set; }
        public string DETAILOCCUP { get; set; }
        public string ACCTNO { get; set; }
        public decimal PREMI { get; set; }
        public string NOREF { get; set; }
    }
}
