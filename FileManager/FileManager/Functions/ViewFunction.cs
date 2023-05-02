using NLog;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;

namespace FileManager.Include
{
    internal static class ViewFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();
        private static string[] Drives = Environment.GetLogicalDrives();

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

            LoadFunction.LoadFilesAndDirectories(isWindowFile, path, selectedItemName, listView);
            path = path.Trim(new[] { '\\' });
            textBox.Text = path;
            _logger.Info("Files and Directories view successfully");
            return path;
        }
    }
}
