﻿using FileManager.Include;
using System.Windows;

namespace FileManager
{
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
        }
        MainWindow mainWindow = new MainWindow();
        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                Function.AddFolder(Function.pathOnTCWindow, NameOfFileOrFolder.Text);
                Close();
            }
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text);
                Close();
            }
        }

        private void AddWordDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text,Function.MSWord);
                Close();
            }
        }

        private void CreatePPTFile_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                Function.AddFile(Function.pathOnTCWindow, NameOfFileOrFolder.Text, Function.MSPP);
                Close();
            }
        }
    }
}
