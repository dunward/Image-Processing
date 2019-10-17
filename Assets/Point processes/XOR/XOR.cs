using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class XOR : MonoBehaviour
    {
        public enum Draw
        {
            Original,
            Grayscale,
            AllXOR,
            BlockXOR,
            ColorXOR
        }

        public Draw type;
        public Texture2D texture2D;
        [Range(0, 255)] public int value;

        private void Update()
        {
            var texture = new Texture2D(texture2D.width, texture2D.height);
            switch (type)
            {
                case Draw.Original:
                    Original(texture);
                    break;

                case Draw.Grayscale:
                    Grayscale(texture);
                    break;

                case Draw.AllXOR:
                    Grayscale(texture);
                    AllXOR(texture);
                    break;

                case Draw.BlockXOR:
                    Grayscale(texture);
                    BlockXOR(texture);
                    break;

                case Draw.ColorXOR:
                    ColorXOR(texture);
                    break;
            }
            GetComponent<RawImage>().texture = texture;
        }
        
        private void Original(Texture2D texture)
        {
            texture.SetPixels(texture2D.GetPixels());
            texture.Apply();
        }

        private void Grayscale(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    var grayscale = (color.r + color.g + color.b) / 3f;
                    texture.SetPixel(i, k, new Color(grayscale, grayscale, grayscale));
                }
            }
            texture.Apply();
        }

        private void AllXOR(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    var color = texture.GetPixel(i, k);
                    var byteGrayscale = color.r * 255;
                    var xor = (byte)byteGrayscale ^ value;
                    texture.SetPixel(i, k, new Color(xor / 255f, xor / 255f, xor / 255f));
                }
            }
            texture.Apply();
        }

        private void BlockXOR(Texture2D texture)
        {
            for (int i = 50; i < 400; i++)
            {
                for (int k = 50; k < 400; k++)
                {
                    var color = texture.GetPixel(i, k);
                    var byteGrayscale = color.r * 255;
                    var xor = (byte)byteGrayscale ^ value;
                    texture.SetPixel(i, k, new Color(xor / 255f, xor / 255f, xor / 255f));
                }
            }
            texture.Apply();
        }

        private void ColorXOR(Texture2D texture)
        {
            for (int i = 0; i < texture2D.width; i++)
            {
                for (int k = 0; k < texture2D.height; k++)
                {
                    var color = texture2D.GetPixel(i, k);
                    var xorbyte = color.r * 255;
                    var rxor = (byte)xorbyte ^ value;
                    xorbyte = color.g * 255;
                    var gxor = (byte)xorbyte ^ value;
                    xorbyte = color.b * 255;
                    var bxor = (byte)xorbyte ^ value;
                    texture.SetPixel(i, k, new Color(rxor / 255f, gxor / 255f, bxor / 255f));
                }
            }
            texture.Apply();
        }
    }
}