using insurranceDemo.Models;
using InsurranceDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace insurranceDemo.Controllers.DataHelper
{
    /// <summary>
    /// 處理保單相關邏輯的物件
    /// </summary>
    public class InsurranceDataHelper
    {

        /// <summary>
        /// 取得要新增保單的實體
        /// </summary>
        /// <param name="name">保單名稱</param>
        /// <param name="description">保單描述</param>
        /// <param name="price">保單價格</param>
        /// <returns>保單的實體</returns>
        public static Insurrance GetInsurrance(string name, string description, decimal price)

        {
            var insurrance = new Insurrance();
            insurrance.name = name;
            insurrance.description = description;
            insurrance.price = price;
            insurrance.createTime = DateTime.Now;
            return insurrance;
        }

        /// <summary>
        /// 取得使用者的保險清單
        /// </summary>
        /// <param name="serverData">資料庫的字串</param>
        /// <returns>使用者的保險清單</returns>
        public static List<ClientInsurrance> GetCustomInsurrance(string serverData)
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
                        result.Add(new ClientInsurrance(insurrance));
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