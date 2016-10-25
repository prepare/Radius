namespace AngleSharp.Dom
{
    using AngleSharp.Attributes;

#if RAD
    public
#endif
    /// <summary>
    /// A list of possible quirks mode states.
    /// </summary>
    enum QuirksMode : byte
    {
        /// <summary>
        /// The quirks mode is deactivated.
        /// </summary>
        Off,
        /// <summary>
        /// The quirks mode is partly activated.
        /// </summary>
        Limited,
        /// <summary>
        /// The quirks mode is activated.
        /// </summary>
        [DomDescription("BackCompat")]
        On
    }
}
