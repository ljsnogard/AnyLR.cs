namespace NsAnyLR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8762 // Parameter must have a non-null value when exiting in some condition.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.

    public static class AnyOf
    {
        public readonly struct BuildNone
        { }

        public readonly struct BuildLeft<L>
        {
            public readonly L Left;

            public BuildLeft(L left)
                => this.Left = left;
        }

        public readonly struct BuildRight<R>
        {
            public readonly R Right;

            public BuildRight(R right)
                => this.Right = right;
        }

        public readonly struct BuildBoth<L, R>
        {
            public readonly L Left;
            public readonly R Right;

            public BuildBoth(L left, R right)
            {
                this.Left = left;
                this.Right = right;
            }
        }

        public static BuildNone None()
            => new BuildNone();

        public static BuildLeft<L> Left<L>(L left)
            => new BuildLeft<L>(left);

        public static BuildRight<R> Right<R>(R right)
            => new BuildRight<R>(right);

        public static BuildBoth<L, R> Both<L, R>(L left, R right)
            => new BuildBoth<L, R>(left, right);
    }

    /// <summary>
    /// May be L, May be R, May both L and R, and May be neither L nor R.
    /// </summary>
    /// <typeparam name="L"></typeparam>
    /// <typeparam name="R"></typeparam>
    public readonly struct AnyOf<L, R> : IAnyLR<L, R>
    {
        private const byte K_L = 0b01;
        private const byte K_R = 0b10;
        private readonly byte st_;
        private readonly L vl_;
        private readonly R vr_;

        private AnyOf(byte k, L vl, R vr)
        {
            this.st_ = k;
            this.vl_ = vl;
            this.vr_ = vr;
        }

        #region Factories

        public static AnyOf<L, R> None()
            => new(0, default, default);

        public static AnyOf<L, R> Left(L val)
            => new(K_L, val, default);

        public static AnyOf<L, R> Right(R val)
            => new(K_R, default, val);

        public static AnyOf<L, R> Both(L vl, R vr)
            => new(K_L | K_R, vl, vr);

        public static implicit operator AnyOf<L, R>(AnyOf.BuildNone buildNon)
            => AnyOf<L, R>.None();

        public static implicit operator AnyOf<L, R>(AnyOf.BuildLeft<L> buildLeft)
            => AnyOf<L, R>.Left(buildLeft.Left);

        public static implicit operator AnyOf<L, R>(AnyOf.BuildRight<R> buildRight)
            => AnyOf<L, R>.Right(buildRight.Right);

        public static implicit operator AnyOf<L, R>(AnyOf.BuildBoth<L, R> buildBoth)
            => AnyOf<L, R>.Both(buildBoth.Left, buildBoth.Right);

        public static implicit operator AnyOf<L, R>(Either<L, R> either)
        {
            if (either.TryLeft(out var left, out var right))
                return AnyOf<L, R>.Left(left);
            else
                return AnyOf<L, R>.Right(right);
        }

        #endregion

        #region Fundamental methods

        public bool HasLeft()
            => (this.st_ & K_L) == K_L;

        public bool HasRight()
            => (this.st_ & K_R) == K_R;

        public bool HasBoth()
            => this.st_ == (K_L | K_R);

        public bool HasNone()
            => this.st_ == 0;

        public bool HasLeft([NotNullWhen(true)] out L leftVal)
        {
            var b = this.HasLeft();
            if (b)
                leftVal = this.vl_;
            else
                leftVal = default;
            return b;
        }

        public bool HasRight([NotNullWhen(true)] out R rightVal)
        {
            var b = this.HasRight();
            if (b)
                rightVal = this.vr_;
            else
                rightVal = default;
            return b;
        }

        #endregion

        #region IAnyLR

        public Option<L> AsLeft()
        {
            if (this.HasLeft(out var left))
                return Option.Some(left);
            else
                return Option.None();
        }

        public Option<R> AsRight()
        {
            if (this.HasRight(out var right))
                return Option.Some(right);
            else
                return Option.None();
        }

        #endregion

        public bool HasBoth([NotNullWhen(true)] out L leftVal, [NotNullWhen(true)] out R rightVal)
        {
            var l = this.HasLeft(out leftVal);
            var r = this.HasRight(out rightVal);
            return l && r;
        }

        #region Monadic

        public AnyOf<T, R> MapLeft<M, T>(in M mapLeft)
            where M : struct, IMap<L, T>
        {
            if (this.HasLeft())
                return new(this.st_, mapLeft.Map(this.vl_), this.vr_);
            else
                return new(this.st_, default, this.vr_);
        }

        public AnyOf<L, T> MapRight<M, T>(in M mapRight)
            where M : struct, IMap<R, T>
        {
            if (this.HasRight())
                return new(this.st_, this.vl_, mapRight.Map(this.vr_));
            else
                return new(this.st_, this.vl_, default);
        }

        #endregion

        #region Monadic convenience

        public AnyOf<T, R> MapLeft<T>(Func<L, T> mapLeft)
            => this.MapLeft<ValFnMap<L, T>, T>(mapLeft.ToFnMap());

        public AnyOf<L, T> MapRight<T>(Func<R, T> mapRight)
            => this.MapRight<ValFnMap<R, T>, T>(mapRight.ToFnMap());

        #endregion
    }

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8762 // Parameter must have a non-null value when exiting in some condition.
}