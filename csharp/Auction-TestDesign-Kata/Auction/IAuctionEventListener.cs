namespace Auction;

public interface IAuctionEventListener
{
  void OnAuctionClosed(string s);
  void OnBidDetails(string auctionId, int price, int increment, string bidder);
  void OnUnknownMessage();
  void OnParseError();
  void OnInvalidField(string messageName, string fieldName);
}