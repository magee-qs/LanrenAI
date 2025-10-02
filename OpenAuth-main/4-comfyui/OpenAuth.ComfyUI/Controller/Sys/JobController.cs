using Microsoft.AspNetCore.Mvc;
using OpenAuth.ComfyUI.Domain.Sys;
using OpenAuth.ComfyUI.Model.System;
using OpenAuth.ComfyUI.Service.Job;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Controller.Sys
{
    [ApiController]
    [Route("/[controller]/[action]")]
    public class JobController : ControllerBase
    {
        private IQuartzJobServcie jobServcie;
        public JobController(IQuartzJobServcie jobServcie) 
        {
            this.jobServcie = jobServcie;
        }

        [HttpPost]
        public AjaxResult Add([FromBody] JobModel job)
        {
            job.Validate();
            var entity = job.MapTo<JobEntity>();
            entity.State = 1;
            jobServcie.Insert(entity);
            jobServcie.SaveChanges();

            return AjaxResult.OK("");
        }

        [HttpPost]
        public AjaxResult StartAll()
        {
            jobServcie.StartAll();
            return AjaxResult.OK("");
        }


        [HttpPost]
        public AjaxResult Start([FromBody] string jobId)
        {
            var jobEntity = jobServcie.Find(jobId);
            jobServcie.Start(jobEntity);
            return AjaxResult.OK("");
        }

        [HttpPost]
        public AjaxResult Stop([FromBody] string jobId)
        {
            var jobEntity = jobServcie.Find(jobId);
            jobServcie.Stop(jobEntity);
            return AjaxResult.OK("");
        }

        [HttpPost]
        public AjaxResult ShutDown()
        {
            jobServcie.Shutdown();
            return AjaxResult.OK("");
        }
    }
}
