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

      var currentPriceString = data["CurrentPrice"];
      var incrementString = data["Increment"];
      if (string.IsNullOrWhiteSpace(currentPriceString))
      {
        listener.OnParseError("PRICE", "CurrentPrice");
        return;
      }

      var currentPrice = int.Parse(currentPriceString);
      var increment = int.Parse(incrementString);
      var bidder = data["Bidder"];

      listener.OnBidDetails(currentPrice, increment, bidder);
    }
    else
    {
      listener.OnUnknownMessage();
    }
  }

  private static Dictionary<string, string> ParseMessageData(string message)
  {
    var data = new Dictionary<string, string>();
    foreach (var element in message.Split(";", StringSplitOptions.RemoveEmptyEntries))
    {
      var pair = element.Split(":");
      data[pair[0].Trim()] = pair[1].Trim();
    }

    return data;
  }
}