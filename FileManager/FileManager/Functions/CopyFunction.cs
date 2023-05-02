using NLog;
using System;
using System.IO;
using System.Windows;

namespace FileManager.Include
{
    internal static class CopyFunction
    {
        private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

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

    }
}
