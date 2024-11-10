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
      return;
    }

    var maybeEventType = data.ValuesByKey.Lookup("Event");
    if (maybeEventType == "CLOSE".Just())
    {
      listener.OnAuctionClosed();
    }
    else if (maybeEventType == "PRICE".Just())
    {
      data.ValuesByKey.TryGetValue("CurrentPrice", out var currentPriceString);
      data.ValuesByKey.TryGetValue("Increment", out var incrementString);
      data.ValuesByKey.TryGetValue("Bidder", out var bidder);

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

      listener.OnBidDetails(currentPrice, increment, bidder);
    }
    else
    {
      listener.OnUnknownMessage();
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