using Runtime.Types;

namespace Runtime.Interfaces.Models
{
    public interface IRequestData<out T>
    {
        RequestType Type { get; }

        T Value { get; }
    }
}