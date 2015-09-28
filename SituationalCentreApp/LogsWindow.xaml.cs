using System;
using System.Windows;
using System.Windows.Media;

namespace SituationalCentreApp
{
    /// <summary>
    /// Interaction logic for Logs.xaml
    /// </summary>
    public partial class LogsWindow : Window
    {
        public LogsWindow()
        {
            InitializeComponent();
            // add callback to new log lines
            Log.SubscribeToLogUpdates(NewLogLine);

            //Logdata.FontFamily = new FontFamily();
            //dynamicRichTextBox.ScrollBars = RichTextBoxScrollBars.Both;
            
//Logdata.FontFamily = new
  //          FontFamily("Georgia");
            //Logdata.FontSize = 10.67f;

            Log.Debug("--T1--");
            Log.Debug("--T2--");
        }

        /// <summary>
        /// Callback to new log lines
        /// </summary>
        /// <param name="line"></param>
        private void NewLogLine(string line)
        {
            Logdata.AppendText(line + Environment.NewLine);
        }
    }
}
