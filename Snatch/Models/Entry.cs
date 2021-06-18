using Microsoft.WindowsAPICodePack.Shell;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Windows.Media.Imaging;

namespace Snatch
{
    public enum ContentTypeEnum
    {
        TEXT = 0, IMAGE_FILE = 1,IMAGE = 2
    }

    [Table("entry")]
    public class Entry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private int _id;

        public int Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                NotifyPropertyChanged();
            }
        }

        private string _content = String.Empty;

        public string Content
        {
            get
            {
                return _content;
            }

            set
            {
                if (value != _content)
                {
                    _content = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private int _type = 0;

        public int Type
        {
            get
            {
                return _type;
            }

            set
            {
                if (value != _type)
                {
                    _type = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _isPinned = false;

        public bool IsPinned
        {
            get
            {
                return _isPinned;
            }

            set
            {
                if (value != _isPinned)
                {
                    _isPinned = value;
                    NotifyPropertyChanged();
                }
            }
        }

        private bool _isVisible = true;

        [NotMapped]
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }

            set
            {
                if (value != _isVisible)
                {
                    _isVisible = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public string DisplayName
        {
            get
            {
                Regex regex = new Regex("[ ]{2,}", RegexOptions.None);
                string withoutNewline = this.Content.Replace("\r\n", "\n").Replace("\n", " ");

                return regex.Replace(withoutNewline, " ").TrimStart();
            }
        }

        public BitmapImage Thumbnail
        {
            get
            {
                if (Type == (int)ContentTypeEnum.IMAGE_FILE)
                {
                    if(File.Exists(Content))
                    {
                        ShellFile shellFile = ShellFile.FromFilePath(Content);
                        Bitmap bitmap = shellFile.Thumbnail.ExtraLargeBitmap;

                        MemoryStream ms = new MemoryStream();
                        bitmap.Save(ms, ImageUtils.GetImageFormat(bitmap));
                        byte[] arr = ms.GetBuffer();
                        ms.Close();

                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(arr);
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }else if (Type == (int) ContentTypeEnum.IMAGE)
                {
                    if (Base64Utils.IsBase64(Content))
                    {
                        byte[] arr = Convert.FromBase64String(Content);
                        BitmapImage bitmapImage = new BitmapImage();
                        bitmapImage.BeginInit();
                        bitmapImage.StreamSource = new MemoryStream(arr);
                        bitmapImage.EndInit();
                        return bitmapImage;
                    }
                }
                return null;
            }
        }
    }
}