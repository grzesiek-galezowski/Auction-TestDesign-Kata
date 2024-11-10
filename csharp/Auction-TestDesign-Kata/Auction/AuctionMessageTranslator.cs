using Core.Maybe;

namespace Auction;

public class AuctionMessageTranslator(IAuctionEventListener listener)
{
  public void ProcessMessage(string message)
  {
    var data = ParseMessageData(message);
    
    if (data.IsParseError)
    {
      listener.OnParseError();
    }
    else
    {
      var valuesByKey = data.ValuesByKey;

      var maybeEventType = valuesByKey.Lookup("Event");
      if (maybeEventType == "CLOSE".Just())
      {
        valuesByKey.TryGetValue("AuctionId", out var auctionId);

        if (string.IsNullOrWhiteSpace(auctionId))
        {
          listener.OnInvalidField("CLOSE", "AuctionId");
        }
        else
        {
          listener.OnAuctionClosed(auctionId);
        }
      }
      else if (maybeEventType == "PRICE".Just())
      {
        valuesByKey.TryGetValue("AuctionId", out var auctionId);
        valuesByKey.TryGetValue("CurrentPrice", out var currentPriceString);
        valuesByKey.TryGetValue("Increment", out var incrementString);
        valuesByKey.TryGetValue("Bidder", out var bidder);

        if (!int.TryParse(currentPriceString, out var currentPrice))
        {
          listener.OnInvalidField("PRICE", "CurrentPrice");
          return;
        }

        if (!int.TryParse(incrementString, out var increment))
        {
          listener.OnInvalidField("PRICE", "Increment");
          return;
        }

        if (string.IsNullOrWhiteSpace(bidder))
        {
          listener.OnInvalidField("PRICE", "Bidder");
          return;
        }

        if (string.IsNullOrWhiteSpace(auctionId))
        {
          listener.OnInvalidField("PRICE", "AuctionId");
          return;
        }

        listener.OnBidDetails(auctionId, currentPrice, increment, bidder);
      }
      else
      {
        listener.OnUnknownMessage();
      }
    }
  }

  private static (bool IsParseError, Dictionary<string, string> ValuesByKey) ParseMessageData(string message)
  {
    var data = new Dictionary<string, string>();
    foreach (var element in message.Split(";", StringSplitOptions.RemoveEmptyEntries))
    {
      var pair = element.Split(":");
      if (pair.Length != 2)
      {
        return (true, new Dictionary<string, string>());
      }

      data[pair[0].Trim()] = pair[1].Trim();
    }

    return (false, data);
  }
}