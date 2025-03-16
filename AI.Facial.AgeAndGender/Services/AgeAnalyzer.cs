using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Interface;
using AI.Facial.AgeAndGender.Models;
using Emgu.CV.Dnn;
namespace AI.Facial.AgeAndGender;

public class AgeAnalyzer : IAgeAnalyzer
{
    private readonly FaceDetector _faceDetector;
    private readonly AgeRecognizer _ageRecognizer;
    private readonly Configuration _configuration;

    public AgeAnalyzer()
    {
        _faceDetector = new FaceDetector();
        _ageRecognizer = new AgeRecognizer();
        _configuration = new Configuration() { Threshold = 0.5f, NmsThreshold = 0.3f, TopK = 3000, TargetHadware = Target.Cpu };
    }
    public AgeAnalyzer(Configuration configuration)
    {
        _faceDetector = new FaceDetector();
        _ageRecognizer = new AgeRecognizer();
        _configuration = configuration;
    }

    public async Task<string> AnalyzeAgeFromUrlAsync(string imageUrl)
    {
        using HttpClient httpClient = new();
        byte[] imageBytes = await httpClient.GetByteArrayAsync(imageUrl);
        return Analyze(imageBytes);
    }

    public Task<string> AnalyzeAgeFromBase64Async(string base64Image)
    {
        byte[] imageBytes = Convert.FromBase64String(base64Image);
        return Task.FromResult(Analyze(imageBytes));
    }

    public async Task<string> AnalyzeAgeFromStreamAsync(Stream fileStream)
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

        return _ageRecognizer.PredictAge(faceBox);
    }
}