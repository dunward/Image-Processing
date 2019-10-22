using AreaProcesses;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AreaProcesses
{
    [ExecuteInEditMode]
    public class Convolutions : MonoBehaviour
    {
        public Texture2D t;
        // Update is called once per frame
        void Update()
        {
            var matrix = new float[3, 3];
            matrix[0, 0] = -1;
            matrix[2, 2] = 1;
            Embossing c = new Embossing(matrix, t);
            GetComponent<RawImage>().texture = c.GetTextureGrayScale();
        }
    }
}