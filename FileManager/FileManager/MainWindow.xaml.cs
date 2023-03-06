using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        DirectoryInfo directory;

        private void LoadFilesAndDirectories(bool isFile, string filePath, string selectedItemName, ListView listView)
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = filePath + "\\" + selectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileList = new DirectoryInfo(filePath);
                    FileInfo[] fileInfo = fileList.GetFiles();
                    DirectoryInfo[] directoryInfo = fileList.GetDirectories();

                    listView.Items.Clear();

                    for (int i = 0; i < fileInfo.Length; i++)
                    {
                        listView.Items.Add(fileInfo[i]);
                    }
                    for (int i = 0; i < directoryInfo.Length; i++)
                    {
                        listView.Items.Add(directoryInfo[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void removeBackSlash(TextBox textBox)
        {
            string path = textBox.Text;
            if (path.LastIndexOf("\\") == path.Length - 1)
            {
                textBox.Text = path.Substring(0, path.Length - 1);
            }
        }
        public void goBack(bool isFile,TextBox textBox)
        {
            try
            {
                removeBackSlash(textBox);
                string path = textBox.Text;
                path = path.Substring(0, path.LastIndexOf("\\"));
                isFile = false;
                textBox.Text = path;
                removeBackSlash(textBox);
            }
            catch (Exception ex)
            {
               Console.WriteLine(ex.Message);
            }
        }


        private void FirstLoadUpdate()
        {
            _firstFilePath = FirstTextPath.Text;
            LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
            isFirstWindowFile = false;
        }
        private void SecondLoadUpdate()
        {
            _secondFilePath = SecondtTextPath.Text;
            LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
            isSecondWindowFile = false;
        }

        private void FirstBackButton_Click(object sender, RoutedEventArgs e)
        {
            goBack(isFirstWindowFile, FirstTextPath);
            FirstLoadUpdate();
        }
        private void SecondBackButton_Click(object sender, RoutedEventArgs e)
        {
            goBack(isSecondWindowFile, SecondtTextPath);
            SecondLoadUpdate();
        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            Process OpenNotepad = Process.Start("notepad.exe");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FirstWindowOnFileManager.Items.Clear();

            for (int i = 0; i < Drives.Length; i++)
            {
                if (FirstDiskList.SelectedItem == Drives[i])
                {
                    _firstFilePath = Convert.ToString(Drives[i]);
                    FirstTextPath.Text = _firstFilePath;
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            FirstFreeSpace.Content = $"{drive.TotalFreeSpace} b of {drive.TotalSize} b";
                            FirstFormatDrive.Content = $"{drive.DriveFormat}";
                            FirstTypeDrive.Content = $"{drive.VolumeLabel}";
                        }
                    }
                }
            }
            LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            FirstLoadUpdate();
            SecondLoadUpdate();
        }

        private void AddFolderInFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            string pathString = System.IO.Path.Combine(_firstFilePath, "New Folder");
            Directory.CreateDirectory(pathString);
            FirstLoadUpdate();
        }

        private void AddFolderInSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            string pathString = System.IO.Path.Combine(_secondFilePath, "New Folder");
            Directory.CreateDirectory(pathString);
            SecondLoadUpdate();
        }

        private void RemoveButtonOnFirstWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Внимание: программа удалит папку в которой вы находитесь и все ее содержимое!", "Warning", MessageBoxButton.YesNo,MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(_firstFilePath, true);
                FirstWindowOnFileManager.Items.Remove(FirstWindowOnFileManager.SelectedItem);
            }
            goBack(isFirstWindowFile, FirstTextPath);
            FirstLoadUpdate();
        }

        private void RemoveButtonOnSecondWindow_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Внимание: программа удалит папку в которой вы находитесь и все ее содержимое!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(_secondFilePath, true);
                SecondWindowOnFileManager.Items.Remove(SecondWindowOnFileManager.SelectedItem);
            }
            goBack(isSecondWindowFile, SecondtTextPath);
            SecondLoadUpdate();
        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SecondWindowOnFileManager.Items.Clear();

            for (int i = 0; i < Drives.Length; i++)
            {
                if (SecondDiskList.SelectedItem == Drives[i])
                {
                    _secondFilePath = Convert.ToString(Drives[i]);
                    SecondtTextPath.Text = _secondFilePath;
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            SecondFreeSpace.Content = $"{drive.TotalFreeSpace} b of {drive.TotalSize} b";
                            SecondFormatDrive.Content = $"{drive.DriveFormat}";
                            SecondTypeDrive.Content = $"{drive.VolumeLabel}";
                        }
                    }
                }
            }
            LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
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
                LoadFilesAndDirectories(isFirstWindowFile, _firstFilePath, _currentlyFirstSelectedItemName, FirstWindowOnFileManager);
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
                LoadFilesAndDirectories(isSecondWindowFile, _secondFilePath, _currentlySecondSelectedItemName, SecondWindowOnFileManager);
            }
        }
    }
}