using Runtime.Interfaces.Models;
using Runtime.Types;

namespace Runtime.Models
{
    /// <summary>
    /// Mediator for event with <see cref="RequestType"/>.
    /// </summary>
    /// <typeparam name="T">type of value.</typeparam>
    public abstract class Operation<T> : IRequestData<T>
    {
        protected Operation(T value, RequestType requestType)
        {
            Type = requestType;
            Value = value;
        }

        public RequestType Type { get; }
        public T Value { get; }
    }

    /// <summary>
    /// Use <see cref="RequestType.Create"/> operation.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class CreateOperation<T> : Operation<T>
    {
        public CreateOperation(T value) : base(value, RequestType.Create)
        {
        }
    }

    /// <summary>
    /// Use <see cref="RequestType.Select"/> operation.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class SelectOperation<T> : Operation<T>
    {
        public SelectOperation(T value) : base(value, RequestType.Select)
        {
        }
    }

    /// <summary>
    /// Use <see cref="RequestType.Edit"/> operation.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class UpdateOperation<T> : Operation<T>
    {
        public UpdateOperation(T value) : base(value, RequestType.Edit)
        {
        }
    }

    /// <summary>
    /// Use <see cref="RequestType.Delete"/> operation.
    /// </summary>
    /// <typeparam name="T">Type of value.</typeparam>
    public sealed class DeleteOperation<T> : Operation<T>
    {
        public DeleteOperation(T value) : base(value, RequestType.Delete)
        {
        }
    }
}