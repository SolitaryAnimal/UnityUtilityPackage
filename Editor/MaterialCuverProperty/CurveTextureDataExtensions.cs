using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Sanimal.Editor
{
    public static class CurveTextureDataExtensions
    {
        public static void CreateTextureForMaterial(this CurveTextureData curveData, int width, Material material, MaterialProperty property)
        {
            Texture2D texture = new Texture2D(width, 1, TextureFormat.R16, false);
            texture.wrapMode = TextureWrapMode.Clamp;
            
            var name = property.name;
            texture.name = (name.StartsWith('_') ? "CurveTexture" : "CurveTexture_") + name;
            
            AssetDatabase.AddObjectToAsset(texture, material);
            AssetDatabase.SaveAssets();

            curveData.texture = texture;

            property.textureValue = texture;
        }
        
        public static void DeleteCurveTestureFromMaterial(this CurveTextureData curveData)
        {
            AssetDatabase.RemoveObjectFromAsset(curveData.texture);
            Object.DestroyImmediate(curveData.texture);
            AssetDatabase.SaveAssets();
        }

        public static void UpdateCurveTexture(this CurveTextureData curveData)
        {
            var texture = curveData.texture;
            var curve = curveData.curve;
            
            CurveToTexture.UpdateTextureFromCurveCPU(curve, texture);
            
            EditorUtility.SetDirty(curveData.texture);
            AssetDatabase.SaveAssets();
        }

        public static void DealCurveTexture(this CurveTextureData curveData, Material material, MaterialProperty property)
        {
            // Deal Texture
            if (curveData.texture == null)
            {
                CreateTextureForMaterial(curveData, curveData.width, material, property);
                UpdateCurveTexture(curveData);
            }
            else if (curveData.texture.width == curveData.width)
            {
                curveData.UpdateCurveTexture();
            }
            else
            {
                curveData.DeleteCurveTestureFromMaterial();
                CreateTextureForMaterial(curveData, curveData.width, material, property);
                UpdateCurveTexture(curveData);
            }

            property.textureValue = curveData.texture;
        }
    }
}