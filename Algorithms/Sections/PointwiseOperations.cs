using Emgu.CV.Structure;
using Emgu.CV;
using System;
using System.Windows.Forms;

namespace Algorithms.Sections
{
    public class PointwiseOperations
    {

        #region Color contrast stretching

        public static Image<Bgr, byte> ColorContrastStretching(Image<Bgr, byte> inputImage)
        {
            #region Convertire BGR -> HSV
            double min, max, delta;
            double hValue, sValue, vValue;
            byte bValueN, gValueN, rValueN;

            Image<Hsv, byte> hsvImage = new Image<Hsv, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bValueN = inputImage.Data[y, x, 0];
                    gValueN = inputImage.Data[y, x, 1];
                    rValueN = inputImage.Data[y, x, 2];

                    min = Math.Min(Math.Min(rValueN, gValueN), bValueN);
                    max = Math.Max(Math.Max(rValueN, gValueN), bValueN);

                    vValue = max;
                    sValue = 0;
                    hValue = 0;

                    delta = max - min;

                    if (max != 0)
                    {
                        sValue = delta / max;
                    }
                    else
                    {
                        sValue = 0;
                    }

                    if (delta != 0)
                    {
                        if (max == rValueN)
                        {
                            hValue = (gValueN - bValueN) / (double)delta;
                        }
                        if (max == gValueN)
                        {
                            hValue = 2 + (bValueN - rValueN) / (double)delta;
                        }
                        if (max == bValueN)
                        {
                            hValue = 4 + (rValueN - gValueN) / (double)delta;
                        }
                    }

                    hValue = hValue * 60;
                    if (hValue < 0)
                    {
                        hValue = hValue + 360;
                    }

                    hsvImage.Data[y, x, 0] = (byte)(hValue / 2);
                    hsvImage.Data[y, x, 1] = (byte)(sValue * 255);
                    hsvImage.Data[y, x, 2] = (byte)(vValue);
                }
            }

            #endregion

            #region Calculare Interval Lmin - Lmax  ptr V
            byte minV = byte.MaxValue;
            byte maxV = byte.MinValue;

            for (int y = 0; y < hsvImage.Height; y++)
            {
                for (int x = 0; x < hsvImage.Width; x++)
                {
                    byte componentaV = hsvImage.Data[y, x, 2];
                    //byte componentaV = (byte)hsvImage[y, x].Value;

                    if (componentaV < minV)
                    {
                        minV = componentaV;
                    }

                    if (componentaV > maxV)
                    {
                        maxV = componentaV;
                    }
                }
            }

            #endregion

            #region Mapare interval [0,255]

            for (int y = 0; y < hsvImage.Height; y++)
            {
                for (int x = 0; x < hsvImage.Width; x++)
                {
                    int vValueCurrent = hsvImage.Data[y, x, 2];
                    double scaledValue = ((vValueCurrent - 0) / (double)(255 - 0)) * (maxV - minV) + minV;
                    hsvImage.Data[y, x, 2] = (byte)Math.Round(scaledValue);
                }
            }

            #endregion

            //#region LUT Tables

            //byte[] lutH = new byte[256];
            //byte[] lutS = new byte[256];
            //byte[] lutV = new byte[256];

            //byte value;

            //for (int y = 0; y < hsvImage.Height; y++)
            //{
            //    for (int x = 0; x < hsvImage.Width; x++)
            //    {
            //        value = hsvImage.Data[y, x, 0];
            //        lutH[value]++;
            //        value = hsvImage.Data[y, x, 1];
            //        lutS[value]++;
            //        value = hsvImage.Data[y, x, 2];
            //        lutV[value]++;
            //    }
            //}

            //#endregion

            //#region Scalare LUT

            //byte[] scaledLUT = new byte[256];
            //for (int i = 0; i < 256; i++)
            //{
            //    int vValueLut = lutV[i];
            //    double vValueLutS = ((vValueLut - 0) / (double)(255 - 0)) * (maxV - minV) + minV;
            //    scaledLUT[i] = (byte)Math.Round(vValueLutS);
            //}

