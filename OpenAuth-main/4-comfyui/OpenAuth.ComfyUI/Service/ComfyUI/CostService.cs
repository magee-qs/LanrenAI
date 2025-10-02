using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using OpenAuth.ComfyUI.Model;
using OpenAuth.ComfyUI.Repository.ComfyUI;
 

namespace OpenAuth.ComfyUI.Service.ComfyUI
{
    public class CostService : BaseService<CostEntity> , ICostService
    {
        private ICostListRepository repository;
        public CostService(OpenAuthDbContext context, IAuthService authService, ICostListRepository repository) 
            : base(context, authService)
        {
            this.repository = repository;
        }

        /// <summary>
        /// 消费记账
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cost"></param>
        public void Cost(string taskId, int cost)
        {
            var userId = AuthService.UserId;

            var date = DateTime.Now.ToDate();
            //获取点数
            List<CostEntity> list = List(t => t.UserId == userId && t.Exipre > date);
            var total = list.Sum(t => t.Leave);
            if (total < cost)
                throw new BizException("消费点数不足");

            //依次扣除点数 优先扣除即将到期的点卡
            list = list.OrderBy(t => t.Exipre).ThenBy(t => t.Leave).ToList();

            var temp = 0;
            List<CostListEntity> costList = new List<CostListEntity>();
            for (int i = 0; i < list.Count; i++)
            {
                var costEntity = list[i];
                //待扣点数
                var t1 = cost - temp;
                //实扣点数
                var t2 = costEntity.Leave >= t1 ? t1: costEntity.Leave;
                temp += t2;
                costEntity.Cost += t2;
                costEntity.Leave -= t2;

                var costListEntity = GetListEntity(taskId, costEntity.Id, t2);
                costList.Add(costListEntity);

                if (temp >= cost)
                    break;

            }

            // 更新数据
            foreach (var costEntity in list)
            {
                Update(costEntity);
            }
            foreach (var listEntity in costList)
            {
               repository.Insert(listEntity);
            }

            SaveChanges();
        }


        private CostListEntity GetListEntity(string taskId, string costId, int cost)
        {
            return new CostListEntity()
            {
                TaskId = taskId,
                CostId = costId,
                Cost = -cost,
                Content = "执行任务扣除" + cost+ "点数"
            };
        }


        /// <summary>
        /// 发放点卡
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="cost"></param>
        /// <param name="expire"></param>
        public void AddCost(string userId, string content, int cost, DateTime expire)
        {
            CostEntity costEntity = new CostEntity()
            {
                Id = IdGenerator.NextId().ToString(),
                UserId = userId,
                Total = cost,
                Leave = cost,
                Cost = 0,
                Content = content,
                Exipre = expire.ToDate()
            };

            CostListEntity listEntity = new CostListEntity()
            {
                CostId = costEntity.Id,
                Cost =  cost,
                Content = content
            };

            Insert(costEntity);
            repository.Insert(listEntity);  

            SaveChanges();
        }


        /// <summary>
        /// 自动发放点卡
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="cost"></param>
        /// <param name="expire"></param>
        public void AddCost(string userId, string content, int cost, DateTime expire , int state)
        {
            CostEntity costEntity = new CostEntity()
            {
                Id = IdGenerator.NextId().ToString(),
                UserId = userId,
                Total = cost,
                Leave = cost,
                Cost = 0,
                Content = content,
                Exipre = expire.ToDate(),
                State = state
            };

            CostListEntity listEntity = new CostListEntity()
            {
                CostId = costEntity.Id,
                Cost = cost,
                Content = content
            };

            Insert(costEntity);
            repository.Insert(listEntity);

            SaveChanges();
        }

        /// <summary>
        /// 任务失败返回扣点
        /// </summary>
        /// <param name="taskId"></param>
        public void ReturnCost(string taskId)
        { 
            var list = repository.Queryable(t=>t.TaskId == taskId).OrderByDescending(t=>t.CreateTime).ToList();
          
            foreach (var item in list)
            {
                //找到扣点卡
                var costEntity = Find(t => t.Id == item.CostId);
                //返回点数
                costEntity.Cost += item.Cost;
                costEntity.Leave -= item.Cost; 
                Update(costEntity);

                CostListEntity entity = new CostListEntity()
                {
                    TaskId = taskId,
                    CostId = item.CostId,
                    Cost = -item.Cost,
                    Content = "执行任务失败返回" + (-item.Cost) + "点数"
                };
                repository.Insert(entity);
            }

            SaveChanges(); 
        }


        public int GetCost()
        {
            var userId = AuthService.UserId;

            if (userId.IsEmpty())
                return 0;

            var date = DateTime.Now.ToDate();
            //获取点数
            List<CostEntity> list = List(t => t.UserId == userId && t.Exipre > date);
            var total = list.Sum(t => t.Leave);

            return total;
        }

        public List<CostListViewModel> GetCostList(Page page)
        {
            return repository.GetCostList(page);
        }
    }

    public interface ICostService : IBaseService<CostEntity>, IScopeDependency
    {
        /// <summary>
        /// 消费记账
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="cost"></param>
        void Cost(string taskId, int cost);

        /// <summary>
        /// 发放点卡
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="cost"></param>
        /// <param name="expire"></param>
        void AddCost(string userId, string content, int cost, DateTime expire);

        /// <summary>
        /// 自动发放点卡
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="content"></param>
        /// <param name="cost"></param>
        /// <param name="expire"></param>
        void AddCost(string userId, string content, int cost, DateTime expire, int state);

        /// <summary>
        /// 任务失败返回扣点
        /// </summary>
        /// <param name="taskId"></param>
        void ReturnCost(string taskId);

        int GetCost();

        List<CostListViewModel> GetCostList(Page page);
    }
}
