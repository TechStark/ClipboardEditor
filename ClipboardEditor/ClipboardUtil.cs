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
        private static extern uint RegisterClipboardFormat(string lpszFormat);

        [DllImport("user32.dll")]
        private static extern bool IsClipboardFormatAvailable(uint uFormat);

        [DllImport("user32.dll")]
        private static extern int GetClipboardFormatName(uint format, StringBuilder lpszFormatName, int cchMaxCount);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern int GlobalSize(IntPtr hMem);
        #endregion

        private static string GetClipboardFormatName(uint format)
        {
            StringBuilder name = new StringBuilder();
            GetClipboardFormatName(format, name, name.MaxCapacity);
            return name.ToString();
        }

        private static uint GetFormatID(string format)
        {
            switch (format)
            {
                case Formats.TEXT:
                    return CFConstants.CF_UNICODETEXT;

                case Formats.METAFILE_PICTURE_FORMAT:
                    return CFConstants.CF_METAFILEPICT;

                case Formats.ENHANCED_METAFILE:
                    return CFConstants.CF_ENHMETAFILE;

                default:
                    return RegisterClipboardFormat(format);
            }
        }

        public static string GetData(string format)
        {
            byte[] buffer = null;

            // get the format ID
            var formatID = GetFormatID(format);

            AccessClipboard(() =>
            {
                var dataPtr = GetClipboardData(formatID);
                buffer = GetBytes(dataPtr);
            });

            if (buffer == null)
            {
                return null;
            }

            if (IsTextFormat(format))
            {
                // text
                var text = Encoding.Unicode.GetString(buffer).TrimEnd('\0');
                return text;
            }

            // binary
            return Convert.ToBase64String(buffer);
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

        public static Dictionary<string, string> GetData()
        {
            var dict = new Dictionary<string, string>();

            var dataObject = Clipboard.GetDataObject();
            var formats = dataObject.GetFormats(false);

            foreach (var format in formats)
            {
                object data = null;

                try
                {
                    data = dataObject.GetData(format);
                }
                catch (Exception ex)
                {
                    data = ex.Message;
                }

                try
                {
                    dict[format] = ConvertToString(data);
                }
                catch (Exception ex)
                {
                    dict[format] = ex.Message;
                }
            }

            return dict;
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
            var formatID = GetFormatID(format);

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
            var objDict = new Dictionary<string, object>();

            foreach (var item in dict)
            {
                var format = item.Key;
                var data = item.Value;

                if (CanBeIncluded(format))
                {
                    objDict[format] = ConvertFromString(format, data);
                }
            }

            var dataObject = new ClipboardDataObject(objDict);
            Clipboard.SetDataObject(dataObject, true);
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
