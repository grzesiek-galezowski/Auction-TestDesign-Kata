using Badeend.ValueCollections;

namespace Auction;

public record struct ParseResult(bool IsParseError, ValueDictionary<string, string> ValuesByKey)
{
  public static ParseResult Failure { get; } = new(true, ValueDictionary<string, string>.Empty);

  public static ParseResult Success(ValueDictionary<string, string> valuesByKey)
  {
    return new ParseResult(false, valuesByKey);
  }
}