using insurranceDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsurranceDemo.Models
{
    /// <summary>
    /// 丟給Client端的客戶物件
    /// </summary>
    public class ClientCustom
    {

        /// <summary>
        /// 會員ID
        /// </summary>
        public long Id = 0;

        /// <summary>
        /// 姓名
        /// </summary>
        public string Name = "";

        /// <summary>
        /// 是否為男性
        /// </summary>
        public bool IsMale = true;

        /// <summary>
        /// 地址
        /// </summary>
        public string Address = "";

        /// <summary>
        /// 保單的清單
        /// </summary>
        public List<ClientInsurrance> InsurranceList = new List<ClientInsurrance>();

        /// <summary>
        /// 使用資料庫的物件初始化
        /// </summary>
        /// <param name="serverData">資料庫的model</param>
        public ClientCustom(Custom serverData)
        {
            Id = serverData.id;
            Name = serverData.name;
            IsMale = serverData.isMale;
            Address = serverData.addresss ?? "";           
        }
        
    }
}