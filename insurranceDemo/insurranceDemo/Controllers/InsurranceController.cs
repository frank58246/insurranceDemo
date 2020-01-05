using InsurranceDemo.Models;
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

        private List<Insurrance> allInsurranceCache 
        {
            get
            { return context.Insurrance.Where(i => !i.isDelete).ToList();
            }
        }

        private Insurrance getInsurranceBy(long id) 
        {
            var list = allInsurranceCache.Where(i => i.id == id).ToList();
            return list.Count > 0 ? list.First() : null;
        }

        /// <summary>
        ///  共用的成功物件
        /// </summary>
        private CommonSuccessReponse commonSuccessResponse = new CommonSuccessReponse();

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
        public HttpResponseMessage AddInsurrance(string name, string description, decimal price) 
        {
            // 檢查有沒有重名
            foreach (var item in allInsurranceCache)
            {
                if (item.name == name)
                {
                   return Request.CreateResponse(HttpStatusCode.BadRequest, "保單名稱重複!");

                }
            }

            var insurrance = new Insurrance();
            insurrance.name = name;
            insurrance.description = description;
            insurrance.price = price;
            insurrance.createTime = DateTime.Now;
            context.Insurrance.Add(insurrance);

            try
            {
                context.SaveChanges();
                var id = new { insurranceId = insurrance.id };
                return Request.CreateResponse(HttpStatusCode.OK, id);
            }
            catch (Exception e)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                
            }
        }

        /// <summary>
        /// 取得所有保單種類的清單
        /// </summary>
        /// <returns>所有保單種類的清單</returns>
        [HttpGet]
        [Route("GetAllInsurrance")]        
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
        /// <returns></returns>        
        [HttpPost]
        [Route("UpdateInsurrance")]
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
                        return Request.CreateResponse(HttpStatusCode.BadRequest, "修改失敗，保單名稱重複!");
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
                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "請求的保單不存在!");

            }

        }


        /// <summary>
        /// 刪除保單
        /// </summary>
        /// <param name="id">要刪除的保單的id</param>
        /// <returns>刪除的結果</returns>
        [HttpPost]
        [Route("DeleteInsurrance")]
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

                    return Request.CreateResponse(HttpStatusCode.InternalServerError, e.Message);
                }
            }
            else 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "要刪除的保單不存在!");

            }
        }

        /// <summary>
        /// 取得使用者的保險清單
        /// </summary>
        /// <param name="serverData">資料庫的字串</param>
        /// <returns>使用者的保險清單</returns>
        public static List<ClientInsurrance> getCustomInsurrance(string serverData)
        {
            InsuranceCompanyEntities context = new InsuranceCompanyEntities();
            List<Insurrance> allAvalibleInsurrance = context.Insurrance.Where(i => !i.isDelete).ToList();
            Dictionary<long, Insurrance> dictInsurrance = new Dictionary<long, Insurrance>();
            foreach (var item in allAvalibleInsurrance)
            {
                dictInsurrance.Add(item.id, item);
            }


            var result = new List<ClientInsurrance>();
            string[] list = serverData.Split(',');
            foreach (var item in list)
            {
                try
                {
                    long intValue = long.Parse(item);
                    var insurrance = dictInsurrance[intValue];
                    if (insurrance != null)
                    {
                        result.Add(new ClientInsurrance(insurrance)) ;
                    }

                }
                catch (Exception)
                {
                    return result;
                }
            }

            return result;
        }
    }

}