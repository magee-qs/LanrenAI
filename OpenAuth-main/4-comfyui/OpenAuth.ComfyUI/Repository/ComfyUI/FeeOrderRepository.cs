using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Model.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Repository.ComfyUI
{
    public class FeeOrderRepository : BaseRepository<FeeOrderEntity>, IFeeOrderRepository
    {
        public FeeOrderRepository(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }

        public List<OrderListViewModel> OrderList(Page page)
        {
            string sqlText = @"SELECT  t1.*, t2.UserLevel, t2.Title, 
		                dbo.getUserNameById(t1.UserId) UserName,
		                dbo.getUserNameById(t1.CreateBy)CreateUser,
		                dbo.getUserNameById(t1.UpdateBy)UpdateUser,
                        dbo.getUserAccountById(t1.UserId) Telephone
                FROM dbo.fee_order t1  LEFT JOIN dbo.fee_level t2 ON t1.FeeId = t2.Id";

            return List<OrderListViewModel>(sqlText, page, null);
        }
    }

    public interface IFeeOrderRepository : IBaseRepository<FeeOrderEntity>, IScopeDependency
    {
        List<OrderListViewModel> OrderList(Page page);
    }
}
