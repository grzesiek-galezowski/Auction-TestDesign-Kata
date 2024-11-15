using Badeend.ValueCollections;

namespace Auction;

public interface IXmppParser
{
  ParseResult ConvertToDictionary(string message);
}

public class XmppParser : IXmppParser
{
  public ParseResult ConvertToDictionary(string message)
  {
    var data = new ValueDictionaryBuilder<string, string>();
    foreach (var element in message.Split(";", StringSplitOptions.RemoveEmptyEntries))
    {
      var pair = element.Split(":");
      if (pair.Length != 2)
      {
        return ParseResult.Failure;
      }

      data = data.Add(pair[0].Trim(), pair[1].Trim());
    }

    return ParseResult.Success(data.Build());
  }
}