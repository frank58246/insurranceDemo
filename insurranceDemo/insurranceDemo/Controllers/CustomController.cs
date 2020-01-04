using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using insurranceDemo.Models;

namespace insurranceDemo.Controllers
{
    /// <summary>
    /// 控制客戶資料的controller
    /// </summary>
    public class CustomController : ApiController
    {

        private InsuranceCompanyEntities context = new InsuranceCompanyEntities();

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }


        /// <summary>
        /// 取得指定id用戶的詳細資料
        /// </summary>
        /// <param name="id">要查詢的用戶id</param>
        /// <returns></returns>
        [Route("GetCustomDetail/{id}")]
        public HttpResponseMessage GetCustomDetail(int id) 
        {
            var target = context.Custom.Where(c => c.id == id).ToList();
            if (target.Count > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, target.First());
            }
            else 
            {
                var message = string.Format("查無此會員!");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }
        }

        [HttpPost ]
        [Route("AddCustom")]
        /// <summary>
        /// 新增用戶
        /// </summary>
        /// <param name="newCustom">新用戶的資料</param>
        /// <returns></returns>
        public HttpResponseMessage AddCustom(string name, bool sex, string identity, string address = "") 
        {
           

            var target = context.Custom.Where(c => c.identity == identity).ToList();
            if (target.Count == 0)
            {
                var newCustomer = new Custom();
                newCustomer.id = 0;
                newCustomer.name = name;
                newCustomer.sex = sex;
                newCustomer.isDelete = false;
                newCustomer.insuranceList = "";
                newCustomer.identity = identity;
                context.Custom.Add(newCustomer);
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, newCustomer.id);

                }
                catch (Exception e)
                {
                    var message = string.Format("新增會員失敗");
                    HttpError err = new HttpError(message);
                    return Request.CreateResponse(HttpStatusCode.NotFound, err);
                }
            }
            else 
            {
                var message = string.Format("新增會員失敗，身分證字號錯誤!");
                HttpError err = new HttpError(message);
                return Request.CreateResponse(HttpStatusCode.NotFound, err);
            }

        }


        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
