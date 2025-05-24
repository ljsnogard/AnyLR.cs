namespace NsAnyLR.NsNUnit.NsTests;

public class ResultTests
{
    const int LEFT = 1;
    const int RIGHT = 2;

    [Test]
    public void EitherWithSameLeftAndRightShouldDiffer()
    {
        Either<int, int> el, er;
        el = Either.Left(LEFT);
        er = Either.Right(RIGHT);
        Assert.That(
            el.IsLeft(out var l) && l == LEFT,
            "Instance of `Either` created by `Either.Left` should return true on IsLeft()");
        Assert.That(
            er.IsRight(out var r) && r == RIGHT,
            "Instance of `Either` created by `Either.Right` should return true on IsRight()"
        );
    }

    [Test]
    public void EitherTryShouldConformToItsState()
    {
        Either<int, int> el, er;
        el = Either.Left(LEFT);
        er = Either.Right(RIGHT);

        Assert.That(el.TryLeft().IsOk());
        Assert.That(el.TryRight().IsErr());

        Assert.That(er.TryRight().IsOk());
        Assert.That(er.TryLeft().IsErr());
    }

    [Test]
    public void EitherMappingShouldWork()
    {
        Either<int, int> el, er;
        el = Either.Left(LEFT);
        er = Either.Right(RIGHT);

        var sqr = (int x) => x * x;
        Assert.That(el.MapLeft(sqr).AsLeft().IsSome(out var lv) && lv == sqr(LEFT));
        Assert.That(el.MapRight(sqr).TryRight().IsErr(out lv) && lv == LEFT);

        Assert.That(er.MapRight(sqr).AsRight().IsSome(out var rv) && rv == sqr(RIGHT));
        Assert.That(er.MapLeft(sqr).TryLeft().IsErr(out rv) && rv == RIGHT);
    }
}