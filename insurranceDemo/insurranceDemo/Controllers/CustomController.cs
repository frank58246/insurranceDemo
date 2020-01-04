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

        private Custom getCustomBy(int id)
        {
            var target = context.Custom.Where(c => c.id == id).ToList();
            return target.Count > 0 ? target.First() : null;
        }

        /// <summary>
        /// 資料庫實體
        /// </summary>
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

        [HttpPost]
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
                newCustomer.name = name;
                newCustomer.sex = sex;
                newCustomer.isDelete = false;
                newCustomer.insuranceList = "";
                newCustomer.identity = identity;
                newCustomer.createTime = DateTime.Now;
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

        /// <summary>
        /// 刪除會員
        /// </summary>
        /// <param name="id">要刪除會員的Id</param>
        /// <returns>刪除操作的回應</returns>
        [HttpPost]
        [Route("deleteCustomer")]
        public HttpResponseMessage deleteCustomer(int id)
        {
            var target = context.Custom.Where(c => c.id == id && !c.isDelete).ToList();
            if (target.Count > 0)
            {
                var customer = target.First();
                customer.isDelete = true;

                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new CommonSuccessReponse());
                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "請求的會員不存在!");

            }

        }

        /// <summary>
        /// 更新客戶的基本資料
        /// </summary>
        /// <param name="id">客戶的id</param>
        /// <param name="name">姓名</param>
        /// <param name="sex">性別</param>
        /// <param name="address">住址</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCustomDetail")]
        public HttpResponseMessage updateCustomDetail(int id, string name, bool sex, string address )                                                          
        {
            var custom = getCustomBy(id);
            if (custom  != null)
            {
                custom.name = name;
                custom.sex = sex;
                custom.addresss = address;
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new CommonSuccessReponse());

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "請求的會員不存在!");
            }
        }

        /// <summary>
        /// 更新客戶的保單資料
        /// </summary>
        /// <param name="id">客戶的id</param>
        /// <param name="insurranceList">保單id清單</param>
        /// <returns></returns>
        [HttpPost]
        [Route("updateCustomInsurrance")]
        public HttpResponseMessage updateCustomInsurrance(int id, int[] insurranceList)
        {
            var custom = getCustomBy(id);

            // 檢查是否給不存在的保單id
            var allInsuranceId = context.Insurrance.Where(i => ! i.isDelete).Select(i => i.id);
            foreach (var oneInsurrance  in insurranceList)
            {
                if (!allInsuranceId.Contains(oneInsurrance)) 
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, $"請求的保單{oneInsurrance}不在資料庫中!");
                }
            }


            if (custom != null)
            {
                string joinedList = String.Join(", ", insurranceList.ToArray());
                custom.insuranceList = joinedList;
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, new CommonSuccessReponse());

                }
                catch (Exception e)
                {
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "請求的會員不存在!");
            }
        
        }

    }
}
