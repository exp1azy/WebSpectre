namespace WebSpectre.Client.EventArgs
{
    public class AgentsStatusEventArgs : System.EventArgs
    {
        public Dictionary<string, bool?> AgentsStatus { get; set; }
    }
}
