using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AreaProcesses
{
    [ExecuteInEditMode]
    public class Mask : MonoBehaviour
    {
        public enum Type
        {
            Roberts,
            Prewitt,
            Sobel,
            FreiChen
        }

        public Type type;

        [Range(0, 1)] public float threshold;

        public Texture2D texture;

        private void Update()
        {
            switch (type)
            {
                case Type.Roberts:
                    ConvolutionRoberts();
                    break;

                case Type.Prewitt:
                    ConvolutionPrewitt();
                    break;

                case Type.Sobel:
                    ConvolutionSobel();
                    break;

                case Type.FreiChen:
                    ConvolutionFreiChen();
                    break;
            }
        }

        public void ConvolutionRoberts()
        {
            float[,] r = {
            { 0, 0, -1 },
            { 0, 1, 0 },
            { 0, 0, 0 } };

            float[,] c = {
            { 0, 0, -1 },
            { 0, 1, 0 },
            { 0, 0, 0 } };

            GetComponent<RawImage>().texture = GetTexture(r, c);
        }

        public void ConvolutionPrewitt()
        {
            float[,] r = {
            { 1, 0, -1 },
            { 1, 0, -1 },
            { 1, 0, -1 } };

            float[,] c = {
            { -1, -1, -1 },
            { 0, 0, 0 },
            { 1, 1, 1 } };

            GetComponent<RawImage>().texture = GetTexture(r, c);
        }

        public void ConvolutionSobel()
        {
            float[,] r = {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 } };

            float[,] c = {
            { -1, -2, -1 },
            { 0, 0, 0 },
            { 1, 2, 1 } };

            GetComponent<RawImage>().texture = GetTexture(r, c);
        }

        public void ConvolutionFreiChen()
        {
            float[,] r = {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 } };

            float[,] c = {
            { 1, 0, -1 },
            { 2, 0, -2 },
            { 1, 0, -1 } };

            GetComponent<RawImage>().texture = GetTexture(r, c);
        }


        public Texture2D GetTexture(float[,] matrix1, float[,] matrix2)
        {
            Texture2D newSource = new Texture2D(texture.width, texture.height);

            int range = 3 / 2;

            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    float colorR = 0;
                    float colorC = 0;
                    for (int mx = -range; mx <= range; mx++)
                    {
                        for (int my = -range; my <= range; my++)
                        {
                            int x = (int)Mathf.Repeat(i - mx, texture.width);
                            int y = (int)Mathf.Repeat(k - my, texture.height);
                            colorR += texture.GetPixel(x, y).grayscale * matrix1[mx + range, my + range];
                            colorC += texture.GetPixel(x, y).grayscale * matrix2[mx + range, my + range];
                        }
                    }
                    var gray = Mathf.Abs(colorR) + Mathf.Abs(colorC);
                    if(gray > threshold)
                    {
                        newSource.SetPixel(i, k, new Color(gray, gray, gray));
                    }
                    else
                    {
                        newSource.SetPixel(i, k, Color.black);
                    }
                }
            }

            newSource.Apply();

            return newSource;
        }
    }
}