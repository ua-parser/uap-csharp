namespace UAParser.Abstraction
{
    /// <summary>
    /// Representing the parse results. Structure of this class aligns with the
    /// ua-parser-output WebIDL structure defined in this document: https://github.com/ua-parser/uap-core/blob/master/docs/specification.md
    /// </summary>
    public interface IUAParserOutput
    {
        /// <summary>
        /// The user agent string, the input for the UAParser
        /// </summary>
        string String { get; }

        /// <summary>
        /// The OS parsed from the user agent string
        /// </summary>
        // ReSharper disable once InconsistentNaming
        OS OS { get; }

        /// <summary>
        /// The Device parsed from the user agent string
        /// </summary>
        Device Device { get; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        UserAgent UA { get; }
    }
}
