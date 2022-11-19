using Model.Nodes;

namespace  Model.Interfaces
{
    public interface IDirectoryScanner{
        Node Start(string path, ushort maxThreadCount);

        void Stop();

        bool IsRunning { get; }
    }
}