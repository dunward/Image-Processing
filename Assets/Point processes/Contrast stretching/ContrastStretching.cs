using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class ContrastStretching : MonoBehaviour
    {
        public Texture2D texture2D;
        public Text max;
        public RawImage histogramImage;

        public Histogram histogram;

        private void Update()
        {
            Stretching();
        }

        private void Stretching()
        {
            int[] histograms = histogram.HistogramDatas;
            int maximumHistogram = 0;

            int low = int.MaxValue, high = int.MinValue;

            for (int i = 0; i < histograms.Length; i++)
            {
                if(histograms[i] > 0)
                {
                    low = Mathf.Min(low, i);
                    high = Mathf.Max(high, i);
                }
            }

            Texture2D texture = new Texture2D(512, 512);

            int[] stretchingHistogram = new int[256];

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    var stretchingColor = ((gray - low) / (float)(high - low));
                    texture.SetPixel(i, k, new Color(stretchingColor, stretchingColor, stretchingColor));
                    stretchingHistogram[(int)Mathf.Clamp(stretchingColor * 255, 0, 255)]++;
                    maximumHistogram = Mathf.Max(maximumHistogram, stretchingHistogram[(int)Mathf.Clamp(stretchingColor * 255, 0, 255)]);
                }
            }

            texture.Apply();

            GetComponent<RawImage>().texture = texture;
            histogramImage.texture = DrawHistogram(stretchingHistogram, maximumHistogram);
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