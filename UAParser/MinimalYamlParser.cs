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
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Just enough string parsing to recognize the regexes.yaml file format. Introduced to remove
    /// dependency on large Yaml parsing lib. Note that a unittest ensures compatibility
    /// by ensuring regexes and properties are read similar to using the full yaml lib
    /// </summary>
    internal class MinimalYamlParser
    {
        internal class Mapping
        {
            private Dictionary<string, string> _lastEntry;

            public Mapping()
            {
                Sequences = new List<Dictionary<string, string>>();
            }

            public List<Dictionary<string, string>> Sequences { get; }

            public void BeginSequence()
            {
                _lastEntry = new Dictionary<string, string>();
                Sequences.Add(_lastEntry);
            }

            public void AddToSequence(string key, string value)
            {
                _lastEntry[key] = value;
            }
        }

        private readonly Dictionary<string, Mapping> _mappings = new Dictionary<string, Mapping>();

        public MinimalYamlParser(string yamlString)
        {
            using (var yamlStringReader = new StringReader(yamlString))
                ReadIntoMappingModel(yamlStringReader);
        }

        public MinimalYamlParser(TextReader yamlReader)
        {
            ReadIntoMappingModel(yamlReader);
        }

        internal IDictionary<string, Mapping> Mappings => _mappings;

        private void ReadIntoMappingModel(TextReader yamlReader)
        {
            var lineCount = 0;
            Mapping activeMapping = null;
            while (yamlReader.Peek() != -1)
            {
                var line = yamlReader.ReadLine();
                lineCount++;
#if (NETSTANDARD1_0 || NET20 || NET35 || NET40)
                MapLine(line, lineCount, ref activeMapping);
#else
                MapLine(line.AsSpan(), lineCount, ref activeMapping);
#endif
            }
        }

#if (NETSTANDARD1_0 || NET20 || NET35 || NET40)
        private void MapLine(string line, int lineCount, ref Mapping activeMapping)
        {
            if (line.Trim().StartsWith("#")) //skipping comments
                return;
            if (line.Trim().Length == 0)
                return;

            //is this a new mapping entity
            if (line[0] != ' ')
            {
                int indexOfMappingColon = line.IndexOf(':');
                if (indexOfMappingColon == -1)
                    throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);
                string name = line.Substring(0, indexOfMappingColon).Trim();
                activeMapping = new Mapping();
                _mappings.Add(name, activeMapping);
                return;
            }

            //reading scalar entries into the active mapping
            if (activeMapping == null)
                throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);

            var seqLine = line.Trim();
            if (seqLine[0] == '-')
            {
                activeMapping.BeginSequence();
                seqLine = seqLine.Substring(1);
            }

            int indexOfColon = seqLine.IndexOf(':');
            if (indexOfColon == -1)
                throw new ArgumentException("YamlParsing: Expecting scalar mapping entry to contain a ':', at line " + lineCount);

            string key = seqLine.Substring(0, indexOfColon).Trim();
            string value = ReadQuotedValue(seqLine.Substring(indexOfColon + 1).Trim());
            activeMapping.AddToSequence(key, value);
        }

        private static string ReadQuotedValue(string value)
        {
            if (value.StartsWith("'") && value.EndsWith("'"))
                return value.Substring(1, value.Length - 2);
            if (value.StartsWith("\"") && value.EndsWith("\""))
                return value.Substring(1, value.Length - 2);
            return value;
        }
#else
        private void MapLine(ReadOnlySpan<char> line, int lineCount, ref Mapping activeMapping)
        {
            if (line.IsEmpty || line.TrimStart().StartsWith("#".AsSpan(), StringComparison.OrdinalIgnoreCase))
                return;

            //is this a new mapping entity
            if (line[0] != ' ')
            {
                var indexOfMappingColon = line.IndexOf(':');
                if (indexOfMappingColon == -1)
                    throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);
                var name = line.Slice(0, indexOfMappingColon).ToString();
                activeMapping = new Mapping();
                _mappings.Add(name, activeMapping);
                return;
            }

            //reading scalar entries into the active mapping
            if (activeMapping == null)
                throw new ArgumentException("YamlParsing: Expecting mapping entry to contain a ':', at line " + lineCount);

            var seqLine = line.Trim();
            if (seqLine[0] == '-')
            {
                activeMapping.BeginSequence();
                seqLine = seqLine.Slice(1);
            }

            var indexOfColon = seqLine.IndexOf(':');
            if (indexOfColon == -1)
                throw new ArgumentException("YamlParsing: Expecting scalar mapping entry to contain a ':', at line " + lineCount);

            var key = seqLine.Slice(0, indexOfColon).Trim().ToString();
            var value = ReadQuotedValue(seqLine.Slice(indexOfColon + 1).Trim());
            activeMapping.AddToSequence(key, value);
        }

        private static string ReadQuotedValue(ReadOnlySpan<char> value)
        {
            if (value[0] == '\'' && value.EndsWith("'".AsSpan()) ||
                value[0] == '"' && value.EndsWith("\"".AsSpan()))
                return value.Slice(1, value.Length - 2).ToString();
            return value.ToString();
        }
#endif

        public IEnumerable<Dictionary<string, string>> ReadMapping(string mappingName)
        {
            if (_mappings.TryGetValue(mappingName, out var mapping))
            {
                foreach (var s in mapping.Sequences)
                {
                    var temp = s;
                    yield return temp;
                }
            }
        }
    }
}
