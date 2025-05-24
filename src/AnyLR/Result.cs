namespace NsAnyLR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Result
    {
        public readonly struct BuildErr<E>
        {
            public readonly E Err;

            public BuildErr(E err)
                => this.Err = err;
        }

        public readonly struct BuildOk<T>
        {
            public readonly T Ok;

            public BuildOk(T ok)
                => this.Ok = ok;
        }

        public static BuildOk<T> Ok<T>(T ok)
            => new BuildOk<T>(ok);

        public static BuildErr<E> Err<E>(E err)
            => new BuildErr<E>(err);
    }

    public readonly struct Result<T, E> : IAnyLeftOrRight<T, E>
    {
        private readonly Either<T, E> either_;

        public Result(Either<T, E> either)
            => this.either_ = either;

        public static Result<T, E> Ok(T data)
            => new(Either<T, E>.Left(data));

        public static Result<T, E> Err(E err)
            => new(Either<T, E>.Right(err));

        public static implicit operator Result<T, E>(Result.BuildOk<T> buildOk)
            => Result<T, E>.Ok(buildOk.Ok);

        public static implicit operator Result<T, E>(Result.BuildErr<E> buildErr)
            => Result<T, E>.Err(buildErr.Err);

        public static implicit operator Result<T, E>(Either<T, E> either)
            => new(either);

        public bool TryOk([NotNullWhen(true)] out T ok, [NotNullWhen(false)] out E err)
            => this.either_.TryLeft(out ok, out err);

        public bool TryErr([NotNullWhen(true)] out E err, [NotNullWhen(false)] out T ok)
            => this.either_.TryRight(out err, out ok);

        public Result<U, E> MapOk<M, U>(in M mapOk)
            where M : struct, IMap<T, U>
        {
            return this.either_.MapLeft<M, U>(in mapOk);
        }

        public Result<T, U> MapErr<M, U>(in M mapErr)
            where M : struct, IMap<E, U>
        {
            return this.either_.MapRight<M, U>(in mapErr);
        }

        public bool IsOk([NotNullWhen(true)] out T ok)
            => this.TryOk(out ok, out _);

        public bool IsOk()
            => this.IsOk(out _);

        public bool IsErr([NotNullWhen(true)] out E err)
        => this.TryErr(out err, out _);

        public bool IsErr()
            => this.IsErr(out _);

        public Option<T> AsOk()
            => this.either_.AsLeft();

        public Option<E> AsErr()
            => this.either_.AsRight();

        bool IAnyLeftOrRight<T, E>.TryLeft([NotNullWhen(true)] out T leftVal, [NotNullWhen(false)] out E rightVal)
            => this.TryOk(out leftVal, out rightVal);
    }
}