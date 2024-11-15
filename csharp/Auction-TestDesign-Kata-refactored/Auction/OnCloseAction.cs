using Badeend.ValueCollections;
using Core.Maybe;

namespace Auction;

public class OnCloseAction : IMessageAction
{
  public void Execute(IAuctionEventListener auctionEventListener, ValueDictionary<string, string> valuesByKey)
  {
    valuesByKey.TryGetValue("AuctionId", out var auctionId);

    if (string.IsNullOrWhiteSpace(auctionId))
    {
      auctionEventListener.OnInvalidField("CLOSE", "AuctionId");
    }
    else
    {
      auctionEventListener.OnAuctionClosed(auctionId);
    }
  }

  public bool Matches(ValueDictionary<string, string> valuesByKey)
  {
    return valuesByKey.Lookup("Event") == "CLOSE".Just();
  }
}