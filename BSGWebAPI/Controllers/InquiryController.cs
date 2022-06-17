using BSGWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSGWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class InquiryController : ControllerBase
    {
        private IConfiguration Configuration;

        public InquiryController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        // POST api/<InquiryController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Post([FromBody] Inquiry_Request inquiry_Request)
        {
            var inquiry_Response = new Inquiry_Response();
            try
            {
                string ACCTNO = inquiry_Request.ACCTNO;
                string strConn = this.Configuration.GetConnectionString("DefaultConnection");
                Inquiry_Result inquiry_Result = new Inquiry_Result();
                DateTime dt;
                using (SqlConnection openCon = new SqlConnection(strConn))
                {
                    string selectInquiry = "SELECT TOP 1 KODECABANG,CIF,NIK,INSURED,DOB,GENDER,ADDRESS,PHONE,STARTDATE,PLAFOND,DURATION,KDOCCUPATION,DETAILOCCUP,";
                    selectInquiry += "ACCTNO,PREMI,NOREF FROM INQUIRY_RESPONSE WHERE ACCTNO = @ACCTNO ORDER BY CREATED_AT DESC";

                    using (SqlCommand querySelectInquiry = new SqlCommand(selectInquiry))
                    {
                        querySelectInquiry.Connection = openCon;
                        querySelectInquiry.Parameters.Add("@ACCTNO", SqlDbType.VarChar, 30).Value = ACCTNO;
                        openCon.Open();

                        using (SqlDataReader reader = querySelectInquiry.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                if (reader.Read())
                                {
                                    inquiry_Result.KODECABANG = reader["KODECABANG"].ToString();
                                    inquiry_Result.CIF = reader["CIF"].ToString();
                                    inquiry_Result.NIK = reader["NIK"].ToString();
                                    inquiry_Result.INSURED = reader["INSURED"].ToString();
                                    inquiry_Result.DOB = reader["DOB"].ToString();
                                    inquiry_Result.GENDER = reader["GENDER"].ToString();
                                    inquiry_Result.ADDRESS = reader["ADDRESS"].ToString();
                                    inquiry_Result.PHONE = reader["PHONE"].ToString();
                                    inquiry_Result.STARTDATE = reader["STARTDATE"].ToString();
                                    inquiry_Result.PLAFOND = (decimal)reader["PLAFOND"];
                                    inquiry_Result.DURATION = (int)reader["DURATION"];
                                    inquiry_Result.KDOCCUPATION = reader["KDOCCUPATION"].ToString();
                                    inquiry_Result.DETAILOCCUP = reader["DETAILOCCUP"].ToString();
                                    inquiry_Result.ACCTNO = reader["ACCTNO"].ToString();
                                    inquiry_Result.PREMI = (decimal)reader["PREMI"];
                                    inquiry_Result.NOREF = reader["NOREF"].ToString();
                                }
                                
                                dt = DateTime.Now;
                                inquiry_Response.status = 200;
                                inquiry_Response.error = "false";
                                inquiry_Response.result = inquiry_Result;
                                inquiry_Response.rCode = "00";
                                inquiry_Response.message = "Inquiry data berhasil";
                                inquiry_Response.timestamp = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                return Ok(inquiry_Response);
                            }
                            else
                            {
                                dt = DateTime.Now;
                                inquiry_Response.status = (int)HttpStatusCode.NotFound;
                                inquiry_Response.error = "true";
                                inquiry_Response.result = null;
                                inquiry_Response.rCode = "76";
                                inquiry_Response.message = "Data not found";
                                inquiry_Response.timestamp = dt.ToString("yyyy-MM-dd HH:mm:ss");
                                return NotFound(inquiry_Response);
                            }                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                DateTime dt = DateTime.Now;
                inquiry_Response.status = (int)HttpStatusCode.InternalServerError;
                inquiry_Response.error = "true";
                inquiry_Response.result = null;
                inquiry_Response.rCode = "96";
                inquiry_Response.message = ex.Message.ToString();
                inquiry_Response.timestamp = dt.ToString("yyyy-MM-dd HH:mm:ss");
                return StatusCode(500, inquiry_Response);
            }
            //return inquiry_Response;
        }

    }
}
