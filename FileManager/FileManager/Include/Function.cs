﻿using NLog;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    public static class Function
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        static string[] Drives = Environment.GetLogicalDrives();
        public static void CopyPath(string path)
        {
            Clipboard.SetText($"{path}");
            _logger.Info("Command copy path to buffer success!");
        }

        public static string help = "\t\t\t\t\tРуководство пользователя Файлового менеджера. v0.1" +
            "\nСоздание папок и файлов: " +
            "\n\tДля создания папки по умолчанию требуется перейти в нужный каталог и нажать кнопку New Folder!" +
            "\n\tДля создания файла по умолчанию требуется перейти в нужный каталог и нажать кнопку New File!" +
            "\n\tДля создания папки или файла с собственным именем нажать ПКМ и в окне ввести название!" +
            "\nКопирование папок:" +
            "\n\tДля копирования папки необходимо в левом окне выбрать папку, необходимую для копирования, а в правом окне папку, в которую нужно скопировать данные, после нажать кнопку Copy!" +
            "\nУдаление папок:" +
            "\n\tДля удаления папок необходимо зайти в папку которую нужно удалить и нажать кнопку Remove, после чего папка и все ее содержимое безвозвратно удалится!" +
            "\nОбновление данных:" +
            "\n\tЕсли файлы были добавлены через другое приложение или с помощью Creation Menu, то следует нажать кнопку Update после любого из действий!" +
            "\nСброс всех окон и их данных:" +
            "\n\tЧтобы сбросить все окна до состояния по умолчанию, необходимо нажать кнопку Reset!" +
            "\nКопирование пути:" +
            "\n\tДля того, чтобы скопировать путь необходимо перейти в нужный каталог и нажать кнопку Copy в зависимости от того, в каком окне вы находитесь!" +
            "\nДля перехода по скопированному пути:" +
            "\n\tДля того, чтобы перейти по скопированному пути, нужно вставить путь в одно из полей, где располагается путь и нажать Enter!" +
            "\nОткрытие Блокнота:" +
            "\n\tДля того, чтобы откыть блокнот, необходимо нажать кнопку Notepad!" +
            "\nДля открытия CMD:" +
            "\n\tДля того, чтобы открыть CMD, необходимо нажать кнопку CMD!" +
            "\nФайловый менеджер предоставляет информацию о памяти текущего диска, его файловой системы и названия, также показывает скрытые папки!";

        public static string MSWord = "docx";
        public static string pathOnTCWindow = "";

        public static void removeBackSlash(TextBox textBox)
        {
            string path = textBox.Text;
            if (path.LastIndexOf("\\") == path.Length - 1)
                textBox.Text = path.Substring(0, path.Length - 1);
        }

        public static void goBack(bool isFile, TextBox textBox)
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

        public static void Remove(string path, ListView listView, TextBox textBox, bool isFile)
        {
            var result = MessageBox.Show("Внимание: программа удалит папку в которой вы находитесь и все ее содержимое!", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Stop);
            if (result == MessageBoxResult.Yes)
            {
                Directory.Delete(path, true);
                listView.Items.Remove(listView.SelectedItem);
                goBack(isFile, textBox);
                _logger.Info("Delete folder success");
            }
            else
                _logger.Info("Folder not deleted");
        }

        public static string ViewDirectoryAndFileOnWindow(bool isWindowFile, string path, string selectedItemName, ListView listView, ComboBox comboBox, TextBox textBox, Label freeSpace, Label formatDrive, Label typeDrive)
        {
            listView.Items.Clear();

            for (int i = 0; i < Drives.Length; i++)
            {
                if ((string)comboBox.SelectedItem == Drives[i])
                {

                    path = Convert.ToString(Drives[i]);

                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            freeSpace.Content = $"{drive.TotalFreeSpace} b of {drive.TotalSize} b";
                            formatDrive.Content = $"{drive.DriveFormat}";
                            typeDrive.Content = $"{drive.VolumeLabel}";
                        }
                    }
                }
            }

            LoadFilesAndDirectories(isWindowFile, path, selectedItemName, listView);
            path = path.Trim(new[] { '\\' });
            textBox.Text = path;
            _logger.Info("Files and Directories view successfully");
            return path;
        }
        public static void LoadFilesAndDirectories(bool isFile, string filePath, string selectedItemName, ListView listView)
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

                    for (int i = 0; i < directoryInfo.Length; i++)
                        listView.Items.Add(directoryInfo[i]);


                    for (int i = 0; i < fileInfo.Length; i++)
                        listView.Items.Add(fileInfo[i]);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Files and Directories not uploaded. Error message: {ex.Message}");
            }
            listView.Items.Refresh();
        }
        public static void AddFolder(string path)
        {
            try
            {
                string pathString = Path.Combine(path, "New Folder");
                if (Directory.Exists(pathString))
                {                  
                    var result = MessageBox.Show("Папка с таким названием уже существует, заменить ее?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        Directory.Delete(pathString,true);
                    else
                        return;
                }             
                Directory.CreateDirectory(pathString);
                _logger.Info("Folder create successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Folder create not successfully. Error message: {ex.Message}");
            }
        }

        public static void AddFolder(string path, string folderName)
        {
            try
            {
                string pathString = Path.Combine(path, folderName);
                if (Directory.Exists(pathString))
                {
                    var result = MessageBox.Show("Папка с таким названием уже существует, заменить ее?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        Directory.Delete(pathString,true);
                    else
                        return;
                }
                Directory.CreateDirectory(pathString);
                _logger.Info("Folder create successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Folder create not successfully. Error message: {ex.Message}");
            }
        }

        public static void AddFile(string path)
        {
            try
            {
                if (File.Exists($"{path}\\FileManager.txt"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\FileManager.txt");
                    else
                        return;
                }

                FileStream fileStream = File.Create($"{path}\\FileManager.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                _logger.Info("File create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

        public static void AddFile(string path, string fileName)
        {
            try
            {
                if (File.Exists($"{path}\\{fileName}.txt"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\{fileName}.txt");
                    else
                        return;
                }

                FileStream fileStream = File.Create($"{path}\\{fileName}.txt");
                byte[] info = new UTF8Encoding(true).GetBytes("This is some text in the file created by File Manager!");
                fileStream.Write(info, 0, info.Length);
                fileStream.Close();
                _logger.Info("File create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

        public static void AddFile(string path, string fileName, string fileType)
        {
            try
            {
                if (File.Exists($"{path}\\{fileName}.{fileType}"))
                {
                    var result = MessageBox.Show("Файл с таким названием уже существует, заменить его?",
                        "Информация",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Information);
                    if (result == MessageBoxResult.Yes)
                        File.Delete($"{path}\\{fileName}.{fileType}");
                    else
                        return;
                }

                FileStream fileStream = File.Create($"{path}\\{fileName}.{fileType}");
                _logger.Info("File create successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }
        public static void CreationMenuView(string path, TextBox textBox)
        {
            if (textBox.Text != "")
            {
                Create create = new Create();
                pathOnTCWindow = path;
                create.Show();
                _logger.Info("Creation Menu loaded");
            }
            _logger.Info("Creation Menu not loaded");
        }

        public static void CopyFileAndDerictories(string sourcePath, string targetPath)
        {
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                try
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("File and Directories copy not successfully");
                }
            }

            foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                try
                {
                    File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    _logger.Error("File and Directories copy not successfully");
                }
            }
            _logger.Info("File and Directories copy successfully");
        }

        public static string LoadUpdate(bool isFile, string path, string selectedItemName, ListView listView, TextBox textBox)
        {
            path = textBox.Text;
            LoadFilesAndDirectories(isFile, path, selectedItemName, listView);
            isFile = false;
            return path;
        }
    }
}