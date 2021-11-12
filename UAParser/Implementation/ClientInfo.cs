using System;
using UAParser.Abstraction;

namespace UAParser
{
    /// <summary>
    /// Represents the user agent client information resulting from parsing
    /// a user agent string
    /// </summary>
    public class ClientInfo : IUAParserOutput
    {
        /// <summary>
        /// The user agent string, the input for the UAParser
        /// </summary>
        public string String { get; }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The OS parsed from the user agent string
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public OS OS { get; }

        /// <summary>
        /// The Device parsed from the user agent string
        /// </summary>
        public Device Device { get; }

        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        [Obsolete("Mirrors the value of the UA property. Will be removed in future versions")]
        public UserAgent UserAgent => UA;

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// The User Agent parsed from the user agent string
        /// </summary>
        public UserAgent UA { get; }

        /// <summary>
        /// Constructs an instance of the ClientInfo with results of the user agent string parsing
        /// </summary>
        public ClientInfo(string inputString, OS os, Device device, UserAgent userAgent)
        {
            String = inputString;
            OS = os;
            Device = device;
            UA = userAgent;
        }

        /// <summary>
        /// A readable description of the user agent client information
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{OS} {Device} {UA}";
        }
    }
}
