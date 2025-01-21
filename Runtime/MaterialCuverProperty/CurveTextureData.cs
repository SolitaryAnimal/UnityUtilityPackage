using System;
using UnityEngine;

namespace Sanimal
{
    [Serializable]
    public class CurveTextureData
    {
        public string name;
        public AnimationCurve curve;
        public Texture2D texture;
        
        #if UNITY_EDITOR
        public bool dirty;
        public int width;

        public CurveTextureData(string name, int width)
        {
            this.name = name;
            curve = new();
            this.width = width;
        }
        #endif
        
        private CurveTextureData() { }
    }
}