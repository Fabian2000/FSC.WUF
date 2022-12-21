using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace FSC.WUF
{
    public class HtmlDocument
    {
        private StringBuilder _js;
        private WindowManager _window;

        internal HtmlDocument(WindowManager window, string element, int i = -1)
        {
            _js = new StringBuilder("document");
            if (i == -1)
            {
                _js.Append($".querySelector('{element}')");
            }
            else
            {
                _js.Append($".querySelectorAll('{element}')[{i}]");
            }
            _window = window;
        }

        public async Task<string> TagName()
        {
            return await ExecuteScript("classList");
        }

        public async Task<string> Id()
        {
            return await ExecuteScript("id");
        }

        public async Task AddClass(string className)
        {
            await ExecuteScript($"classList.add('{className}')");
        }

        public async Task RemoveClass(string className)
        {
            await ExecuteScript($"classList.remove('{className}')");
        }

        public async Task<List<string>> ClassList()
        {
            var classList = await ExecuteScript("classList");
            
            return JsListToList(classList);
        }

        public async Task InnerHtml(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"innerHTML = '{html.resource}'");
        }

        public async Task<string> InnerHtml()
        {
            return await ExecuteScript($"innerHTML");
        }

        public async Task Append(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"insertAdjacentHTML('beforeend', '{html.resource.ReplaceLineEndings("")}')");
        }

        public async Task Append(Css css)
        {
            if (!css.IsValid())
            {
                throw new Exception("Invalid Css");
            }

            css.BuildDataUrl("css");

            await ExecuteScript($"""insertAdjacentHTML('beforeend', '<link rel="stylesheet" href="{css.dataURL}">')""");
        }

        public async Task Prepend(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"insertAdjacentHTML('afterbegin', '{html.resource.ReplaceLineEndings("")}')");
        }

        public async Task InnerText(string text)
        {
            await ExecuteScript($"innerText = '{text}'");
        }

        public async Task<string> InnerText()
        {
            return await ExecuteScript($"innerText");
        }

        public async Task<string> Style(string cssProp)
        {
            char[] cssPropChars = cssProp.ToCharArray();
            cssPropChars[0] = char.ToLower(cssPropChars[0]);
            return await ExecuteScript($"style.{cssProp}");
        }

        public async Task<string> Value()
        {
            return await ExecuteScript($"value");
        }

        public async Task Value(string value)
        {
            await ExecuteScript($"value = '{value}'");
        }

        private Task<string> ExecuteScript(string scriptAttachment)
        {
            return _window.ExecuteScript($"{_js.ToString()}.{scriptAttachment};");
        }

        private List<string> JsListToList(string jsList)
        {
            // {"0":"btn","1":"btn-primary","2":"ddd","3":"float-end"} <- Example

            var list = new List<string>();

            jsList = jsList.Trim('{', '}');

            if (string.IsNullOrWhiteSpace(jsList))
            {
                return list;
            }

            var keyValues = jsList.Split(',');
            for (var i = 0; i < keyValues.Length; i++)
            {
                var value = keyValues[i].Split(':')[1];
                value = value.Trim('"');
                list.Add(value);
            }

            return list;
        }
    }

    public static class HtmlDocumentEx
    {
        public static HtmlDocument GetElement(this WindowManager window, string element)
        {
            return new HtmlDocument(window, element);
        }

        public static HtmlDocument GetElement(this WindowManager window, string element, int index)
        {
            return new HtmlDocument(window, element, index);
        }
    }
}
