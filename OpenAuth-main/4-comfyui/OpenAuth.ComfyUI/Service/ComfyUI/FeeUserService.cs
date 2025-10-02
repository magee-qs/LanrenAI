using Microsoft.Extensions.Logging;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Service.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    public class FeeUserService: BaseService<FeeUserEntity>, IFeeUserService
    {
        protected IUserService userService { get; set; }
        public ICostService costService { get; set; }
        public FeeUserService(OpenAuthDbContext context, IAuthService authService, ICostService costService, IUserService userService) 
            : base(context, authService)
        {
            this.costService = costService;
            this.userService = userService;
        }

        public void Renew(string userId, string feeId, int month)
        {
            var entity = Find(t => t.UserId == userId);
            if (entity == null)
            {
                var expire = DateTime.Now.ToMax().AddDays(1).AddMonths(month);
                entity = new FeeUserEntity()
                {
                    FeeId = feeId,
                    UserId = userId,
                    Expire = expire.ToDate()
                };
                Insert(entity);
            }
            else
            { 
                var date = entity.Expire.ToDate();
                date = date.AddMonths(month);
                var expire = date.ToDate();
                entity.Expire  = expire;
                Update(entity);
            }
            SaveChanges();
        }


        /// <summary>
        /// 自动发放点数
        /// </summary>
        public void AutoExecute(DateTime date)
        {
            var expire = date.ToDate();
            var min = date.ToFisrtDay()  ;
            var max = date.ToLastDay() ;
            var max1 = date.ToLastDay().AddDays(1) ;


            var levels = List<FeeLevelEntity>();
            var users =  List(t => t.Expire >= max1.ToDate());

            logger.LogDebug("开始自动发放点券:" + date.ToString("yyyy-MM"));

            //当前月份没有发放点卡
            foreach (var user in users)
            {
                logger.LogDebug($"点券发放--当前用户: [userId:{user.UserId}, FeeId: {user.FeeId}]");
                var costEntity = Find<CostEntity>(t => t.UserId == user.UserId && t.CreateTime >= min 
                    && t.CreateTime < max1 && t.State == 1);

                if (costEntity != null)
                {
                    logger.LogDebug($"点券发放--当前用户: [userId:{user.UserId}, FeeId: {user.FeeId}]已发放，跳过本次操作");
                    continue;
                }
                   

                var level = levels.Where(t => t.Id == user.FeeId).FirstOrDefault();
                if (level == null)
                    continue;

                //每月有效
                costService.AddCost(user.UserId, "会员点数自动发放", level.Cost, max,1);
                logger.LogDebug($"点券发放--当前用户: [userId:{user.UserId}, FeeId: {user.FeeId}]发放完成");
            }
        }

        public void Execute(ExecuteCostModel costModel)
        {
           
            var min = costModel.Month.ToFisrtDay();
            var max = costModel.Month.ToLastDay();
            var max1 = costModel.Month.ToLastDay().AddDays(1);

            var user =  userService.Find(costModel.UserId);
            if (user == null)
                throw new BizException("用户不能存在");

            if(user.State == 0)
                throw new BizException("用户被禁用");

            var content = costModel.Content.IsEmpty() ? "会员点数自动发放" : costModel.Content;
            //当月有效
            costService.AddCost(costModel.UserId, content, costModel.Cost, max); 

        }
    }

    public interface IFeeUserService : IBaseService<FeeUserEntity>, IScopeDependency
    {
        void Renew(string userId ,string feeId, int month);


        /// <summary>
        /// 自动发放点数
        /// </summary>
        void AutoExecute(DateTime date);


        void Execute(ExecuteCostModel costModel);
    }
}
