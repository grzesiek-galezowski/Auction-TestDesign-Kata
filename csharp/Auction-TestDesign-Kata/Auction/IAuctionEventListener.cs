namespace Auction;

public interface IAuctionEventListener
{
  // TODO: add methods here
  void OnAuctionClosed();
  void OnBidDetails(int price, int increment, string bidder);
  void OnUnknownMessage();
  void OnParseError(string messageName, string invalidField);
}