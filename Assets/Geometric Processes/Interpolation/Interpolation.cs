using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GeometricProcesses
{
    [ExecuteInEditMode]
    public class Interpolation : MonoBehaviour
    {
        public Texture2D texture;
        public Texture2D origin;
        public RawImage nearest;
        public RawImage nearestCompare;
        public RawImage bilinear;
        public RawImage bilinearCompare;
        public RawImage bicubic;
        public RawImage bicubicCompare;
        public RawImage bspline;
        public RawImage bsplineCompare;

        private void Update()
        {
            GetComponent<RawImage>().texture = texture;
            Nearest();
            Bilinear();
            Bicubic();
            BSpline();
        }

        private void Nearest()
        {
            float v = 512 / 64f;

            Texture2D newTexture = new Texture2D(512, 512);

            for (int i = 0; i < newTexture.width; i++)
            {
                for (int k = 0; k < newTexture.height; k++)
                {
                    newTexture.SetPixel(i, k, texture.GetPixel((int)(i / v), (int)(k / v)));
                }
            }

            newTexture.Apply();
            nearest.texture = newTexture;
            Texture2D compare = new Texture2D(512, 512);

            for (int i = 0; i < compare.width; i++)
            {
                for (int k = 0; k < compare.height; k++)
                {
                    var color = origin.GetPixel(i, k) - newTexture.GetPixel(i, k);
                    if (color.grayscale == 0)
                    {
                        compare.SetPixel(i, k, color);
                    }
                    else
                    {
                        compare.SetPixel(i, k, Color.black);
                    }
                }
            }

            compare.Apply();

            nearestCompare.texture = compare;
        }

        private void Bilinear()
        {
            Texture2D newTexture = new Texture2D(512, 512);

            for (int i = 0; i < newTexture.width; i++)
            {
                for (int k = 0; k < newTexture.height; k++)
                {
                    int fx = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, i));
                    int fy = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, k));
                    int fx1 = fx + 1;
                    int fy1 = fy + 1;

                    float ix = Mathf.InverseLerp(0, 64, fx);
                    float iy = Mathf.InverseLerp(0, 64, fy);

                    float ix1 = Mathf.InverseLerp(0, 64, fx1);
                    float iy1 = Mathf.InverseLerp(0, 64, fy1);

                    int x = (int)Mathf.Lerp(0, 512, ix);
                    int y = (int)Mathf.Lerp(0, 512, iy);

                    int x1 = (int)Mathf.Lerp(0, 512, ix1);
                    int y1 = (int)Mathf.Lerp(0, 512, iy1);

                    float xWeight = Mathf.InverseLerp(x, x1, i);
                    float yWeight = Mathf.InverseLerp(y, y1, k);

                    var tl = texture.GetPixel(fx, fy1);
                    var tr = texture.GetPixel(fx1, fy1);
                    var bl = texture.GetPixel(fx, fy);
                    var br = texture.GetPixel(fx1, fy);

                    var color1 = Color.Lerp(tl, tr, xWeight);
                    var color2 = Color.Lerp(bl, br, xWeight);
                    var color = Color.Lerp(color2, color1, yWeight);

                    newTexture.SetPixel(i, k, color);
                }
            }

            newTexture.Apply();
            bilinear.texture = newTexture;
            Texture2D compare = new Texture2D(512, 512);

            for (int i = 0; i < compare.width; i++)
            {
                for (int k = 0; k < compare.height; k++)
                {
                    var color = origin.GetPixel(i, k) - newTexture.GetPixel(i, k);
                    if(color.grayscale == 0)
                    {
                        compare.SetPixel(i, k, color);
                    }
                    else
                    {
                        compare.SetPixel(i, k, Color.black);
                    }
                }
            }

            compare.Apply();

            bilinearCompare.texture = compare;
        }

        private void Bicubic()
        {
            Texture2D newTexture = new Texture2D(512, 512);

            for (int i = 0; i < newTexture.width; i++)
            {
                for (int k = 0; k < newTexture.height; k++)
                {
                    int fx = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, i));
                    int fy = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, k));
                    int fx1 = fx + 1;
                    int fy1 = fy + 1;

                    float ix = Mathf.InverseLerp(0, 64, fx);
                    float iy = Mathf.InverseLerp(0, 64, fy);

                    float ix1 = Mathf.InverseLerp(0, 64, fx1);
                    float iy1 = Mathf.InverseLerp(0, 64, fy1);

                    int x = (int)Mathf.Lerp(0, 512, ix);
                    int y = (int)Mathf.Lerp(0, 512, iy);

                    int x1 = (int)Mathf.Lerp(0, 512, ix1);
                    int y1 = (int)Mathf.Lerp(0, 512, iy1);

                    float xWeight = Mathf.InverseLerp(x, x1, i);
                    float yWeight = Mathf.InverseLerp(y, y1, k);

                    var c0 = BicubicInterpolation(texture.GetPixel(fx - 1, fy - 1),
                                                    texture.GetPixel(fx, fy - 1),
                                                    texture.GetPixel(fx + 1, fy - 1),
                                                    texture.GetPixel(fx + 2, fy - 1), xWeight);

                    var c1 = BicubicInterpolation(texture.GetPixel(fx - 1, fy),
                                                    texture.GetPixel(fx, fy),
                                                    texture.GetPixel(fx + 1, fy),
                                                    texture.GetPixel(fx + 2, fy), xWeight);

                    var c2 = BicubicInterpolation(texture.GetPixel(fx - 1, fy + 1),
                                                    texture.GetPixel(fx, fy + 1),
                                                    texture.GetPixel(fx + 1, fy + 1),
                                                    texture.GetPixel(fx + 2, fy + 1), xWeight);

                    var c3 = BicubicInterpolation(texture.GetPixel(fx - 1, fy + 2),
                                                    texture.GetPixel(fx, fy + 2),
                                                    texture.GetPixel(fx + 1, fy + 2),
                                                    texture.GetPixel(fx + 2, fy + 2), xWeight);

                    var color = BicubicInterpolation(c0, c1, c2, c3, yWeight);

                    newTexture.SetPixel(i, k, color);
                }
            }

            newTexture.Apply();
            bicubic.texture = newTexture;
            Texture2D compare = new Texture2D(512, 512);

            for (int i = 0; i < compare.width; i++)
            {
                for (int k = 0; k < compare.height; k++)
                {
                    var color = origin.GetPixel(i, k) - newTexture.GetPixel(i, k);
                    if (color.grayscale == 0)
                    {
                        compare.SetPixel(i, k, color);
                    }
                    else
                    {
                        compare.SetPixel(i, k, Color.black);
                    }
                }
            }

            compare.Apply();

            bicubicCompare.texture = compare;
        }
        private void BSpline()
        {
            Texture2D newTexture = new Texture2D(512, 512);

            for (int i = 0; i < newTexture.width; i++)
            {
                for (int k = 0; k < newTexture.height; k++)
                {
                    int fx = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, i));
                    int fy = (int)Mathf.Lerp(0, 64, Mathf.InverseLerp(0, 512, k));
                    int fx1 = fx + 1;
                    int fy1 = fy + 1;

                    float ix = Mathf.InverseLerp(0, 64, fx);
                    float iy = Mathf.InverseLerp(0, 64, fy);

                    float ix1 = Mathf.InverseLerp(0, 64, fx1);
                    float iy1 = Mathf.InverseLerp(0, 64, fy1);

                    int x = (int)Mathf.Lerp(0, 512, ix);
                    int y = (int)Mathf.Lerp(0, 512, iy);

                    int x1 = (int)Mathf.Lerp(0, 512, ix1);
                    int y1 = (int)Mathf.Lerp(0, 512, iy1);

                    float xWeight = Mathf.InverseLerp(x, x1, i);
                    float yWeight = Mathf.InverseLerp(y, y1, k);

                    var c0 = BSplineInterpolation(texture.GetPixel(fx - 1, fy - 1),
                                                    texture.GetPixel(fx, fy - 1),
                                                    texture.GetPixel(fx + 1, fy - 1),
                                                    texture.GetPixel(fx + 2, fy - 1), xWeight);

                    var c1 = BSplineInterpolation(texture.GetPixel(fx - 1, fy),
                                                    texture.GetPixel(fx, fy),
                                                    texture.GetPixel(fx + 1, fy),
                                                    texture.GetPixel(fx + 2, fy), xWeight);

                    var c2 = BSplineInterpolation(texture.GetPixel(fx - 1, fy + 1),
                                                    texture.GetPixel(fx, fy + 1),
                                                    texture.GetPixel(fx + 1, fy + 1),
                                                    texture.GetPixel(fx + 2, fy + 1), xWeight);

                    var c3 = BSplineInterpolation(texture.GetPixel(fx - 1, fy + 2),
                                                    texture.GetPixel(fx, fy + 2),
                                                    texture.GetPixel(fx + 1, fy + 2),
                                                    texture.GetPixel(fx + 2, fy + 2), xWeight);

                    var color = BSplineInterpolation(c0, c1, c2, c3, yWeight);
                    color.a = 1;

                    newTexture.SetPixel(i, k, color);
                }
            }

            newTexture.Apply();
            bspline.texture = newTexture;
            Texture2D compare = new Texture2D(512, 512);

            for (int i = 0; i < compare.width; i++)
            {
                for (int k = 0; k < compare.height; k++)
                {
                    var color = origin.GetPixel(i, k) - newTexture.GetPixel(i, k);
                    if (color.grayscale == 0)
                    {
                        compare.SetPixel(i, k, color);
                    }
                    else
                    {
                        compare.SetPixel(i, k, Color.black);
                    }
                }
            }

            compare.Apply();

            bsplineCompare.texture = compare;
        }

        private Color BicubicInterpolation(Color c0, Color c1, Color c2, Color c3, float t)
        {
            var a0 = -0.5f * c0 + 1.5f * c1 - 1.5f * c2 + 0.5f * c3;
            var a1 = c0 - 2.5f * c1 + 2 * c2 - 0.5f * c3;
            var a2 = -0.5f * c0 + 0.5f * c2;
            var a3 = c1;

            return ((a0 * t + a1) * t + a2) * t + a3;
        }

        private Color BSplineInterpolation(Color c0, Color c1, Color c2, Color c3, float t)
        {
            var a0 = -1f * c0 + 3f * c1 - 3f * c2 + 1f * c3;
            var a1 = 3f * c0 - 6f * c1 + 3 * c2;
            var a2 = -3f * c0 + 3f * c2;
            var a3 = c0 + 4f * c1 + c2;

            return (((a0 * t + a1) * t + a2) * t + a3) / 6f;
        }
    }
}