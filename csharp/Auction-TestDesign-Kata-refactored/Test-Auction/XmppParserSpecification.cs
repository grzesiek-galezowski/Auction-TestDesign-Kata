using Auction;
using FluentAssertions;
using Badeend.ValueCollections;

namespace Test_Auction_TestDesign_Kata;

[TestFixture]
public class XmppParserSpecification
{
  [Test]
  public void ShouldParseValidMessageWithSingleKeyValuePair()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "Event: CLOSE";
    var expectedDict = new ValueDictionaryBuilder<string, string>()
        .Add("Event", "CLOSE")
        .Build();

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Success(expectedDict));
  }

  [Test]
  public void ShouldParseValidMessageWithMultipleKeyValuePairs()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "Event: PRICE; CurrentPrice: 100; Increment: 10; Bidder: Someone";
    var expectedDict = new ValueDictionaryBuilder<string, string>()
        .Add("Event", "PRICE")
        .Add("CurrentPrice", "100")
        .Add("Increment", "10")
        .Add("Bidder", "Someone")
        .Build();

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Success(expectedDict));
  }

  [Test]
  public void ShouldHandleWhitespaceAroundKeysAndValues()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "  Event  :  CLOSE  ;  AuctionId  :  123  ";
    var expectedDict = new ValueDictionaryBuilder<string, string>()
        .Add("Event", "CLOSE")
        .Add("AuctionId", "123")
        .Build();

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Success(expectedDict));
  }

  [Test]
  public void ShouldReturnFailureForMissingColon()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "Event CLOSE";

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Failure);
  }

  [Test]
  public void ShouldReturnFailureForExtraColon()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "Event: CLOSE: extra";

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Failure);
  }

  [Test]
  public void ShouldHandleEmptyMessage()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = "";

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Success(ValueDictionary<string, string>.Empty));
  }

  [Test]
  public void ShouldHandleMessageWithOnlySemicolons()
  {
    // GIVEN
    var parser = new XmppParser();
    var message = ";;;";

    // WHEN
    var result = parser.ConvertToDictionary(message);

    // THEN
    result.Should().Be(ParseResult.Success(ValueDictionary<string, string>.Empty));
  }
}
