using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClipboardEditor
{
    public class ClipboardDataObject : IDataObject
    {
        private readonly Dictionary<string, object> dict;

        public ClipboardDataObject(Dictionary<string, object> dict)
        {
            this.dict = dict;
        }

        public object GetData(Type format)
        {
            throw new NotImplementedException();
        }

        public object GetData(string format)
        {
            return GetDataPresent(format, true);
        }

        public object GetData(string format, bool autoConvert)
        {
            return dict[format];
        }

        public bool GetDataPresent(Type format)
        {
            throw new NotImplementedException();
        }

        public bool GetDataPresent(string format)
        {
            return GetDataPresent(format, true);
        }

        public bool GetDataPresent(string format, bool autoConvert)
        {
            return dict.ContainsKey(format);
        }

        public string[] GetFormats()
        {
            return GetFormats(true);
        }

        public string[] GetFormats(bool autoConvert)
        {
            return dict.Keys.ToArray();
        }

        public void SetData(object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(Type format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(string format, object data)
        {
            throw new NotImplementedException();
        }

        public void SetData(string format, object data, bool autoConvert)
        {
            throw new NotImplementedException();
        }
    }
}
