using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AreaProcesses
{
    [ExecuteInEditMode]
    public class Sharpening : MonoBehaviour
    {
        public enum Type
        {
            A,
            B,
            C
        }

        public Type type;

        public Texture2D texture;

        private void Update()
        {
            switch (type)
            {
                case Type.A:
                    ConvolutionA();
                    break;

                case Type.B:
                    ConvolutionB();
                    break;

                case Type.C:
                    ConvolutionC();
                    break;
            }
        }

        public void ConvolutionA()
        {
            float[,] sharpening = {
            { 0, -1, 0 },
            { -1, 5, -1 },
            { 0, -1, 0 } };

            ConvolutionMask c = new ConvolutionMask(sharpening, texture);
            GetComponent<RawImage>().texture = c.GetTexture();
        }

        public void ConvolutionB()
        {
            float[,] sharpening = {
            { -1, -1, -1 },
            { -1, 9, -1 },
            { -1, -1, -1 } };

            ConvolutionMask c = new ConvolutionMask(sharpening, texture);
            GetComponent<RawImage>().texture = c.GetTexture();
        }

        public void ConvolutionC()
        {
            float[,] sharpening = {
            { 1, -2, 1 },
            { -2, 5, -2 },
            { 1, -2, 1 } };

            ConvolutionMask c = new ConvolutionMask(sharpening, texture);
            GetComponent<RawImage>().texture = c.GetTexture();
        }
    }
}