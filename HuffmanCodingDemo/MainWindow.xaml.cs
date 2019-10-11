﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HuffmanCodingCore;

using MessageBox = System.Windows.MessageBox;
using Path = System.IO.Path;

namespace HuffmanCodingDemo
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Compress_Button_Click(object sender, RoutedEventArgs e)
        {
            var encoding = new UTF8Encoding();
            try
            {
                using (var unCompressStream = new MemoryStream(encoding.GetBytes(textbox_uncompressed.Text)))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cw = new CompressStreamWriter(ms, encoding, true))
                        {
                            cw.Write(new StreamWrapper(unCompressStream));
                        }
                        ms.Seek(0, SeekOrigin.Begin);
                        textbox_compressed.Text = Convert.ToBase64String(ms.GetBuffer());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "哎呀，发生了一个错误~", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnCompress_Button_Click(object sender, RoutedEventArgs e)
        {
            var encoding = new UTF8Encoding();
            try
            {
                using (var unCompressStream = new MemoryStream())
                {
                    using (var compressStream = new MemoryStream(Convert.FromBase64String(textbox_compressed.Text)))
                    {
                        using (var cr = new CompressStreamReader(compressStream))
                        {
                            cr.Read(() => unCompressStream);
                        }
                    }
                    unCompressStream.Seek(0, SeekOrigin.Begin);
                    textbox_uncompressed.Text = encoding.GetString(unCompressStream.GetBuffer());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "哎呀，发生了一个错误~", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Compress_Files_Button_Click(object sender, RoutedEventArgs e)
        {
            // 选择要压缩的文件
            var fileOpenDialog = new System.Windows.Forms.OpenFileDialog
            {
                Multiselect = true,
                Title = "选择你要压缩的文件（可以多选）",
                CheckFileExists = true
            };
            fileOpenDialog.FileOk += (o2, e2) =>
            {
                Dictionary<string, FileStream> fileStreamWithName = new Dictionary<string, FileStream>();
                var filePaths = fileOpenDialog.FileNames;
                foreach (var filePath in filePaths)
                {
                    var fileName = Path.GetFileName(filePath);
                    var fileStream = File.OpenRead(filePath);
                    fileStreamWithName[fileName] = fileStream;
                }

                var fileSaveDialog = new SaveFileDialog()
                {
                    Title = "保存你的压缩文件到指定的目录",
                    AddExtension = true,
                    DefaultExt = "huf",
                    Filter = "HUF压缩文件|*.huf",
                    CreatePrompt = true,
                    CheckPathExists = true
                };

                fileSaveDialog.FileOk += (o3, e3) =>
                {
                    using (var outputCompressStream = fileSaveDialog.OpenFile())
                    {
                        using (var cw = new CompressStreamWriter(outputCompressStream))
                        {
                            cw.Write(new FileStreamWrapper(fileStreamWithName));
                        }
                    }
                    MessageBox.Show("压缩成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                };
                fileSaveDialog.ShowDialog();
            };

            fileOpenDialog.ShowDialog(); 
        }

        private void UnCompress_Files_Button_Click(object sender, RoutedEventArgs e)
        {
            // 选择要压缩的文件
            var fileOpenDialog = new OpenFileDialog
            {
                Multiselect = false,
                Filter = "HUF压缩文件|*.huf",
                Title = "选择你要解压缩的文件",
                CheckFileExists = true
            };

            fileOpenDialog.FileOk += (o2, e2) =>
            {
                var floderDialog = new FolderBrowserDialog() {
                    Description="选择你要解压到的文件夹"
                };

                if (floderDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var cr = new CompressStreamReader(((OpenFileDialog)o2).OpenFile()))
                    {
                        cr.Read(null, s => File.Create(Path.Combine(floderDialog.SelectedPath, s)),null,true);
                    }
                    MessageBox.Show("解压成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            };

            fileOpenDialog.ShowDialog();
        }
    }
}
