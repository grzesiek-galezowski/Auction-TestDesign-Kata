using Badeend.ValueCollections;

namespace Auction;

public interface IMessageAction
{
  void Execute(IAuctionEventListener auctionEventListener, ValueDictionary<string, string> valuesByKey);
  bool Matches(string eventName);
}