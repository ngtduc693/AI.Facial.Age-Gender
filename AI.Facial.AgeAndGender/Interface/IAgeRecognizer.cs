using Emgu.CV;

namespace AI.Facial.AgeAndGender.Interface;

internal interface IAgeRecognizer
{
    string PredictAge(Mat faceImage);
}