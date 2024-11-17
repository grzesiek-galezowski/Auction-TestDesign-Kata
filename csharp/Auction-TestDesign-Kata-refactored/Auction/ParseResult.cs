using Badeend.ValueCollections;

namespace Auction;

public record struct ParseResult(ValueDictionary<string, string> ValuesByKey, string EventType)
{
  public static ParseResult Failure { get; } = new(ValueDictionary<string, string>.Empty, "PARSE_FAILURE");
  public static ParseResult Unknown { get; } = new(ValueDictionary<string, string>.Empty, string.Empty);
  public static ParseResult Success(ValueDictionary<string, string> valuesByKey, string eventType) =>
    new(valuesByKey, eventType);
}