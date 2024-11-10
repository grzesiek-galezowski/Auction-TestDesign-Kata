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
  public void NotifiesParseErrorWhenPriceMessageMissingCurrentPriceValueIsReceived()
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
  public void NotifiesParseErrorWhenPriceMessageWithInvalidCurrentPriceIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: Johnny; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageWithoutPriceFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageMissingIncrementValueIsReceived()
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
  public void NotifiesParseErrorWhenPriceMessageWithoutIncrementFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "Increment");
  }

  [Test]
  public void NotifiesContentParseErrorTooManyFieldSeparators()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; ; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("MESSAGE", "Content");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageWithInvalidIncrementIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: JOHNNY; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "Increment");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageMissingBidderValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7; Bidder: ;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "Bidder");
  }

  [Test]
  public void NotifiesParseErrorWhenPriceMessageWithoutBidderFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 192; Increment: 7;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("PRICE", "Bidder");
  }

  [Test]
  public void NotifiesOnParseErrorWhenMalformedMessageReceived()
  {
    //GIVEN
    var message = Any.String();
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError("MESSAGE", "Content");
  }

  [Test]
  public void NotifiesOnUnknownMessageWhenUnsupportedMessageTypeReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: LOL; CurrentPrice: 192; Increment: 7;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnUnknownMessage();
  }
}