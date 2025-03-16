# AI.Facial.AgeAndGender

**AI.Facial.AgeAndGender** is a .NET library for analyzing facial attributes, including age and gender. It seamlessly integrates with C#.NET, providing efficient and secure facial analysis with embedded
AI models. Ideal for chatbots, customer insights, security, and healthcare applications.

![Illustration](https://raw.githubusercontent.com/ngtduc693/AI.Facial.Age-Gender/refs/heads/main/imgs/ageandgender.png)

## 🚀 Features

- Age and Gender Detection.
- Optimized for .NET – Fully compatible with .NET 6/8/9.
- **Easy Integration** – Works seamlessly with ASP.NET Web APIs, Windows Application, allowing quick integration into existing projects.
- Supports multiple input formats: **URL, Base64, File Stream**

## 📦 Installation

You can install this library via NuGet Package Manager:

```bash
dotnet add package AI.Facial.AgeAndGender
```

## 📦 Mandatory ingredients

- If your server runs windows operating system

```bash
dotnet add package Emgu.CV.runtime.windows
```

- If your server runs ubuntu operating system

```bash
dotnet add package Emgu.CV.runtime.ubuntu-x64
```


## ⚡ Usage

For example

1️⃣ Analyze Age from an image URL

````csharp
using AI.Facial.AgeAndGender;

var analyzer = new AgeAnalyzer();
var result = await analyzer.AnalyzeAgeFromUrlAsync("https://example.com/image.jpg");

Console.WriteLine($"Age: {result}");
````

2️⃣ Analyze Gender from a Base64 image string

````csharp
var base64Image = "iVBORw0KGgoAAAANSUhEUgAA...";
var result = await analyzer.AnalyzeGenderFromBase64Async(base64Image);

Console.WriteLine($"Gender: {result}");
````

3️⃣ Analyze Age and Gender from a file stream

````csharp
using var fileStream = File.OpenRead("image.jpg");
var ageResult = await analyzer.AnalyzeAgeFromStreamAsync(fileStream);
var genderResult = await analyzer.AnalyzeGenderFromStreamAsync(fileStream);

Console.WriteLine($"Age: {ageResult} - Gender: {genderResult}");
````

## Contact

For any questions, feel free to contact me or create an issue in the repository.