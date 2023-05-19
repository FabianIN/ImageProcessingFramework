using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Windows.Forms;

namespace Algorithms.Sections
{
    public class GeometricTransformations
    {

        #region Scalare

        public static Image<Gray, byte> ScalareImage(Image<Gray, byte> inputImage, double factorScalare)
        {
            #region Initializare si "generare"

            int newWidth = (int)Math.Ceiling(inputImage.Width * factorScalare);
            int newHeight = (int)Math.Ceiling(inputImage.Height * factorScalare);

            Image<Gray, byte> imageScaled = new Image<Gray, byte>(newHeight, newWidth);

            for (int y = 0; y < imageScaled.Height; y++)
            {
                for (int x = 0; x < imageScaled.Width; x++)
                {
                    double xc = (double)x / factorScalare;
                    double yc = (double)y / factorScalare;
                    imageScaled[y, x] = inputImage[(int)yc, (int)xc];
                }
            }

            Image<Gray, byte> pozaProba = imageScaled.Clone();

            #endregion

            #region Interpolare biliniara

            double xC, yC;
            int x0, x1, y0, y1;
            double alpha, beta;
            double fY0, fY1;
            double interpolare;

            for (int y = 0; y < imageScaled.Height; y++)
            {
                for (int x = 0; x < imageScaled.Width; x++)
                {
                    xC = x / factorScalare;
                    yC = y / factorScalare;

                    x0 = (int)Math.Floor(xC);
                    x1 = (int)Math.Ceiling(xC);
                    y0 = (int)Math.Floor(yC);
                    y1 = (int)Math.Ceiling(yC);

                    if (x1 >= inputImage.Width)
                        x1 = inputImage.Width - 1;
                    if (y1 >= inputImage.Height)
                        y1 = inputImage.Height - 1;

                    alpha = xC - x0;
                    beta = yC - y0;

                    fY0 = ((inputImage.Data[y0, x1, 0] - inputImage.Data[y0, x0, 0]) * alpha) + inputImage.Data[y0, x0, 0];
                    fY1 = ((inputImage.Data[y1, x1, 0] - inputImage.Data[y1, x0, 0]) * alpha) + inputImage.Data[y1, x0, 0];

                    interpolare = (((fY1 - fY0) * beta) + fY0);

                    imageScaled.Data[y, x, 0] = (byte)interpolare;
                }
            }

            #endregion

            return imageScaled;
        }

        #endregion
    }
}
