using Badeend.ValueCollections;

namespace Auction;

public class ParseFailureAction : IMessageAction
{
  public void Execute(IAuctionEventListener auctionEventListener, ValueDictionary<string, string> valuesByKey)
  {
    auctionEventListener.OnParseError();
  }

  public bool Matches(string eventName)
  {
    return eventName == "PARSE_FAILURE";
  }
}