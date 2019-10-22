using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AreaProcesses
{
    public class Embossing : ConvolutionMask
    {
        public Embossing(float[,] matrix, Texture2D source) : base(matrix, source)
        {

        }

        protected override Color ColorEffect(Color color)
        {
            return color + new Color(0.5f, 0.5f, 0.5f);
        }
    }
}