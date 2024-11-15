using Auction;
using Badeend.ValueCollections;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Strings;

namespace Test_Auction_TestDesign_Kata;

public class OnCloseActionSpecification
{
  [Test]
  public void ShouldMatchCloseEvent()
  {
    // GIVEN
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("Event", "CLOSE")
      .Build();

    // WHEN
    var result = new OnCloseAction().Matches(valuesByKey);

    // THEN
    result.Should().BeTrue();
  }

  [Test]
  public void ShouldNotMatchNonCloseEvent()
  {
    // GIVEN
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("Event", Any.OtherThan("CLOSE"))
      .Build();

    // WHEN
    var result = new OnCloseAction().Matches(valuesByKey);

    // THEN
    result.Should().BeFalse();
  }

  [Test]
  public void ShouldReportAuctionClosedWhenAuctionIdIsPresent()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var auctionId = Any.String();
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("AuctionId", auctionId)
      .Build();

    // WHEN
    new OnCloseAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.Received(1).OnAuctionClosed(auctionId);
  }

  [Test]
  public void ShouldReportInvalidFieldWhenAuctionIdIsMissing()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValueDictionary<string, string>.Empty;

    // WHEN
    new OnCloseAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.Received(1).OnInvalidField("CLOSE", "AuctionId");
  }
}