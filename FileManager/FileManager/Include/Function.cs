﻿using NLog;
using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace FileManager.Include
{
    public static class Function
    {      
        public static string help = $"\t\t\t\t\tРуководство пользователя Файлового менеджера. v0.7" +
            "\n\tВсе кнопки на интерфейсе заменены соответствующими значками, при наведении на кнопку, будет написано ее название." +
            "\nСоздание папок и файлов: " +
            "\n\tДля создания папки по умолчанию требуется перейти в нужный каталог и нажать кнопку New Folder!" +
            "\n\tДля создания файла по умолчанию требуется перейти в нужный каталог и нажать кнопку New File!" +
            "\n\tДля создания папки или файла с собственным именем нажать ПКМ и в окне ввести название!" +
            "\nКопирование папок:" +
            "\n\tДля копирования папки необходимо в левом окне выбрать папку, необходимую для копирования, а в правом окне папку, в которую нужно скопировать данные, после нажать кнопку Copy!" +
            "\nУдаление папок:" +
            "\n\tДля копирования только файлов:" +
            "\nЧтобы скопировать только файлы из текущей папки в папку, необходимую для копирования, нужно нажать на кнопку Copy Files! (в левом окне папка откуда копировать, в правом куда)" +
            "\n\tДля удаления папок необходимо зайти в папку которую нужно удалить и нажать кнопку Remove, после чего папка и все ее содержимое безвозвратно удалится!" +
            "\nОбновление данных:" +
            "\n\tЕсли файлы были добавлены через другое приложение, то следует нажать кнопку Update!" +
            "\nСброс всех окон и их данных:" +
            "\n\tЧтобы сбросить все окна до состояния по умолчанию, необходимо нажать кнопку Reset!" +
            "\nКопирование пути:" +
            "\n\tДля того, чтобы скопировать путь необходимо перейти в нужный каталог и нажать кнопку Copy в зависимости от того, в каком окне вы находитесь!" +
            "\nДля перехода по скопированному пути:" +
            "\n\tДля того, чтобы перейти по скопированному пути, нужно вставить путь в одно из полей, где располагается путь и нажать Enter или на кнопку Next!" +
            "\nОткрытие Блокнота:" +
            "\n\tДля того, чтобы откыть блокнот, необходимо нажать кнопку Notepad!" +
            "\nДля открытия CMD:" +
            "\n\tДля того, чтобы открыть CMD, необходимо нажать кнопку CMD!" +
            "\nФайловый менеджер предоставляет информацию о памяти текущего диска, его файловой системы и названия, также показывает скрытые папки!" +
            "\n\tДля поиска файлов или директорий:" +
            "\nЧтобы найти нужный файл или папку, необходимо выбрать папку в которой нужно искать этот файл или папку (нельзя искать в корне диска, потому что" +
            "\nтам могут находится папки к которым у приложения нет доступа, что приведет к отрицательным результатам поиска), поиск требует точного названия файла (с расширением) " +
            "\nили папки, к регистру не чувствителен!";

        public static string MSWord = "docx", MSPP = "ppt";
        public static string pathOnTCWindow = "", selectedonTCItemName = "";
        public static bool isFileTCwindow;
        static ListView listViewOnTC = new ListView();
        static TextBox textBoxOnTC = new TextBox();
        static Thread thread;
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        static string[] Drives = Environment.GetLogicalDrives();
        private static string projectPath = AppDomain.CurrentDomain.BaseDirectory;

        public static void CopyPath(string path)
        {
            try
            {
                Clipboard.SetText($"{path}");
                _logger.Info("Command copy path to buffer success!");
            }
            catch (Exception ex)
            {
                _logger.Error($"Command copy path to buffer not success. Error message: {ex.Message}");
            }
        }
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
                _logger.Info($"Go back button click success. Path: {path}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Go back button click error. Error message: {ex.Message}");
            }
        }

        public static void Remove(string path, ListView listView, TextBox textBox, bool isFile)
        {
            try
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
            }catch (Exception ex)
            {
                _logger.Error($"Error remove. Error message: {ex.Message}");
            }
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
                            long freeSpaceOnDrive = drive.TotalFreeSpace / 1024, totalSizeOnDrive = drive.TotalSize / 1024;
                            freeSpace.Content = $"{freeSpaceOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k of {totalSizeOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k free";
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
                        Directory.Delete(pathString, true);
                    else
                        return;
                }
                Directory.CreateDirectory(pathString);
                _logger.Info($"Folder create successfully. Folder name: New Folder. Folder path: {pathString}");
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
                        Directory.Delete(pathString, true);
                    else
                        return;
                }
                Directory.CreateDirectory(pathString);
                LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"Folder create successfully. Folder name: {folderName}. Folder path: {pathString}");
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
                _logger.Info($"File create successfully. File name: FileManager.txt. File path: {path}");
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
                LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"File create successfully. File name: {fileName}.txt. File path: {path}");
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
                fileStream.Close();
                LoadUpdate(isFileTCwindow, pathOnTCWindow, selectedonTCItemName, listViewOnTC, textBoxOnTC);
                _logger.Info($"File create successfully. File name: {fileName}.{fileType}. File path: {path}");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.Error("File create not successfully");
            }
        }

        public static void CreationMenuView(bool isFile, string path, string selectedItemName, ListView listView, TextBox textBox)
        {
            bool isWindowOpen = false;

            if (textBox.Text != "")
            {

                foreach (Window window in Application.Current.Windows)
                {
                    if (window is Create)
                    {
                        isWindowOpen = true;
                        window.Activate();
                    }
                }

                if (!isWindowOpen)
                {
                    Create create = new Create();
                    pathOnTCWindow = path;
                    selectedonTCItemName = selectedItemName;
                    isFileTCwindow = isFile;
                    listViewOnTC = listView;
                    textBoxOnTC = textBox;
                    create.Show();
                    _logger.Info("Creation Menu loaded");
                }
            }
            else
                _logger.Error("Creation Menu not loaded");
        }
        public static void CopyFileAndDerictories(string sourcePath, string targetPath)
        {
            try
            {
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    try
                    {
                        Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"File and Directories copy not successfully. Error message: {ex.Message}");
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
                        _logger.Error($"File and Directories copy not successfully. Error message: {ex.Message}");
                    }
                }
                _logger.Info("File and Directories copy successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"File and Directories copy error. Error message: {ex.Message}");
            }
        }

        public static void CopyFiles(string sourcePath, string targetPath)
        {
            try
            {
                foreach (string newPath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                {
                    try
                    {
                        File.Copy(newPath, newPath.Replace(sourcePath, targetPath), true);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Files copy not successfully. Error message: {ex.Message}");
                    }
                }
                _logger.Info("Files copy successfully");
            }
            catch (Exception ex)
            {
                _logger.Error($"Files copy error. Error message: {ex.Message}");
            }
        }
        public static string LoadUpdate(bool isFile, string path, string selectedItemName, ListView listView, TextBox textBox)
        {
            try
            {

                path = textBox.Text;
                LoadFilesAndDirectories(isFile, path, selectedItemName, listView);
                isFile = false;
                _logger.Info("Load and Update successfully");
                return path;
            }
            catch (Exception ex)
            {
                _logger.Info($"Load and Update not successfully. Error message: {ex.Message}");
            }
            return null;
        }
        public static void Search(string path, TextBox textBox, ListView listView)
        {
            if (textBox.Text != "")
            {
                try
                {
                    listView.Items.Clear();

                    foreach (var searchFile in Directory.GetFiles(path, textBox.Text, SearchOption.AllDirectories))
                    {
                        FileInfo fileInfo = new FileInfo(searchFile);
                        listView.Items.Add(fileInfo);
                    }

                    foreach (var searchDirectory in Directory.GetDirectories(path, textBox.Text, SearchOption.AllDirectories))
                    {
                        DirectoryInfo directoryInfo = new DirectoryInfo(searchDirectory);
                        listView.Items.Add(directoryInfo);
                    }

                    _logger.Info($"Search complited");
                }
                catch (Exception ex)
                {
                    _logger.Error($"Search not complited. Error message: {ex.Message}");
                }
            }
        }

        public static string NextPath(bool isFile, string path, string selectedItemName, TextBox text, ListView listView, ComboBox comboBox)
        {
            try
            {
                if (text.Text != "")
                {
                    _logger.Info($"Following the next button click path on {listView.Name} window succes");
                    path = text.Text;

                    string copyPath = path;
                    TextBox textBox = new TextBox();
                    textBox.Text = copyPath;

                    string driveName = path;
                    driveName = driveName.Remove(3, driveName.Length - 3);

                    for (int i = 0; i < comboBox.Items.Count; i++)
                        if (comboBox.Items[i].ToString() == driveName)
                            comboBox.SelectedItem = comboBox.Items[i];

                    LoadUpdate(isFile, copyPath, selectedItemName, listView, textBox);
                    text.Text = copyPath;
                    path = copyPath;
                    return path;
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Error following the next button click path on {listView.Name} window. Error message: {ex.Message}");
            }
            return null;
        }
        public static void Help()
        {

            try
            {
                thread = new Thread(() =>
                {
                    string userName = Environment.UserName;

                    string path = $"C:\\Users\\{userName}\\Downloads\\Help.txt";
                    _logger.Info($"Help file path: {path}");

                    if (File.Exists($"{path}"))
                        File.Delete(path);

                    FileStream fileStream = File.Create($"{path}");
                    byte[] info = new UTF8Encoding(true).GetBytes(help);
                    fileStream.Write(info, 0, info.Length);
                    fileStream.Close();

                    Process.Start("notepad.exe", path);
                });
                thread.Start();
            }
            catch (Exception ex)
            {
                _logger.Error($"Help file not created. Error message: {ex.Message}");
            }
        }

        public static void UpdateInfoDrive(ComboBox comboBox, Label freeSpace)
        {
            for (int i = 0; i < Drives.Length; i++)
            {
                if ((string)comboBox.SelectedItem == Drives[i])
                {
                    foreach (var drive in DriveInfo.GetDrives())
                    {
                        if (Drives[i] == drive.Name)
                        {
                            long freeSpaceOnDrive = drive.TotalFreeSpace / 1024, totalSizeOnDrive = drive.TotalSize / 1024;
                            freeSpace.Content = $"{freeSpaceOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k of {totalSizeOnDrive.ToString("#,#", new CultureInfo("ru-RU"))} k free";
                        }
                    }
                }
            }
        }

        public static string[] LoadDialogWindowInformation()
        {
            string path = $"{projectPath}\\load.txt";
            string userName = Environment.UserName;
            if (!File.Exists(path))
            {
                using (FileStream fileStream = File.Create(path))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes($"C:\\Users\\{userName}\nC:\\Users\\{userName}");
                    fileStream.Write(info, 0, info.Length);
                }
            }
            string[] lines = File.ReadAllLines(path);
            _logger.Info($"Successfully load paths of all windows");
            return lines;
        }

        public static void SaveDialogWindowInformation(params string[] paths)
        {
            string path = Path.Combine(projectPath, "load.txt");
            StringBuilder sb = new StringBuilder();
            foreach (string p in paths)
            {
                sb.AppendLine(p);
            }
            File.WriteAllText(path, sb.ToString(), Encoding.UTF8);
            _logger.Info("Successfully saved paths of all windows");
        }
    }
}