using OpenAuth.ComfyUI.Domain;
using OpenAuth.ComfyUI.Domain.ComfyUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Repository.ComfyUI
{
    public class TaskFileRepository : BaseRepository<TaskFileEntity>, ITaskFileRepository
    {
        public TaskFileRepository(OpenAuthDbContext context, IAuthService authService) 
            : base(context, authService)
        {
        }
    }

    public interface ITaskFileRepository : IBaseRepository<TaskFileEntity>, IScopeDependency
    { }
}
