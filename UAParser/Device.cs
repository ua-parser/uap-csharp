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
    using System;

    /// <summary>
    /// Represents the physical device the user agent is using
    /// </summary>
    public sealed class Device
    {
        /// <summary>
        /// Constructs a Device instance
        /// </summary>
        public Device(string family, string brand, string model)
        {
            Family = family.Trim();
            if (brand != null)
                Brand = brand.Trim();
            if (model != null)
                Model = model.Trim();
        }

        /// <summary>
        /// Returns true if the device is likely to be a spider or a bot device
        /// </summary>
        public bool IsSpider => "Spider".Equals(Family, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        ///The brand of the device
        /// </summary>
        public string Brand { get; }
        /// <summary>
        /// The family of the device, if available
        /// </summary>
        public string Family { get; }
        /// <summary>
        /// The model of the device, if available
        /// </summary>
        public string Model { get; }

        /// <summary>
        /// A readable description of the device
        /// </summary>
        public override string ToString()
        {
            return Family;
        }
    }

}
