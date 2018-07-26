using System;

namespace EncompassLoadTest.DataInitialization
{
    public delegate TryResult<T> Try<T>();

    public struct TryResult<T>
    {
        public readonly T Value;
        public readonly Exception Exception;

        public TryResult(T value)
        {
            Value = value;
            Exception = null;
        }

        public TryResult(Exception e)
        {
            Exception = e ?? throw new ArgumentNullException(nameof(e));
            Value = default(T);
        }

        public static implicit operator TryResult<T>(T value)
        {
            return new TryResult<T>(value);
        }

        public bool IsFaulted => Exception != null;

        public override string ToString()
        {
            return IsFaulted
                ? Exception.ToString()
                : Value != null
                    ? Value.ToString()
                    : "[null]";
        }
    }

    public static class TryExt
    {
        public static TryResult<T> Try<T>(this Try<T> self)
        {
            try
            {
                return self();
            }
            catch (Exception e)
            {
                return new TryResult<T>(e);
            }
        }
    }

    public class Try
    {
        /// <summary>
        /// Mempty
        /// </summary>
        public static Try<T> Mempty<T>()
        {
            return () => default(T);
        }
    }
}