using FileManager.Include;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace FileManager
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            foreach (string disk in Drives)
            {
                FirstDiskList.Items.Add(disk);
                SecondDiskList.Items.Add(disk);
            }
        }

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        string[] Drives = Environment.GetLogicalDrives();
        public  string _firstFilePath = "", _secondFilePath = "";
        public  bool isFirstWindowFile = false, isSecondWindowFile = false;
        public  string _currentlyFirstSelectedItemName = "", _currentlySecondSelectedItemName = "";
        public void Reset()
        {
            FirstDiskList.Items.Clear();
            SecondDiskList.Items.Clear();
            foreach (string disk in Drives)
            {
                FirstDiskList.Items.Add(disk);
                SecondDiskList.Items.Add(disk);
            }
            FirstWindowOnFileManager.Items.Clear();
            SecondWindowOnFileManager.Items.Clear();
            FirstFreeSpace.Content = SecondFreeSpace.Content = "Space";
            FirstFormatDrive.Content = SecondFormatDrive.Content = "Format";
            FirstTypeDrive.Content = SecondTypeDrive.Content = "Name";
            FirstTextPath.Text = SecondtTextPath.Text = "";
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Function reset succes");
        }

        private void FirstBackButton_Click(object sender, RoutedEventArgs e)
        {
            Function.goBack(isFirstWindowFile, FirstTextPath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _logger.Info("Back Button click on first window");
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            Function.goBack(isSecondWindowFile, SecondtTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Back Button click on second window");
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            Process OpenNotepad = Process.Start("notepad.exe");
            _logger.Info("Open notepad");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _firstFilePath = Function.ViewDirectoryAndFileOnWindow(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstDiskList, FirstTextPath, FirstFreeSpace, FirstFormatDrive, FirstTypeDrive);
            _logger.Info("Click on view drive button in first window");
        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _secondFilePath = Function.ViewDirectoryAndFileOnWindow(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondDiskList, SecondtTextPath, SecondFreeSpace, SecondFormatDrive, SecondTypeDrive);
            _logger.Info("Click on view drive button in second window");
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            _logger.Info("Click on reset button");
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Click on update button");
        }

        private void AddFolderInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFolder(_firstFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _logger.Info("Click on default add folder on first window button");
        }

        private void AddFolderInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFolder(_secondFilePath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Click on default add folder on second window button");
        }

        private void RemoveButtonOnFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.Remove(_firstFilePath, FirstWindowOnFileManager, FirstTextPath, isFirstWindowFile);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _logger.Info("Click on remove folder on first window button");
        }

        private void RemoveButtonOnSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.Remove(_secondFilePath, SecondWindowOnFileManager, SecondtTextPath, isSecondWindowFile);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Click on remove folder on second window button");
        }

        private void CopyFirstPathButton_Click(object sender, RoutedEventArgs e)
        {
            Function.CopyPath(_firstFilePath);
            _logger.Info("Click on copy path first window button");
        }
        private void CopySecondPathButton_Click(object sender, RoutedEventArgs e)
        {
            Function.CopyPath(_secondFilePath);
            _logger.Info("Click on copy path second window button");
        }

        private void CreateFileInFirstWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFile(_firstFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _logger.Info("Click on default add file on first window button");
        }

        private void CreateFileInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFile(_secondFilePath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Click on default add file on second window button");
        }

        private void OpenInCMDFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe");
            _logger.Info("Click on Open CMD button");
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            Function.CopyFileAndDerictories(_firstFilePath, _secondFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
            _logger.Info("Click on copy button");
        }

        private void FirstWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            try
            {
                if (item != null && item.IsSelected)
                {
                    _currentlyFirstSelectedItemName = FirstWindowOnFileManager.SelectedItem.ToString();

                    FileAttributes fileAttr = File.GetAttributes(_firstFilePath + "\\" + _currentlyFirstSelectedItemName);

                    if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        isFirstWindowFile = false;
                        FirstTextPath.Text += "\\" + _currentlyFirstSelectedItemName;
                        _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
                    }
                    else
                        isFirstWindowFile = true;
                    Function.LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
                    FirstWindowOnFileManager.UnselectAll();
                }
                _logger.Info($"Open next folder on first window, path: {_firstFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Open next filder error. Error message: {ex.Message}");
            }
        }
        private void SecondWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            try
            {
                if (item != null && item.IsSelected)
                {
                    _currentlySecondSelectedItemName = SecondWindowOnFileManager.SelectedItem.ToString();

                    FileAttributes fileAttr = File.GetAttributes(_secondFilePath + "\\" + _currentlySecondSelectedItemName);

                    if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        isSecondWindowFile = false;
                        SecondtTextPath.Text += "\\" + _currentlySecondSelectedItemName;
                        _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
                    }
                    else
                        isSecondWindowFile = true;
                    Function.LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
                    SecondWindowOnFileManager.UnselectAll();
                }
                _logger.Info($"Open next folder on second window, path: {_secondFilePath}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Open next filder error. Error message: {ex.Message}");
            }
        }

        private void EnterKeyDownOnFirstPath(object sender, KeyEventArgs e)
        {
            try
            {
                if (FirstTextPath.Text != "")
                {
                    if (e.Key == Key.Enter)
                    {
                        _firstFilePath = FirstTextPath.Text;
                        Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
                        _logger.Info($"Following the entered path on first window succes");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error following the entered path on first window. Error message: {ex.Message}");
            }
        }

        private void EnterKeyDownOnSecondPath(object sender, KeyEventArgs e)
        {
            try
            {
                if (SecondtTextPath.Text != "")
                {
                    if (e.Key == Key.Enter)
                    {
                        _secondFilePath = SecondtTextPath.Text;
                        Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
                        _logger.Info($"Following the entered path on second window succes");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error following the entered path on second window. Error message: {ex.Message}");
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            string userName = Environment.UserName;

            string path = $"C:\\Users\\{userName}\\Downloads\\Help.txt";

            if (File.Exists($"{path}"))
                File.Delete(path);

            FileStream fileStream = File.Create($"{path}");
            byte[] info = new UTF8Encoding(true).GetBytes(Function.help);
            fileStream.Write(info, 0, info.Length);
            fileStream.Close();

            Process.Start("notepad.exe", path);
        }

        private void FirstWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Function.CreationMenuView(_firstFilePath, FirstTextPath);
            _logger.Info("Open Creation Menu on first window");
        }
        private void SecondWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            Function.CreationMenuView(_secondFilePath, SecondtTextPath);
            _logger.Info("Open Creation Menu on second window");
        }
    }
}