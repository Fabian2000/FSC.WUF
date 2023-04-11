using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace FSC.WUF
{
    /// <summary>
    /// HtmlDocument is a JavaScript - C# bridge
    /// </summary>
    public class HtmlDocument
    {
        private StringBuilder _js;
        private WindowManager _window;
        private string _element;

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
            _element = element;
            _window = window;
        }

        /// <summary>
        /// Gets the parent of an element. Similar to parentElement in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public async Task<HtmlDocument> ParentElement()
        {
            return await GetOtherElementThan("parentElement");
        }

        /// <summary>
        /// Gets the first child element of an element. Similar to firstElementChild in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public async Task<HtmlDocument> FirstElementChild()
        {
            return await GetOtherElementThan("firstElementChild");
        }

        /// <summary>
        /// Gets the last child element of an element. Similar to lastElementChild in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public async Task<HtmlDocument> LastElementChild()
        {
            return await GetOtherElementThan("lastElementChild");
        }

        /// <summary>
        /// Gets the element after the element. Similar to nextElementSibling in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public async Task<HtmlDocument> NextElementSibling()
        {
            return await GetOtherElementThan("nextElementSibling");
        }

        /// <summary>
        /// Gets the element before the element. Similar to previousElementSibling in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public async Task<HtmlDocument> PreviousElementSibling()
        {
            return await GetOtherElementThan("previousElementSibling");
        }

        /// <summary>
        /// Gets the selected option of an element
        /// </summary>
        /// <returns>Returns the selected option</returns>
        public async Task<HtmlDocument> SelectedOptions(int index)
        {
            return await GetOtherElementThan($"selectedOptions[{index}]");
        }

        private async Task<HtmlDocument> GetOtherElementThan(string js)
        {
            StringBuilder element = new StringBuilder();
            element.Append(_js.ToString());
            element.Append($".{js}");
            element.Append(".getAttribute('element-guid');");

            var elementGuid = await _window.ExecuteScript(element.ToString());

            return new HtmlDocument(_window, $"""[element-guid="{elementGuid}"]""");
        }

        /// <summary>
        /// Gets the TagName of an element
        /// </summary>
        /// <returns>Returns the TagName as a string</returns>
        public async Task<string> TagName()
        {
            return await ExecuteScript("classList");
        }

        /// <summary>
        /// Starts a Video or Audio player
        /// </summary>
        /// <returns></returns>
        public async Task Play()
        {
            await ExecuteScript("play()");
        }

        /// <summary>
        /// Pauses a Video or Audio player
        /// </summary>
        /// <returns></returns>
        public async Task Pause()
        {
            await ExecuteScript("pause()");
        }

        /// <summary>
        /// Reloads the Video or Audio player
        /// </summary>
        /// <returns></returns>
        public async Task Load()
        {
            await ExecuteScript("load()");
        }

        /// <summary>
        /// Gets the Id of an element
        /// </summary>
        /// <returns>Returns the Id as a string</returns>
        public async Task<string> Id()
        {
            return await ExecuteScript("id");
        }

        /// <summary>
        /// Gets the Name attribute of an element
        /// </summary>
        /// <returns>Returns the Name as a string</returns>
        public async Task<string> Name()
        {
            return await ExecuteScript("name");
        }

        /// <summary>
        /// Gets an attribute
        /// </summary>
        /// <returns></returns>
        public async Task<string> Attr(string attrName)
        {
            return await ExecuteScript($"getAttribute('{attrName}')");
        }

        /// <summary>
        /// Sets an attribute
        /// </summary>
        /// <returns></returns>
        public async Task Attr(string attrName, string attrValue)
        {
            await ExecuteScript($"setAttribute('{attrName}', '{attrValue}')");
        }

        /// <summary>
        /// Adds a new class to the class list
        /// </summary>
        /// <returns></returns>
        public async Task AddClass(string className)
        {
            await ExecuteScript($"classList.add('{className}')");
        }

        /// <summary>
        /// Removes a class from the class list
        /// </summary>
        /// <returns></returns>
        public async Task RemoveClass(string className)
        {
            await ExecuteScript($"classList.remove('{className}')");
        }

        /// <summary>
        /// Gets a list of all classes of an element
        /// </summary>
        /// <returns>Returns a string list</returns>
        public async Task<List<string>> ClassList()
        {
            var classList = await ExecuteScript("classList");

            return JsListToList(classList);
        }

        /// <summary>
        /// Removes the element from the html viewer
        /// </summary>
        /// <returns></returns>
        public async Task Remove()
        {
            await ExecuteScript($"remove()");
        }

        /// <summary>
        /// Scrolls an element into the view area
        /// </summary>
        /// <returns></returns>
        public async Task ScrollIntoView(string behavior = "smooth", string block = "center", string inline = "nearest")
        {
            await ExecuteScript($$"""scrollIntoView({behavior: {{behavior}}, block: {{block}}, inline: {{inline}})""");
        }

        /// <summary>
        /// Adds html content into an element
        /// </summary>
        /// <returns></returns>
        public async Task InnerHtml(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"innerHTML = '{html.resource}'");
        }

        /// <summary>
        /// Gets the the html content of an element
        /// </summary>
        /// <returns>Returns the html content as a string</returns>
        public async Task<string> InnerHtml()
        {
            return await ExecuteScript($"innerHTML");
        }

        /// <summary>
        /// Similar to innerHtml, but instead of replacing the whole content of an element, it will add the new content after the last child element
        /// </summary>
        /// <returns></returns>
        public async Task Append(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"insertAdjacentHTML('beforeend', '{html.resource.ReplaceLineEndings("")}')");
        }

        /// <summary>
        /// Similar to innerHtml, but instead of replacing the whole content of an element, it will add the new content after the last child element
        /// </summary>
        /// <returns></returns>
        public async Task Append(Css css)
        {
            if (!css.IsValid())
            {
                throw new Exception("Invalid Css");
            }

            css.BuildDataUrl("css");

            await ExecuteScript($"""insertAdjacentHTML('beforeend', '<link rel="stylesheet" href="{css.dataURL}">')""");
        }

        /// <summary>
        /// Similar to innerHtml, but instead of replacing the whole content of an element, it will add the new content before the first child element
        /// </summary>
        /// <returns></returns>
        public async Task Prepend(Html html)
        {
            if (!html.IsValid())
            {
                throw new Exception("Invalid html");
            }

            await ExecuteScript($"insertAdjacentHTML('afterbegin', '{html.resource.ReplaceLineEndings("")}')");
        }

        /// <summary>
        /// Inserts a text into an element
        /// </summary>
        /// <returns></returns>
        public async Task InnerText(string text)
        {
            await ExecuteScript($"innerText = '{text}'");
        }

        /// <summary>
        /// Gets the text from an element
        /// </summary>
        /// <returns>Returns the inner text as a string</returns>
        public async Task<string> InnerText()
        {
            return await ExecuteScript($"innerText");
        }

        /// <summary>
        /// Gets the style value of a css property
        /// </summary>
        /// <returns>Returns the style value as a string</returns>
        public async Task<string> Style(string cssProp)
        {
            List<char> cssPropChars = new List<char>(cssProp.ToLower().ToCharArray());

            while (cssPropChars.Contains('-'))
            {
                var dashPosition = cssPropChars.IndexOf('-');
                cssPropChars.RemoveAt(dashPosition);
                cssPropChars[dashPosition] = char.ToUpper(cssPropChars[dashPosition]);
            }

            cssProp = string.Concat(cssPropChars);
            return await _window.ExecuteScript($"window.getComputedStyle({_js.ToString()}).{cssProp}");
        }

        /// <summary>
        /// Set a css property to a new value
        /// </summary>
        /// <returns></returns>
        public async Task Style<T>(string cssProp, T cssValue)
        {
            List<char> cssPropChars = new List<char>(cssProp.ToLower().ToCharArray());

            while (cssPropChars.Contains('-'))
            {
                var dashPosition = cssPropChars.IndexOf('-');
                cssPropChars.RemoveAt(dashPosition);
                cssPropChars[dashPosition] = char.ToUpper(cssPropChars[dashPosition]);
            }

            cssProp = string.Concat(cssPropChars);

            await ExecuteScript($"style.{cssProp} = '{cssValue?.ToString()}'");
        }

        /// <summary>
        /// Gets the amount of elements from GetElement()
        /// </summary>
        /// <returns>Returns a number of all elements that were found by GetElement()</returns>
        public async Task<int> Count()
        {
            var count = await _window.ExecuteScript($"document.querySelectorAll('{_element}').length");
            count = count?.Trim('"');
            if (int.TryParse(count, out int result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Gets the value of an element
        /// </summary>
        /// <returns>Returns the value as a string</returns>
        public async Task<string> Value()
        {
            return await ExecuteScript("value");
        }

        /// <summary>
        /// Sets the value of an element
        /// </summary>
        /// <returns></returns>
        public async Task Value(string value)
        {
            await ExecuteScript($"value = '{value}'");
        }

        /// <summary>
        /// Gets the checked value of an element
        /// </summary>
        /// <returns>Returns the value</returns>
        public async Task<bool> Checked()
        {
            return await ExecuteScript("checked") == "true";
        }

        /// <summary>
        /// Sets the checked value of an element
        /// </summary>
        /// <returns></returns>
        public async Task Checked(bool value)
        {
            await ExecuteScript($"checked = '{value ? "true" : "false"}'");
        }

        /// <summary>
        /// Gets the disabled status of an element
        /// </summary>
        /// <returns>Returns the status</returns>
        public async Task<bool> Disabled()
        {
            return await ExecuteScript("disabled") == "true";
        }

        /// <summary>
        /// Sets the disabled status of an element
        /// </summary>
        /// <returns></returns>
        public async Task Disabled(bool value)
        {
            await ExecuteScript($"disabled = '{value ? "true" : "false"}'");
        }

        /// <summary>
        /// Gets the readonly status of an element
        /// </summary>
        /// <returns>Returns the status</returns>
        public async Task<bool> Readonly()
        {
            return await ExecuteScript("readonly") == "true";
        }

        /// <summary>
        /// Sets the readonly status of an element
        /// </summary>
        /// <returns></returns>
        public async Task Readonly(bool value)
        {
            await ExecuteScript($"readonly = '{value ? "true" : "false"}'");
        }

        /// <summary>
        /// Gets the scroll height of an element
        /// </summary>
        /// <returns>Returns the scrollHeight</returns>
        public async Task<int> ScrollHeight()
        {
            return await Convert.ToInt32(ExecuteScript("scrollHeight"));
        }

        /// <summary>
        /// Gets the scroll width of an element
        /// </summary>
        /// <returns>Returns the scrollWidth</returns>
        public async Task<int> ScrollWidth()
        {
            return await Convert.ToInt32(ExecuteScript(".scrollWidth"));
        }

        /// <summary>
        /// Sets the scroll position of an element vertically
        /// </summary>
        /// <returns></returns>
        public async Task ScrollTop(int value)
        {
            await ExecuteScript($"scrollTop = '{value.ToString()}'");
        }

        /// <summary>
        /// Sets the scroll position of an element vertically
        /// </summary>
        /// <returns>Returns the scrollTop</returns>
        public async Task<int> ScrollTop()
        {
            await Convert.ToInt32(ExecuteScript("scrollTop"));
        }

        /// <summary>
        /// Gets the client height of an element
        /// </summary>
        /// <returns>Returns the clientHeight</returns>
        public async Task<int> ClientHeight()
        {
            return await Convert.ToInt32(ExecuteScript("clientHeight"));
        }

        /// <summary>
        /// Gets the client width of an element
        /// </summary>
        /// <returns>Returns the clientWidth</returns>
        public async Task<int> ClientWidth()
        {
            return await Convert.ToInt32(ExecuteScript("clientWidth"));
        }

        /// <summary>
        /// Sets the scroll position of an element horizontally
        /// </summary>
        /// <returns></returns>
        public async Task ScrollLeft(int value)
        {
            await ExecuteScript($"scrollLeft = '{value.ToString()}'");
        }

        /// <summary>
        /// Gets the scroll position of an element horizontally
        /// </summary>
        /// <returns>Returns the scrollLeft</returns>
        public async Task<int> ScrollLeft()
        {
            return await Convert.ToInt32(ExecuteScript("scrollLeft"));
        }

        /// <summary>
        /// Sets the focus to an element
        /// </summary>
        /// <returns></returns>
        public async Task Focus()
        {
            await ExecuteScript("focus();");
        }

        /// <summary>
        /// Performs a click to an element
        /// </summary>
        /// <returns></returns>
        public async Task Click()
        {
            await ExecuteScript("click();");
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

    /// <summary>
    /// 
    /// </summary>
    public static class HtmlDocumentEx
    {
        /// <summary>
        /// Gets an element. Similar to querySelector in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public static HtmlDocument GetElement(this WindowManager window, string element)
        {
            return new HtmlDocument(window, element);
        }

        /// <summary>
        /// Gets an element. Similar to querySelector/querySelectorAll in JavaScript
        /// </summary>
        /// <returns>Returns a new HtmlDocument</returns>
        public static HtmlDocument GetElement(this WindowManager window, string element, int index)
        {
            return new HtmlDocument(window, element, index);
        }

        internal static HtmlDocument GetElement(this WindowManager window, Guid guid)
        {
            return GetElement(window, $"""[element-guid="{guid}"]""");
        }
    }
}
