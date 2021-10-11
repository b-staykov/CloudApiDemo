using Domain.Events;
using Domain.Models;
using System;
using System.Threading.Tasks;
using System.Timers;

namespace Domain.CloudAdapters
{
    /*
     * <summary>
     * Simulates cloud work and communication logic
     * </summary>
     */
    public class DemoCloudAdapter : ICloudAdapter
    {
        private const int _maxAllowedRunningMachines = 300;

        private static readonly object _syncRoot = new object();
        
        private volatile int _runningMachines;
        private volatile int _queuedTasks;
        private static Timer aTimer;

        public event EventHandler<TranslateTasksEventArgs> ActiveTasksNumberChanged;
        
        public DemoCloudAdapter()
        {
            _runningMachines = 1;

            SetTimer();
        }

        public int QueuedTasks
        {
            get => _queuedTasks;
        }

        public int RunningMachines
        {
            get => _runningMachines;            
        }
        
        public int MaxAllowedRunningMachines => _maxAllowedRunningMachines;
                
        public void SendTranslateTasks(TranslateRequest tasks)
        {
            lock (_syncRoot)
            {
                _queuedTasks += tasks.NumberOfTasks;
            }

            OnActiveTasksNumberChanged(new TranslateTasksEventArgs(_queuedTasks));            
        }

        public void RunMachines(int numberOfMachines)
        {
            lock(_syncRoot)
            {
                if (_runningMachines + numberOfMachines > 300)
                {
                    _runningMachines = 300;
                }
                else
                {
                    _runningMachines += numberOfMachines;
                }
            }            
        }

        public void ShutDownMachines(int numberOfMachines)
        {
            lock(_syncRoot)
            {
                if (_runningMachines <= numberOfMachines)
                {
                    _runningMachines = 1;
                }
                else
                {
                    _runningMachines -= numberOfMachines;
                }
            }            
        }
                
        protected virtual void OnActiveTasksNumberChanged(TranslateTasksEventArgs e)
        {
            EventHandler<TranslateTasksEventArgs> handler = ActiveTasksNumberChanged;
            handler?.Invoke(this, e);
        }
        
        private void SetTimer()
        {            
            aTimer = new Timer(3000);            
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            lock(_syncRoot)
            {
                if (_queuedTasks > 0)
                {
                    var dequeue = _runningMachines * 100;

                    if (dequeue > _queuedTasks)
                    {
                        _queuedTasks = dequeue;
                    }

                    _queuedTasks -= dequeue;                    
                }
            }

            OnActiveTasksNumberChanged(new TranslateTasksEventArgs(_queuedTasks));
        }
    }
}
