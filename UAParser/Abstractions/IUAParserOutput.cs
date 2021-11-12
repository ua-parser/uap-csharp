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


namespace UAParser.Abstractions
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
