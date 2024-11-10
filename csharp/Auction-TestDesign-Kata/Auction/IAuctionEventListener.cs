namespace Auction;

public interface IAuctionEventListener
{
  // TODO: add methods here
  void OnAuctionClosed();
  void OnBidDetails(int price, int increment, string bidder);
  void OnUnknownMessage();
  void OnParseError(); //bug
  void OnInvalidField(string messageName, string fieldName);
}