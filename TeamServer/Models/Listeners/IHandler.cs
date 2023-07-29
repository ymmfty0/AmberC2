namespace TeamServer.Models.Listeners
{
    public interface IHandler
    {
        public string Id { get;}
        Task Start(string BindHost,  int BindPort);
        public void Stop();
    }
}
