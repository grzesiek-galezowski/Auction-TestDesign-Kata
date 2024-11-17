using Auction;
using Badeend.ValueCollections;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;

namespace Test_Auction_TestDesign_Kata;

public class UnknownMessageActionSpecification
{
  [Test]
  public void ShouldMatchAnyEventType()
  {
    // GIVEN
    var action = new UnknownMessageAction();

    // WHEN
    var result = action.Matches(Any.String());

    // THEN
    result.Should().BeTrue();
  }

  [Test]
  public void ShouldNotifyListenerOfUnknownMessage()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
        .Add("Event", Any.String())
        .Build();
    var action = new UnknownMessageAction();

    // WHEN
    action.Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnUnknownMessage();
  }
}
