using UnityEditor;
using UnityEngine;

namespace Sanimal.Editor
{
    [CustomEditor(typeof(MaterialCuverData))]
    public class MaterialCuverDataEditor : UnityEditor.Editor
    {
        public static MaterialCuverData CreateDataForMaterial(Material material)
        {
            var curveData = CreateInstance<MaterialCuverData>();
            curveData.name = "CurveData";
            AssetDatabase.AddObjectToAsset(curveData, material);
            AssetDatabase.SaveAssets();
            return curveData;
        }
    }
}