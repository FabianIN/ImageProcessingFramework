using Emgu.CV.Structure;
using Emgu.CV;
using System;

namespace Algorithms.Sections
{
    public class Filters
    {

        #region Low Pass Filters

        #region Unsharp Mask

            public static Image<Gray, byte> UnsharpMaskImage(Image<Gray, byte> inputImage)
            {
            double[,] filtruGaussian = new double[,]
                {
                    { 0.018, 0.135, 0.018 },
                    { 0.135, 1.0, 0.135 },
                    { 0.018, 0.135, 0.018 }
                };

            Image<Gray, byte> filteredImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);

            double sum;
            filteredImage.Data = inputImage.Data;

            for (int y = 1; y < inputImage.Height - 1; y++)
            {
                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    sum = 0;
                    for (int i = -1; i <= 1; i++)
                    {
                        for (int j = -1; j <= 1; j++)
                        {
                            sum = sum + (inputImage.Data[y + i, x + j, 0] * filtruGaussian[i + 1, j + 1]);
                        }
                    }
                    filteredImage.Data[y, x, 0] = (byte)(inputImage.Data[y, x, 0] - Math.Round(1 / 1.612 * sum)); 
                }
            }

            Image<Gray, byte> finalImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);

            finalImage.Data = inputImage.Data;

            for (int y = 1; y < inputImage.Height - 1; y++)
            {
                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    finalImage.Data[y, x, 0] = (byte)(inputImage.Data[y, x, 0] + filteredImage.Data[y, x, 0]);
                }
            }

            return finalImage;
            }

            #endregion

            #endregion

            #region High Pass Filters

            #endregion


        }
}