using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Interface;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AI.Facial.AgeAndGender;

internal class AgeRecognizer : IAgeRecognizer
{
    private readonly InferenceSession _session;
    private readonly List<string> _ageList = new List<string> { "(0-6)", "(8-12)", "(15-20)", "(25-32)", "(33-48)", "(49-59)", "(60-79)", "(80-100)" };

    public AgeRecognizer()
    {
        _session = new InferenceSession(Utils.LoadEmbeddedResource("AI.Facial.AgeAndGender.Resources.age.onnx"));
    }

    public string PredictAge(Mat faceImage)
    {
        DenseTensor<float> inputTensor = Recognizer.PreprocessFace(faceImage);
        List<NamedOnnxValue> inputs = new()
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor)
        };

        using var results = _session.Run(inputs);
        float[] scores = results.First().AsTensor<float>().ToArray();
        int maxIndex = Array.IndexOf(scores, scores.Max());
        return _ageList[maxIndex];
    }
}