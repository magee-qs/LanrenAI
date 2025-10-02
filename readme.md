# LanrenAI

#### 介绍

使用 NetCore+EF+Dapper 基于大模型进行电商图片快速加工处理的 AI 工具

即使用户不会使用 AI 的工作流，不擅长 AI 关键字描述，lanrenAI 会根据需求自动补充关键字、运镜、图片布局,快速辅助用户设计出满意的图片

#### 开发框架简介

服务端: NetCore + EF + Dapper
前端 : Vue3 + TypeScript
手机端： UniApp

#### 功能介绍

LanrenAI 主要服务于电商设计师或从事电商的相关人员, 用于电商产品图辅助设计,主要包括以下功能:

- 会员充值: 普通会员、VIP , 月卡充值、年卡充值
- 电量管理: 会员充值后每月自动发送电量, 每次出图根据基础电量\* 图片张数扣除,未成功不扣电量; 电量消耗查询
- 工作流管理: 用于加载新的工作流(需要添加工作流模板和工作流模板加载类)
- 产品出图： 根据流程进行电商图片处理; 提交任务后进入队列; VIP 会员具备优先权;
- 常用电商工作流： 人物抠图、动物抠图、物品一键消除、图片高清放大、图片阔图、老旧照片修复、一键换装、
  一键打光、补色、产品精修、文生图、图生图、证件照等常用功能
- 文件存储支持两种模式: 本地存储 阿里云 oss 存储
- 简单的代码生成器，辅助用户进行功能扩展

 
#### 功能案例图片
<table  style="border: none; border-collapse: collapse; width: 100%;">
<tr><td width="50%"><img src="/images/5nlnini98qpzcw8e6lm8c835d9akyal1caawqzoqwyylhp20oivsvbur74izv5xy.png" alt="图片1"></td><td><img src="/images/tqqz27b7sm6son4wdqv90t2k36vbyc7jq725t2m8u44qtce1tgkan5p6wdhd7s6w.png" alt="图片2"></td></tr> 
<tr><td width="50%"><img src="/images/2431c7b6807c255ae7975bb22ff7b060cf43814c4fe6498adffd15bc73a251c6.webp" alt="图片1"></td><td><img src="/images/ab8f7aae1fbbeb7fa9642fb34389dbfc3da66e4b590a761e669fce0d94de4e27.webp" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/6gqf7dc5l41dm190jmt71whpe4ti6vdl1zn0ifgsupbsddn84p8nx0jovnodnnct.png" alt="图片1"></td><td><img src="/images/r79rx8czh73omcjoqpit692n6ybyydjivbmhcemihdbjbqt67fbk0yljksifmtjh.png" alt="图片2"></td></tr>
<tr><td width="50%"> <img src="/images/jpPrI564Lqrlq7YzJF13YmnxtNGevOQbZo0dZzOUORT1pcsiFKjfIrssfTkOaiPE16vJAhp0o80Hg8R4N6I96eYivqL6gX3RpriwuTYp8zgIFPm5D2dt1Krj9yHcj8wy.png" alt="图片1"></td><td> <img src="/images/mjkGpJWJhkVa2UIF0yqyqQAI0QoXu1TRRW3VLT0gA7bddxNjV7JGxh5Oirpkh5LH7nCW8OZ41HadpZdsECp5r43PwiBHZ2kzz8oM7WfEhn7aGkUqzTeOI8rbrL6C5Zo5.png" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/3b2b78903eff77a0634d2a41deda317c4c20fd0df21a16eb22ecd4529142df03.webp" alt="图片1"></td><td><img src="/images/310b137b3c5952e2e38cab76aeffc8e41d6466d4ef97e475765efa6e3659f1d6.webp" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/ComfyUI_temp_avxju_00040_.png" alt="图片1"></td><td><img src="/images/ComfyUI_temp_avxju_00025_.png" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/ComfyUI_temp_avxju_00026_.png" alt="图片1"></td><td> <img src="/images/ComfyUI_temp_avxju_00025_.png" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/ComfyUI_temp_avxju_00029_.png" alt="图片1"></td><td><img src="/images/ComfyUI_temp_avxju_00032_.png" alt="图片2"></td></tr>
<tr><td width="50%"><img src="/images/ComfyUI_temp_avxju_00042_.png" alt="图片1"></td><td><img src="/images/ComfyUI_temp_avxju_00046_.png" alt="图片2"></td></tr>
</table>
 
  

#### 源码和配置文件说明
* OpenAuth-main: 服务端代码,提供接口服务,需要开通以下账号
##### 阿里在线支付
  ![测试PNG](/images/164423129.png)
##### 短信验证码
  ![测试PNG](/images/164423587.png)
##### 百度翻译
  ![测试PNG](/images/164423958.png)
##### 阿里云oss
  ![测试PNG](/images/164907.png)

#### 配置文件中删除example字段生成自己的配置文件,一共三个地方
* comfyui-vue3-master\env.example.production | env.example.development
* OpenAuth-main\4-comfyui\OpenAuth.ComfyUI.WebAPI\appsetting.example.json
* uniapp-vue2-admin\utils\config.example.js

#### 添加模板和工作流
1. 添加模板到template目录下
  ![测试PNG](/images/165127132.png)
2. 自定义类继承方法  NodeService, IPicBgNodeService， 重写PrepareJsonParam方法
  ![测试PNG](/images/165127584.png)

##### 可以根据个人需求进行工作流的定制
  接受电商公司AI出图定制开发
  1. 工作流定制
  2. 服务器硬件和大模型部署定制
  3. 模型训练、lora训练

#### 欢迎扫码咨询
 （备注来源:gitee.com/magee-qs）;如果以上代码对您有帮助，也可以打赏作者一杯奶茶
<table style="border: none; border-collapse: collapse; width: 100%;">
<tr><td width="50%"> <img src="/images/202510021657393027.jpg" alt="图片1"></td><td> <img src="/images/20251002165738.jpg" alt="图片1"></td></tr>
</table>
 


