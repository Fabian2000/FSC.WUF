using FSC.WUF.Elements;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace FSC.WUF
{
    /// <summary>
    /// The CanvasRenderingContext2D class is used to draw text, lines, boxes, circles, and more onto a canvas element.
    /// </summary>
    public class CanvasRenderingContext2D
    {
        private HtmlDocument _element;
        private WindowManager _window;
        private string _contextVariable = string.Empty;
        internal CanvasRenderingContext2D(WindowManager window, HtmlDocument element)
        {
            _element = element;
            _window = window;
        }

        /// <summary>
        /// Draws a filled rectangle.
        /// </summary>
        public async Task FillRect(int x, int y, int width, int height)
        {
            await ExecuteScript($"fillRect({x}, {y}, {width}, {height})");
        }

        /// <summary>
        /// Clears the specified pixels within a given rectangle.
        /// </summary>
        public async Task ClearRect(int x, int y, int width, int height)
        {
            await ExecuteScript($"clearRect({x}, {y}, {width}, {height})");
        }

        /// <summary>
        /// Draws a rectangular outline.
        /// </summary>
        public async Task StrokeRect(int x, int y, int width, int height)
        {
            await ExecuteScript($"strokeRect({x}, {y}, {width}, {height})");
        }

        /// <summary>
        /// Creates a path.
        /// </summary>
        public async Task BeginPath()
        {
            await ExecuteScript("beginPath()");
        }

        /// <summary>
        /// Closes the path.
        /// </summary>
        public async Task ClosePath()
        {
            await ExecuteScript("closePath()");
        }

        /// <summary>
        /// Moves the path to the start point in the current sub-path, with the specified point.
        /// </summary>
        public async Task MoveTo(int x, int y)
        {
            await ExecuteScript($"moveTo({x}, {y})");
        }

        /// <summary>
        /// Adds a straight line to the current sub-path by connecting the sub-path's last point to the specified (x, y) coordinates.
        /// </summary>
        public async Task LineTo(int x, int y)
        {
            await ExecuteScript($"lineTo({x}, {y})");
        }

        /// <summary>
        /// Draws the current path.
        /// </summary>
        public async Task Stroke()
        {
            await ExecuteScript("stroke()");
        }

        /// <summary>
        /// Draws and fills the current path.
        /// </summary>
        public async Task Fill()
        {
            await ExecuteScript("fill()");
        }

        /// <summary>
        /// Draws the filled text at the position.
        /// </summary>
        public async Task FillText(JsString text, int x, int y)
        {
            await ExecuteScript($"fillText({text}, {x}, {y})");
        }

        /// <summary>
        /// Draws the outlined text at the position.
        /// </summary>
        public async Task StrokeText(JsString text, int x, int y)
        {
            await ExecuteScript($"strokeText({text}, {x}, {y})");
        }

        /// <summary>
        /// Sets the font properties for the text.
        /// </summary>
        public async Task SetFont(JsString font)
        {
            await ExecuteScript($"font = {font}");
        }

        /// <summary>
        /// Gets the font properties for the text.
        /// </summary>
        public async Task<string> GetFont()
        {
            return (await ExecuteScript("font")).Trim('"');
        }

        /// <summary>
        /// Sets the color of the fill style.
        /// </summary>
        public async Task SetFillStyle(JsString color)
        {
            await ExecuteScript($"fillStyle = {color}");
        }

        /// <summary>
        /// Gets the color of the fill style.
        /// </summary>
        public async Task<string> GetFillStyle()
        {
            return (await ExecuteScript("fillStyle")).Trim('"');
        }

        /// <summary>
        /// Sets the color of the outline.
        /// </summary>
        public async Task SetStrokeStyle(JsString color)
        {
            await ExecuteScript($"strokeStyle = {color}");
        }

        /// <summary>
        /// Gets the color of the stroke style.
        /// </summary>
        public async Task<string> GetStrokeStyle()
        {
            return (await ExecuteScript("strokeStyle")).Trim('"');
        }

        /// <summary>
        /// Sets the thickness of the lines.
        /// </summary>
        public async Task SetLineWidth(int width)
        {
            await ExecuteScript($"lineWidth = {width}");
        }

        /// <summary>
        /// Gets the thickness of the lines.
        /// </summary>
        public async Task<int> GetLineWidth()
        {
            var result = await ExecuteScript("lineWidth");
            return int.Parse(result);
        }

        /// <summary>
        /// Sets the line cap style.
        /// </summary>
        public async Task SetLineCap(JsString style)
        {
            await ExecuteScript($"lineCap = {style}");
        }

        /// <summary>
        /// Gets the line cap style.
        /// </summary>
        public async Task<string> GetLineCap()
        {
            return (await ExecuteScript($"lineCap")).Trim('"');
        }

        /// <summary>
        /// Sets the line join style.
        /// </summary>
        public async Task SetLineJoin(JsString style)
        {
            await ExecuteScript($"lineJoin = {style}");
        }

        /// <summary>
        /// Gets the line join style.
        /// </summary>
        public async Task<string> GetLineJoin()
        {
            return (await ExecuteScript($"lineJoin")).Trim('"');
        }

        /// <summary>
        /// Sets the miter limit ratio.
        /// </summary>
        public async Task SetMiterLimit(int limit)
        {
            await ExecuteScript($"miterLimit = {limit}");
        }

        /// <summary>
        /// Gets the miter limit ratio.
        /// </summary>
        public async Task<int> GetMiterLimit()
        {
            var result = await ExecuteScript("miterLimit");
            return int.Parse(result.Trim('"'));
        }

        /// <summary>
        /// Sets the shadow color.
        /// </summary>
        /// <param name="color">The color of the shadow</param>
        public async Task SetShadowColor(JsString color)
        {
            await ExecuteScript($"shadowColor = {color}");
        }

        /// <summary>
        /// Gets the shadow color.
        /// </summary>
        public async Task<string> GetShadowColor()
        {
            return (await ExecuteScript("shadowColor")).Trim('"');
        }

        /// <summary>
        /// Sets the shadow blur radius.
        /// </summary>
        /// <param name="blur">The blur radius of the shadow</param>
        public async Task SetShadowBlur(int blur)
        {
            await ExecuteScript($"shadowBlur = {blur}");
        }

        /// <summary>
        /// Gets the shadow blur radius.
        /// </summary>
        public async Task<int> GetShadowBlur()
        {
            var result = await ExecuteScript("shadowBlur");
            return int.Parse(result.Trim('"'));
        }

        /// <summary>
        /// Sets the shadow offset.
        /// </summary>
        /// <param name="x">The horizontal offset of the shadow</param>
        /// <param name="y">The vertical offset of the shadow</param>
        public async Task SetShadowOffset(int x, int y)
        {
            await ExecuteScript($"shadowOffsetX = {x}; shadowOffsetY = {y}");
        }

        /// <summary>
        /// Gets the shadow offset.
        /// </summary>
        public async Task<(int x, int y)> GetShadowOffset()
        {
            var result = await ExecuteScript("shadowOffsetX + ',' + shadowOffsetY");
            var values = result.Split(',');
            return (int.Parse(values[0].Trim('"')), int.Parse(values[1].Trim('"')));
        }

        /// <summary>
        /// Sets the global alpha value.
        /// </summary>
        /// <param name="alpha">The alpha value</param>
        public async Task SetGlobalAlpha(double alpha)
        {
            await ExecuteScript($"globalAlpha = {alpha}");
        }

        /// <summary>
        /// Gets the global alpha value.
        /// </summary>
        public async Task<double> GetGlobalAlpha()
        {
            var result = await ExecuteScript("globalAlpha");
            return double.Parse(result.Trim('"'));
        }

        /// <summary>
        /// Sets the global composite operation.
        /// </summary>
        /// <param name="operation">The composite operation</param>
        public async Task SetGlobalCompositeOperation(GlobalCompositeOperation operation)
        {
            var operationString = operation.ToString().ToLower();
            await ExecuteScript($"globalCompositeOperation = {operationString}");
        }

        /// <summary>
        /// Gets the global composite operation.
        /// </summary>
        public async Task<GlobalCompositeOperation> GetGlobalCompositeOperation()
        {
            var result = await ExecuteScript("globalCompositeOperation");
            return (GlobalCompositeOperation)Enum.Parse(typeof(GlobalCompositeOperation), result.Trim('"'), true);
        }

        /// <summary>
        /// Draws an image onto the canvas.
        /// </summary>
        /// <param name="image">The image to draw</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the image</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the image</param>
        public async Task DrawImage(HtmlDocument image, int x, int y)
        {
            await DrawImage(image, x, y, await image.Width(), await image.Height());
        }

        /// <summary>
        /// Draws an image onto the canvas.
        /// </summary>
        /// <param name="image">The image to draw</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the image</param>
        /// <param name="y">The y-coordinate of the upper-left corner of the image</param>
        /// <param name="width">The width of the image</param>
        /// <param name="height">The height of the image</param>
        public async Task DrawImage(HtmlDocument image, int x, int y, int width, int height)
        {
            var imageElement = image.PreparedQuerySelector();
            await ExecuteScript($"drawImage({imageElement}, {x}, {y}, {width}, {height})");
        }

        /// <summary>
        /// Draws an arc with a specified center, radius, and angle range.
        /// </summary>
        /// <param name="x">The X-coordinate of the arc's center.</param>
        /// <param name="y">The Y-coordinate of the arc's center.</param>
        /// <param name="radius">The radius of the arc.</param>
        /// <param name="startAngle">The starting angle of the arc in radians.</param>
        /// <param name="endAngle">The ending angle of the arc in radians.</param>
        public async Task Arc(int x, int y, int radius, double startAngle, double endAngle)
        {
            await ExecuteScript($"arc({x}, {y}, {radius}, {startAngle}, {endAngle})");
        }

        /// <summary>
        /// Rotates the canvas coordinate system by the specified angle.
        /// </summary>
        /// <param name="angle">The angle in radians by which the canvas coordinate system will be rotated. Positive values rotate clockwise, negative values rotate counterclockwise.</param>
        public async Task Rotate(double angle)
        {
            await ExecuteScript($"rotate({angle})");
        }

        /// <summary>
        /// Scales the canvas coordinate system by the specified factors along the x and y axes.
        /// </summary>
        /// <param name="x">The scaling factor along the x-axis. A value greater than 1 stretches the canvas, a value between 0 and 1 shrinks it.</param>
        /// <param name="y">The scaling factor along the y-axis. A value greater than 1 stretches the canvas, a value between 0 and 1 shrinks it.</param>
        public async Task Scale(double x, double y)
        {
            await ExecuteScript($"scale({x}, {y})");
        }

        /// <summary>
        /// Translates (moves) the canvas coordinate system by the specified distances along the x and y axes.
        /// </summary>
        /// <param name="x">The distance to move the canvas along the x-axis.</param>
        /// <param name="y">The distance to move the canvas along the y-axis.</param>
        public async Task Translate(double x, double y)
        {
            await ExecuteScript($"scale({x}, {y})");
        }

        private async Task<string> ExecuteScript(string script)
        {
            // If no context variable is set, create a new one
            // This is important because the context variable is needed to execute the script
            // It also needs to remember is original intern values
            if (string.IsNullOrEmpty(_contextVariable))
            {
                _contextVariable = "customContext" + RandomNumberGenerator.GetInt32(999999);
                _contextVariable = $"let {_contextVariable} = {_element.PreparedQuerySelector()}.getContext('2D');";
                await _window.ExecuteScript(_contextVariable);
            }
            var executePreview = $"{_contextVariable}.{script};";
            return await _window.ExecuteScript(executePreview);
        }
    }
}
