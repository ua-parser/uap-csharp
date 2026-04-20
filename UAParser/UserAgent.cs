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


namespace UAParser
{
    /// <summary>
    /// Represents a user agent, commonly a browser
    /// </summary>
    public sealed class UserAgent
    {
        /// <summary>
        /// Construct a UserAgent instance
        /// </summary>
        public UserAgent(string family, string major, string minor, string patch)
        {
            Family = family;
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        /// <summary>
        /// The family of user agent
        /// </summary>
        public string Family { get; }
        /// <summary>
        /// Major version of the user agent, if available
        /// </summary>
        public string Major { get; }
        /// <summary>
        /// Minor version of the user agent, if available
        /// </summary>
        public string Minor { get; }
        /// <summary>
        /// Patch version of the user agent, if available
        /// </summary>
        public string Patch { get; }

        /// <summary>
        /// The user agent as a readbale string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var version = VersionString.Format(Major, Minor, Patch);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }

}
