namespace NotificationSystem.Contracts.Worker
{
    public class WorkerConfigurations
    {
        public int Instances { get; set; }
        public string Name { get; set; }
        public int SleepTimeInSeconds { get; set; }
    }
}
