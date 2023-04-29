namespace Karin.Core
{
    public class Encryption
    {
        public string Method { get; }
        public Dictionary<string, string> Parameters { get; }

        public Encryption(string method, Dictionary<string, string> parameters)
        {
            Method = method;
            Parameters = parameters;
        }
    }
}
