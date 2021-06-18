using ICSharpCode.AvalonEdit;
using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Snatch
{
    public partial class ViewWindow : Window
    {
        private Entry entry;

        public ViewWindow()
        {
            InitializeComponent();
        }

        public ViewWindow(Entry entry)
        {
            InitializeComponent();
            this.entry = entry;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (entry != null)
            {
                if (entry.Type == (int) ContentTypeEnum.TEXT)
                {
                    TextEditor textEditor = new TextEditor();
                    textEditor.FontFamily = new System.Windows.Media.FontFamily("Consolas");
                    textEditor.FontSize = 12;

                    MenuItem copyMenuItem = new MenuItem();
                    copyMenuItem.Header = "Copy";
                    copyMenuItem.Click += CopyMenuItemOnClick;
                    textEditor.ContextMenu = new ContextMenu() {Items = {copyMenuItem}};
                    ;
                    textEditor.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                    textEditor.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
                    textEditor.SyntaxHighlighting =
                        ICSharpCode.AvalonEdit.Highlighting.HighlightingManager.Instance.GetDefinition("C#");

                    textEditor.Text = entry.Content;

                    this.Content.Children.Add(textEditor);
                }
                else if (entry.Type == (int) ContentTypeEnum.IMAGE)
                {
                    if (Base64Utils.IsBase64(entry.Content))
                    {
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();

                        byte[] arr = Convert.FromBase64String(entry.Content);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(arr);
                        bitmapImage.EndInit();

                        image.Source = bitmapImage;

                        this.Content.Children.Add(image);
                    }
                }
                else if (entry.Type == (int) ContentTypeEnum.IMAGE_FILE)
                {
                    if (File.Exists(entry.Content))
                    {
                        System.Windows.Controls.Image image = new System.Windows.Controls.Image();

                        ShellFile shellFile = ShellFile.FromFilePath(entry.Content);
                        Bitmap bitmap = shellFile.Thumbnail.ExtraLargeBitmap;

                        MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, ImageUtils.GetImageFormat(bitmap));
                        byte[] arr = ms.GetBuffer();
                        ms.Close();

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(arr);
                        bitmapImage.EndInit();

                        image.Source = bitmapImage;

                        this.Content.Children.Add(image);
                    }
                }
            }
        }

        private void CopyMenuItemOnClick(object sender, RoutedEventArgs e)
        {
            if (this.Content.Children[0] is TextEditor)
            {
                TextEditor textEditor = this.Content.Children[0] as TextEditor;
                Clipboard.SetText(textEditor.SelectedText);
            }
        }
    }
}