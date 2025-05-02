namespace MoneyTracker.Test.Common;

using MoneyTracker.Domain.Common;
using MoneyTracker.Domain.Entities.Config;
using Xunit;

public class TextEncryptorTest
{
    private readonly MoneyTrackerSettings mockSettings;
    private readonly TextEncryptor textEncryptor;

    public TextEncryptorTest()
    {
        mockSettings = new MoneyTrackerSettings
        {
            Encryptor = new MoneyTrackerSettings.EncryptorModel
            {
                Key = "11111111111111111111111111111111",
                IV = "1111111111111111"
            }
        };

        textEncryptor = new TextEncryptor(mockSettings);
    }

    [Fact]
    public void Encrypt_ShouldReturnEncryptedString()
    {
        var plainText = "Hello, World!";

        var encryptedText = textEncryptor.Encrypt(plainText);

        Assert.NotNull(encryptedText);
        Assert.NotEqual(plainText, encryptedText);
    }

    [Fact]
    public void Encrypt_ShouldThrowException_WhenKeyIsNull()
    {
        mockSettings.Encryptor.Key = null;

        Assert.Throws<ArgumentNullException>(() => new TextEncryptor(mockSettings));
    }

    [Fact]
    public void Encrypt_ShouldThrowException_WhenIVIsNull()
    {
        mockSettings.Encryptor.IV = null;

        Assert.Throws<ArgumentNullException>(() => new TextEncryptor(mockSettings));
    }
}