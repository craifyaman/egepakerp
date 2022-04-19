using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EgepakErp.Custom
{
    public class ValidationResponse<T>
    {
        private string _typeName;
        public ValidationResponse(T value)
        {
            Value = value;
        }

        public string TypeName
        {
            get { return _typeName ?? typeof(T).Name; }
            set { _typeName = value; }
        }

        public T Value { get; set; }
    }
    public class ValidationError
    {
        public ValidationError(string key, string message)
        {
            Key = key;
            Message = message;
        }

        public string Key { get; set; }
        public string Message { get; set; }
    }
}