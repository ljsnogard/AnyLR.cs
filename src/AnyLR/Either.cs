namespace NsAnyLR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

#pragma warning disable CS8762 // Parameter must have a non-null value when exiting in some condition.
#pragma warning disable CS8604 // Possible null reference argument.
#pragma warning disable CS8601 // Possible null reference assignment.

    public static class Either
    {
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

        public static BuildRight<T> Right<T>(T ok)
            => new BuildRight<T>(ok);

        public static BuildLeft<E> Left<E>(E err)
            => new BuildLeft<E>(err);
    }

    /// <summary>
    /// Struct storing instance of either left type or right type.
    /// </summary>
    /// <typeparam name="L">The left-armed type.</typeparam>
    /// <typeparam name="R">The right-armed type.</typeparam>
    public readonly struct Either<L, R> : IMaybe<L, R>
    {
        private readonly bool isLeft_;
        private readonly L vl_;
        private readonly R vr_;

        private Either(bool isLeft, L vl, R vr)
        {
            this.isLeft_ = isLeft;
            this.vl_ = vl;
            this.vr_ = vr;
        }

        #region Factories

        public static Either<L, R> Left(L val)
            => new Either<L, R>(true, val, default);

        public static Either<L, R> Right(R val)
            => new Either<L, R>(false, default, val);

        public static implicit operator Either<L, R>(Either.BuildLeft<L> buildLeft)
            => Either<L, R>.Left(buildLeft.Left);

        public static implicit operator Either<L, R>(Either.BuildRight<R> buildRight)
            => Either<L, R>.Right(buildRight.Right);

        #endregion

        #region Fundamental methods

        public bool IsLeft()
            => this.isLeft_;

        public bool IsRight()
            => !this.isLeft_;

        public bool TryLeft([NotNullWhen(true)] out L leftVal, [NotNullWhen(false)] out R rightVal)
        {
            if (this.IsLeft())
            {
                leftVal = this.vl_;
                rightVal = default;
                return true;
            }
            else
            {
                leftVal = default;
                rightVal = this.vr_;
                return false;
            }
        }

        public bool TryRight([NotNullWhen(true)] out R rightVal, [NotNullWhen(false)] out L leftVal)
        {
            if (this.IsRight())
            {
                leftVal = default;
                rightVal = this.vr_;
                return true;
            }
            else
            {
                leftVal = this.vl_;
                rightVal = default;
                return false;
            }
        }

        #endregion

        #region Convert to Result and Option

        public Result<L, R> TryLeft()
        {
            if (this.IsLeft())
                return Result.Ok(this.vl_);
            else
                return Result.Err(this.vr_);
        }

        public Result<R, L> TryRight()
        {
            if (this.IsRight())
                return Result.Ok(this.vr_);
            else
                return Result.Err(this.vl_);
        }

        public Option<L> AsLeft()
        {
            if (this.IsLeft(out var left))
                return Option.Some(left);
            else
                return Option.None();
        }

        public Option<R> AsRight()
        {
            if (this.IsRight(out var right))
                return Option.Some(right);
            else
                return Option.None();
        }

        #endregion

        #region Monadic

        public Either<U, R> MapLeft<M, U>(in M mapLeft)
            where M : struct, IMap<L, U>
        {
            if (this.TryLeft(out var leftVal, out var rightVal))
                return new(true, mapLeft.Map(leftVal), default);
            else
                return new(false, default, rightVal);
        }

        public Either<L, U> MapRight<M, U>(in M mapRight)
            where M : struct, IMap<R, U>
        {
            if (this.TryRight(out var rightVal, out var leftVal))
                return new Either<L, U>(false, default, mapRight.Map(rightVal));
            else
                return new Either<L, U>(true, leftVal, default);
        }

        #endregion

        #region Monadic convenience

        public Either<U, R> MapLeft<U>(Func<L, U> mapLeft)
            => this.MapLeft<ValFnMap<L, U>, U>(mapLeft.ToFnMap());

        public Either<L, U> MapRight<U>(Func<R, U> mapRight)
            => this.MapRight<ValFnMap<R, U>, U>(mapRight.ToFnMap());

        #endregion

        #region Misc convenience

        public bool IsLeft([NotNullWhen(true)] out L val)
            => this.TryLeft(out val, out _);

        public bool IsRight([NotNullWhen(true)] out R val)
            => this.TryRight(out val, out _);

        public bool HasLeft([NotNullWhen(true)] out L left)
            => this.IsLeft(out left);

        public bool HasRight([NotNullWhen(true)] out R right)
            => this.IsRight(out right);

        #endregion
    }

#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8762 // Parameter must have a non-null value when exiting in some condition.
}