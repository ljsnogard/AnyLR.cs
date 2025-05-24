# ANYLR.CS

This C# library provides discriminated union implementaions `AnyOf<L, R>`, `Either<L, R>`, `Option<T>`, `Result<T, E>` under namespace `NsAnyLR`.  
These are useful when `out` modified parameter types are not available, for example, parameter types in async methods.  

## `AnyOf<L, R>`

May contain zero or one L, and zero or one R.  
See tests code below:

```csharp
const int LEFT = 1;
const uint RIGHT = 2u;

AnyOf<int, uint> a0, al, ar, a2;
a0 = AnyOf.None();
al = AnyOf.Left(LEFT);
ar = AnyOf.Right(RIGHT);
a2 = AnyOf.Both(LEFT, RIGHT);

Assert.That(a0.HasNone());
Assert.That(!a0.HasLeft());
Assert.That(!a0.HasRight());
Assert.That(!a0.HasBoth());

Assert.That(!al.HasNone());
Assert.That(al.HasLeft());
Assert.That(!al.HasRight());
Assert.That(!al.HasBoth());

Assert.That(!ar.HasNone());
Assert.That(!ar.HasLeft());
Assert.That(ar.HasRight());
Assert.That(!ar.HasBoth());

Assert.That(!a2.HasNone());
Assert.That(a2.HasLeft());
Assert.That(a2.HasRight());
Assert.That(a2.HasBoth());
```

## `Either<L, R>`

May contain one and exactly only one value of type either L or R.
See tests code below:

```csharp
Either<int, int> el, er;
el = Either.Left(LEFT);
er = Either.Right(RIGHT);

var sqr = (int x) => x * x;
Assert.That(el.MapLeft(sqr).AsLeft().IsSome(out var lv) && lv == sqr(LEFT));
Assert.That(el.MapRight(sqr).TryRight().IsErr(out lv) && lv == LEFT);

Assert.That(er.MapRight(sqr).AsRight().IsSome(out var rv) && rv == sqr(RIGHT));
Assert.That(er.MapLeft(sqr).TryLeft().IsErr(out rv) && rv == RIGHT);
```
