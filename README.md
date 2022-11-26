# OpenAI

[![Discord](https://img.shields.io/discord/855294214065487932.svg?label=&logo=discord&logoColor=ffffff&color=7389D8&labelColor=6A7EC2)](https://discord.gg/xQgMW9ufN4)
[![openupm](https://img.shields.io/npm/v/com.openai.unity?label=openupm&registry_uri=https://package.openupm.com)](https://openupm.com/packages/com.openai.unity/)

A [OpenAI](https://openai.com/) package for the [Unity](https://unity.com/) Game Engine.

Based on [OpenAI-DotNet](https://github.com/RageAgainstThePixel/OpenAI-DotNet)

## Installing

### Via Unity Package Manager and OpenUPM

- Open your Unity project settings
- Add the OpenUPM package registry:
  - `Name: OpenUPM`
  - `URL: https://package.openupm.com`
  - `Scope(s):`
    - `com.openai`
    - `com.utilities`

![scoped-registries](OpenAI/Packages/com.openai.unity/Documentation~/images/package-manager-scopes.png)

- Open the Unity Package Manager window
- Change the Registry from Unity to `My Registries`
- Add the `OpenAI` package

### Via Unity Package Manager and Git url

- Open your Unity Package Manager
- Add package from git url: `https://github.com/RageAgainstThePixel/com.openai.unity.git#upm`

## Getting started

### Quick Start

Uses the default authentication from the current directory, the default user directory or system environment variables

```csharp
OpenAI api = new OpenAIClient(Engine.Davinci);
```

### Authentication

There are 3 ways to provide your API keys, in order of precedence:

1. Pass keys directly to `Authentication(string key)` constructor
2. Set environment variables
3. Include a config file in the local directory or in your user directory named `.openai` and containing the line:

```shell
OPENAI_KEY=sk-aaaabbbbbccccddddd
```

You use the `Authentication` when you initialize the API as shown:

#### If you want to provide a key manually

```csharp
OpenAI api = new OpenAIClient("sk-mykeyhere");
```

#### Create a `Authentication` object manually

```chsarp
OpenAI api = new OpenAIClient(new Authentication("sk-secretkey"));
```

#### Use System Environment Variables

> Use `OPENAI_KEY` or `OPENAI_SECRET_KEY` specify a key defined in the system's local environment:

```chsarp
OpenAI api = new OpenAIClient(Authentication LoadFromEnv());
```

#### Load key from specified directory

> Attempts to load api keys from a configuration file, by default `.openai` in the current directory, optionally traversing up the directory tree.

```chsarp
OpenAI api = new OpenAIClient(Authentication.LoadFromDirectory("C:\\MyProject"));;
```

### Completions

The Completion API is accessed via `OpenAI.CompletionEndpoint`:

```csharp
var result = await api.CompletionEndpoint.CreateCompletionAsync("One Two Three One Two", temperature: 0.1, engine: Engine.Davinci);
Console.WriteLine(result);
```

 Get the `CompletionResult` (which is mostly metadata), use its implicit string operator to get the text if all you want is the completion choice.

#### Streaming

Streaming allows you to get results are they are generated, which can help your application feel more responsive, especially on slow models like Davinci.

```csharp
var api = new OpenAIClient();
await api.CompletionEndpoint.StreamCompletionAsync(result =>
{
    foreach (var choice in result.Completions)
    {
        Console.WriteLine(choice);
    }
}, "My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", max_tokens: 200, temperature: 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1, engine: Engine.Davinci);
```

The result.Completions

Or if using [`IAsyncEnumerable{T}`](https://docs.microsoft.com/en-us/dotnet/api/system.collections.generic.iasyncenumerable-1?view=net-5.0) ([C# 8.0+](https://docs.microsoft.com/en-us/archive/msdn-magazine/2019/november/csharp-iterating-with-async-enumerables-in-csharp-8))

```csharp
var api = new OpenAIClient();
await foreach (var token in api.CompletionEndpoint.StreamCompletionEnumerableAsync("My name is Roger and I am a principal software engineer at Salesforce.  This is my resume:", max_tokens: 200, temperature: 0.5, presencePenalty: 0.1, frequencyPenalty: 0.1, engine: Engine.Davinci))
{
  Console.Write(token);
}
```

### Document Search

The Search API is accessed via `OpenAI.SearchEndpoint`:

#### You can get all results as a dictionary using

```csharp
var api = new OpenAIClient();
string query = "Washington DC";
string[] documents = { "Canada", "China", "USA", "Spain" };

Dictionary<string, double> results = await api.SearchEndpoint.GetSearchResultsAsync(query, documents, Engine.Curie);
// result["USA"] == 294.22
// result["Spain"] == 73.81
```

> The returned dictionary maps documents to scores.

#### You can get only the best match using

```csharp
var api = new OpenAIClient();
string query = "Washington DC";
string[] documents = { "Canada", "China", "USA", "Spain" };
string result = await api.SearchEndpoint.GetBestMatchAsync(query, documents, Engine.Curie);
// result == "USA"
```

> The returned document result string.

#### And if you only want the best match but still want to know the score, use

```csharp
var api = new OpenAIClient();
string query = "Washington DC";
string[] documents = { "Canada", "China", "USA", "Spain" };
Tuple<string, double> result = await await api.SearchEndpoint.GetBestMatchWithScoreAsync(query, documents, Engine.Curie);
// (result, score) == "USA", 294.22
```

> returned Tuple result with score

### Classifications

The Classification API is accessed via `OpenAI.ClassificationEndpoint`:

Given a query and a set of labeled examples, the model will predict the most likely label for the query.

```csharp
var api = new OpenAIClient();

string query = "It is a raining day :(";
string[] labels = { "Positive", "Negative", "Neutral" };
Dictionary<string, string> examples = new Dictionary<string, string>
{
    { "A happy moment", "Positive" },
    { "I am sad.", "Negative" },
    { "I am feeling awesome", "Positive"}
};

var result = await api.ClassificationEndpoint.CreateClassificationAsync(new ClassificationRequest(query, examples, labels));
// result.Label == "Negative"
```

### Image Generation

The Image Generation API is accessed via `OpenAI.ImageGenerationEndpoint`:

```csharp
var api = new OpenAIClient();
var results = await api.ImageGenerationEndPoint.GenerateImageAsync("A house riding a velociraptor", 1, ImageSize.Small);
var image = results[0];
// result == Texture2D generated image
```
