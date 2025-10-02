using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Model.ComfyUI
{
    /// <summary>
    /// 订单明细
    /// </summary>
    public class OrderListViewModel : EntityString
    {
        public string UserId { get; set; }

        public string OrderId { get; set; }

        public string FeeId { get; set; }

        public int Cost { get; set; }

        public int Month { get; set; }

        public string? PayType { get; set; }

        public int Amount { get; set; }

        public string? CreateBy { get; set; }

        public DateTime? CreateTime { get; set; }

        public string? UpdateBy { get; set; }

        public DateTime? UpdateTime { get; set; }

        public int State { get; set; }

        public string UserName { get; set; }

        public string Telephone { get; set; }

        public string? CreateUser { get; set; }

        public string? UpdateUser { get; set; }

        public string UserLevel { get; set; }

        public string FeeTitle { get; set; }
    }
}
