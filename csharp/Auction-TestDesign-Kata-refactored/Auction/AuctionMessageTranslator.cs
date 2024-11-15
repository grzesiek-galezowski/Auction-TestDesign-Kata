using Core.Maybe;

namespace Auction;

public class AuctionMessageTranslator(IAuctionEventListener listener, List<IMessageAction> actions, XmppParser xmppParser)
{
  public static AuctionMessageTranslator CreateInstance(IAuctionEventListener listener)
  {
    return new AuctionMessageTranslator(listener,
      [new OnCloseAction(), new OnNewPriceAction(), new UnknownMessageAction()], new XmppParser());
  }

  public void ProcessMessage(string message)
  {
    var data = xmppParser.ConvertToDictionary(message);

    if (data is { IsParseError: true })
    {
      listener.OnParseError();
    }
    else
    {
      var valuesByKey = data.ValuesByKey;

      actions
        .FirstMaybe(a => a.Matches(valuesByKey))
        .Do(a => a.Execute(listener, valuesByKey));
    }
  }
}