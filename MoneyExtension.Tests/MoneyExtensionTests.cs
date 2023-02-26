namespace MoneyExtension.Tests;

[TestClass]
public class MoneyExtensionTests
{
    [TestMethod]
    public void ShouldConvertDecimalToInt()
    {
        decimal valor = 279.98M;
        var cents = valor.ToCents();

        // Faz uma asserção para verificar o resultado do teste se é verdadeiro.
        Assert.AreEqual(27998, cents);
    }
}