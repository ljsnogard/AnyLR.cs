namespace NsAnyLR
{
    using System.Diagnostics.CodeAnalysis;

    public interface IMaybe<T>
    {
        public bool Matches()
            => this.Matches(out _);

        public bool Matches([NotNullWhen(true)] out T t);
    }

    /// <summary>
    /// Mutual exclusive existence of L and R.
    /// That is, the existency of L or R must differs from other.
    /// For example, Either monad.
    /// </summary>
    /// <typeparam name="L"></typeparam>
    /// <typeparam name="R"></typeparam>
    public interface IMaybe<L, R> : IAnyLR<L, R>
    {
        public Result<L, R> TryLeft();

        public Result<R, L> TryRight();
    }
}
