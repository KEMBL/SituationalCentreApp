using System;
using System.IO;
using System.Threading;
using System.Windows;

namespace SituationalCentreApp
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        /// <summary>
        /// Is window opened?
        /// </summary>
        private bool _windowOpened;
        public delegate void UpdateTextCallback(string message);

        public LogsWindow()
        {            
            InitializeComponent();
            
            _windowOpened = true; //flag that window is currently open
            Closed += WindowClosed; //add callback to windows close

            //start watching for log file changes at background thread
            new Thread(() => WaitForLogFileChanges(Log.GetLogFileName())).Start();
        }
        /// <summary>
        /// Waiting for system log file changes and append text in window
        /// </summary>
        /// <param name="fileName"></param>
        private void WaitForLogFileChanges(string fileName)
        {
            // open log file
            using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // make stream reader
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (_windowOpened) //while log window is open
                    {
                        if (!sr.EndOfStream) //not end of the stream
                            //update text box in main thread
                            Logdata.Dispatcher.Invoke( new UpdateTextCallback(this.UpdateText), sr.ReadLine() );
                        else
                            Thread.Sleep(300); //waiting for changes
                    }
                }
            }
        }
        /// <summary>
        /// Update text in log window
        /// </summary>
        /// <param name="message"></param>
        private void UpdateText(string message)
        {
            Logdata.AppendText(message);
            Logdata.ScrollToEnd();
            Logdata.AppendText(System.Environment.NewLine);
        }
        /// <summary>
        /// Callback to new log lines
        /// </summary>
        /// <param name="line"></param>
        private void NewLogLine(string line)
        {
            Logdata.AppendText(line + Environment.NewLine);
        }
        /// <summary>
        /// Log window closed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowClosed(object sender, System.EventArgs e)
        {
            Closed -= WindowClosed;
            _windowOpened = false;
            Logdata.Document.Blocks.Clear(); //clear current content            
        }
    }
}
