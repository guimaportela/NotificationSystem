namespace NotificationSystem.Exceptions
{
    public class GatewayInternalException : Exception
    {
        public readonly bool wasRetried;

        public GatewayInternalException(bool _wasRetried = default)
        {
            wasRetried = _wasRetried;
        }
    }
}
