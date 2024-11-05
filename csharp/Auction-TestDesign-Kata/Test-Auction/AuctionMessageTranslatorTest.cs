using Auction;
using NSubstitute;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;

namespace Test_Auction_TestDesign_Kata;

public class AuctionMessageTranslatorTest
{
  [Test]
  public void NotifiesAuctionClosedWhenCloseMessageReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: CLOSE;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnAuctionClosed();
  }

  [Test]
  public void NotifiesBidDetailsWhenPriceMessageReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnBidDetails(192, 7, "Someone else");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageMissingCurrentPriceIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: ; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageMissingIncrementIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: ; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "Increment");
  }

  [Test]
  public void NotifiesUnknownMessageMalformedMessageReceived()
  {
    //GIVEN
    var message = Any.String();
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnUnknownMessage();
  }
}