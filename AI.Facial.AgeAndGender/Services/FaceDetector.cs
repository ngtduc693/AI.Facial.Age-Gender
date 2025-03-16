using AI.Facial.AgeAndGender.Helpers;
using AI.Facial.AgeAndGender.Models;
using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Dnn;
using System.Drawing;

namespace AI.Facial.AgeAndGender;

internal class FaceDetector
{
    private const int RequiredSize = 320;

    public List<Mat> DetectFaces(byte[] imageBytes, Configuration configuration)
    {
        using Mat image = new();

        CvInvoke.Imdecode(imageBytes, ImreadModes.Color, image);

        if (image.IsEmpty)
        {
            throw new Exception(ErrorMessage.IMG_COULD_LOAD);
        }
        if (image.Width != image.Height)
        {
            throw new Exception(ErrorMessage.IMG_INVALID_RATIO +
                              $"{image.Width}x{image.Height}");
        }
        Mat processedImage;
        if (image.Width != RequiredSize || image.Height != RequiredSize)
        {
            processedImage = new Mat();
            CvInvoke.Resize(image, processedImage, new Size(RequiredSize, RequiredSize));
        }
        else
        {
            processedImage = image;
        }

        using FaceDetectorYN model = InitializeFaceDetectionModel(new Size(RequiredSize, RequiredSize), configuration);
        Mat faces = new();
        _ = model.Detect(processedImage, faces);

        List<Mat> facesList = CropFaces(processedImage, faces);

        if (processedImage != image)
        {
            processedImage.Dispose();
        }
        return facesList;
    }

    private FaceDetectorYN InitializeFaceDetectionModel(Size inputSize, Configuration configuration)
    {
        return new FaceDetectorYN(
            model: Utils.ExtractEmbeddedResource("AI.Facial.AgeAndGender.Resources.detectionv2.onnx"),
            config: string.Empty,
            inputSize: inputSize,
            scoreThreshold: configuration.Threshold,
            nmsThreshold: configuration.NmsThreshold,
            topK: configuration.TopK,
            backendId: Emgu.CV.Dnn.Backend.Default,
            targetId: configuration.TargetHadware);
    }

    private List<Mat> CropFaces(Mat image, Mat faces, int padding = 2)
    {
        List<Mat> faceImage = new();
        if (faces == null || faces.IsEmpty || faces.Rows <= 0)
        {
            throw new Exception(ErrorMessage.IMG_NO_FACE);
        }

        float[,] facesData = (float[,])faces.GetData(jagged: true);

        for (int i = 0; i < facesData.GetLength(0); i++)
        {
            int x = (int)facesData[i, 0];
            int y = (int)facesData[i, 1];
            int width = (int)facesData[i, 2];
            int height = (int)facesData[i, 3];
            _ = facesData[i, 14];

            x = Math.Max(0, x - padding);
            y = Math.Max(0, y - padding);
            width = Math.Min(image.Width - x, width + 2 * padding);
            height = Math.Min(image.Height - y, height + 2 * padding);

            Rectangle faceRectangle = new(x, y, width, height);
            faceImage.Add(new Mat(image, faceRectangle));
        }
        return faceImage;
    }
}