namespace MEFLib
{
    public interface IPlugin
    {
        string PuginName { get; set; }
        void Start();
        void Stop();
        void Pasue();

    }
}
