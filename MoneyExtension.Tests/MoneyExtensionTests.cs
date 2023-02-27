namespace MoneyExtension.Tests;

[TestClass]
public class MoneyExtensionTests
{
    [TestMethod]
    public void ShouldConvertDecimalToInt()
    {
        decimal valor = 279.98M;
        var cents = valor.ToCents();

        // Faz uma asserção para verificar se o resultado do teste é verdadeiro.
        Assert.AreEqual(27998, cents);
    }
}