using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenAuth.ComfyUI.Service.ComfyUI.TaskQueue
{
    public static class TaskPriority
    {
        public const int High = 1;

        public const int Medium = 100;

        public const int Low = 1000;

        public const int Default = int.MaxValue;
    }
}
