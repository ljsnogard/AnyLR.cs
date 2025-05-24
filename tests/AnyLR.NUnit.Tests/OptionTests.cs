namespace NsAnyLR.NsNUnit.NsTests;

public class OptionTests
{
    [SetUp]
    public void Setup()
    { }

    [Test]
    public void OptionFromNullTypeShouldBeNone()
    {
        string? s = null;
        Option<object> opt = s;
        Assert.That(opt.IsNone(), "Assign null value to Option should be None");
    }

    [Test]
    public void OptionDefaultShouldBeNone()
    {
        Option<byte> opt = default;
        Assert.That(Option<byte>.None().IsNone(), "Option instance returned by Option<T>.None should be None");
        Assert.That(opt.IsNone(), "default value of Option should be None");
        Assert.That(!opt.IsSome(), "default value of Option should return false when IsSome() called");
    }

    [Test]
    public void OptionSomeOrElseShouldWork()
    {
        const byte DEFAULT = 1;
        Option<byte> opt = default;
        Assert.That(opt.SomeOrDefault(DEFAULT) == DEFAULT);
    }

    [Test]
    public void OptionShouldMatchWhenSome()
    {
        const int DEFAULT = 42;
        var opt = Option.Some(DEFAULT);

        var map = (int k) => k - DEFAULT;

        Assert.That(opt.IsSome(out var v1) && v1 == DEFAULT);
        Assert.That(opt.Map(map).Unwrap() == 0);
        Assert.That(opt.Matches(out var v2) && v2 == DEFAULT);
    }
}
