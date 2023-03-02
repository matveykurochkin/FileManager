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

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NotepadButton_Click(object sender, RoutedEventArgs e)
        {
            Process OpenNotepad = Process.Start("notepad.exe");
        }

        private void FirstDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FirstWindowOnFileManager.Clear();
            for (int i = 0; i < Drives.Length; i++)
            {
                if (FirstDiskList.SelectedItem == Drives[i])
                {
                    _firstFilePath = Convert.ToString(Drives[i]);
                    FirstTextPath.Text = _firstFilePath;
                }
            }

             Directory.GetFiles(_firstFilePath)
                .ToList()
                .ForEach(f => FirstWindowOnFileManager.Text += $"{ System.IO.Path.GetFileName(f)}\n" );

        }

        private void SecondDiskList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SecondWindowOnFileManager.Clear();
            for (int i = 0; i < Drives.Length; i++)
            {
                if (SecondDiskList.SelectedItem == Drives[i])
                {
                    _secondFilePath = Convert.ToString(Drives[i]);
                    SecondtTextPath.Text = _secondFilePath;
                }
            }

            Directory.GetFiles(_secondFilePath)
                .ToList()
                .ForEach(f => SecondWindowOnFileManager.Text += $"{ System.IO.Path.GetFileName(f)}\n");
        }
    }
}