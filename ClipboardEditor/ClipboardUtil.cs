using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClipboardEditor
{
    public class ClipboardUtil
    {
        #region win32 APIs
        [DllImport("user32.dll")]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern bool SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        [DllImport("user32.dll")]
        private static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint uFormat);

        [DllImport("user32.dll")]
        private static extern uint EnumClipboardFormats(uint format);

        [DllImport("user32.dll")]
        private static extern bool GetClipboardFormatName(uint format, StringBuilder lpszFormatName, int cchMaxCount);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern int GlobalSize(IntPtr hMem);
        #endregion

        private static uint[] GetClipboardFormats()
        {
            var formats = new List<uint>();

            AccessClipboard(() =>
            {
                uint lastRetrievedFormat = 0;
                while ((lastRetrievedFormat = EnumClipboardFormats(lastRetrievedFormat)) != 0)
                {
                    formats.Add(lastRetrievedFormat);
                }
            });

            return formats.ToArray();
        }

        private static uint GetClipboardFormatID(string format)
        {
            var dataFormat = DataFormats.GetDataFormat(format);
            return (uint)dataFormat.Id;
        }

        private static string GetClipboardFormatName(uint format)
        {
            var dataFormat = DataFormats.GetDataFormat((int)format);
            return dataFormat.Name;
        }

        public static Dictionary<string, string> GetData()
        {
            var dict = new Dictionary<string, string>();

            var formats = GetClipboardFormats();
            foreach (var format in formats)
            {
                string name = GetClipboardFormatName(format);
                try
                {
                    dict[name] = GetData(format);
                }
                catch (Exception e)
                {
                    dict[name] = e.Message;
                }
            }

            return dict;
        }

        public static string GetData(string format)
        {
            // get the format ID
            var formatID = GetClipboardFormatID(format);
            return GetData(formatID);
        }

        public static string GetData(uint format)
        {
            byte[] buffer = null;

            AccessClipboard(() =>
            {
                var dataPtr = GetClipboardData(format);
                buffer = GetBytes(dataPtr);
            });

            if (buffer == null)
            {
                return null;
            }

            string result;

            switch (format)
            {
                case CFConstants.CF_TEXT:
                case CFConstants.CF_OEMTEXT:
                    // ascii text
                    result = Encoding.ASCII.GetString(buffer).TrimEnd('\0');
                    break;

                case CFConstants.CF_UNICODETEXT:
                    // unicode text
                    result = Encoding.Unicode.GetString(buffer).TrimEnd('\0');
                    break;

                default:
                    // binary
                    result = Convert.ToBase64String(buffer);
                    break;
            }

            return result;
        }

        private static byte[] GetBytes(IntPtr handle)
        {
            if (handle == IntPtr.Zero)
            {
                return null;
            }

            // lock
            var pointer = GlobalLock(handle);
            if (pointer == IntPtr.Zero)
            {
                return null;
            }

            var size = GlobalSize(handle);
            var buffer = new byte[size];

            Marshal.Copy(pointer, buffer, 0, size);

            // unlock
            GlobalUnlock(handle);

            return buffer;
        }

        private static void AccessClipboard(Action action)
        {
            if (!OpenClipboard(IntPtr.Zero))
            {
                // open clipboard failed
                return;
            }

            try
            {
                action?.Invoke();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                CloseClipboard();
            }
        }

        public static void SetData(string format, string data)
        {
            IntPtr dataPtr;

            var obj = ToClipboardData(format, data);
            if (obj is string)
            {
                dataPtr = Marshal.StringToHGlobalUni(obj as string);
            }
            else if (obj is byte[])
            {
                byte[] bytes = obj as byte[];
                dataPtr = Marshal.AllocHGlobal(bytes.Length);
                Marshal.Copy(bytes, 0, dataPtr, bytes.Length);
            }
            else
            {
                dataPtr = IntPtr.Zero;
            }

            // get the format ID
            var formatID = GetClipboardFormatID(format);

            AccessClipboard(() =>
            {
                SetClipboardData(formatID, dataPtr);
            });

            //Marshal.FreeHGlobal(dataPtr);
        }

        private static object ToClipboardData(string format, string data)
        {
            if (IsTextFormat(format))
            {
                return data;
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                return bytes;
            }
            catch (Exception)
            {
            }

            // text if failed to be converted to stream
            return data;
        }

        public static void SetData(Dictionary<string, string> dict)
        {
            Clipboard.Clear();

            foreach (var item in dict)
            {
                var format = item.Key;
                var data = item.Value;

                SetData(format, data);
            }
        }

        private static bool CanBeIncluded(string format)
        {
            if (format == "Object Descriptor")
            {
                // adding "Object Descriptor" will cause this app crash
                return false;
            }

            return true;
        }

        public static string ConvertToString(object data)
        {
            if (data == null)
            {
                return string.Empty;
            }

            if (data is string)
            {
                return data as string;
            }

            if (data is bool)
            {
                return data.ToString();
            }

            if (data is MemoryStream)
            {
                var stream = data as MemoryStream;
                string base64 = Convert.ToBase64String(stream.ToArray());
                return base64;
            }

            // array
            if (data is string[])
            {
                string[] stringArray = data as string[];
                string json = JsonConvert.SerializeObject(stringArray);
                return json;
            }

            if (data is Array)
            {
                Array objArray = data as Array;
                string[] stringArray = new string[objArray.Length];
                for (int i = 0; i < objArray.Length; i++)
                {
                    stringArray[i] = ConvertToString(objArray.GetValue(i));
                }
                string json = JsonConvert.SerializeObject(stringArray);
                return json;
            }

            // metafile
            if (data is Metafile)
            {
                Metafile wmf = data as Metafile;
                var stream = MetafileUtil.ToStream(wmf);
                string base64 = Convert.ToBase64String(stream.ToArray());
                return base64;
            }

            // by default
            return data.ToString();
        }

        private static bool IsTextFormat(string format)
        {
            if (format == Formats.TEXT
                || format == "UnicodeText"
                || format == "OEMText"
                || format == "System.String"
                || format == "Rich Text Format"
                || format == "HTML Format")
            {
                return true;
            }

            return false;
        }

        public static object ConvertFromString(string format, string data)
        {
            // text
            if (format == Formats.TEXT
                || format == "UnicodeText"
                || format == "OEMText"
                || format == "System.String"
                || format == "Rich Text Format"
                || format == "HTML Format")
            {
                return data;
            }

            // metafile
            if (format == Formats.METAFILE_PICTURE_FORMAT
                || format == Formats.ENHANCED_METAFILE)
            {
                try
                {
                    byte[] bytes = Convert.FromBase64String(data);
                    using (var stream = new MemoryStream(bytes))
                    {
                        var wmf = MetafileUtil.FromStream(stream);
                        return wmf;
                    }
                }
                catch (Exception)
                {
                }
            }

            try
            {
                byte[] bytes = Convert.FromBase64String(data);
                return new MemoryStream(bytes);
            }
            catch (Exception)
            {
            }

            // by default
            return data;
        }
    }
}
