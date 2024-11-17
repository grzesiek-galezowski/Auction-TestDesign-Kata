using Core.Maybe;

namespace Auction;

public class AuctionMessageTranslator(
  IAuctionEventListener listener,
  List<IMessageAction> actions,
  IXmppParser xmppParser)
{
  public static AuctionMessageTranslator CreateInstance(IAuctionEventListener listener)
  {
    return new AuctionMessageTranslator(listener,
    [
      new ParseFailureAction(),
      new OnCloseAction(),
      new OnNewPriceAction(),
      new UnknownMessageAction()
    ], new XmppParser());
  }

  public void ProcessMessage(string message)
  {
    var result = xmppParser.ConvertToDictionary(message);

    actions
      .FirstMaybe(a => a.Matches(result.EventType))
      .Do(a => a.Execute(listener, result.ValuesByKey));
  }
}