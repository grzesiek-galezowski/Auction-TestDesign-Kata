using Auction;
using Badeend.ValueCollections;
using NSubstitute;
using TddXt.AnyRoot.Collections;
using TddXt.AnyRoot.Exploding;
using TddXt.AnyRoot.Strings;
using TddXt.XNSubstitute;

namespace Test_Auction_TestDesign_Kata;

public class AuctionMessageTranslatorSpecification
{
  [Test]
  public void ShouldNotifyListenerAboutParseErrorWhenParserReturnsFailure()
  {
    // GIVEN
    var message = Any.String();
    var listener = Substitute.For<IAuctionEventListener>();
    var parser = Substitute.For<IXmppParser>();
    var translator = new AuctionMessageTranslator(
      listener,
      [Any.Exploding<IMessageAction>(), Any.Exploding<IMessageAction>(), Any.Exploding<IMessageAction>()],
      parser);

    parser.ConvertToDictionary(message).Returns(ParseResult.Failure);

    // WHEN
    translator.ProcessMessage(message);

    // THEN
    listener.ReceivedOnly(1).OnParseError();
  }

  [Test]
  public void ShouldExecuteFirstMatchingAction()
  {
    // GIVEN
    var message = Any.String();
    var valuesByKey = AnyValueDictionary();
    var listener = Substitute.For<IAuctionEventListener>();
    var parser = Substitute.For<IXmppParser>();
    var action1 = Substitute.For<IMessageAction>();
    var action2 = Substitute.For<IMessageAction>();
    var action3 = Substitute.For<IMessageAction>();
    var translator = new AuctionMessageTranslator(
      listener,
      [action1, action2, action3],
      parser);
    parser.ConvertToDictionary(message).Returns(ParseResult.Success(valuesByKey));
    action1.Matches(valuesByKey).Returns(false);
    action2.Matches(valuesByKey).Returns(true);
    action3.Matches(valuesByKey).Returns(false);

    // WHEN
    translator.ProcessMessage(message);

    // THEN
    action2.Received(1).Execute(listener, valuesByKey);
    action1.ReceivedNoCommands();
    action3.ReceivedNoCommands();
  }

  [Test]
  public void ShouldExecuteFirstMatchingActionWhenMultipleActionsMatch()
  {
    // GIVEN
    var message = Any.String();
    var valuesByKey = AnyValueDictionary();
    var listener = Substitute.For<IAuctionEventListener>();
    var parser = Substitute.For<IXmppParser>();
    var action1 = Substitute.For<IMessageAction>();
    var action2 = Substitute.For<IMessageAction>();
    var action3 = Substitute.For<IMessageAction>();
    var translator = new AuctionMessageTranslator(
      listener,
      [action1, action2, action3],
      parser);

    parser.ConvertToDictionary(message).Returns(ParseResult.Success(valuesByKey));
    action1.Matches(valuesByKey).Returns(true);
    action2.Matches(valuesByKey).Returns(true);
    action3.Matches(valuesByKey).Returns(true);

    // WHEN
    translator.ProcessMessage(message);

    // THEN
    action1.Received(1).Execute(listener, valuesByKey);
    action2.ReceivedNoCommands();
    action3.ReceivedNoCommands();
  }

  [Test]
  public void ShouldNotExecuteAnyActionWhenNoActionMatches()
  {
    // GIVEN
    var message = Any.String();
    var valuesByKey = AnyValueDictionary();
    var listener = Substitute.For<IAuctionEventListener>();
    var parser = Substitute.For<IXmppParser>();
    var action1 = Substitute.For<IMessageAction>();
    var action2 = Substitute.For<IMessageAction>();
    var action3 = Substitute.For<IMessageAction>();
    var translator = new AuctionMessageTranslator(
      listener,
      [action1, action2, action3],
      parser);
    parser.ConvertToDictionary(message).Returns(ParseResult.Success(valuesByKey));
    action1.Matches(valuesByKey).Returns(false);
    action2.Matches(valuesByKey).Returns(false);
    action3.Matches(valuesByKey).Returns(false);

    // WHEN
    translator.ProcessMessage(message);

    // THEN
    action1.ReceivedNoCommands();
    action2.ReceivedNoCommands();
    action3.ReceivedNoCommands();
  }

  private static ValueDictionary<string, string> AnyValueDictionary()
  {
    return new ValueDictionaryBuilder<string, string>(Any.Dictionary<string, string>()).Build();
  }
}
