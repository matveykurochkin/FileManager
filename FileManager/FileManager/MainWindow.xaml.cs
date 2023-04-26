using FileManager.Include;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

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
            _logger.Info($"File Manager (x64) v0.7.2 running. Time: {DateTime.Now}");
            KeyDown += new KeyEventHandler(MainWindowKeyDown);

            _firstFolderPath = Function.ViewDirectoryAndFileOnWindow(isFirstWindowFile, Function.LoadDialogWindowInformation()[0], _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstDiskList, FirstTextPath, FirstFreeSpace, FirstFormatDrive, FirstTypeDrive);
            _secondFolderPath = Function.ViewDirectoryAndFileOnWindow(isSecondWindowFile, Function.LoadDialogWindowInformation()[1], _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondDiskList, SecondtTextPath, SecondFreeSpace, SecondFormatDrive, SecondTypeDrive);

            _firstFolderPath = Function.NextPath(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstTextPath, FirstWindowOnFileManager, FirstDiskList);
            _secondFolderPath = Function.NextPath(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondtTextPath, SecondWindowOnFileManager, SecondDiskList);
        }
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private string[] Drives = Environment.GetLogicalDrives();
        private string _firstFolderPath = "", _secondFolderPath = "";
        private bool isFirstWindowFile = false, isSecondWindowFile = false;
        private string _currentlyFirstSelectedItemName = "", _currentlySecondSelectedItemName = "";
        ProcessStartInfo processStartInfo;
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

            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);

            AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = OpenCMDInFirstWindow.IsEnabled = false;
            AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = OpenCMDInSecondWindow.IsEnabled = false;
            CopyButton.IsEnabled = CopyFiles.IsEnabled = false;
            SearchInFirstWindow.IsEnabled = SearchInSecondWindow.IsEnabled = false;

            SearchInFirstWindow.Text = SearchInSecondWindow.Text = "";

            _logger.Info("Function reset succes");
        }

        private void FirstBackButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Back Button click on first window");
            Function.goBack(isFirstWindowFile, FirstTextPath);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Back Button click on second window");
            Function.goBack(isSecondWindowFile, SecondtTextPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Open notepad");
            Process.Start("notepad.exe");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Click on view drive button in first window");
            _firstFolderPath = Function.ViewDirectoryAndFileOnWindow(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstDiskList, FirstTextPath, FirstFreeSpace, FirstFormatDrive, FirstTypeDrive);
            AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = SearchInFirstWindow.IsEnabled = OpenCMDInFirstWindow.IsEnabled = true;
            if (SecondDiskList.SelectedIndex > -1)
                CopyButton.IsEnabled = CopyFiles.IsEnabled = true;
        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _logger.Info("Click on view drive button in second window");
            _secondFolderPath = Function.ViewDirectoryAndFileOnWindow(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondDiskList, SecondtTextPath, SecondFreeSpace, SecondFormatDrive, SecondTypeDrive);
            AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = SearchInSecondWindow.IsEnabled = OpenCMDInSecondWindow.IsEnabled = true;
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
            Function.UpdateInfoDrive(FirstDiskList, FirstFreeSpace);
            Function.UpdateInfoDrive(SecondDiskList, SecondFreeSpace);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void AddFolderInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add folder on first window button");
            Function.AddFolder(_firstFolderPath);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void AddFolderInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add folder on second window button");
            Function.AddFolder(_secondFolderPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void RemoveButtonOnFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on remove folder on first window button");
            Function.Remove(_firstFolderPath, FirstWindowOnFileManager, FirstTextPath, isFirstWindowFile);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void RemoveButtonOnSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on remove folder on second window button");
            Function.Remove(_secondFolderPath, SecondWindowOnFileManager, SecondtTextPath, isSecondWindowFile);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void CopyFirstPathButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy path first window button");
            Function.CopyPath(_firstFolderPath);
        }
        private void CopySecondPathButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy path second window button");
            Function.CopyPath(_secondFolderPath);
        }

        private void CreateFileInFirstWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add file on first window button");
            Function.AddFile(_firstFolderPath);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }

        private void CreateFileInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on default add file on second window button");
            Function.AddFile(_secondFolderPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void OpenCMD_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on Open CMD button");
            string username = Environment.UserName;
            processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = $"C:\\Users\\{username}",
                UseShellExecute = false
            };
            Process.Start(processStartInfo);
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy button");
            Function.CopyFileAndDerictories(_firstFolderPath, _secondFolderPath);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void FirstWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            try
            {
                if (item != null && item.IsSelected)
                {
                    _currentlyFirstSelectedItemName = FirstWindowOnFileManager.SelectedItem.ToString();

                    FileAttributes fileAttr = File.GetAttributes(_firstFolderPath + "\\" + _currentlyFirstSelectedItemName);

                    if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        isFirstWindowFile = false;
                        FirstTextPath.Text += "\\" + _currentlyFirstSelectedItemName;
                        _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
                    }
                    else
                        isFirstWindowFile = true;
                    Function.LoadFilesAndDirectories(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
                    isFirstWindowFile = false;
                    FirstWindowOnFileManager.UnselectAll();
                    _logger.Info($"Open next folder on first window, path: {_firstFolderPath}");
                    Function.SaveDialogWindowInformation(_firstFolderPath, _secondFolderPath);
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

                    FileAttributes fileAttr = File.GetAttributes(_secondFolderPath + "\\" + _currentlySecondSelectedItemName);

                    if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        isSecondWindowFile = false;
                        SecondtTextPath.Text += "\\" + _currentlySecondSelectedItemName;
                        _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
                    }
                    else
                        isSecondWindowFile = true;
                    Function.LoadFilesAndDirectories(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
                    isSecondWindowFile = false;
                    SecondWindowOnFileManager.UnselectAll();
                    _logger.Info($"Open next folder on second window, path: {_secondFolderPath}");
                    Function.SaveDialogWindowInformation(_firstFolderPath, _secondFolderPath);
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
                        _firstFolderPath = Function.NextPath(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstTextPath, FirstWindowOnFileManager, FirstDiskList);
                        if (_firstFolderPath != null)
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
                        _secondFolderPath = Function.NextPath(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondtTextPath, SecondWindowOnFileManager, SecondDiskList);
                        if (_secondFolderPath != null)
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
                Function.Search(_firstFolderPath, SearchInFirstWindow, FirstWindowOnFileManager);
            }
        }
        private void EnterKeyDownSearchInSecondWindow(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _logger.Info("Press enter for search in second window");
                Function.Search(_secondFolderPath, SearchInSecondWindow, SecondWindowOnFileManager);
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
            Function.Search(_firstFolderPath, SearchInFirstWindow, FirstWindowOnFileManager);
        }

        private void SearchInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on second search button");
            Function.Search(_secondFolderPath, SearchInSecondWindow, SecondWindowOnFileManager);
        }

        private void FirstNextButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on first next button");
            _firstFolderPath = Function.NextPath(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstTextPath, FirstWindowOnFileManager, FirstDiskList);
            if (_firstFolderPath != null)
                AddFolderInFirstWindow.IsEnabled = CreateFileInFirstWindowButton.IsEnabled = RemoveButtonOnFirstWindow.IsEnabled = SearchInFirstWindowButton.IsEnabled = true;
        }

        private void SedondNextButton_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on second next button");
            _secondFolderPath = Function.NextPath(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondtTextPath, SecondWindowOnFileManager, SecondDiskList);
            if (_secondFolderPath != null)
                AddFolderInSecondWindow.IsEnabled = CreateFileInSecondWindowButton.IsEnabled = RemoveButtonOnSecondWindow.IsEnabled = SearchInSecondWindowButton.IsEnabled = true;
        }

        private void CloseMainWindow(object sender, EventArgs e)
        {
            _logger.Info("Close main window and close all window");
            Function.SaveDialogWindowInformation(_firstFolderPath, _secondFolderPath);
            foreach (Window window in Application.Current.Windows)
                window.Close();
        }

        private void CopyFiles_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on copy files button");
            Function.CopyFiles(_firstFolderPath, _secondFolderPath);
            _firstFolderPath = Function.LoadUpdate(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
            _secondFolderPath = Function.LoadUpdate(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }

        private void OpenCMDInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on Open CMD In First Window button");
            processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = $"{_firstFolderPath}",
                UseShellExecute = false
            };
            Process.Start(processStartInfo);
        }

        private void OpenCMDInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            _logger.Info("Click on Open CMD In Second Window button");
            processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                WorkingDirectory = $"{_secondFolderPath}",
                UseShellExecute = false
            };
            Process.Start(processStartInfo);
        }

        private Dictionary<Key, DispatcherTimer> _timers = new Dictionary<Key, DispatcherTimer>()
            {
                { Key.F3, null },
                { Key.F4, null },
                { Key.F5, null },
                { Key.F6, null },
                { Key.F7, null },
                { Key.F8, null },
                { Key.F9, null }
            };

        private void MainWindowKeyDown(object sender, KeyEventArgs e)
        {
            Dictionary<Key, Action> actions = new Dictionary<Key, Action>()
            {
                { Key.F3, () => RebootLabel_MouseLeftButtonDown(null, null) },
                { Key.F4, () => HelpButton_Click(null, null) },
                { Key.F5, () => UpdateButton_Click(null, null) },
                { Key.F6, () => ResetButton_Click(null, null) },
                { Key.F7, () => CopyButton_Click(null, null) },
                { Key.F8, () => OpenCMD_Click(null, null)},
                { Key.F9, () => NotepadButton_Click(null, null) }
            };

            if (actions.ContainsKey(e.Key))
            {
                _logger.Info($"Press button {e.Key}");
                if (_timers[e.Key] == null)
                {
                    var timer = new DispatcherTimer();
                    timer.Interval = TimeSpan.FromSeconds(1);
                    timer.Tick += (s, args) =>
                    {
                        timer.Stop();
                    };
                    _timers[e.Key] = timer;
                }
                if (!_timers[e.Key].IsEnabled)
                {
                    actions[e.Key].Invoke();
                    _timers[e.Key].Start();
                }
            }
        }

        private void MainWindowKeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (_timers[e.Key] != null && _timers[e.Key].IsEnabled)
                    _timers[e.Key].Stop();
            }
            catch (Exception ex)
            {
                _logger.Info($"This button is not a quick command. Message catch block: {ex.Message}");
            }
        }

        private void UpdateLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press update label");
            UpdateButton_Click(null, null);
        }

        private void ResetLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press reset label");
            ResetButton_Click(null, null);
        }

        private void CopyLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press copy label");
            CopyButton_Click(null, null);
        }

        private void CMDLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press open CMD label");
            OpenCMD_Click(null, null);
        }

        private void NotepadLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press open notepad label");
            NotepadButton_Click(null, null);
        }

        private void HelpLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info($"Press help label");
            HelpButton_Click(null, null);
        }

        private void RebootLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Restart app");
            string appName = Process.GetCurrentProcess().ProcessName + ".exe";
            Process.Start(appName);
            Process.GetCurrentProcess().Kill();
        }

        private void FirstWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click to open Creation Menu on first window");
            Function.CreationMenuView(isFirstWindowFile, _firstFolderPath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstTextPath);
        }
        private void SecondWindowOnFileManager_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click to open Creation Menu on second window");
            Function.CreationMenuView(isSecondWindowFile, _secondFolderPath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondtTextPath);
        }
        private void ExitLabel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _logger.Info("Click on Label Exit");
            Application.Current.Shutdown();
        }
    }
}