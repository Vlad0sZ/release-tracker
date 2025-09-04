namespace Runtime.Interfaces.IO
{
    public interface IPathable
    {
        string Extension { get; }

        string FileName { get; }
    }
}