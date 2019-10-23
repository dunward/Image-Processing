using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AreaProcesses
{
    [ExecuteInEditMode]
    public class HomogeneityOperator : MonoBehaviour
    {
        public Texture2D source;
        [Range(0, 1)] public float threshold;

        private void Start()
        {
            GetComponent<RawImage>().texture = GetTexture();
        }

        public Texture2D GetTexture()
        {
            Texture2D newSource = new Texture2D(source.width, source.height);

            for (int i = 0; i < source.width; i++)
            {
                for (int k = 0; k < source.height; k++)
                {
                    float maximum = 0;
                    for (int mx = -1; mx <= 1; mx++)
                    {
                        for (int my = -1; my <= 1; my++)
                        {
                            if (mx == 0 && my == 0)
                                continue;

                            int x = (int)Mathf.Repeat(i + mx, source.width);
                            int y = (int)Mathf.Repeat(k + my, source.height);

                            maximum = Mathf.Max(maximum, source.GetPixel(i + mx, k + my).grayscale - source.GetPixel(i, k).grayscale);
                        }

                        if(threshold < maximum)
                        {
                            newSource.SetPixel(i, k, new Color(maximum, maximum, maximum));
                        }
                        else
                        {
                            newSource.SetPixel(i, k, Color.black);
                        }
                    }
                }
            }

            newSource.Apply();

            return newSource;
        }
    }
}