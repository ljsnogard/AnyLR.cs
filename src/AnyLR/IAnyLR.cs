namespace NsAnyLR
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public interface IAnyLeftOrRight
    { }

    public interface IAnyLeftOrRight<L, R> : IAnyLeftOrRight
    {
        public bool TryLeft([NotNullWhen(true)] out L leftVal, [NotNullWhen(false)] out R rightVal);

        public bool TryRight([NotNullWhen(true)] out R rightVal, [NotNullWhen(false)] out L leftVal)
            => !this.TryLeft(out leftVal, out rightVal);
    }

    public static class AnyLeftOrRightExt
    {
        public static bool IsLeft<A, L, R>(in A a, [NotNullWhen(true)] out L val)
            where A : struct, IAnyLeftOrRight<L, R>
        {
            return a.TryLeft(out val, out _);
        }

        public static bool IsRight<A, L, R>(in A a, [NotNullWhen(true)] out R val)
            where A : struct, IAnyLeftOrRight<L, R>
        {
            return a.TryRight(out val, out _);
        }

        public static bool IsLeft<A, L, R>(in A a)
            where A : struct, IAnyLeftOrRight<L, R>
        {
            return AnyLeftOrRightExt.IsLeft<A, L, R>(in a, out _);
        }

        public static bool IsRight<A, L, R>(in A a)
            where A : struct, IAnyLeftOrRight<L, R>
        {
            return AnyLeftOrRightExt.IsRight<A, L, R>(in a);
        }
    }
}
