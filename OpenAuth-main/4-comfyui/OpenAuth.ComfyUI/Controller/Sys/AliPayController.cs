
using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using Aop.Api.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Service.ComfyUI;
using System.Collections.Specialized;


namespace OpenAuth.ComfyUI.Controller.Sys
{
    [ApiController] 
    public class AliPayController: ControllerBase
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppSetting appSetting;
        
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();


         
        public AliPayController(IHttpContextAccessor httpContextAccessor, 
            AppSetting appSetting  )
        { 
            this.httpContextAccessor = httpContextAccessor;
            this.appSetting = appSetting; 
        }


        /// <summary>
        /// 根据订单号生成阿里支付订单
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/alipay/order")]
        public AjaxResult Order([FromBody] string  orderId)
        {
            var _config = appSetting.AlipayConfig;
            //获取参数
            var config = new Aop.Api.AlipayConfig()
            {
                ServerUrl = _config.gatewayUrl, 
                AppId = _config.appId, 
                Format = _config.format,
                Charset = _config.charset, 
                SignType = _config.signType, 
                PrivateKey = _config.privateKey,
                AlipayPublicKey = _config.publicKey, 
                EncryptKey = _config.encryptKey 
            };

            ////判断订单号是否存在
            //var order = cardService.Find<OrderEntity>(t=>t.OrderId == orderId);
            //if (order == null)
            //    return AjaxResult.Error("支付订单不存在");

            //var card = cardService.Find<CardEntity>(order.CardId);


            // 业务参数
            AlipayTradePrecreateModel model = new AlipayTradePrecreateModel()
            {
                OutTradeNo = orderId,
                Subject ="充值会员",
                TotalAmount = "100",
                Body = "充值会员",
                SellerId = _config.sellerId
            };

            var client = new DefaultAopClient(config);

            AlipayTradePrecreateRequest request = new AlipayTradePrecreateRequest();
            request.SetBizModel(model);
            request.SetNotifyUrl(_config.notifyUrl);
            try
            {
                AlipayTradePrecreateResponse response = client.Execute(request);
                if (!response.IsError)
                {
                    // 调用成功
                    var qrCode = response.QrCode; 
                    return AjaxResult.OK(qrCode, "ok");
                }
                else 
                {
                    var msg = response.SubMsg != null ? response.SubMsg : response.Msg;
                    // 调用失败
                    logger.Error("调用支付失败:" + msg);
                    return AjaxResult.Error("订单支付异常:" + msg);
                } 
            }
            catch (Exception ex)
            {
                logger.Error("alipay订单支付异常:" + ex.Message, ex);
                return AjaxResult.Error("订单支付异常");
            } 
        }


        /* 实际验证过程建议商户添加以下校验。
        1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
        2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
        3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
        4、验证app_id是否为该商户本身。
        */
        [HttpPost]
        [Route("/alipay/notice")]
        [AllowAnonymous]
        public string Notice()
        {
            Dictionary<string, string> dict = GetParams();
            var config = appSetting.AlipayConfig;
            //var isChekced = AlipaySignature.RSACertCheckV1(dict, config.publicKey, config.charset, config.signType);
            var isChekced = true;
            if (isChekced)
            {
                //交易状态
                //判断该笔订单是否在商户网站中已经做过处理
                //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                //如果有做过处理，不执行商户的业务程序

                //注意：
                //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                string trade_status = Request.Form["trade_status"];
                // TRADE_FINISHED  TRADE_SUCCESS  WAIT_BUYER_PAY TRADE_CLOSED
                if (trade_status == "TRADE_SUCCESS")
                {
                    //支付成功
                    var trade_no = Request.Form["trade_no"];
                    var out_trade_no = Request.Form["out_trade_no"];
                    var seller_id = Request.Form["seller_id"];
                    var app_id = Request.Form["app_id"];
                    var total_amount = Request.Form["total_amount"];

                    //保存订单支付信息
                    //var ajaxResult = cardService.UpdateOrder(out_trade_no, total_amount.ToDecimal(0), CommonConstant.Alipay, trade_no);
                    //if (ajaxResult.success)
                    //{
                    //    return "success";
                    //}
                    //else 
                    //{
                    //    return "fail";
                    //}
                    return "success";
                } 
                return "success";
            }
            else 
            {
                return "fail";
            }
        }


        private Dictionary<string, string> GetParams()
        { 
            Dictionary<string,string> dict  = new Dictionary<string,string>();
            var form =  Request.Form;
            
            foreach (var key in form.Keys)
            {
                var data = form[key];
                dict.Add(key, data);
            }
            return dict;
        }
    }
}
