using Badeend.ValueCollections;

namespace Auction;

public class UnknownMessageAction : IMessageAction
{
  public void Execute(IAuctionEventListener auctionEventListener, ValueDictionary<string, string> valuesByKey)
  {
    auctionEventListener.OnUnknownMessage();
  }

  public bool Matches(ValueDictionary<string, string> valuesByKey)
  {
    return true;
  }
}