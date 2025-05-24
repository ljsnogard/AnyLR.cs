namespace NsAnyLR.NsNUnit.NsTests;

public class ResultTests
{
    [Test]
    public void EitherWithSameLeftAndRightShouldDiffer()
    {
        const int LEFT = 1;
        const int RIGHT = 2;

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
}