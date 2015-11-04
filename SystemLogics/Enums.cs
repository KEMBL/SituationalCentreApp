namespace SystemLogics
{
    /// <summary>
    /// Status of connection with GMS device
    /// </summary>
    public enum DeviceState
    {
        Disconnected,
        Connecting,
        Connected
    }
    /// <summary>
    /// Status of a program component
    /// </summary>
    public enum ComponentState
    {
        NoReady,
        Ready        
    }
}