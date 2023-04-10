using System.Drawing;
using System;
using Emgu.CV.Structure;
using Emgu.CV;
using static System.Net.Mime.MediaTypeNames;

namespace Algorithms.Sections
{
    public class Thresholding
    {

        #region Calcul Treshold ptr. Binarizare MinERR

        public static int CalculThreshold(Image<Gray, byte> inputImage)
        {
            double[] histogram = new double[256];
            byte pixelValue;

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    pixelValue = inputImage.Data[y, x, 0];
                    histogram[pixelValue]++;
                }
            }

            for (int i = 0; i<256;i++)
            {
                histogram[i] = histogram[i] / (inputImage.Height*inputImage.Width);
            }
            double epsilonMin = 256;
            int thresholdMin = 0;
            double p1, p2, delta1, delta2, mu1, mu2, media, variatia;
            double epsilon = 0;

            for (int threshold = 0; threshold<256; threshold++)
            {

                p1 = 0;
                media = 0;
                variatia= 0;
                for(int k = 0; k <= threshold; k++)
                {
                    //p1 = suma de p(k), cu k = 0 -> t
                    p1 += histogram[k];
                    media = (histogram[k]*k)+media;
                }
                //media = (suma de k*p(k), cu k = 0 -> t)/p1
                if(p1 != 0)
                    mu1 = media / p1;
                else
                    mu1 = 0;

                for (int k = 0; k <= threshold; k++)
                {
                    variatia = (k - mu1) * (k - mu1) * histogram[k];
                }
                //variatia = (suma de (k-medie)^2*p(k), cu k = 0 -> t)/p1
                if (p1 != 0)
                    delta1 = variatia / p1;
                else
                    delta1= 0;


                p2 = 0;
                media = 0;
                variatia = 0;
                for (int k = threshold+1; k < 256; k++)
                {
                    //p1 = suma de p(k), cu k = t+1 -> 255
                    p2 += histogram[k];
                    media = (histogram[k] * k) + media;
                }
                p2 = 1 - p1;
                //media = (suma de k*p(k), cu k = t+1 -> 255)/p1
                if(p2 != 0)
                    mu2 = media / p2;
                else
                    mu2 = 0;

                for (int k = threshold + 1; k < 256; k++)
                {
                    variatia = (k - mu2) * (k - mu2) * histogram[k];
                }
                //variatia = (suma de (k-medie)^2*p(k), cu k = 0 -> t)/p1
                if (p2 != 0)
                    delta2 = variatia / p2;
                else
                    delta2 = 0;

                epsilon = (1 + (p1 * Math.Log(delta1)) + (p2 * Math.Log(delta2)) - (2 * p1 * Math.Log(p1)) - (2 * p2 * Math.Log(p2)));

                if ((epsilon>=0) && (epsilon < epsilonMin))
                {
                    epsilonMin = epsilon;
                    thresholdMin = threshold;
                }

            }

            return thresholdMin;
        }

        #endregion

    }
}