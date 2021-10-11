using Domain.CloudAdapters;
using Domain.Events;
using Domain.Models;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Domain
{
    /*
     * <summary>
     * Autoscalling service
     * Helds the business logic for auto-scalling functionallity
     * </summary>
     */
    public class CloudScaleService : ICloudScaleService
    {
        private const int _tasksPerMachine = 100;
        private const int _maxMachines = 300;
        private const string _logfile = "service.log";

        private readonly object _syncRoot = new object();        
        private ICloudAdapter _adapter;        

        public CloudScaleService(ICloudAdapter adapter)
        {
            _adapter = adapter;
            _adapter.ActiveTasksNumberChanged += OnActiveTasksNumberChange;
        }

        public void SendTranslateTasks(TranslateRequest tasks)
        {
            LogToFile($"{tasks.NumberOfTasks} new tasks received.");

            _adapter.SendTranslateTasks(tasks);
        }

        private void OnActiveTasksNumberChange(object sender, TranslateTasksEventArgs e)
        {
            var currentMachines = _adapter.RunningMachines;

            var neededMachines = e.NumberOfTasks / _tasksPerMachine;

            if (e.NumberOfTasks % _tasksPerMachine > 0)
            {
                neededMachines += 1;
            }

            if (neededMachines == 0)
            {
                neededMachines = 1;
            }

            if (neededMachines > _maxMachines)
            {
                neededMachines = _maxMachines;
            }
            
            if (neededMachines > currentMachines)
            {
                _adapter.RunMachines(neededMachines - currentMachines);

                LogToFile($"Active tasks changed to {e.NumberOfTasks}. Current machines: {currentMachines}. Needed machines: {neededMachines}. Requested {neededMachines - currentMachines} new machines to run.");
            }

            if (neededMachines < currentMachines)
            {
                _adapter.ShutDownMachines(currentMachines - neededMachines);

                LogToFile($"Active tasks changed to {e.NumberOfTasks}. Current machines: {currentMachines}. Needed machines: {neededMachines}. Requesed {currentMachines - neededMachines} machines to stop.");
            }
        }

        private void LogToFile(string log)
        {
            lock(_syncRoot)
            {
                var text = $"{DateTime.Now}: {log}{Environment.NewLine}";
                File.AppendAllText(_logfile, text);

                Console.WriteLine(text);
            }            
        }
    }
}
