using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace insurranceDemo.Models
{
    /// <summary>
    /// 共用的api錯誤回應
    /// </summary>
    public class CommonErrorResponse
    {
        /// <summary>
        /// 錯誤代碼
        /// </summary>
        public int Code;

        /// <summary>
        /// 錯誤描述
        /// </summary>
        public string Description;
        
        /// <summary>
        /// 建構子
        /// </summary>
        /// <param name="code"></param>
        /// <param name="description"></param>
        public CommonErrorResponse(int code, string description)
        {
            Code = code;
            Description = description;
        }
    }
}