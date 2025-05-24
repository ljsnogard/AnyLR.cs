namespace NsAnyLR
{
    using System.Diagnostics.CodeAnalysis;

    /// <summary>
    /// Overlapped existence of L and R.
    /// That is, the existence may or may not be the same as others for both L and R.
    /// </summary>
    public interface IAnyLR<L, R>
    {
        public Option<L> AsLeft();

        public Option<R> AsRight();

        public (Option<L>, Option<R>) Destructed()
            => (this.AsLeft(), this.AsRight());
    }
}