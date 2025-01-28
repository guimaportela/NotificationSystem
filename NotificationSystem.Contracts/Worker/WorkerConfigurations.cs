using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotificationSystem.Contracts.Worker
{
    public class WorkerConfigurations
    {
        //public int BatchSize { get; set; }
        public int Instances { get; set; }
        public string Name { get; set; }
        //public int NoItemsSleepTimeInSeconds { get; set; }
        public int SleepTimeInSeconds { get; set; }
    }
}
