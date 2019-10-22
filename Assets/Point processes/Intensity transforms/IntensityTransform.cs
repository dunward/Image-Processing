using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class IntensityTransform : MonoBehaviour
    {
        public enum Draw
        {
            Original,
            Inverse,
            Gamma
        }

        public Draw type;
        public Texture2D texture2D;
        public RawImage histogramImage;
        public float value;

        private void Update()
        {
            switch (type)
            {
                case Draw.Original:
                    Original();
                    break;

                case Draw.Inverse:
                    Inverse();
                    break;

                case Draw.Gamma:
                    Gamma();
                    break;
            }
        }

        private void Original()
        {
            int[] intensity = GetBasicIntensity();

            Texture2D texture = new Texture2D(512, 512);

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    var grayIntensity = intensity[gray] / 255f;
                    texture.SetPixel(i, k, new Color(grayIntensity, grayIntensity, grayIntensity));
                }
            }

            texture.Apply();

            GetComponent<RawImage>().texture = texture;
            histogramImage.texture = DrawCurve(intensity);
        }

        private void Gamma()
        {
            int[] intensity = GetBasicIntensity();

            for (int i = 0; i < intensity.Length; i++)
            {
                Debug.Log($"{Mathf.Pow(255, 1 / value)}, {Mathf.Pow(intensity[i], 1 / value)}");
                intensity[i] = (int)Mathf.Lerp(0, 255, Mathf.InverseLerp(0, Mathf.Pow(255, 1 / value), Mathf.Pow(intensity[i], 1 / value)));
            }

            Texture2D texture = new Texture2D(512, 512);

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    var grayIntensity = intensity[gray] / 255f;
                    texture.SetPixel(i, k, new Color(grayIntensity, grayIntensity, grayIntensity));
                }
            }

            texture.Apply();

            GetComponent<RawImage>().texture = texture;
            histogramImage.texture = DrawCurve(intensity);
        }

        private void Inverse()
        {
            int[] intensity = GetBasicIntensity();

            for (int i = 0; i < intensity.Length; i++)
            {
                intensity[i] = 255 - i;
            }

            Texture2D texture = new Texture2D(512, 512);

            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    int gray = (int)(color.grayscale * 255);
                    var grayIntensity = intensity[gray] / 255f;
                    texture.SetPixel(i, k, new Color(grayIntensity, grayIntensity, grayIntensity));
                }
            }

            texture.Apply();

            GetComponent<RawImage>().texture = texture;
            histogramImage.texture = DrawCurve(intensity);
        }

        private int[] GetBasicIntensity()
        {
            int[] intensity = new int[256];

            for (int i = 0; i < intensity.Length; i++)
            {
                intensity[i] = i;
            }

            return intensity;
        }

        private Texture2D DrawCurve(int[] curves)
        {
            Texture2D texture = new Texture2D(256, 256);

            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    if(k == curves[i])
                    {
                        texture.SetPixel(i, k, Color.black);
                    }
                    else
                    {
                        texture.SetPixel(i, k, Color.white);
                    }
                }
            }

            texture.Apply();

            return texture;
        }
    }
}