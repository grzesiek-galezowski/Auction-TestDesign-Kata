namespace Auction;

public class AuctionMessageTranslator(IAuctionEventListener listener)
{
  public void ProcessMessage(string message)
  {
    if (message.Contains("CLOSE"))
    {
      listener.OnAuctionClosed();
    }
    else if (message.Contains("PRICE"))
    {
      var data = ParseMessageData(message);

      if (data.IsParseError)
      {
        listener.OnParseError("PRICE", "Content");
        return;
      }

      data.Value.TryGetValue("CurrentPrice", out var currentPriceString);
      data.Value.TryGetValue("Increment", out var incrementString);
      data.Value.TryGetValue("Bidder", out var bidder);

      if (!int.TryParse(currentPriceString, out var currentPrice))
      {
        listener.OnParseError("PRICE", "CurrentPrice");
        return;
      }
      if (!int.TryParse(incrementString, out var increment))
      {
        listener.OnParseError("PRICE", "Increment");
        return;
      }
      if (string.IsNullOrWhiteSpace(bidder))
      {
        listener.OnParseError("PRICE", "Bidder");
        return;
      }

      listener.OnBidDetails(currentPrice, increment, bidder);
    }
    else
    {
      listener.OnUnknownMessage();
    }
  }

  private static (bool IsParseError, Dictionary<string, string> Value) ParseMessageData(string message)
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