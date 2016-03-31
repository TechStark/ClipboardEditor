using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClipboardEditor
{
    public class ClipboardUtil
    {
        public static string GetData(string format)
        {
            var data = Clipboard.GetData(format);
            return ConvertToString(data);
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
            var obj = ConvertFromString(format, data);
            Clipboard.SetData(format, obj);
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
