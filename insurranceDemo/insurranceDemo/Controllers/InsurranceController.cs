﻿using insurranceDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace insurranceDemo.Controllers
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
        /// 新增保單
        /// </summary>
        /// <param name="name">保單名稱</param>
        /// <param name="description">保單名稱</param>
        /// <param name="price">價格</param>
        /// <returns>新的保單的id</returns>
        /// 
        [HttpPost]
        [Route("AddInsurrance")]
        public HttpResponseMessage AddInsurrance(string name, string description, decimal price) 
        {
            /// 檢查有沒有重名
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


        [HttpGet]
        [Route("GetAllInsurrance")]
        /// <summary>
        /// 取得所有保單種類的清單
        /// </summary>
        /// <returns>所有保單種類的清單</returns>
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
        [Route("updateInsurrance")]
        public HttpResponseMessage updateInsurrance(long id, string name, string description,decimal price)
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
                    return Request.CreateResponse(HttpStatusCode.OK, new CommonSuccessReponse());
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
    }

}