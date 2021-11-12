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
    /// Represents the operating system the user agent runs on
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public sealed class OS : Part
    {
        /// <summary>
        /// Constructs an OS instance
        /// </summary>
        public OS(string family, string major, string minor, string patch, string patchMinor)
            : base (family)
        {
            Major      = major;
            Minor      = minor;
            Patch      = patch;
            PatchMinor = patchMinor;
        }

        /// <summary>
        /// The major version of the OS, if available
        /// </summary>
        public string Major      { get; }

        /// <summary>
        /// The minor version of the OS, if available
        /// </summary>
        public string Minor      { get; }

        /// <summary>
        /// The patch version of the OS, if available
        /// </summary>
        public string Patch      { get; }

        /// <summary>
        /// The minor patch version of the OS, if available
        /// </summary>
        public string PatchMinor { get; }

        /// <summary>
        /// A readable description of the OS
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var version = VersionString.Format(Major, Minor, Patch, PatchMinor);
            return Family + (!string.IsNullOrEmpty(version) ? " " + version : null);
        }
    }
}
