using Auction;
using Badeend.ValueCollections;
using FluentAssertions;
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
    var message = "SOLVersion: 1.1; Event: CLOSE; AuctionId: 1";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnAuctionClosed("1");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenCloseMessageMissingAuctionIdValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: CLOSE; AuctionId: ;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("CLOSE", "AuctionId");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenCloseMessageWithoutAuctionIdFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: CLOSE;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("CLOSE", "AuctionId");
  }

  [Test]
  public void NotifiesBidDetailsWhenPriceMessageReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnBidDetails("1", 192, 7, "Someone else");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageMissingCurrentPriceValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: ; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageMissingAuctionIdValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: ; CurrentPrice: 5; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "AuctionId");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithoutAuctionIdFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; CurrentPrice: 5; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "AuctionId");
  }
  
  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithInvalidCurrentPriceIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: Johnny; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithoutPriceFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; Increment: 7; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "CurrentPrice");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageMissingIncrementValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Increment: ; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "Increment");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithoutIncrementFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "Increment");
  }

  [Test]
  public void NotifiesContentParseErrorTooManyFieldSeparators()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; ; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnParseError();
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithInvalidIncrementIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Increment: JOHNNY; Bidder: Someone else;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "Increment");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageMissingBidderValueIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Increment: 7; Bidder: ;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "Bidder");
  }

  [Test]
  public void NotifiesOnInvalidFieldWhenPriceMessageWithoutBidderFieldIsReceived()
  {
    //GIVEN
    var message = "SOLVersion: 1.1; Event: PRICE; AuctionId: 1; CurrentPrice: 192; Increment: 7;";
    var listener = Substitute.For<IAuctionEventListener>();
    var translator = new AuctionMessageTranslator(listener);

    //WHEN
    translator.ProcessMessage(message);

    //THEN
    listener.ReceivedOnly(1).OnInvalidField("PRICE", "Bidder");
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
    listener.ReceivedOnly(1).OnParseError();
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