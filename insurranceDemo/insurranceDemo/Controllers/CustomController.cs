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

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
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
