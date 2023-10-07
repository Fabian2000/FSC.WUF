using System;
using System.Text;

namespace FSC.WUF.Elements
{
    /// <summary>
    /// A string class that protect javascript from evil code injection
    /// </summary>
    public class JsString
    {
        private string _str;

        private JsString(string str)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(str);
            _str = BitConverter.ToString(byteArray).Replace("-", "");
        }

        /// <summary>
        /// A implizit conversion from string to JSString
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator JsString(string value)
        {
            return new JsString(value);
        }

        /// <summary>
        /// The implizit conversion from JSString to string
        /// </summary>
        /// <param name="value"></param>
        public static implicit operator string(JsString value)
        {
            return $"restoreString('{value._str}')";
        }

        /// <summary>
        /// Converts the JSString to a string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return (string)this;
        }
    }
}
