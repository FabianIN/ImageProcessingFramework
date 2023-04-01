using Emgu.CV.Structure;
using Emgu.CV;
using System.Diagnostics.Contracts;
using System;
using System.Drawing;

namespace Algorithms.Sections
{
    public class PointwiseOperations
    {

        #region Color contrast stretching

        public static Image<Bgr, byte> ColorContrastStretching(Image<Bgr, byte> inputImage)
        {
            #region Convertire BGR -> HSV
            //convertire imagine din Bgr in Hsv
            float min, max, delta;
            float hValue, sValue, vValue;

            Image<Hsv, byte> hsvImage = new Image<Hsv, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    min = Math.Min(Math.Min(inputImage.Data[y, x, 2], inputImage.Data[y, x, 1]), inputImage.Data[y, x, 0]);
                    max = Math.Max(Math.Max(inputImage.Data[y, x, 2], inputImage.Data[y, x, 1]), inputImage.Data[y, x, 0]);
                    vValue = max;

                    delta = max - min;

                    if (max != 0)
                    {
                        sValue = delta / max;
                    }
                    else
                    {
                        sValue = 0;
                        hValue = -1;
                    }

                    if (inputImage.Data[y, x, 2] == max)
                    {
                        hValue = (inputImage.Data[y, x, 1] - inputImage.Data[y, x, 0]) / delta;
                    }
                    else if (inputImage.Data[y, x, 1] == max)
                    {
                        hValue = 2 + (inputImage.Data[y, x, 0] - inputImage.Data[y, x, 2]) / delta;
                    }
                    else
                    {
                        hValue = 4 + (inputImage.Data[y, x, 0] - inputImage.Data[y, x, 1]) / delta;
                    }

                    hValue *= 60;

                    if (hValue < 0)
                    {
                        hValue += 360;
                    }
                    hsvImage.Data[y, x, 0] = (byte)hValue;
                    hsvImage.Data[y, x, 1] = (byte)sValue;
                    hsvImage.Data[y, x, 2] = (byte)vValue;
                }
            }

            hsvImage.Data = inputImage.Data;

            #endregion

            #region Calculare Interval Lmin - Lmax  ptr V
            //calculare interval Lmin si Lmax ptr. componenta V
            byte minV = byte.MaxValue;
            byte maxV = byte.MinValue;

            for(int y = 0; y<hsvImage.Height;  y++)
            {
                for(int x = 0; x<hsvImage.Width; x++)
                {
                    byte componentaV = hsvImage.Data[y, x, 2];

                    if (componentaV < minV)
                    {
                        minV = componentaV;
                    }

                    if (componentaV >maxV)
                    {
                        maxV = componentaV;
                    }
                }
            }

            #endregion

            //mapare interval Lmin si Lmax ptr componenta V
            byte[] lut = new byte[256];

            // Calcularea valorilor LUT-ului pe baza transformării liniare definite mai sus
            for (int i = 0; i < 256; i++)
            {
                float vVal = i / 255f * (maxV - minV) + minV;
                byte VTransformed = (byte)((vVal - minV) * 255 / (maxV - minV));
                lut[i] = VTransformed;
            }

            //float hValue, sValue, vValue;
            double rValue, gValue, bValue;
            Image<Bgr, byte> bgrImageNew = new Image<Bgr, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < hsvImage.Height; y++)
            {
                for (int x = 0; x < hsvImage.Width; x++)
                {
                    hValue = hsvImage.Data[y, x, 0];
                    sValue = hsvImage.Data[y, x, 1];
                    vValue = hsvImage.Data[y, x, 2];
                    byte VTransformed = lut[(int)(vValue * 255)];

                    double chroma = VTransformed * sValue;
                    double hueDash = hValue * 6.0;
                    double xDash = chroma * (1.0 - Math.Abs(hueDash % 2.0 - 1.0));
                    
                    if (hueDash >= 0.0 && hueDash < 1.0)
                    {
                        rValue = chroma;
                        gValue = xDash;
                        bValue = 0.0;
                    }
                    else if (hueDash >= 1.0 && hueDash < 2.0)
                    {
                        rValue = xDash;
                        gValue = chroma;
                        bValue = 0.0;
                    }
                    else if (hueDash >= 2.0 && hueDash < 3.0)
                    {
                        rValue = 0.0;
                        gValue = chroma;
                        bValue = xDash;
                    }
                    else if (hueDash >= 3.0 && hueDash < 4.0)
                    {
                        rValue = 0.0;
                        gValue = xDash;
                        bValue = chroma;
                    }
                    else if (hueDash >= 4.0 && hueDash < 5.0)
                    {
                        rValue = xDash;
                        gValue = 0.0;
                        bValue = chroma;
                    }
                    else
                    {
                        rValue = chroma;
                        gValue = 0.0;
                        bValue = xDash;
                    }

                    double m = VTransformed - chroma;
                    bgrImageNew.Data[y, x, 0] = (byte)bValue;
                    bgrImageNew.Data[y, x, 1] = (byte)gValue;
                    bgrImageNew.Data[y, x, 2] = (byte)rValue;
                }
            }

            return bgrImageNew;
        }

        #region test

        public static Image<Hsv, byte> ContrastStretching(Image<Bgr, byte> inputImage)
        {
            float min, max, delta;
            float hValue, sValue, vValue;

            Image<Hsv, byte> hsvImage = new Image<Hsv, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    min = Math.Min(Math.Min(inputImage.Data[y, x, 2], inputImage.Data[y, x, 1]), inputImage.Data[y, x, 0]);
                    max = Math.Max(Math.Max(inputImage.Data[y, x, 2], inputImage.Data[y, x, 1]), inputImage.Data[y, x, 0]);
                    vValue = max;           

                    delta = max - min;

                    if (max != 0)
                    {
                        sValue = delta / max;   
                    }
                    else
                    {
                        sValue = 0;
                        hValue = -1;
                    }

                    if (inputImage.Data[y, x, 2] == max)
                    {
                        hValue = (inputImage.Data[y, x, 1] - inputImage.Data[y, x, 0]) / delta; 
                    }
                    else if (inputImage.Data[y, x, 1] == max)
                    {
                        hValue = 2 + (inputImage.Data[y, x, 0] - inputImage.Data[y, x, 2]) / delta; 
                    }
                    else
                    {
                        hValue = 4 + (inputImage.Data[y, x, 0] - inputImage.Data[y, x, 1]) / delta; 
                    }

                    hValue *= 60;               

                    if (hValue < 0)
                    {
                        hValue += 360;
                    }
                    hsvImage.Data[y, x, 0] = (byte)hValue;
                    hsvImage.Data[y, x, 1] = (byte)sValue;
                    hsvImage.Data[y, x, 2] = (byte)vValue;
                }
            }
                    hsvImage.Data = inputImage.Data;


            return hsvImage;
        }
        #endregion

        #endregion

    }
}