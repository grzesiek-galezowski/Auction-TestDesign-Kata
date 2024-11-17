using Badeend.ValueCollections;

namespace Auction;

public class XmppParser : IXmppParser
{
  public ParseResult ConvertToDictionary(string message)
  {
    var data = new ValueDictionaryBuilder<string, string>();
    var elements = message.Split(";", StringSplitOptions.RemoveEmptyEntries);
    foreach (var element in elements)
    {
      var pair = element.Split(":");
      if (pair.Length != 2)
      {
        return ParseResult.Failure;
      }

      data = data.Add(pair[0].Trim(), pair[1].Trim());
    }

    var valuesByKey = data.Build();

    if (!valuesByKey.ContainsKey("Event"))
    {
      return ParseResult.Unknown;
    }

    return ParseResult.Success(valuesByKey, valuesByKey["Event"]);
  }
}