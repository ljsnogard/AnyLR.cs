namespace NsAnyLR.NsNUnit.NsTests;

public class AnyOfTests
{
    const int LEFT = 1;
    const uint RIGHT = 2u;

    public static int Sqr(int x)
        => x * x;

    public static uint Sqr(uint x)
        => x * x;

    [Test]
    public void AnyOfDefaultShouldBeNone()
    {
        AnyOf<int, uint> a = default;
        Assert.That(a.HasNone());
    }

    [Test]
    public void AnyOfStateShouldWork()
    {
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
    }

    [Test]
    public void AnyOfMappingShouldWork()
    {
        AnyOf<int, uint> a0, al, ar, a2;
        a0 = AnyOf.None();
        al = AnyOf.Left(LEFT);
        ar = AnyOf.Right(RIGHT);
        a2 = AnyOf.Both(LEFT, RIGHT);

        Assert.That(a0.MapLeft(Sqr).AsLeft().IsNone());
        Assert.That(a0.MapRight(Sqr).AsRight().IsNone());

        Assert.That(al.MapLeft(Sqr).AsLeft().IsSome(out var lv) && lv == Sqr(LEFT));
        Assert.That(al.MapRight(Sqr).AsRight().IsNone());

        Assert.That(ar.MapLeft(Sqr).AsLeft().IsNone());
        Assert.That(ar.MapRight(Sqr).AsRight().IsSome(out var rv) && rv == Sqr(RIGHT));

        Assert.That(a2.MapLeft(Sqr).AsLeft().IsSome(out var lv2) && lv2 == Sqr(LEFT));
        Assert.That(a2.MapRight(Sqr).AsRight().IsSome(out var rv2) && rv2 == Sqr(RIGHT));
    }
}