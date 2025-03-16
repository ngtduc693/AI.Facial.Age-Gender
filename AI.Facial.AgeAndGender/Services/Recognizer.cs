using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Interface;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Microsoft.ML.OnnxRuntime;
using Microsoft.ML.OnnxRuntime.Tensors;

namespace AI.Facial.AgeAndGender;

internal static class Recognizer
{    
    internal static DenseTensor<float> PreprocessFace(Mat faceImage)
    {
        Mat resizedImage = new Mat();
        CvInvoke.Resize(faceImage, resizedImage, new System.Drawing.Size(224, 224));

        Mat rgbImage = new Mat();
        if (faceImage.NumberOfChannels == 1)
        {
            CvInvoke.CvtColor(resizedImage, rgbImage, ColorConversion.Gray2Rgb);
        }
        else if (faceImage.NumberOfChannels == 3)
        {
            CvInvoke.CvtColor(resizedImage, rgbImage, ColorConversion.Bgr2Rgb);
        }
        else
        {
            throw new Exception("Unsupported image format.");
        }

        DenseTensor<float> tensor = new DenseTensor<float>(new[] { 1, 3, 224, 224 });
        float[] mean = new float[] { 104f, 117f, 123f };

        byte[] imageData = rgbImage.ToImage<Rgb, byte>().Bytes;
        int width = rgbImage.Width;
        int height = rgbImage.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = ((y * width) + x) * 3;
                tensor[0, 0, y, x] = imageData[index] - mean[0];
                tensor[0, 1, y, x] = imageData[index + 1] - mean[1];
                tensor[0, 2, y, x] = imageData[index + 2] - mean[2];
            }
        }

        resizedImage.Dispose();
        rgbImage.Dispose();

        return tensor;
    }
}