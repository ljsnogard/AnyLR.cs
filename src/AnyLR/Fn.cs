namespace NsAnyLR
{
    using System;

    /// <summary>
    /// Mapping object of type T to type U
    /// </summary>
    /// <typeparam name="T">The input type of the mapping.</typeparam>
    /// <typeparam name="U">The output type of the mapping.</typeparam>
    public interface IMap<in T, out U>
    {
        public U Map(T t);
    }

    /// <summary>
    /// The wrapper around <typeparamref name="System.Func"/> 
    /// </summary>
    /// <typeparam name="T">The input type of the mapping</typeparam>
    /// <typeparam name="U">The output type of the mapping</typeparam>
    public readonly struct ValFnMap<T, U> : IMap<T, U>
    {
        private readonly System.Func<T, U> mapFn_;

        public ValFnMap(System.Func<T, U> mapFn)
            => this.mapFn_ = mapFn;

        public U Map(T t)
            => this.mapFn_(t);

        public static implicit operator ValFnMap<T, U>(System.Func<T, U> mapFn)
            => new ValFnMap<T, U>(mapFn);
    }

    public interface IGen<out T>
    {
        public T Gen();
    }

    public readonly struct ValFnGen<T> : IGen<T>
    {
        private readonly System.Func<T> genFn_;

        public ValFnGen(System.Func<T> genFn)
            => this.genFn_ = genFn;

        public T Gen()
            => this.genFn_();

        public static implicit operator ValFnGen<T>(System.Func<T> genFn)
            => new ValFnGen<T>(genFn);
    }

    public static class FnExt
    {
        public static Func<T, Option.NonExisting> ToFunc<T>(this Action<T> f)
        {
            Option.NonExisting TakeAction(T t)
            {
                f(t);
                return new();
            }
            return TakeAction;
        }

        public static ValFnMap<T, U> ToFnMap<T, U>(this Func<T, U> f)
            => new(f);

        public static ValFnMap<T, Option.NonExisting> ToFnMap<T>(this Action<T> f)
            => new(f.ToFunc());

        public static ValFnGen<T> ToFnGen<T>(this Func<T> f)
            => new(f);
    }
}