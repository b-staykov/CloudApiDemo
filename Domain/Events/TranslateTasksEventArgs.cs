using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events
{
    public class TranslateTasksEventArgs : EventArgs
    {
        public readonly int NumberOfTasks;

        public TranslateTasksEventArgs(int numberOfTasks)
        {
            NumberOfTasks = numberOfTasks;
        }
    }
}
