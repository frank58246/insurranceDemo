using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using insurranceDemo.Controllers.DataHelper;
using insurranceDemo.Models;
using InsurranceDemo.Models;
using Swagger.Net.Annotations;

namespace InsurranceDemo.Controllers
{
    /// <summary>
    /// 控制客戶資料的controller
    /// </summary>
    public class CustomController : ApiController
    {
        /// <summary>
        /// 找出指定id的用戶
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Custom getCustomBy(int id)
        {
            var target = context.Custom.Where(c => c.id == id).ToList();
            return target.Count > 0 ? target.First() : null;
        }

        /// <summary>
        /// 資料庫實體
        /// </summary>
        private InsuranceCompanyEntities context = new InsuranceCompanyEntities();


        /// <summary>
        ///  共用的成功回應物件
        /// </summary>
        private CommonSuccessReponse commonSuccessResponse = new CommonSuccessReponse();
      

        /// <summary>
        /// 取得指定id用戶的詳細資料
        /// </summary>
        /// <param name="id">要查詢的用戶id</param>
        /// <returns>指定的用戶</returns>
        [HttpGet]
        [Route("GetCustomDetail/{id}")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ClientCustom))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage GetCustomDetail(int id)
        {
            var custom = getCustomBy(id);
            if (custom != null)
            {
                var clientCustom = new ClientCustom(custom);
                clientCustom.InsurranceList = InsurranceDataHelper.GetCustomInsurrance(custom.insuranceList);
                return Request.CreateResponse(HttpStatusCode.OK, clientCustom);
            }
            else
            {
                var errorResponse = new CommonErrorResponse(101, "查無此會員!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }
        }

        /// <summary>
        /// 取得指定id列表的客戶資料
        /// </summary>
        /// <param name="idList">要查詢的id列表</param>
        /// <returns>指定id列表的客戶清單</returns>
        [HttpPost]
        [Route("GetCustomList")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(List<ClientCustom>))]
        public HttpResponseMessage GetCustomList(List<int> idList)
        {
            var result = new List<ClientCustom>();

            foreach (var id in idList)
            {
                var client = getCustomBy(id);
                if (client != null)
                {
                    result.Add(new ClientCustom(client));
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, result);
        }

        /// <summary>
        /// 新增客戶
        /// </summary>
        /// <param name="name">姓名</param>
        /// <param name="sex">性別</param>
        /// <param name="identity">身分證字號</param>
        /// <param name="address">地址</param>
        /// <returns>新客戶的實體</returns>
        [HttpPost]
        [Route("AddCustom")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ClientCustom))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage AddCustom(string name, bool sex, string identity, string address = "")
        {
            var target = context.Custom.Where(c => c.identity == identity).ToList();
            if (target.Count == 0)
            {
                var newCustomer = CustomDataHelper.getValidCustom(name, sex, identity, address);
                if (newCustomer != null)
                {
                     context.Custom.Add(newCustomer);
                    try
                    {
                        context.SaveChanges();
                        var clientCustom = new ClientCustom(newCustomer);
                        return Request.CreateResponse(HttpStatusCode.OK, clientCustom);

                    }
                    catch (Exception e)
                    {
                        var errorResponse = new CommonErrorResponse(201, e.Message);
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }
                }
                else 
                {
                    var errorResponse = new CommonErrorResponse(101, "會員資料有誤!");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }
            }
            else
            {
                var errorResponse = new CommonErrorResponse(102, "身分證字號已存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

        }

        /// <summary>
        /// 刪除會員
        /// </summary>
        /// <param name="id">要刪除會員的Id</param>
        /// <returns>刪除操作的回應</returns>
        [HttpPost]
        [Route("DeleteCustomer")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommonSuccessReponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage DeleteCustomer(int id)
        {
            var target = context.Custom.Where(c => c.id == id && !c.isDelete).ToList();
            if (target.Count > 0)
            {
                var customer = target.First();
                customer.isDelete = true;

                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, commonSuccessResponse);
                }
                catch (Exception e)
                {
                    var errorResponse = new CommonErrorResponse(201, e.Message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);

                }
            }
            else
            {
                var errorResponse = new CommonErrorResponse(101, "請求的會員不存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

        }

        /// <summary>
        /// 更新客戶的基本資料
        /// </summary>
        /// <param name="id">客戶的id</param>
        /// <param name="name">姓名</param>
        /// <param name="isMale">性別</param>
        /// <param name="address">住址</param>
        /// <returns></returns>
        [HttpPost]
        [Route("UpdateCustomDetail")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommonSuccessReponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage UpdateCustomDetail(int id, string name, bool isMale, string address )                                                          
        {
            var custom = getCustomBy(id);
            if (custom  != null)
            {
                custom.name = name;
                custom.isMale = isMale;
                custom.addresss = address;
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, commonSuccessResponse);

                }
                catch (Exception e)
                {
                    var errorResponse = new CommonErrorResponse(201, e.Message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }
            }
            else
            {
                var errorResponse = new CommonErrorResponse(101, "請求的會員不存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }
        }

      /// <summary>
      /// 更新客戶的保險清單
      /// </summary>
      /// <param name="id">客戶的ID</param>
      /// <param name="insurranceList">客戶擁有的保險清單列表</param>
      /// <returns>是否成功</returns>
        [HttpPost]
        [Route("UpdateCustomInsurrance")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommonSuccessReponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(string))]
        public HttpResponseMessage UpdateCustomInsurrance(int id, int[] insurranceList)
        {

            // 檢查是否給不存在的保單id
            var allInsuranceId = context.Insurrance.Where(i => ! i.isDelete).Select(i => i.id);
            foreach (var oneInsurrance  in insurranceList)
            {
                if (!allInsuranceId.Contains(oneInsurrance)) 
                {
                    var errorResponse = new CommonErrorResponse(101, $"請求的保單{oneInsurrance}不在資料庫中!");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }
            }

            // 檢查用戶是否存在
            var custom = getCustomBy(id);
            if (custom != null)
            {
                string joinedList = String.Join(", ", insurranceList.ToArray());
                custom.insuranceList = joinedList;
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, commonSuccessResponse);

                }
                catch (Exception e)
                {
                    var errorResponse = new CommonErrorResponse(201, e.Message);
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);

                }
            }
            else
            {
                var errorResponse = new CommonErrorResponse(102, "請求的會員不存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }        
        }

    }
}
