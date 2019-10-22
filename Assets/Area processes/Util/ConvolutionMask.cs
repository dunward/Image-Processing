using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaProcesses
{
    public class ConvolutionMask
    {
        private int maskSize;
        private float[,] matrix;
        private Texture2D source;

        public ConvolutionMask(float[,] matrix, Texture2D source)
        {
            this.matrix = matrix;
            this.source = source;

            maskSize = (int)Mathf.Sqrt(matrix.Length);
        }

        public Texture2D GetTexture()
        {
            Texture2D newSource = new Texture2D(source.width, source.height);

            int range = maskSize / 2;

            for (int i = 0; i < source.width; i++)
            {
                for (int k = 0; k < source.height; k++)
                {
                    Color color = new Color(0, 0, 0);
                    for (int mx = -range; mx <= range; mx++)
                    {
                        for (int my = -range; my <= range; my++)
                        {
                            int x = (int)Mathf.Repeat(i - mx, source.width);
                            int y = (int)Mathf.Repeat(k - my, source.height);
                            color += source.GetPixel(x, y) * matrix[mx + range, my + range];
                        }
                    }
                    SetPixel(i, k, newSource, color);
                }
            }

            newSource.Apply();

            return newSource;
        }

        public Texture2D GetTextureGrayScale()
        {
            Texture2D newSource = new Texture2D(source.width, source.height);

            int range = maskSize / 2;

            for (int i = 0; i < source.width; i++)
            {
                for (int k = 0; k < source.height; k++)
                {
                    float gray = 0;
                    for (int mx = -range; mx <= range; mx++)
                    {
                        for (int my = -range; my <= range; my++)
                        {
                            int x = (int)Mathf.Repeat(i - mx, source.width);
                            int y = (int)Mathf.Repeat(k - my, source.height);
                            gray += source.GetPixel(x, y).grayscale * matrix[mx + range, my + range];
                        }
                    }
                    SetPixel(i, k, newSource, new Color(gray, gray, gray));
                }
            }

            newSource.Apply();

            return newSource;
        }

        private void SetPixel(int x, int y, Texture2D source, Color color)
        {
            source.SetPixel(x, y, ColorEffect(color));
        }

        protected virtual Color ColorEffect(Color color)
        {
            return color;
        }
    }
}