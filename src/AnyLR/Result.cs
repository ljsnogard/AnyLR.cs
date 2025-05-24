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

    public readonly struct Result<T, E> : IMaybe<T, E>, IMaybe<T>
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

        public static implicit operator Either<T, E>(Result<T, E> result)
            => result.either_;

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

        public Result<U, E> MapOk<U>(Func<T, U> mapOk)
            => this.MapOk<ValFnMap<T, U>, U>(new(mapOk));

        public Result<T, U> MapErr<U>(Func<E, U> mapErr)
            => this.MapErr<ValFnMap<E, U>, U>(new(mapErr));

        public bool IsOk([NotNullWhen(true)] out T ok)
            => this.either_.IsLeft(out ok);

        public bool IsOk()
            => this.either_.IsLeft();

        public bool IsErr([NotNullWhen(true)] out E err)
            => this.either_.IsRight(out err);

        public bool IsErr()
            => this.either_.IsRight();

        public Option<T> AsOk()
            => this.either_.AsLeft();

        public Option<E> AsErr()
            => this.either_.AsRight();

        public bool Matches([NotNullWhen(true)] out T t)
            => this.IsOk(out t);

        Option<T> IAnyLR<T, E>.AsLeft()
            => this.either_.AsLeft();

        Option<E> IAnyLR<T, E>.AsRight()
            => this.either_.AsRight();

        public Result<T, E> TryLeft()
            => this;

        public Result<E, T> TryRight()
            => this.either_.TryRight();
    }
}