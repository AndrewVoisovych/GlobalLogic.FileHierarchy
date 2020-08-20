using GlobalLogic.FileHierarchy.Backend;
using GlobalLogic.FileHierarchy.Backend.Utils;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;

namespace GlobalLogic.FileHierarchy.WpfCore
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }

        /// <summary>
        /// The path for the file to be opened from the button
        /// </summary>
        private string _jsonFilePath;

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
          
            FolderBrowserDialog folderDialog = new FolderBrowserDialog();
            if (folderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string selectedPath = folderDialog.SelectedPath ?? throw new ArgumentNullException("You have not selected a folder");
                    
                //Since reading a large number of files can lead to system freeze, asynchrony is added in this way.
                string jsonString = await Task.Run( ()=> HierarchyReader.Read(selectedPath));
                
                // Json  File button Activate
                _jsonFilePath = await Task.Run( ()=>  JsonFile.Create(jsonString, $"{Environment.CurrentDirectory}/json/", HierarchyReader.RootDirectoryName));
                if (!string.IsNullOrEmpty(_jsonFilePath))
                {
                    System.Windows.MessageBox.Show($"The read was successful {Environment.NewLine} Click on the Json icon to view the file","Success");

                    JsonButton.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/Resources/jsonIco.png", UriKind.Absolute)));
                    
                }
                   
                // Window settings
                PathLabel.Visibility = Visibility.Visible;
                PathLabel.Content = $"Select Path: {selectedPath}";

            }


           

        }

        private void JsonButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_jsonFilePath))
            {
                return;
            }

            try
            {
                Process.Start("notepad.exe", _jsonFilePath);
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show($"File opening error.{Environment.NewLine} " +
                    $"Try to find it along the way:" +
                    $"{_jsonFilePath} ", "Error");
                
            }
        }
    }
}
