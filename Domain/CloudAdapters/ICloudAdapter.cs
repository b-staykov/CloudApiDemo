using Domain.Events;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.CloudAdapters
{
    public interface ICloudAdapter
    {
        event EventHandler<TranslateTasksEventArgs> ActiveTasksNumberChanged;
        
        int MaxAllowedRunningMachines { get; }
        int RunningMachines { get;  }
        int QueuedTasks { get; }

        void RunMachines(int numberOfMachines);
        void ShutDownMachines(int numberOfMachines);
        void SendTranslateTasks(TranslateRequest tasks);
    }
}
