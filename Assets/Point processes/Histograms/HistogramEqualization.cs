using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class HistogramEqualization : MonoBehaviour
    {
        public Texture2D texture2D;
        public Text max;
        public RawImage histogramImage;

        public Histogram histogram;

        private void Update()
        {
            Equalization();
        }

        private void Equalization()
        {
            int[] histograms = histogram.HistogramDatas;
            int maximumHistogram = 0;

            int total = 0;
            int[] equalization = new int[256];

            for (int i = 0; i < histograms.Length; i++)
            {
                total += histograms[i];
                equalization[i] = (int)(total * (255f / (512f * 512f)));
            }

            Texture2D texture = new Texture2D(512, 512);

            int[] equalHistograms = new int[256];

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    var grayScale = equalization[gray] / 255f;
                    equalHistograms[equalization[gray]]++;
                    texture.SetPixel(i, k, new Color(grayScale, grayScale, grayScale));
                    maximumHistogram = Mathf.Max(maximumHistogram, equalHistograms[equalization[gray]]);
                }
            }

            texture.Apply();

            GetComponent<RawImage>().texture = texture;
            histogramImage.texture = DrawHistogram(equalHistograms, maximumHistogram);
        }

        private Texture2D DrawHistogram(int[] histograms, int maximumHistogram)
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
                for (int k = 0; k < histograms[i] * (256f / maximumHistogram); k++)
                {
                    texture.SetPixel(i, k, Color.white);
                }
            }

            texture.Apply();

            max.text = $"Max : {maximumHistogram}";

            return texture;
        }
    }
}