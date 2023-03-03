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

        private string _firstFilePath = "";
        private string _secondFilePath = "";
        private bool isFile = false;
        private string _currentlySelectedItemName = "";

        private void LoadFilesAndDirectories()
        {
            DirectoryInfo fileList;
            string tempFilePath = "";
            FileAttributes fileAttr;
            try
            {
                if (isFile)
                {
                    tempFilePath = _firstFilePath + "/" + _currentlySelectedItemName;
                    FileInfo fileDetails = new FileInfo(tempFilePath);
                    fileAttr = File.GetAttributes(tempFilePath);
                    Process.Start(tempFilePath);
                }
                else
                {
                    fileList = new DirectoryInfo(_firstFilePath);
                    FileInfo[] fileInfo = fileList.GetFiles();
                    DirectoryInfo[] directoryInfo = fileList.GetDirectories();

                    FirstWindowOnFileManager.Items.Clear();

                    for (int i = 0; i < fileInfo.Length; i++)
                    {
                        FirstWindowOnFileManager.Items.Add(fileInfo[i]);
                    }
                    for (int i = 0; i < directoryInfo.Length; i++)
                    {
                        FirstWindowOnFileManager.Items.Add(directoryInfo[i]);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private void LoadButton()
        {
            _firstFilePath = FirstTextPath.Text;
            LoadFilesAndDirectories();
            isFile = false;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            LoadFilesAndDirectories();
        }
        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            LoadButton();
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
                }
            }
            LoadFilesAndDirectories();

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
                }
            }

            DirectoryInfo fileList = new DirectoryInfo(_secondFilePath);
            DirectoryInfo[] dirs = fileList.GetDirectories();

            for (int i = 0; i < dirs.Length; i++)
            {
                SecondWindowOnFileManager.Items.Add(dirs[i].Name);
            }
        }

        private void FirstWindowOnFileManager_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;

            if (item != null && item.IsSelected)
            {
                _currentlySelectedItemName = FirstWindowOnFileManager.SelectedItem.ToString();

                FileAttributes fileAttr = File.GetAttributes(_firstFilePath + "/" + _currentlySelectedItemName);

                if ((fileAttr & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    isFile = false;
                    FirstTextPath.Text += "/" + _currentlySelectedItemName;        
                    LoadButton();
                }
                else
                    isFile = true;
                LoadFilesAndDirectories();
            }
        }
    }
}