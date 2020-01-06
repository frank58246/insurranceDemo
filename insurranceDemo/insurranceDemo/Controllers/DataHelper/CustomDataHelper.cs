using insurranceDemo.Models;
using InsurranceDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace insurranceDemo.Controllers.DataHelper
{
    /// <summary>
    /// 處理客戶資料的物件
    /// </summary>
    public class CustomDataHelper
    {
        /// <summary>
        /// 取得正確的客戶物件
        /// </summary>
        /// <param name="name">客戶姓名</param>
        /// <param name="isMale">是否為男性</param>
        /// <param name="identity">身分證</param>
        /// <param name="address">地址</param>
        /// <returns>客戶的物件</returns>
        public static Custom getValidCustom(string name, bool isMale, string identity, string address = "")
        {
                        
            if (!Regex.IsMatch(identity, @"^[A-Z][1-2]{1}\d{8}$"))
            {
                return null;
            }

            if (identity[1] == '1' && !isMale)
            {
                return null;
            }

            if (identity[1] == '2' && isMale)
            {
                return null;
            }


            var newCustomer = new Custom();
            newCustomer.name = name;
            newCustomer.isMale = isMale;
            newCustomer.isDelete = false;
            newCustomer.insuranceList = "";
            newCustomer.identity = identity;
            newCustomer.createTime = DateTime.Now;            
            
            return newCustomer;
        }
        
    }
}