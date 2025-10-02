using Microsoft.AspNetCore.Builder;
using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Model.ComfyUI;
using OpenAuth.ComfyUI.Repository.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI
{

    public class FeeOrderService : BaseService<FeeOrderEntity>, IFeeOrderService
    {
        protected IFeeUserService feeService;

        protected IFeeOrderRepository repository;
        public FeeOrderService(OpenAuthDbContext context, IAuthService authService, IFeeUserService feeService, IFeeOrderRepository repository)
            : base(context, authService)
        {
            this.feeService = feeService;
            this.repository = repository;
        }

        public void AddOrder(FeeOrderModel orderModel)
        {
            orderModel.Validate();

            orderModel.OrderId = CommonHelper.CreaetDateNO(); 
            var entity = orderModel.MapTo<FeeOrderEntity>();
            
            Insert(entity);
            SaveChanges();
        }

        

        public void PayOrder(string orderId)
        {

            BeginTransaction(() =>
            {
                var entity = Find(t=>t.OrderId == orderId);
                if (entity == null)
                    throw new BizException("订单号不存在");

                if (entity.State == 1)
                    throw new BizException("订单已经支付");

                entity.State = 1;
                entity.Amount = entity.Cost;
                Update(entity);
                SaveChanges();


                feeService.Renew(entity.UserId, entity.FeeId, entity.Month);
            });
            
        }

        public void AbandonOrder(string orderId)
        {
            var entity = Find(t => t.OrderId == orderId);
            if (entity == null)
                throw new BizException("订单号不存在");

            if (entity.State == 1)
                throw new BizException("订单已经支付");

            if (entity.State == -1)
                throw new BizException("无效操作");

            entity.State = -1;
            Update(entity);
            SaveChanges();
        }


        public List<OrderListViewModel> OrderList(Page page)
        {
            return repository.OrderList(page);
        }

    }

    public interface IFeeOrderService : IBaseService<FeeOrderEntity>, IScopeDependency
    {
        void AddOrder(FeeOrderModel orderModel);

        void PayOrder(string orderId);

        void AbandonOrder(string orderId);

        List<OrderListViewModel> OrderList(Page page);

    }
}
