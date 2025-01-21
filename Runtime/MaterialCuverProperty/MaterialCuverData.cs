using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sanimal
{
    public class MaterialCuverData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField]
        private CurveTextureData[] cuverDatas;

        public Dictionary<string, CurveTextureData> Curves { get; private set; } = new();

        public void OnBeforeSerialize()
        {
            if (Curves == null)
            {
                Curves = new();
                return;
            }

            cuverDatas = Curves.Values.ToArray();
        }

        public void OnAfterDeserialize()
        {

            if (cuverDatas == null)
            {
                Curves = new();
                return;
            }

            Curves = cuverDatas.ToDictionary(k => k.name, v => v);
        }

        // private void OnDestroy()
        // {
        //     foreach (var texture in textures)
        //     {
        //         AssetDatabase.RemoveObjectFromAsset(texture);
        //         DestroyImmediate(texture);
        //     }
        // }
    }
}
