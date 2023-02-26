# Nuget e GitHub Packages

Projeto para aprendizado e revisão de conceitos.

## CI e CD

As siglas representam Continuous Integration e Continuos Delivery.

Estamos falando sobre o processo de manter a aplicação integra e funcional, com um fluxo de entrega rápido para publicação (deployment).

# Nuget

O [Nuget](https://www.nuget.org/) é o gerenciador de pacotes oficial para que usa a tecnologia .NET

Aqui podemos armazenar e publicar pacotes de forma publica ou privada para uso em diversos projetos.

# Criando Solução

Crie uma nova pasta e depois crie uma biblioteca (lib) e um projeto de testes

```Csharp
mkdir MoneyExtension

// Cria um projeto do tipo lib
dotnet new classlib -o MoneyExtension

// Cria um projeto para realizar testes na lib
dotnet new mstest -o MoneyExtension.Tests

// Cria uma solution para organizar e agrupar os projetos
dotnet new sln

// Adiciona os projetos a solution
dotnet sln add .\MoneyExtension\
dotnet sln add .\MoneyExtension.Tests\

// Acessa o projeto de testes e adiciona uma referencia para o projeto lib
cd .\MoneyExtension.Tests\
dotnet add reference ..\MoneyExtension\

// Constroi (build) a solução com as configurações especificadas. Analise se está tudo de acordo.
dotnet build
```

Para testar o pacote, faça como no exemplo:

Crie a classe com o metódo para ser executado.

```Csharp
namespace MoneyExtension;

public static class Money
{
    public static int ToCents(this decimal amount)
    {
        if (amount <= 0) return 0;

        var text = amount
            .ToString("N2")
            .Replace(",", "")
            .Replace(".", "");

        if (string.IsNullOrEmpty(text)) return 0;

        int.TryParse(text, out var result);
        return result;
    }
}
```

Crie a classe de teste

```Csharp
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
```

Agora use o comando **dotnet test** dentro do projeto MoneyExtension.Tests.

Esse comando executa o **Assert** que valida o se o comportamento esperado retorna verdadeiro.
