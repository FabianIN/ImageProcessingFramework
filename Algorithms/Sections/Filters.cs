using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Windows.Forms;
using System.Drawing;

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

            //Image<Gray, byte> filteredImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);
            Image<Gray, byte> filteredImage = inputImage.Clone();

            double sum;
            //filteredImage.Data = inputImage.Data;

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

            //Image<Gray, byte> finalImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);
            Image<Gray, byte> finalImage = inputImage.Clone();

            //finalImage.Data = inputImage.Data;

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

        #region Canny

        #region Unsharp Mask

        public static Image<Gray, byte> CannyProcessingImage(Image<Gray, byte> inputImage, int thresholdT1, int thresholdT2)
        {
            #region Gaussian Filter

            double[,] filtruGaussian = new double[,]
                {
                    { 0.018, 0.082, 0.135, 0.082, 0.018 },
                    { 0.082, 0.367, 0.606, 0.367, 0.082 },
                    { 0.135, 0.606, 1.000, 0.606, 0.135 },
                    { 0.082, 0.367, 0.606, 0.367, 0.082 },
                    { 0.018, 0.082, 0.135, 0.082, 0.018 },
                };

            //Image<Gray, byte> filteredImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);
            Image<Gray, byte> filteredImage = inputImage.Clone();

            double sum;

            //filteredImage.Data = inputImage.Data;

            for (int y = 2; y < inputImage.Height - 2; y++)
            {
                for (int x = 2; x < inputImage.Width - 2; x++)
                {
                    sum = 0;
                    for (int i = -2; i <= 2; i++)
                    {
                        for (int j = -2; j <= 2; j++)
                        {
                            sum = sum + (inputImage.Data[y + i, x + j, 0] * filtruGaussian[i + 2, j + 2]);
                        }
                    }
                    filteredImage.Data[y, x, 0] = (byte)(Math.Round(1 / 6.160 * sum));
                }
            }

            #endregion

            #region Sobel

            //Image<Gray, double> sobelImage = new Image<Gray, double>(inputImage.Width, inputImage.Height);
            Image<Gray, byte> sobelImage = inputImage.Clone();

            //sobelImage = filteredImage.Clone();

            double sx, sy, normaGrad;
            double[,] gradient = new double[inputImage.Height, inputImage.Width];
            
            for (int y = 1; y < inputImage.Height - 1; y++)
            {
                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    sx = filteredImage.Data[y + 1, x - 1, 0] - filteredImage.Data[y - 1, x - 1, 0] + (filteredImage.Data[y + 1, x, 0] * 2) - (filteredImage.Data[y - 1, x, 0] * 2) + filteredImage.Data[y + 1, x + 1, 0] - filteredImage.Data[y - 1, x + 1, 0];

                    sy = filteredImage.Data[y - 1, x + 1, 0] - filteredImage.Data[y - 1, x - 1, 0] + (filteredImage.Data[y, x + 1, 0] * 2) - (filteredImage.Data[y, x - 1, 0] * 2) + filteredImage.Data[y + 1, x + 1, 0] - filteredImage.Data[y + 1, x - 1, 0];

                    normaGrad = Math.Sqrt((sx * sx) + (sy * sy));

                    //sobelImage.Data[y, x, 0] = normaGrad;
                    sobelImage.Data[y, x, 0] = (byte)normaGrad;

                    gradient[y,x] = Math.Atan(sy / sx);

                }
            }

            #endregion

            #region Non-Maxima Suppresion

            Image<Gray, byte> maximaImage = sobelImage.Clone();

            for (int y = 1; y < inputImage.Height - 1; y++)
            {
                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    //horizontal [-0.3927, 0.3927] U [-2.7489, 2.7489]
                    if (((gradient[y, x] <= 0.3927) && (gradient[y, x] >= -0.3927)) || ((gradient[y, x] <= 2.7489) && (gradient[y, x] >= -2.7489)))
                    {
                        if ((sobelImage.Data[y, x, 0] < sobelImage.Data[y, x + 1, 0]) || (sobelImage.Data[y, x, 0] < sobelImage.Data[y, x - 1, 0]))
                            maximaImage.Data[y, x, 0] = 0;
                    }

                    //45° [-2.7489, -1.9635] U [0.3927, 1.1781]
                    if (((gradient[y, x] > -2.7489) && (gradient[y, x] < -1.9635)) || ((gradient[y, x] > 0.3927) && (gradient[y, x] < 1.1781)))
                    {
                        if ((sobelImage.Data[y, x, 0] < sobelImage.Data[y - 1, x + 1, 0]) || (sobelImage.Data[y, x, 0] < sobelImage.Data[y + 1, x - 1, 0]))
                            maximaImage.Data[y, x, 0] = 0;
                    }

                    //vertical [-1.9635, -1.1781] U [1.1781, 1.9635]
                    if (((gradient[y, x] >= -1.9635) && (gradient[y, x] <= -1.1781)) || ((gradient[y, x] >= 1.1781) && (gradient[y, x] <= 1.9635)))
                    {
                        if ((sobelImage.Data[y, x, 0] < sobelImage.Data[y - 1, x, 0]) || (sobelImage.Data[y, x, 0] < sobelImage.Data[y + 1, x, 0]))
                            maximaImage.Data[y, x, 0] = 0;
                    }

                    //-45° [-1.1781, -0.3927] U [1.9635, 2.7489]
                    if (((gradient[y, x] > -1.1781) && (gradient[y, x] < -0.3927)) || ((gradient[y, x] > 1.9635) && (gradient[y, x] < 2.7489)))
                    {
                        if ((sobelImage.Data[y, x, 0] < sobelImage.Data[y + 1, x - 1, 0]) || (sobelImage.Data[y, x, 0] < sobelImage.Data[y - 1, x + 1, 0]))
                            maximaImage.Data[y, x, 0] = 0;
                    }
                }
            }

            #endregion

            #region Hysteresys Tresholding

            //Image<Gray, byte> finalImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);
            Image<Gray, byte> finalImage = inputImage.Clone();

            bool conectare;
            
            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    if (maximaImage.Data[y, x, 0] <= thresholdT1)
                    {
                        finalImage.Data[y, x, 0] = 0;
                    }
                    if (maximaImage.Data[y,x,0] > thresholdT2)
                    {
                        finalImage.Data[y, x, 0] = 255;
                    }
                    if ((maximaImage.Data[y,x,0]>thresholdT1) && (maximaImage.Data[y,x,0]<=thresholdT2))
                    {
                        conectare = false;

                        for (int i = -1; i <= 1; i++)
                        {
                            for (int j = -1; j <= 1; j++)
                            {
                                if (maximaImage.Data[i,j,0] > thresholdT2)
                                {
                                    conectare = true; break;
                                }
                            }
                        }

                        if (conectare)
                        {
                            finalImage.Data[y, x, 0] = 255;
                        }
                        else
                        {
                            finalImage.Data[y, x, 0] = 0;
                        }
                    }
                   
                }
            }

            return finalImage;

            #endregion

        }

        #endregion

        #endregion

        #endregion


    }
}