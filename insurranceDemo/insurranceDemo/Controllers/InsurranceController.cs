using insurranceDemo.Controllers.DataHelper;
using insurranceDemo.Models;
using InsurranceDemo.Models;
using Swagger.Net.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace InsurranceDemo.Controllers
{
    /// <summary>
    /// 保單相關處理
    /// </summary>
    public class InsurranceController : ApiController
    {
        /// <summary>
        /// 資料庫實體
        /// </summary>
        private InsuranceCompanyEntities context = new InsuranceCompanyEntities();

        /// <summary>
        /// 所有保單的快取
        /// </summary>
        private List<Insurrance> allInsurranceCache 
        {
            get
            { 
                return context.Insurrance.Where(i => !i.isDelete).ToList();
            }
        }

        /// <summary>
        /// 取得指定id的保單
        /// </summary>
        /// <param name="id">保單的id</param>
        /// <returns>指定的保單</returns>
        private Insurrance getInsurranceBy(long id) 
        {
            var list = allInsurranceCache.Where(i => i.id == id).ToList();
            return list.Count > 0 ? list.First() : null;
        }

        /// <summary>
        ///  共用的成功回應物件
        /// </summary>
        private CommonSuccessReponse commonSuccessResponse = new CommonSuccessReponse();


        /// <summary>
        /// 取得共用的失敗回應物件
        /// </summary>
        /// <param name="e">內部例外</param>
        /// <returns>共用的失敗回應物件</returns>
        private HttpResponseMessage getCommonErrorResponse(Exception e)
        {
            var errorResponse = new CommonErrorResponse(201, e.Message);
            return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
        }


        /// <summary>
        /// 新增保單
        /// </summary>
        /// <param name="name">保單名稱</param>
        /// <param name="description">保單描述</param>
        /// <param name="price">價格</param>
        /// <returns>新的保單的id</returns>
        /// 
        [HttpPost]
        [Route("AddInsurrance")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(ClientInsurrance))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage AddInsurrance(string name, string description, decimal price) 
        {
            // 檢查有沒有重名
            foreach (var item in allInsurranceCache)
            {
                if (item.name == name)
                {
                    var errorResponse = new CommonErrorResponse(101, "保單名稱重複!");
                    return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                }
            }

            var insurrance = InsurranceDataHelper.GetInsurrance(name, description, price);
            context.Insurrance.Add(insurrance);

            try
            {
                context.SaveChanges();
                var clientInsurrance = new ClientInsurrance(insurrance);
                return Request.CreateResponse(HttpStatusCode.OK, clientInsurrance);
            }
            catch (Exception e)
            {
                return getCommonErrorResponse(e);
            }
        }

        /// <summary>
        /// 取得所有保單種類的清單
        /// </summary>
        /// <returns>所有保單種類的清單</returns>
        [HttpGet]
        [Route("GetAllInsurrance")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(IEnumerable<ClientInsurrance>))]
        public HttpResponseMessage GetAllInsurrance()
        {
            var result = new List<ClientInsurrance>();
            foreach (var item in allInsurranceCache)
            {
                result.Add( new ClientInsurrance(item));
            }

            return Request.CreateResponse(HttpStatusCode.OK, result); 
        }


        /// <summary>
        /// 更新保單的內容
        /// </summary>
        /// <param name="id">保單的id</param>
        /// <param name="name">保單名稱</param>
        /// <param name="description">保單描述</param>
        /// <param name="price">保單價格</param>
        /// <returns>是否成功的回應</returns>        
        [HttpPost]
        [Route("UpdateInsurrance")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommonSuccessReponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage UpdateInsurrance(long id, string name, string description,decimal price)
        {          
            var insurrance = getInsurranceBy(id);
            if (insurrance != null)
            {
                // 檢查有沒有蓋掉其他保單
                foreach (var item in allInsurranceCache)
                {
                    if (item.name == name && item.id != id)
                    {
                        var errorResponse = new CommonErrorResponse(101, "修改失敗，保單名稱重複!");
                        return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
                    }
                }

                insurrance.name = name;
                insurrance.description = description;
                insurrance.price = price;

                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, commonSuccessResponse);
                }
                catch (Exception e)
                {
                    return getCommonErrorResponse(e);
                }
            }
            else
            {
                var errorResponse = new CommonErrorResponse(102, "請求的保單不存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }

        }


        /// <summary>
        /// 刪除保單
        /// </summary>
        /// <param name="id">要刪除的保單的id</param>
        /// <returns>刪除的結果</returns>
        [HttpPost]
        [Route("DeleteInsurrance")]
        [SwaggerResponse(HttpStatusCode.OK, Type = typeof(CommonSuccessReponse))]
        [SwaggerResponse(HttpStatusCode.BadRequest, Type = typeof(CommonErrorResponse))]
        public HttpResponseMessage DeleteInsurrance(long id)
        {
            var insurrance = getInsurranceBy(id);
            if(insurrance != null)
            {
                insurrance.isDelete = true;
                try
                {
                    context.SaveChanges();
                    return Request.CreateResponse(HttpStatusCode.OK, commonSuccessResponse);

                }
                catch (Exception e)
                {
                    return getCommonErrorResponse(e);
                }
            }
            else 
            {
                var errorResponse = new CommonErrorResponse(101, "要刪除的保單不存在!");
                return Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);

            }
        }
       
    }

}