using Badeend.ValueCollections;
using Core.Maybe;

namespace Auction;

public class OnNewPriceAction : IMessageAction
{
  public void Execute(IAuctionEventListener auctionEventListener, ValueDictionary<string, string> valuesByKey)
  {
    valuesByKey.TryGetValue("AuctionId", out var auctionId);
    valuesByKey.TryGetValue("CurrentPrice", out var currentPriceString);
    valuesByKey.TryGetValue("Increment", out var incrementString);
    valuesByKey.TryGetValue("Bidder", out var bidder);

    if (!int.TryParse(currentPriceString, out var currentPrice))
    {
      auctionEventListener.OnInvalidField("PRICE", "CurrentPrice");
      return;
    }

    if (!int.TryParse(incrementString, out var increment))
    {
      auctionEventListener.OnInvalidField("PRICE", "Increment");
      return;
    }

    if (string.IsNullOrWhiteSpace(bidder))
    {
      auctionEventListener.OnInvalidField("PRICE", "Bidder");
      return;
    }

    if (string.IsNullOrWhiteSpace(auctionId))
    {
      auctionEventListener.OnInvalidField("PRICE", "AuctionId");
      return;
    }

    auctionEventListener.OnBidDetails(auctionId, currentPrice, increment, bidder);
  }

  public bool Matches(ValueDictionary<string, string> valuesByKey)
  {
    return valuesByKey.Lookup("Event") == "PRICE".Just();
  }
}