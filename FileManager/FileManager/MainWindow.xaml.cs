using FileManager.Include;
using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
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
            _logger.Info($"File Manager (x64) v0.4.1 running. Time: {DateTime.Now}");
        }

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private string[] Drives = Environment.GetLogicalDrives();
        private string _firstFilePath = "", _secondFilePath = "";
        private bool isFirstWindowFile = false, isSecondWindowFile = false;
        private string _currentlyFirstSelectedItemName = "", _currentlySecondSelectedItemName = "";
        public void Reset()
        {
            string[] Drives = Environment.GetLogicalDrives();
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

            AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = false;
            AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = false;
            CopyButton.IsEnabled = CopyFiles.IsEnabled = false;
            SearchInFirstWindow.IsEnabled = SearchInSecondWindow.IsEnabled = false;

            SearchInFirstWindow.Text = SearchInSecondWindow.Text = "";

            _logger.Info("Function reset succes");
        }

        private void FirstBackButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Back Button click on first window");
            Function.goBack(isFirstWindowFile, FirstTextPath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Back Button click on second window");
            Function.goBack(isSecondWindowFile, SecondtTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Open notepad");
            Process OpenNotepad = Process.Start("notepad.exe");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Click on view drive button in first window");
            _firstFilePath = Function.ViewDirectoryAndFileOnWindow(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstDiskList, FirstTextPath, FirstFreeSpace, FirstFormatDrive, FirstTypeDrive);
            AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = SearchInFirstWindow.IsEnabled = true;
            if (SecondDiskList.SelectedIndex > -1)
                CopyButton.IsEnabled = CopyFiles.IsEnabled = true;
        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Click on view drive button in second window");
            _secondFilePath = Function.ViewDirectoryAndFileOnWindow(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondDiskList, SecondtTextPath, SecondFreeSpace, SecondFormatDrive, SecondTypeDrive);
            AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = SearchInSecondWindow.IsEnabled = true;
            if (FirstDiskList.SelectedIndex > -1)
                CopyButton.IsEnabled = CopyFiles.IsEnabled = true;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on reset button");
            Reset();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on update button");
            Function.UpdateInfoDrive(FirstDiskList,FirstFreeSpace);
            Function.UpdateInfoDrive(SecondDiskList, SecondFreeSpace);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void AddFolderInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add folder on first window button");
            Function.AddFolder(_firstFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void AddFolderInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add folder on second window button");
            Function.AddFolder(_secondFilePath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void RemoveButtonOnFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on remove folder on first window button");
            Function.Remove(_firstFilePath, FirstWindowOnFileManager, FirstTextPath, isFirstWindowFile);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void RemoveButtonOnSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on remove folder on second window button");
            Function.Remove(_secondFilePath, SecondWindowOnFileManager, SecondtTextPath, isSecondWindowFile);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void CopyFirstPathButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy path first window button");
            Function.CopyPath(_firstFilePath);
        }
        private void CopySecondPathButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy path second window button");
            Function.CopyPath(_secondFilePath);
        }

        private void CreateFileInFirstWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add file on first window button");
            Function.AddFile(_firstFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void CreateFileInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add file on second window button");
            Function.AddFile(_secondFilePath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void OpenInCMDFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on Open CMD button");
            Process.Start("cmd.exe");
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy button");
            Function.CopyFileAndDerictories(_firstFilePath, _secondFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
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
                    isFirstWindowFile = false;
                    FirstWindowOnFileManager.UnselectAll();
                    _logger.Info($"Open next folder on first window, path: {_firstFilePath}");
                }
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
                    isSecondWindowFile = false;
                    SecondWindowOnFileManager.UnselectAll();
                    _logger.Info($"Open next folder on second window, path: {_secondFilePath}");
                }
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
                        _logger.Info($"Following the entered path on first window succes");
                        _firstFilePath = Function.NextPath(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstTextPath, FirstWindowOnFileManager, FirstDiskList);
                        if (_firstFilePath != null)
                            AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = true;
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
                        _logger.Info($"Following the entered path on second window succes");
                        _secondFilePath = Function.NextPath(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondtTextPath, SecondWindowOnFileManager, SecondDiskList);
                        if (_secondFilePath != null)
                            AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error following the entered path on second window. Error message: {ex.Message}");
            }
        }

        private void EnterKeyDownSearchInFirstWindow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _logger.Info("Press enter for search in first window");
                Function.Search(_firstFilePath, SearchInFirstWindow, FirstWindowOnFileManager);
            }
        }
        private void EnterKeyDownSearchInSecondWindow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _logger.Info("Press enter for search in second window");
                Function.Search(_secondFilePath, SearchInSecondWindow, SecondWindowOnFileManager);
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info($"Click button help");
            Function.Help();
        }

        private void SearchInFirstWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on first search button");
            Function.Search(_firstFilePath, SearchInFirstWindow, FirstWindowOnFileManager);
        }

        private void SearchInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on second search button");
            Function.Search(_secondFilePath, SearchInSecondWindow, SecondWindowOnFileManager);
        }

        private void FirstNextButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on first next button");
            _firstFilePath = Function.NextPath(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstTextPath, FirstWindowOnFileManager, FirstDiskList);
            if (_firstFilePath != null)
                AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = true;
        }

        private void SedondNextButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on second next button");
            _secondFilePath = Function.NextPath(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondtTextPath, SecondWindowOnFileManager, SecondDiskList);
            if (_secondFilePath != null)
                AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = true;
        }

        private void CloseMainWindow(object sender, EventArgs e)
        {
            _logger.Info("Close main window and close all window");
            foreach (Window window in App.Current.Windows)
                window.Close();
        }

        private void CopyFiles_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy files button");
            Function.CopyFiles(_firstFilePath,_secondFilePath);
            _firstFilePath = Function.LoadUpdate(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFilePath = Function.LoadUpdate(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void FirstWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click to open Creation Menu on first window");
            Function.CreationMenuView(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }
        private void SecondWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click to open Creation Menu on second window");
            Function.CreationMenuView(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }
        private void ExitLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click on Label Exit");
            Application.Current.Shutdown();
        }
    }
}