using Xunit;
using NewtonsoftJson = Newtonsoft.Json;
using SystemTextJson = System.Text.Json;
using SystemTextJsonSerialization = System.Text.Json.Serialization;

namespace JsonNetDeserialization;

public class Item
{
    public required string SerialNo { get; set; }
    public required string AffiliationOrgCode { get; set; }
}

public class Items
{
    public required List<Item> Item { get; set; }
}

public class Root
{
    [SystemTextJsonSerialization.JsonPropertyName("mt_items")]
    [NewtonsoftJson.JsonProperty("mt_items")]
    public required Items Items { get; set; }
}

public class DeserializationTests
{
    private const string JsonString = @"
                {
                  ""mt_items"": {
                    ""item"": [
                      {
                        ""serialNo"": ""000000000002200878"",
                        ""affiliationOrgCode"": ""OrgCode1""
                      },
                      {
                        ""serialNo"": ""000000000002201675"",
                        ""affiliationOrgCode"": ""OrgCode1""
                      }
                    ]
                  }
                }
            ";

    [Fact]
    public void Simple_Deserialization_With_NewtonsoftJson()
    {
        var deserializedClass = NewtonsoftJson.JsonConvert.DeserializeObject<Root>(JsonString);

        Assert.NotNull(deserializedClass);
        Assert.Equal("000000000002201675", deserializedClass.Items.Item[1].SerialNo);
    }

    [Fact]
    public void Simple_Deserialization_With_SystemTextJson()
    {
        var options = new SystemTextJson.JsonSerializerOptions
        {
            PropertyNamingPolicy = SystemTextJson.JsonNamingPolicy.CamelCase
        };
        var deserializedClass = SystemTextJson.JsonSerializer.Deserialize<Root>(JsonString, options);

        Assert.NotNull(deserializedClass);
        Assert.Equal("000000000002201675", deserializedClass.Items.Item[1].SerialNo);
    }
}