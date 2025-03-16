using Emgu.CV;

namespace AI.Facial.AgeAndGender.Interface;

internal interface IGenderRecognizer
{
    string PredictGender(Mat faceImage);
}