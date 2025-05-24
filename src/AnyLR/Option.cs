namespace NsAnyLR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public static class Option
    {
        public readonly struct NonExisting
        { }

        public static Option<T> Some<T>(T data)
            => Option<T>.Some(data);

        public static Option.NonExisting None()
            => new NonExisting();
    }

    public readonly struct Option<T>
    {
        private readonly Either<T, Option.NonExisting> either_;

        private Option(Either<T, Option.NonExisting> either)
            => this.either_ = either;

        public static Option<T> Some(T data)
            => new(Either.Left(data));

        public static Option<T> None()
            => new(Either<T, Option.NonExisting>.Right(new()));

        public static implicit operator Option<T>(Option.NonExisting none)
            => new();

        public static implicit operator Option<T>(T? data)
        {
            if (data is T t)
                return Option<T>.Some(t);
            else
                return Option<T>.None();
        }

        public Option<U> Map<M, U>(in M map)
            where M : struct, IMap<T, U>
        {
            return new(this.either_.MapLeft<M, U>(in map));
        }

        public bool IsSome([NotNullWhen(true)] out T val)
            => this.either_.IsLeft(out val);

        public bool IsSome()
            => this.IsSome(out _);

        public T Unwrap(Func<System.Exception>? createException = null)
        {
            if (this.either_.IsLeft(out var data))
                return data;
            if (createException is Func<System.Exception> f)
                throw f();
            else
                throw new Exception($"Unwrap on None");
        }

        public T SomeOrElse<G>(in G makeElse)
            where G : struct, IGen<T>
        {
            if (this.IsSome(out var x))
                return x;
            else
                return makeElse.Gen();
        }

        public T SomeOrDefault(T val)
        {
            if (this.IsSome(out var x))
                return x;
            else
                return val;
        }

        public bool IsNone()
            => this.either_.IsRight();
    }
}