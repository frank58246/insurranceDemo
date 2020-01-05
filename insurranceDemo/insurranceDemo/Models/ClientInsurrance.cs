using insurranceDemo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace InsurranceDemo.Models
{
    /// <summary>
    /// 丟給client端的保單物件
    /// </summary>
    public class ClientInsurrance
    {
        /// <summary>
        /// id
        /// </summary>
        public long Id = 0;

        /// <summary>
        /// 名稱
        /// </summary>
        public string Name = "";

        /// <summary>
        /// 價格
        /// </summary>
        public decimal Price = 0;

        /// <summary>
        /// 保單描述
        /// </summary>
        public string Description = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serverData"></param>
        public ClientInsurrance(Insurrance serverData)
        {
            Id = serverData.id;
            Name = serverData.name;
            Price = serverData.price;
            Description = serverData.description;
        }
    }
}