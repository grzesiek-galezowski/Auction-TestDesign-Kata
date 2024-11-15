using Auction;
using Badeend.ValueCollections;
using FluentAssertions;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.AnyRoot;
using TddXt.AnyRoot.Numbers;
using TddXt.XNSubstitute;

namespace Test_Auction_TestDesign_Kata;

public class OnNewPriceActionSpecification
{
  [Test]
  public void ShouldMatchPriceEvent()
  {
    // GIVEN
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("Event", "PRICE")
      .Build();

    // WHEN
    var result = new OnNewPriceAction().Matches(valuesByKey);

    // THEN
    result.Should().BeTrue();
  }

  [Test]
  public void ShouldNotMatchNonPriceEvent()
  {
    // GIVEN
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("Event", Any.OtherThan("PRICE"))
      .Build();

    // WHEN
    var result = new OnNewPriceAction().Matches(valuesByKey);

    // THEN
    result.Should().BeFalse();
  }

  [Test]
  public void ShouldReportNewPriceWhenAllRequiredFieldsArePresent()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var auctionId = Any.String();
    var currentPrice = Any.Integer();
    var increment = Any.Integer();
    var bidder = Any.String();
    var valuesByKey = new ValueDictionaryBuilder<string, string>()
      .Add("AuctionId", auctionId)
      .Add("CurrentPrice", currentPrice.ToString())
      .Add("Increment", increment.ToString())
      .Add("Bidder", bidder)
      .Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnBidDetails(auctionId, currentPrice, increment, bidder);
  }

  [Test]
  public void ShouldReportInvalidFieldWhenAuctionIdIsMissing()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().Remove("AuctionId").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "AuctionId");
  }

  [Test]
  public void ShouldReportInvalidFieldWhenAuctionIdIsEmpty()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().SetItem("AuctionId", " ").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "AuctionId");
  }
  
  [Test]
  public void ShouldReportInvalidFieldWhenBidderIsMissing()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().Remove("Bidder").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "Bidder");
  }

  [Test]
  public void ShouldReportInvalidFieldWhenBidderIsEmpty()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().SetItem("Bidder", " ").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "Bidder");
  }


  [Test]
  public void ShouldReportInvalidFieldWhenCurrentPriceIsMissing()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().Remove("CurrentPrice").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "CurrentPrice");
  }

  [Test]
  public void ShouldReportInvalidFieldWhenCurrentPriceIsNotAnInteger()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().SetItem("CurrentPrice", Any.String())
      .Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "CurrentPrice");
  }

  [Test]
  public void ShouldReportInvalidFieldWhenIncrementIsMissing()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().Remove("Increment").Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "Increment");
  }

  [Test]
  public void ShouldReportInvalidFieldWhenIncrementIsNotAnInteger()
  {
    // GIVEN
    var auctionEventListener = Substitute.For<IAuctionEventListener>();
    var valuesByKey = ValidDictionary().ToBuilder().SetItem("Increment", Any.String())
      .Build();

    // WHEN
    new OnNewPriceAction().Execute(auctionEventListener, valuesByKey);

    // THEN
    auctionEventListener.ReceivedOnly(1).OnInvalidField("PRICE", "Increment");
  }

  private static ValueDictionary<string, string> ValidDictionary()
  {
    return new ValueDictionaryBuilder<string, string>()
      .Add("AuctionId", Any.String())
      .Add("CurrentPrice", Any.Integer().ToString())
      .Add("Increment", Any.Integer().ToString())
      .Add("Bidder", Any.String()).Build();
  }
}