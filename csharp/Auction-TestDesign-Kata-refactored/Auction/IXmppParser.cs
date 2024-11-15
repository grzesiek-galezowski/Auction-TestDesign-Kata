namespace Auction;

public interface IXmppParser
{
  ParseResult ConvertToDictionary(string message);
}
