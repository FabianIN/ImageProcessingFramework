using Emgu.CV.Structure;
using Emgu.CV;
using System;

namespace Algorithms.Sections
{
    public class MorphologicalOperations
    {
        #region XOR

        public static Image<Gray, byte> XorProcessingImageD(Image<Gray, byte> inputImage, byte fundal)
        {

            #region Dilatare

            Image<Gray, byte> expandImage = inputImage.Clone();

            if (fundal == 0)
            {
                #region tratare camp

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    for (int x = 1; x < inputImage.Width - 1; x++)
                    {
                        if ((inputImage.Data[y - 1, x - 1, 0] == 255) ||
                           (inputImage.Data[y, x - 1, 0] == 255) ||
                           (inputImage.Data[y + 1, x - 1, 0] == 255) ||
                           (inputImage.Data[y - 1, x, 0] == 255) ||
                           (inputImage.Data[y, x, 0] == 255) ||
                           (inputImage.Data[y + 1, x, 0] == 255) ||
                           (inputImage.Data[y - 1, x + 1, 0] == 255) ||
                           (inputImage.Data[y, x + 1, 0] == 255) ||
                           (inputImage.Data[y + 1, x + 1, 0] == 255))
                        {
                            expandImage.Data[y, x, 0] = 255;
                        }
                    }
                }

                #endregion

                #region tratare colturi

                if ((inputImage.Data[0, 0, 0] == 255) ||
                           (inputImage.Data[0, 1, 0] == 255) ||
                           (inputImage.Data[1, 0, 0] == 255) ||
                           (inputImage.Data[1, 1, 0] == 255))
                {
                    expandImage.Data[0, 0, 0] = 255;
                }

                if ((inputImage.Data[inputImage.Height - 1, inputImage.Width - 1, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 2, inputImage.Width - 1, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 2, inputImage.Width - 2, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 1, inputImage.Width - 2, 0] == 255))
                {
                    expandImage.Data[inputImage.Height - 1, inputImage.Width - 1, 0] = 255;
                }

                if ((inputImage.Data[0, inputImage.Width - 1, 0] == 255) ||
                           (inputImage.Data[0, inputImage.Width - 2, 0] == 255) ||
                           (inputImage.Data[1, inputImage.Width - 1, 0] == 255) ||
                           (inputImage.Data[1, inputImage.Width - 2, 0] == 255))
                {
                    expandImage.Data[0, inputImage.Width - 1, 0] = 255;
                }

                if ((inputImage.Data[inputImage.Height - 1, 0, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 2, 0, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 2, 1, 0] == 255) ||
                           (inputImage.Data[inputImage.Height - 1, 1, 0] == 255))
                {
                    expandImage.Data[inputImage.Height - 1, 0, 0] = 255;
                }

                #endregion

                #region tratare margini

                for (int x = 1; x < inputImage.Width - 1; x++)
                    {
                    if ((inputImage.Data[0, x, 0] == 255) || 
                        (inputImage.Data[1, x, 0] == 255) ||
                        (inputImage.Data[0, x - 1, 0] == 255) ||
                        (inputImage.Data[0, x + 1, 0] == 255) ||
                        (inputImage.Data[1, x - 1, 0] == 255) ||
                        (inputImage.Data[1, x + 1, 0] == 255))
                    {
                        expandImage.Data[0, x, 0] = 255;
                    }
                }

                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    if ((inputImage.Data[inputImage.Height - 1, x, 0] == 255) || 
                        (inputImage.Data[inputImage.Height - 2, x, 0] == 255) ||
                        (inputImage.Data[inputImage.Height - 2, x - 1, 0] == 255) ||
                        (inputImage.Data[inputImage.Height - 2, x + 1, 0] == 255) ||
                        (inputImage.Data[inputImage.Height - 1, x - 1, 0] == 255) ||
                        (inputImage.Data[inputImage.Height - 1, x + 1, 0] == 255))
                    {
                        expandImage.Data[inputImage.Height - 1, x, 0] = 255;
                    }
                }

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    if ((inputImage.Data[y, 0, 0] == 255) || 
                        (inputImage.Data[y, 1, 0] == 255) ||
                        (inputImage.Data[y - 1, 0, 0] == 255) ||
                        (inputImage.Data[y - 1, 1, 0] == 255) ||
                        (inputImage.Data[y + 1, 0, 0] == 255) ||
                        (inputImage.Data[y + 1, 1, 0] ==255))
                    {
                        expandImage.Data[y, 0, 0] = 255;
                    }
                }

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    if ((inputImage.Data[y, inputImage.Width - 1, 0] == 255) || 
                        (inputImage.Data[y, inputImage.Width - 2, 0] == 255) ||
                        (inputImage.Data[y - 1, inputImage.Width - 1, 0] == 255) ||
                        (inputImage.Data[y - 1, inputImage.Width - 2, 0] == 255) ||
                        (inputImage.Data[y + 1, inputImage.Width - 1, 0] == 255) ||
                        (inputImage.Data[y + 1, inputImage.Width - 2, 0] == 255))
                    {
                        expandImage.Data[y, inputImage.Width - 1, 0] = 255;
                    }
                }

                #endregion
            }
            else
            {
                #region tratare camp

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    for (int x = 1; x < inputImage.Width - 1; x++)
                    {
                        if ((inputImage.Data[y - 1, x - 1, 0] == 0) ||
                           (inputImage.Data[y, x - 1, 0] == 0) ||
                           (inputImage.Data[y + 1, x - 1, 0] == 0) ||
                           (inputImage.Data[y - 1, x, 0] == 0) ||
                           (inputImage.Data[y, x, 0] == 0) ||
                           (inputImage.Data[y + 1, x, 0] == 0) ||
                           (inputImage.Data[y - 1, x + 1, 0] == 0) ||
                           (inputImage.Data[y, x + 1, 0] == 0) ||
                           (inputImage.Data[y + 1, x + 1, 0] == 0))
                        {
                            expandImage.Data[y, x, 0] = 0;
                        }
                    }
                }

                #endregion

                #region tratare colturi

                if ((inputImage.Data[0, 0, 0] == 0) ||
                           (inputImage.Data[0, 1, 0] == 0) ||
                           (inputImage.Data[1, 0, 0] == 0) ||
                           (inputImage.Data[1, 1, 0] == 0))
                {
                    expandImage.Data[0, 0, 0] = 0;
                }

                if ((inputImage.Data[inputImage.Height - 1, inputImage.Width - 1, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 2, inputImage.Width - 1, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 2, inputImage.Width - 2, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 1, inputImage.Width - 2, 0] == 0))
                {
                    expandImage.Data[inputImage.Height - 1, inputImage.Width - 1, 0] = 0;
                }

                if ((inputImage.Data[0, inputImage.Width - 1, 0] == 0) ||
                           (inputImage.Data[0, inputImage.Width - 2, 0] == 0) ||
                           (inputImage.Data[1, inputImage.Width - 1, 0] == 0) ||
                           (inputImage.Data[1, inputImage.Width - 2, 0] == 0))
                {
                    expandImage.Data[0, inputImage.Width - 1, 0] = 0;
                }

                if ((inputImage.Data[inputImage.Height - 1, 0, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 2, 0, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 2, 1, 0] == 0) ||
                           (inputImage.Data[inputImage.Height - 1, 1, 0] == 0))
                {
                    expandImage.Data[inputImage.Height - 1, 0, 0] = 0;
                }

                #endregion

                #region tratare margini

                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    if ((inputImage.Data[0, x, 0] == 0) ||
                        (inputImage.Data[1, x, 0] == 0) ||
                        (inputImage.Data[0, x - 1, 0] == 0) ||
                        (inputImage.Data[0, x + 1, 0] == 0) ||
                        (inputImage.Data[1, x - 1, 0] == 0) ||
                        (inputImage.Data[1, x + 1, 0] == 0))
                    {
                        expandImage.Data[0, x, 0] = 0;
                    }
                }

                for (int x = 1; x < inputImage.Width - 1; x++)
                {
                    if ((inputImage.Data[inputImage.Height - 1, x, 0] == 0) ||
                        (inputImage.Data[inputImage.Height - 2, x, 0] == 0) ||
                        (inputImage.Data[inputImage.Height - 2, x - 1, 0] == 0) ||
                        (inputImage.Data[inputImage.Height - 2, x + 1, 0] == 0) ||
                        (inputImage.Data[inputImage.Height - 1, x - 1, 0] == 0) ||
                        (inputImage.Data[inputImage.Height - 1, x + 1, 0] == 0))
                    {
                        expandImage.Data[inputImage.Height - 1, x, 0] = 0;
                    }
                }

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    if ((inputImage.Data[y, 0, 0] == 0) ||
                        (inputImage.Data[y, 1, 0] == 0) ||
                        (inputImage.Data[y - 1, 0, 0] == 0) ||
                        (inputImage.Data[y - 1, 1, 0] == 0) ||
                        (inputImage.Data[y + 1, 0, 0] == 0) ||
                        (inputImage.Data[y + 1, 1, 0] == 0))
                    {
                        expandImage.Data[y, 0, 0] = 0;
                    }
                }

                for (int y = 1; y < inputImage.Height - 1; y++)
                {
                    if ((inputImage.Data[y, inputImage.Width - 1, 0] == 0) ||
                        (inputImage.Data[y, inputImage.Width - 2, 0] == 0) ||
                        (inputImage.Data[y - 1, inputImage.Width - 1, 0] == 0) ||
                        (inputImage.Data[y - 1, inputImage.Width - 2, 0] == 0) ||
                        (inputImage.Data[y + 1, inputImage.Width - 1, 0] == 0) ||
                        (inputImage.Data[y + 1, inputImage.Width - 2, 0] == 0))
                    {
                        expandImage.Data[y, inputImage.Width - 1, 0] = 0;
                    }
                }

                #endregion
            }

            #endregion

            #region XOR Imagini

            Image<Gray, byte> finalImage = new Image<Gray, byte>(inputImage.Width, inputImage.Height);

            if (fundal == 0)
            {
                for (int y = 0; y < inputImage.Height; y++)
                {
                    for (int x = 0; x < inputImage.Width; x++)
                    {
                        if (inputImage.Data[y, x, 0] == expandImage.Data[y, x, 0])
                            finalImage.Data[y, x, 0] = 0;
                        else
                            finalImage.Data[y, x, 0] = 255;
                    }
                }
            }
            else
            {
                for (int y = 0; y < inputImage.Height; y++)
                {
                    for (int x = 0; x < inputImage.Width; x++)
                    {
                        if (inputImage.Data[y, x, 0] == expandImage.Data[y, x, 0])
                            finalImage.Data[y, x, 0] = 255;
                        else
                            finalImage.Data[y, x, 0] = 0;
                    }
                }
            }

            return finalImage;

            #endregion

        }

        #endregion
    }
}