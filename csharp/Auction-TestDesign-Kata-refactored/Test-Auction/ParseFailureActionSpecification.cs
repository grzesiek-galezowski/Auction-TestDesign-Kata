using Auction;
using Badeend.ValueCollections;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;

namespace Test_Auction_TestDesign_Kata;

public class ParseFailureActionSpecification
{
  [Test]
  public void ShouldMatchParseFailure()
  {
    // GIVEN
    var action = new ParseFailureAction();

    // WHEN
    var result = action.Matches("PARSE_FAILURE");

    // THEN
    result.Should().BeTrue();
  }

  [Test]
  public void ShouldNotMatchNonParseFailure()
  {
    // GIVEN
    var action = new ParseFailureAction();

    // WHEN
    var result = action.Matches(Any.OtherThan("PARSE_FAILURE"));

    // THEN
    result.Should().BeFalse();
  }

  [Test]
  public void ShouldNotifyListenerOfParseFailureWhenExecuted()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("Event", Any.String())
      .Build();
    var action = new ParseFailureAction();

    // WHEN
    action.Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnParseError();
  }
}