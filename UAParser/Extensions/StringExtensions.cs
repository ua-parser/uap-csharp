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


namespace UAParser.Extensions
{
    using System;

    internal static class StringExtensions
    {
        public static string ReplaceFirstOccurence(this string input, string search, string replacement)
        {
            if (input == null) throw new ArgumentNullException(nameof(input));
            var index = input.IndexOf(search, StringComparison.Ordinal);
            return index >= 0
                 ? input.Substring(0, index) + replacement + input.Substring(index + search.Length)
                 : input;
        }
    }

}
