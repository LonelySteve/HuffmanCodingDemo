using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using HuffmanCodingCore;
using MessageBox = System.Windows.MessageBox;

namespace HuffmanCodingDemo
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
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
                using (var unCompressStream = new MemoryStream(encoding.GetBytes(TextboxUncompressed.Text)))
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var cw = new CompressStreamWriter(ms, encoding, true))
                        {
                            cw.Write(unCompressStream);
                        }

                        ms.Seek(0, SeekOrigin.Begin);
                        TextboxCompressed.Text = Convert.ToBase64String(ms.GetBuffer());
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
                    using (var compressStream = new MemoryStream(Convert.FromBase64String(TextboxCompressed.Text)))
                    {
                        using (var cr = new CompressStreamReader(compressStream))
                        {
                            cr.Read(() => unCompressStream);
                        }
                    }

                    unCompressStream.Seek(0, SeekOrigin.Begin);
                    TextboxUncompressed.Text = encoding.GetString(unCompressStream.GetBuffer());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "哎呀，发生了一个错误~", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Compress_Files_Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // 选择要压缩的文件
                var fileOpenDialog = new OpenFileDialog
                {
                    Multiselect = true,
                    Title = "选择你要压缩的文件（可以多选）",
                    CheckFileExists = true
                };
                fileOpenDialog.FileOk += (o2, e2) =>
                {
                    var fileStreamWithName = new Dictionary<string, FileStream>();
                    var filePaths = fileOpenDialog.FileNames;
                    foreach (var filePath in filePaths)
                    {
                        var fileName = Path.GetFileName(filePath);
                        var fileStream = File.OpenRead(filePath);
                        fileStreamWithName[fileName] = fileStream;
                    }

                    var fileSaveDialog = new SaveFileDialog
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
                                cw.Write(fileStreamWithName);
                            }
                        }

                        MessageBox.Show("压缩成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    };
                    fileSaveDialog.ShowDialog();
                };
                MessageBox.Show("压缩过大的文件可能会导致程序暂时失去响应，建议使用总量不大于 50 kb 的文件进行压缩测试", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                fileOpenDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "哎呀，发生了一个错误~", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UnCompress_Files_Button_Click(object sender, RoutedEventArgs e)
        {
            try
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
                    var folderBrowserDialog = new FolderBrowserDialog
                    {
                        Description = "选择你要解压到的文件夹"
                    };

                    if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        using (var cr = new CompressStreamReader(((OpenFileDialog)o2).OpenFile()))
                        {
                            cr.Read(null, s => File.Create(Path.Combine(folderBrowserDialog.SelectedPath, s)));
                        }

                        MessageBox.Show("解压成功！", "信息", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                };
                MessageBox.Show("解压过大的文件可能会导致程序暂时失去响应，建议使用不大于 50 kb 的 HUF 压缩文件解压缩测试", "警告", MessageBoxButton.OK, MessageBoxImage.Warning);
                fileOpenDialog.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "哎呀，发生了一个错误~", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}