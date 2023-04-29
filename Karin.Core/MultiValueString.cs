using System.Text.RegularExpressions;

namespace Karin.Core
{
    public class MultiValueString
    {
        private const string COMMA_REGEX = "(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)";
        private const string EQUAL_REGEX = "(?:^|=)(\"(?:[^\"]+|\"\")*\"|[^=]*)";

        public Dictionary<string, string> Values { get; }

        public MultiValueString(string raw)
        {
            var commaEntries = new Regex(COMMA_REGEX).Matches(raw);

            Values = new Dictionary<string, string>();
            for (int i = 0; i < commaEntries.Count; i++)
            {
                var commaEntry = commaEntries[i].Groups[1].Value;

                var keyValuePair = new Regex(EQUAL_REGEX).Matches(commaEntry);
                var key = keyValuePair[0].Groups[1].Value;
                var value = keyValuePair[1].Groups[1].Value.Trim('"');

                Values.Add(key, value);
            }
        }
    }
}