            //lutV = scaledLUT;

            //#endregion


            #region Convertire HSV -> BGR
            double rValue, gValue, bValue;
            Image<Bgr, byte> bgrImageNew = new Image<Bgr, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    hValue = hsvImage.Data[y, x, 0]*2;
                    sValue = hsvImage.Data[y, x, 1];
                    vValue = hsvImage.Data[y, x, 2];

                    double chroma = vValue * sValue / 255;
                    double xDash = chroma * (1 - Math.Abs((hValue / 60) % 2 - 1));
                    double m = vValue - chroma;
                    bValue = 0;
                    rValue = 0;
                    gValue = 0;

                    if (hValue >= 0 && hValue < 60)
                    {
                        rValue = chroma;
                        gValue = xDash;
                    }
                    else if (hValue >= 60 && hValue < 120)
                    {
                        rValue = xDash;
                        gValue = chroma;
                    }
                    else if (hValue >= 120 && hValue < 180)
                    {
                        gValue = chroma;
                        bValue = xDash;
                    }
                    else if (hValue >= 180 && hValue < 240)
                    {
                        gValue = xDash;
                        bValue = chroma;
                    }
                    else if (hValue >= 240 && hValue < 300)
                    {
                        rValue = xDash;
                        bValue = chroma;
                    }
                    else if (hValue >= 300 && hValue < 360)
                    {
                        rValue = chroma;
                        bValue = xDash;
                    }

                    bgrImageNew.Data[y, x, 0] = (byte)((bValue + m));
                    bgrImageNew.Data[y, x, 1] = (byte)((gValue + m));
                    bgrImageNew.Data[y, x, 2] = (byte)((rValue + m));
                }
            }

            #endregion

            return bgrImageNew;
        }

        #endregion

        #region test

        public static Image<Hsv, byte> ContrastStretching(Image<Bgr, byte> inputImage)
        {
            #region Convertire BGR -> HSV
            //convertire imagine din Bgr in Hsv
            double min, max, delta;
            double hValue, sValue, vValue;
            byte bValueN, gValueN, rValueN;

            Image<Hsv, byte> hsvImage = new Image<Hsv, byte>(inputImage.Width, inputImage.Height);

            for (int y = 0; y < inputImage.Height; y++)
            {
                for (int x = 0; x < inputImage.Width; x++)
                {
                    bValueN = inputImage.Data[y, x, 0];
                    gValueN = inputImage.Data[y, x, 1];
                    rValueN = inputImage.Data[y, x, 2];

                    min = Math.Min(Math.Min(rValueN, gValueN), bValueN);
                    max = Math.Max(Math.Max(rValueN, gValueN), bValueN);

                    vValue = max;
                    sValue = 0;
                    hValue = 0;

                    delta = max - min;

                    if (max != 0)
                    {
                        sValue = delta / max;
                    }
                    else
                    {
                        sValue = 0;
                    }

                    if (delta != 0)
                    {
                        if (max == rValueN)
                        {
                            hValue = (gValueN - bValueN) / (double)delta;
                        }
                        if (max == gValueN)
                        {
                            hValue = 2 + (bValueN - rValueN) / (double)delta;
                        }
                        if (max == bValueN)
                        {
                            hValue = 4 + (rValueN - gValueN) / (double)delta;
                        }
                    }

                    hValue = hValue * 60;
                    if (hValue < 0)
                    {
                        hValue = hValue + 360;
                    }

                    hsvImage.Data[y, x, 0] = (byte)(hValue / 2);
                    hsvImage.Data[y, x, 1] = (byte)(sValue * 255);
                    hsvImage.Data[y, x, 2] = (byte)(vValue);
                }
            }

            return hsvImage;
        }
            #endregion

        #endregion

        }
}