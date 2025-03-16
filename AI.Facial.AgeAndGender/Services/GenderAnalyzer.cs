using AI.Facial.AgeAndGender.Interface;
using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Models;
using Emgu.CV.Dnn;
using AI.Facial.AgeAndGender;
namespace AI.Facial.AgeAndGender;

public class GenderAnalyzer : IGenderAnalyzer
{
    private readonly FaceDetector _faceDetector;
    private readonly GenderRecognizer _genderRecognizer;
    private readonly Configuration _configuration;

    public GenderAnalyzer()
    {
        _faceDetector = new FaceDetector();
        _genderRecognizer = new GenderRecognizer();
        _configuration = new Configuration() { Threshold = 0.5f, NmsThreshold = 0.3f, TopK = 5000, TargetHadware = Target.Cpu };
    }
    public GenderAnalyzer(Configuration configuration)
    {
        _faceDetector = new FaceDetector();
        _genderRecognizer = new GenderRecognizer();
        _configuration = configuration;
    }

    public async Task<string> AnalyzeGenderFromUrlAsync(string imageUrl)
    {
        using HttpClient httpClient = new();
        byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        return Analyze(imageBytes);
    }

    public Task<string> AnalyzeGenderFromBase64Async(string base64Image)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        return Task.FromResult(Analyze(imageBytes));
    }

    public async Task<string> AnalyzeGenderFromStreamAsync(Stream fileStream)
    {
        using MemoryStream memoryStream = new();
        await fileStream.CopyToAsync(memoryStream);
        return Analyze(memoryStream.ToArray());
    }

    private string Analyze(byte[] imageData)
    {
        var detectedFaces = _faceDetector.DetectFaces(imageData, _configuration);

        if (detectedFaces.Count == 0)
        {
            throw new Exception(ErrorMessage.IMG_NO_FACE);
        }

        var faceBox = detectedFaces.First();

        return _genderRecognizer.PredictGender(faceBox);
    }
}