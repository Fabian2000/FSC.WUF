namespace FSC.WUF
{
    /// <summary>
    /// Specifies the text alignment in relation to the starting point of the text.
    /// </summary>
    public enum TextAlign
    {
        /// <summary>
        /// Aligns the text at the start of the specified direction (left for LTR, right for RTL).
        /// </summary>
        Start,

        /// <summary>
        /// Aligns the text at the end of the specified direction (right for LTR, left for RTL).
        /// </summary>
        End,

        /// <summary>
        /// Aligns the text to the left of the starting point.
        /// </summary>
        Left,

        /// <summary>
        /// Aligns the text to the right of the starting point.
        /// </summary>
        Right,

        /// <summary>
        /// Centers the text horizontally relative to the starting point.
        /// </summary>
        Center
    }

    /// <summary>
    /// Specifies the vertical alignment of the text relative to its starting point.
    /// </summary>
    public enum TextBaseline
    {
        /// <summary>
        /// Aligns the text at the top of the em box.
        /// </summary>
        Top,

        /// <summary>
        /// Aligns the text at the hanging baseline, which is common in certain scripts.
        /// </summary>
        Hanging,

        /// <summary>
        /// Aligns the text at the middle of the em box.
        /// </summary>
        Middle,

        /// <summary>
        /// Aligns the text at the alphabetic baseline, which is used for most Latin-based scripts.
        /// </summary>
        Alphabetic,

        /// <summary>
        /// Aligns the text at the ideographic baseline, which is used for Chinese, Japanese, and Korean scripts.
        /// </summary>
        Ideographic,

        /// <summary>
        /// Aligns the text at the bottom of the em box.
        /// </summary>
        Bottom
    }
}
