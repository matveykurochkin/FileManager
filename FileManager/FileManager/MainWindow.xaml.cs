using FileManager.Include;
using System;
using System.Diagnostics;
using System.IO;
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

        string[] Drives = Environment.GetLogicalDrives();
        private string _firstFilePath = "", _secondFilePath = "";
        private bool isFirstWindowFile = false, isSecondWindowFile = false;
        private string _currentlyFirstSelectedItemName = "", _currentlySecondSelectedItemName = "";

        private void FirstLoadUpdate()
        {
            _firstFilePath = FirstTextPath.Text;
            Function.LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
            isFirstWindowFile = false;
        }

        private void SecondLoadUpdate()
        {
            _secondFilePath = SecondtTextPath.Text;
            Function.LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
            isSecondWindowFile = false;
        }

        private void FirstBackButton_Click(object sender, RoutedEventArgs e)
        {
            Function.goBack(isFirstWindowFile, FirstTextPath);
            FirstLoadUpdate();
        }

        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            Function.goBack(isSecondWindowFile, SecondtTextPath);
            SecondLoadUpdate();
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            Process OpenNotepad = Process.Start("notepad.exe");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _firstFilePath = Function.ViewDirectoryAndFileOnWindow(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager, FirstDiskList, FirstTextPath, FirstFreeSpace, FirstFormatDrive, FirstTypeDrive);
        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _secondFilePath = Function.ViewDirectoryAndFileOnWindow(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager, SecondDiskList, SecondtTextPath, SecondFreeSpace, SecondFormatDrive, SecondTypeDrive);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FirstLoadUpdate();
            SecondLoadUpdate();
        }

        private void AddFolderInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFolder(_firstFilePath);
            FirstLoadUpdate();
        }

        private void AddFolderInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFolder(_secondFilePath);
            SecondLoadUpdate();
        }

        private void RemoveButtonOnFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.Remove(_firstFilePath, FirstWindowOnFileManager, FirstTextPath, isFirstWindowFile);
            FirstLoadUpdate();
        }

        private void RemoveButtonOnSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            Function.Remove(_secondFilePath, SecondWindowOnFileManager, SecondtTextPath, isSecondWindowFile);
            SecondLoadUpdate();
        }

        private void CopyFirstPathButton_Click(object sender, RoutedEventArgs e)
        {
            Function.CopyPath(_firstFilePath);
        }
        private void CopySecondPathButton_Click(object sender, RoutedEventArgs e)
        {
            Function.CopyPath(_secondFilePath);
        }

        private void CreateFileInFirstWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFile(_firstFilePath);
            FirstLoadUpdate();
        }

        private void CreateFileInSecondWindowButton_Click(object sender, RoutedEventArgs e)
        {
            Function.AddFile(_secondFilePath);
            SecondLoadUpdate();
        }

        private void OpenInCMDFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("cmd.exe");
        }

        private void FirstCopyButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void FirstWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                _currentlyFirstSelectedItemName = FirstWindowOnFileManager.SelectedItem.ToString();

                FileAttributes fileAttr = File.GetAttributes(_firstFilePath + "\\" + _currentlyFirstSelectedItemName);

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    isFirstWindowFile = false;
                    FirstTextPath.Text += "\\" + _currentlyFirstSelectedItemName;
                    FirstLoadUpdate();
                }
                else
                    isFirstWindowFile = true;
                Function.LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
            }
        }

        private void SecondWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                _currentlySecondSelectedItemName = SecondWindowOnFileManager.SelectedItem.ToString();

                FileAttributes fileAttr = File.GetAttributes(_secondFilePath + "\\" + _currentlySecondSelectedItemName);

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    isSecondWindowFile = false;
                    SecondtTextPath.Text += "\\" + _currentlySecondSelectedItemName;
                    SecondLoadUpdate();
                }
                else
                    isSecondWindowFile = true;
                Function.LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
            }
        }
    }
}