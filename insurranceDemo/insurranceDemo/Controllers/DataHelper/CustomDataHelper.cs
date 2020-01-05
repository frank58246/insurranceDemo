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
        public static Custom getValidCustom(string name, bool sex, string identity, string address = "")
        {
                        
            if (!Regex.IsMatch(identity, @"^[A-Z]\d{9}$"))
            {
                return null;
            }

            var newCustomer = new Custom();
            newCustomer.name = name;
            newCustomer.sex = sex;
            newCustomer.isDelete = false;
            newCustomer.insuranceList = "";
            newCustomer.identity = identity;
            newCustomer.createTime = DateTime.Now;
            
            
            return newCustomer;
        }
        
    }
}