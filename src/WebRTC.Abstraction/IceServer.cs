using System.Text;

namespace WebRTC.Abstraction
{
    public class IceServer
    {
        public IceServer(string uri, string username = "", string password = "",
            TlsCertPolicy tlsCertPolicy = TlsCertPolicy.Secure) : this(new[] {uri}, username, password, tlsCertPolicy)
        {
        }

        public IceServer(string[] urls, string username, string password,
            TlsCertPolicy tlsCertPolicy = TlsCertPolicy.Secure)
        {
            Urls = urls;
            Username = username;
            Password = password;
            TlsCertPolicy = tlsCertPolicy;
        }

        public string[] Urls { get; }
        public string Username { get; }
        public string Password { get; }
        public TlsCertPolicy TlsCertPolicy { get; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[");
            foreach (var url in Urls)
            {
                sb.Append(url).Append(", ");
            }

            sb.Remove(sb.Length - 2, 2);
            sb.Append("] ");
            if (!string.IsNullOrEmpty(Username))
                sb.Append("[").Append(Username).Append(":").Append("] ");
            sb.Append("[").Append(TlsCertPolicy).Append("]");
            return sb.ToString();
        }
    }
}