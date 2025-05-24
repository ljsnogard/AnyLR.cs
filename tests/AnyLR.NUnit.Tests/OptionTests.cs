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
}
