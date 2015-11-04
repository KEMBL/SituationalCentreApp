using log4net;
using log4net.Config;
using System;
using System.Data;
using System.IO;
using log4net.Appender;

/// <summary>
/// Perform log actions
/// </summary>
public static class Log
{    
    /// <summary>
    /// New log appear
    /// </summary>
    static private event Action<string> _newLogArrived; 
    /// <summary>
    /// Logger reference
    /// </summary>
    static private readonly ILog _logger; 
    /// <summary>
    /// Static constructor
    /// </summary>
    static Log()
    {
        //workaround to make a destructor kind procedure for static class
        AppDomain.CurrentDomain.ProcessExit += StaticClass_Destructor;

        // create ref to Logger
        //to make possible on fly log configuration changes uncomment this and move log4net configuration into log4net.config from app.conf
//        BinaryPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
//        FileInfo configFileInfo = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
//        XmlConfigurator.ConfigureAndWatch(configFileInfo);
        _logger = LogManager.GetLogger(typeof (Log));
        
        // This method initializes the log4net system to use a simple Console appender.
        //BasicConfigurator.Configure();
        DOMConfigurator.Configure();
#if DEBUG
        Debug("Log system starting!");        
#endif
    }


    static public string GetLogFileName()
    {
        String filename = null;

        IAppender[] appenders = _logger.Logger.Repository.GetAppenders();
        // Check each appender this logger has
        foreach (IAppender appender in appenders)
        {
            Type t = appender.GetType();
            // Get the file name from the first FileAppender found and return
            if (t.Equals(typeof(FileAppender)) || t.Equals(typeof(RollingFileAppender)))
            {
                filename = ((FileAppender)appender).File;
                break;
            }
        }
        return filename;
    }

    /// <summary>
    /// Log Debug message
    /// </summary>
    /// <param name="message"></param>
    public static void Debug(string message)
    {
        _logger.Debug(message);
        PostProcess("DEBUG " + message);
    }
    /// <summary>
    /// Log Info message
    /// </summary>
    /// <param name="message"></param>
    public static void Info(string message)
    {
        _logger.Info(message);
        PostProcess("INFO " + message);
    }
    /// <summary>
    /// Log Warn message
    /// </summary>
    /// <param name="message"></param>
    public static void Warn(string message)
    {
        _logger.Warn(message);
        PostProcess("WARN " + message);
    }
    /// <summary>
    /// Log Error message
    /// </summary>
    /// <param name="message"></param>
    public static void Error(string message)
    {        
        _logger.Error(message);
        PostProcess("ERROR " + message);
    }
    /// <summary>
    /// Log Fatal message
    /// </summary>
    /// <param name="message"></param>
    public static void Fatal(string message)
    {
        _logger.Fatal(message);
        PostProcess("FATAL " + message);
    }
    /// <summary>
    /// MAke some actions with log message
    /// </summary>
    /// <param name="message"></param>
    public static void PostProcess(string message)
    {
#if DEBUG
        Console.WriteLine(DateTime.Now + " " + message);
#endif
        if (_newLogArrived != null)
        {
            _newLogArrived(DateTime.Now + " " + message);
        }
    }
    /// <summary>
    /// Subscribe someone to new log lones
    /// </summary>
    /// <param name="_newLogCallback"></param>
    static public void SubscribeToLogUpdates(Action<string> _newLogCallback)
    {
        _newLogArrived += _newLogCallback;
    }
    /// <summary>
    /// Destructor
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    static void StaticClass_Destructor(object sender, EventArgs e)
    {
        // clean it up
        // clear event subscriptions
        _newLogArrived = null;
    }
}
