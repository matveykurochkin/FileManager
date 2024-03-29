﻿using FileManager.Include;
using NLog;
using System;
using System.Windows;
using System.Windows.Input;

namespace FileManager
{
    public partial class Create : Window
    {
        public Create()
        {
            InitializeComponent();
            _logger.Info($"Creation Menu v0.8 running. Time: {DateTime.Now}");
        }

        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

        private void AddFolderButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add folder on Creation menu, process success");
                CreateFunction.AddFolder(CreateFunction.pathOnTCWindow, NameOfFileOrFolder.Text);
                LoadFunction.LoadInfoDirectory(CreateFunction.pathOnTCWindow, CreateFunction.labelOnTC);
                Close();
            }
            else
                _logger.Error("Click button add folder on Creation menu, process not success. Error: folder name not specified");
        }

        private void AddFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add file on Creation menu, process success");
                CreateFunction.AddFile(CreateFunction.pathOnTCWindow, NameOfFileOrFolder.Text);
                LoadFunction.LoadInfoDirectory(CreateFunction.pathOnTCWindow, CreateFunction.labelOnTC);
                Close();
            }
            else
                _logger.Error("Click button add file on Creation menu, process not success. Error: file name not specified");
        }

        private void AddWordDocumentButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add MS Word document on Creation menu, process success");
                CreateFunction.AddFile(CreateFunction.pathOnTCWindow, NameOfFileOrFolder.Text, CreateFunction.MSWord);
                LoadFunction.LoadInfoDirectory(CreateFunction.pathOnTCWindow, CreateFunction.labelOnTC);
                Close();
            }
            else
                _logger.Error("Click button add MS Word document on Creation menu, process not success. Error: MS Word document name not specified");
        }

        private void CreatePPTFile_Click(object sender, RoutedEventArgs e)
        {
            if (NameOfFileOrFolder.Text != "")
            {
                _logger.Info("Click button add MS Power Point document on Creation menu, process success");
                CreateFunction.AddFile(CreateFunction.pathOnTCWindow, NameOfFileOrFolder.Text, CreateFunction.MSPP);
                LoadFunction.LoadInfoDirectory(CreateFunction.pathOnTCWindow, CreateFunction.labelOnTC);
                Close();
            }
            else
                _logger.Error("Click button add MS Power Point document on Creation menu, process not success. Error: MS Power Point document name not specified");
        }

        private void EnterKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                _logger.Info("Press enter on input panel on creation menu");
                AddFolderButton_Click(null, null);
            }
        }
    }
}