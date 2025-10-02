using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Repository.ComfyUI
{
    public class CostListRepository : BaseRepository<CostListEntity>, ICostListRepository
    {
        public CostListRepository(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }

        public List<CostListViewModel> GetCostList(Page page)
        {
            string sqlText = @"SELECT t1.* , t2.Account Telephone, t2.UserName, 
                            dbo.getUserNameById(t1.CreateBy)CreateUser
                    FROM dbo.cost t1 LEFT JOIN dbo.sys_user t2 ON t1.UserId = t2.Id";

            return List<CostListViewModel>(sqlText, page, null);
        }
    }

    public interface ICostListRepository : IBaseRepository<CostListEntity>, IScopeDependency
    {
        List<CostListViewModel> GetCostList(Page page);
    }
}
