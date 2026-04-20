#region Apache License, Version 2.0
//
// Copyright 2014 Atif Aziz
// Portions Copyright 2012 Søren Enemærke
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion


namespace UAParser.Implementations
{
    using System;
    using UAParser.Abstractions;

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
