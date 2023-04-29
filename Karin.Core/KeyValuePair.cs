namespace Karin.Core
{
    public class KeyValuePair
    {
        public string Key { get; }
        public string Value { get; }

        public KeyValuePair(string raw)
        {
            var pair = raw.Split(':');

            Key = pair[0];
            Value = pair.Length > 1 ? string.Join(':', pair, 1, pair.Length - 1) : string.Empty;
        }
    }
}
