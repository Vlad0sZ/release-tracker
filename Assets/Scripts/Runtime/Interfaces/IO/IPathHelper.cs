namespace Runtime.Interfaces.IO
{
    public interface IPathHelper
    {
        string GetDataPath();

        string GetFilePathOf<T>(T data)
            where T : IPathable;
    }
}