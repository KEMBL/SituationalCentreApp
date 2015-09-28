using BusinessLogic;
using System.Windows;

namespace SituationalCentreApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // Start application            
            SituationalCentre sCentre = SituationalCentre.Initialization();
        }
        /// <summary>
        /// Menu Item Preferences is clicked!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PreferencesClicked(object sender, RoutedEventArgs e)
        {
            PreferencesWindow pWindow = new PreferencesWindow();
            pWindow.ShowDialog();// show modal window
        }
        /// <summary>
        /// Menu Item Logs is clicked!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LogsClicked(object sender, RoutedEventArgs e)
        {
            LogsWindow lWindow = new LogsWindow();
            lWindow.Show();//Dialog();// show window
        }
    }
}
