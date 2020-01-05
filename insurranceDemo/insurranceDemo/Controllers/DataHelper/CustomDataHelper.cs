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