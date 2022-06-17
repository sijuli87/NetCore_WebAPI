using BSGWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BSGWebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private IConfiguration Configuration;

        public UploadController(IConfiguration _configuration)
        {
            Configuration = _configuration;
        }

        // POST api/<UploadController>
        [HttpPost]
        [Authorize(AuthenticationSchemes = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme)]
        public IActionResult Post([FromBody] Upload_Request upload_Request)
        {
            Upload_Response upload_Response = new Upload_Response();
            try
            {
                try
                {
                    string validateJSON = JsonConvert.SerializeObject(upload_Request);
                    Upload_Request validateObjJSON = JsonConvert.DeserializeObject<Upload_Request>(validateJSON);
                }
                catch (JsonException je)
                {
                    DateTime dti = DateTime.Now;
                    upload_Response.status = 400;
                    upload_Response.error = "false";
                    upload_Response.result = string.Empty;
                    upload_Response.rCode = "30";
                    upload_Response.message = je.Message;
                    upload_Response.timestamp = dti.ToString("yyyy-MM-dd HH:mm:ss");
                    return BadRequest();
                }

                string strConn = this.Configuration.GetConnectionString("DefaultConnection");
                using (SqlConnection openCon = new SqlConnection(strConn))
                {
                    string saveInquiry = "INSERT INTO INQUIRY_RESPONSE (KODECABANG,JNSKREDIT,CIF,NIK,INSURED,DOB,GENDER,ADDRESS,PHONE,";
                    saveInquiry += "KDKREDIT,NOREF,STARTDATE,PLAFOND,DURATION,KDOCCUPATION,DETAILOCCUP,ACCTNO,PREMI,CREATED_AT) ";
                    saveInquiry += "VALUES (@KODECABANG,@JNSKREDIT,@CIF,@NIK,@INSURED,@DOB,@GENDER,@ADDRESS,@PHONE,";
                    saveInquiry += "@KDKREDIT,@NOREF,@STARTDATE,@PLAFOND,@DURATION,@KDOCCUPATION,@DETAILOCCUP,@ACCTNO,@PREMI,@CREATED_AT)";

                    using (SqlCommand querySaveInquiry = new SqlCommand(saveInquiry))
                    {
                        querySaveInquiry.Connection = openCon;
                        querySaveInquiry.Parameters.Add("@KODECABANG", SqlDbType.VarChar, 10).Value = upload_Request.KODECABANG;
                        querySaveInquiry.Parameters.Add("@JNSKREDIT", SqlDbType.VarChar, 1).Value = (int)upload_Request.JNSKREDIT;
                        querySaveInquiry.Parameters.Add("@CIF", SqlDbType.VarChar, 20).Value = upload_Request.CIF;
                        querySaveInquiry.Parameters.Add("@NIK", SqlDbType.VarChar, 32).Value = upload_Request.NIK;
                        querySaveInquiry.Parameters.Add("@INSURED", SqlDbType.VarChar, 200).Value = upload_Request.INSURED;
                        querySaveInquiry.Parameters.Add("@DOB", SqlDbType.VarChar, 15).Value = upload_Request.DOB;
                        querySaveInquiry.Parameters.Add("@GENDER", SqlDbType.VarChar, 2).Value = upload_Request.GENDER;
                        querySaveInquiry.Parameters.Add("@ADDRESS", SqlDbType.VarChar, 255).Value = upload_Request.ADDRESS;
                        querySaveInquiry.Parameters.Add("@PHONE", SqlDbType.VarChar, 30).Value = upload_Request.PHONE;
                        querySaveInquiry.Parameters.Add("@KDKREDIT", SqlDbType.VarChar, 10).Value = upload_Request.KDKREDIT;
                        querySaveInquiry.Parameters.Add("@NOREF", SqlDbType.VarChar, 50).Value = upload_Request.NOREF;
                        querySaveInquiry.Parameters.Add("@STARTDATE", SqlDbType.VarChar, 15).Value = upload_Request.STARTDATE;
                        querySaveInquiry.Parameters.Add("@PLAFOND", SqlDbType.Decimal).Value = (decimal)upload_Request.PLAFOND;
                        querySaveInquiry.Parameters.Add("@DURATION", SqlDbType.Int).Value = (int)upload_Request.DURATION;
                        querySaveInquiry.Parameters.Add("@KDOCCUPATION", SqlDbType.VarChar, 10).Value = upload_Request.KDOCCUPATION;
                        querySaveInquiry.Parameters.Add("@DETAILOCCUP", SqlDbType.VarChar, 50).Value = upload_Request.DETAILOCCUP;
                        querySaveInquiry.Parameters.Add("@ACCTNO", SqlDbType.VarChar, 15).Value = upload_Request.ACCTNO;
                        querySaveInquiry.Parameters.Add("@PREMI", SqlDbType.Decimal).Value = (decimal)upload_Request.PREMI;
                        querySaveInquiry.Parameters.Add("@CREATED_AT", SqlDbType.DateTime, 30).Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        openCon.Open();

                        querySaveInquiry.ExecuteNonQuery();
                    }
                }
                DateTime dt = DateTime.Now;
                upload_Response.status = 200;
                upload_Response.error = "false";
                upload_Response.result = string.Empty;
                upload_Response.rCode = "00";
                upload_Response.message = "Success";
                upload_Response.timestamp = dt.ToString("yyyy-MM-dd HH:mm:ss");
                return Ok(upload_Response);
            }
            catch (Exception ex)
            {
                DateTime dt = DateTime.Now;
                upload_Response.status = 500;
                upload_Response.error = "true";
                upload_Response.result = string.Empty;
                upload_Response.rCode = "30";
                upload_Response.message = ex.Message.ToString();
                upload_Response.timestamp = dt.ToString("yyyy-MM-dd HH:mm:ss");
                return StatusCode(500, upload_Response);
            }
            //return upload_Response;
        }

        
    }
}
