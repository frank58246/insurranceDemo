//------------------------------------------------------------------------------
// <auto-generated>
//     這個程式碼是由範本產生。
//
//     對這個檔案進行手動變更可能導致您的應用程式產生未預期的行為。
//     如果重新產生程式碼，將會覆寫對這個檔案的手動變更。
// </auto-generated>
//------------------------------------------------------------------------------

namespace insurranceDemo.Models
{
    using System;
    using System.Collections.Generic;
    
    /// <summary>
    /// 客戶的實體
    /// </summary>
    public partial class Custom
    {
        /// <summary>
        /// 資料庫的id(DB自動產生)
        /// </summary>
        public long id { get; set; }

        /// <summary>
        /// 客戶的名稱
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 客戶性別
        /// </summary>
        public bool sex { get; set; }

        /// <summary>
        /// 客戶地址
        /// </summary>
        public string addresss { get; set; }

        /// <summary>
        /// 建立日期(DB自動產生)
        /// </summary>
        public System.DateTime createTime { get; set; }

        public string insuranceList { get; set; }
        public string isDelete { get; set; }
    }
}