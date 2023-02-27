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

Para enviar para o GitHub:

1. Execute o **git init** no diretorio do projeto.
1. Execute o **dotnet new gitignore** para criar o arquivo com excessões de arquivos e pastas.
1. Adicione o repositório remoto (crie no GitHub caso necessário). Use **git remote add origin url-do-repo**.
1. Adicione os arquivos e pastas para envio com **git add -all**.
1. Para verificar quais arquivos estão preparados para envio utilize **git status**.
1. Faça o commit para gravar as alterações **git commit -m "Edição Inicial"**.
1. Envie os arquivos para branch de destino com **git push -u origin main**.

## Melhores práticas Deploy

- Testes e Builds não devem ser enviados para produção.
- Esses processos devem ser executados em uma maquina temporaria para serem efetivos.
- O GitHub só deve conter os fontes.

## Iniciando o Workflow no GitHub

O GitHub Actions le o arquivos YML. Esse arquivo contém as instruções para automatizar processos como build, testes e deploy.

Crie um novo arquivo na raiz com o nome **.github/workflows/main.yml**

Esse tipo de arquivo deve seguir a regra de identação por espaços.

```Yaml
# Nome da ação a ser executada
# As actions default podem ser consultadas em https://github.com/actions
name: Build and deploy package

# Onde será executado
on:
  push:
    branches:
      - main

# Em cima de qual S.O. será executado
jobs:
  build-and-deploy:
    runs-on: ubuntu-latest

    # Passos para execução
    steps:
      - uses: actions/checkout@main

      - name: Set up .NET Core
        uses: actions/setup-dotnet@v2
        with:
          dotnet-version: '7.0.x' # Instala a versão do .NET

      - name: Build with dotnet
        run: dotnet build --configuration Release # Cria uma build com a versão de Release

      - name: Run test
        run: dotnet test # Executa o teste da aplicação. Se houver falha, encerra a execução

      - name: Create the package
        run: dotnet pack --configuration Release # Cria o pacote com a versão de Release

      - name: Publish # Publica o pacote no NuGet. O parametro -k retorna os segredos e -s envia para API NuGet.
        run: >- # a combinação > - faz a quebra de linha para comandos longos
             dotnet nuget push "MoneyExtension/bin/Release/*.nupkg"
             -k ${{ secrets.NUGET_TOKEN }} -s https://api.nuget.org/v3/index.json
```

Uma boa prática é executar os comandos de forma local, garantindo que está tudo funcionando.

As informações como versão de release, icones, briefing e outros devem ser configuradas no MoneyExtension.csproj

```xml
Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net7.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>

    <PackageId>MoneyExtension</PackageId>
    <Version>1.0.0</Version>
    <License>MIT</License>
    <Authors>Thiago</Authors>
    <Company>student</Company>
    <PackageDescription>Another conversor int to decimal (just test project)</PackageDescription>
    <Description>Another conversor int to decimal (just test project)</Description>
    <RepositoryUrl>https://github.com/thiagokj/balta-money-extensions</RepositoryUrl>
    <PackageProjectUrl>https://github.com/thiagokj/balta-money-extensions</PackageProjectUrl>
    <PackageLicenseExpression>MIT</PackageLicenseExpression>
    <PackageIcon>icon.png</PackageIcon>
    <PackageTags>money-extension-converter-int-decimal</PackageTags>
    <ReleaseNotes>Initial Release</ReleaseNotes>
    <Title>MoneyExtension</Title>
    <Summary>Another conversor int to decimal (just test project).</Summary>
  </PropertyGroup>

  <ItemGroup>
    <None Include="..\Media\icon.png" Pack="true" PackagePath="\"/>
  </ItemGroup>

</Project>
```

## GitHub Secrets

Acesse Settings -> Secrets and variables -> New repository secret.

Essa configuração do GitHub permite armazenar chaves de API e dados sensíveis para aplicação de forma segura e encriptada, sem expor de forma publica ao publicar os projetos. Ex: NUGET_TOKEN

As API keys podem ser criadas no https://www.nuget.org/account/apikeys
