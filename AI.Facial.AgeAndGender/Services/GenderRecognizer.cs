using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Interface;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AI.Facial.AgeAndGender;

internal class GenderRecognizer : IGenderRecognizer
{
    private readonly InferenceSession _session;
    private readonly List<string> _genderList = new List<string> { "Male", "Female" };

    public GenderRecognizer()
    {
        _session = new InferenceSession(Utils.LoadEmbeddedResource("AI.Facial.AgeAndGender.Resources.gender.onnx"));
    }

    public string PredictGender(Mat faceImage)
    {
        DenseTensor<float> inputTensor = Recognizer.PreprocessFace(faceImage);
        List<NamedOnnxValue> inputs = new()
        {
            NamedOnnxValue.CreateFromTensor(_session.InputMetadata.Keys.First(), inputTensor)
        };

        using var results = _session.Run(inputs);
        float[] scores = results.First().AsTensor<float>().ToArray();
        int maxIndex = Array.IndexOf(scores, scores.Max());
        return _genderList[maxIndex];
    }
}