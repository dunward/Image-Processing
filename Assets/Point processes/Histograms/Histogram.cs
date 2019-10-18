using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class Histogram : MonoBehaviour
    {
        public enum Draw
        {
            R,
            G,
            B,
            Grayscale
        }

        public Draw type;
        public Texture2D texture2D;
        public Text max;

        public int[] HistogramDatas;

        private void Update()
        {
            Texture2D texture = new Texture2D(0, 0);
            switch (type)
            {
                case Draw.R:
                    texture = R();
                    break;

                case Draw.G:
                    texture = G();
                    break;

                case Draw.B:
                    texture = B();
                    break;

                case Draw.Grayscale:
                    texture = Grayscale();
                    break;
            }
            GetComponent<RawImage>().texture = texture;
        }

        private Texture2D Grayscale()
        {
            int[] histogram = new int[256];
            int maximumHistogram = 0;

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    histogram[gray]++;
                    maximumHistogram = Mathf.Max(maximumHistogram, histogram[gray]);
                }
            }

            return DrawHistogram(histogram, maximumHistogram);
        }

        private Texture2D R()
        {
            int[] histogram = new int[256];
            int maximumHistogram = 0;

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int red = (int)(color.r * 255);
                    histogram[red]++;
                    maximumHistogram = Mathf.Max(maximumHistogram, histogram[red]);
                }
            }

            return DrawHistogram(histogram, maximumHistogram);
        }

        private Texture2D G()
        {
            int[] histogram = new int[256];
            int maximumHistogram = 0;

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int red = (int)(color.g * 255);
                    histogram[red]++;
                    maximumHistogram = Mathf.Max(maximumHistogram, histogram[red]);
                }
            }

            return DrawHistogram(histogram, maximumHistogram);
        }

        private Texture2D B()
        {
            int[] histogram = new int[256];
            int maximumHistogram = 0;

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int red = (int)(color.b * 255);
                    histogram[red]++;
                    maximumHistogram = Mathf.Max(maximumHistogram, histogram[red]);
                }
            }

            return DrawHistogram(histogram, maximumHistogram);
        }

        private Texture2D DrawHistogram(int[] histogram, int maximumHistogram)
        {
            Texture2D texture = new Texture2D(256, (int)(maximumHistogram * (256f / maximumHistogram)));

            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    texture.SetPixel(i, k, Color.black);
                }
            }

            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < histogram[i] * (256f / maximumHistogram); k++)
                {
                    texture.SetPixel(i, k, Color.white);
                }
            }

            texture.Apply();

            max.text = $"Max : {maximumHistogram}";
            HistogramDatas = histogram;

            return texture;
        }
    }
}