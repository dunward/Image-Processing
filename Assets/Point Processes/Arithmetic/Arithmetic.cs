using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PointProcesses
{
    [ExecuteInEditMode]
    public class Arithmetic : MonoBehaviour
    {
        public enum Draw
        {
            Original,
            Plus,
            Minus,
            Multiply,
            Divide
        }

        public Draw type;
        public Texture2D texture2D;
        public float value;

        private void Update()
        {
            var texture = new Texture2D(texture2D.width, texture2D.height);
            switch (type)
            {
                case Draw.Original:
                    Original(texture);
                    break;

                case Draw.Plus:
                    Plus(texture);
                    break;

                case Draw.Minus:
                    Minus(texture);
                    break;

                case Draw.Multiply:
                    Multiply(texture);
                    break;

                case Draw.Divide:
                    Divide(texture);
                    break;
            }
            GetComponent<RawImage>().texture = texture;
        }
        
        private void Original(Texture2D texture)
        {
            texture.SetPixels(texture2D.GetPixels());
            texture.Apply();
        }

        private void Plus(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    texture.SetPixel(i, k, texture2D.GetPixel(i, k) + new Color(value / 255f, value / 255f, value / 255f));
                }
            }
            texture.Apply();
        }

        private void Minus(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    texture.SetPixel(i, k, texture2D.GetPixel(i, k) + new Color(-value / 255f, -value / 255f, -value / 255f));
                }
            }
            texture.Apply();
        }

        private void Multiply(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    var color = texture2D.GetPixel(i, k) * value;
                    color.a = 1;
                    texture.SetPixel(i, k, color);
                }
            }
            texture.Apply();
        }

        private void Divide(Texture2D texture)
        {
            for (int i = 0; i < texture.width; i++)
            {
                for (int k = 0; k < texture.height; k++)
                {
                    var color = texture2D.GetPixel(i, k) / value;
                    color.a = 1;
                    texture.SetPixel(i, k, color);
                }
            }
            texture.Apply();
        }
    }
}