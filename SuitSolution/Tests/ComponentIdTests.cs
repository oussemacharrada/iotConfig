using Xunit;
using YourNamespace; // Replace with the correct namespace
using System.Collections.Generic;

public class SUITComponentIdTests
{
    [Fact]
    public void SUITComponentId_Should_Be_Valid_With_Random_Data()
    {
        // Arrange
        var suitComponentIdData = SUITComponentIdSeeder.GenerateRandomSUITComponentIdJson();

        // Act
        var suitComponentId = new SUITComponentId();

        
        Assert.NotNull(suitComponentIdData);
    }
    [Fact]
    public void SUITComponentId_Should_Serialize_And_Deserialize_Correctly()
    {
        // Arrange
        var suitComponentIdData = SUITComponentIdSeeder.GenerateRandomSUITComponentIdJson();
        var suitComponentId = new SUITComponentId();
            
        suitComponentId.FromJson(suitComponentIdData);

        // Act
        var serializedSUITComponentId = suitComponentId.ToJson();
        var deserializedSUITComponentId = new SUITComponentId();
        deserializedSUITComponentId.FromJson(serializedSUITComponentId);

        // Assert
        Assert.NotNull(deserializedSUITComponentId);
        Assert.Equal(suitComponentId, deserializedSUITComponentId);
    }
}
