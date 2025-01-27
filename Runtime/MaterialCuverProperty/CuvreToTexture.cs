using System.Linq;
using UnityEngine;

namespace Sanimal
{
    public static class CurveToTexture
    {
        public static void UpdateTextureFromCurveCPU(AnimationCurve curve, Texture2D texture)
        {
            var width = texture.width;
            var range = Enumerable.Range(0, width);
            width--;
            texture.SetPixels(range.Select(x => new Color(curve.Evaluate((float)x / width), 0, 0)).ToArray());
            texture.Apply();
        }
    }
}