using System;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Sanimal.Editor
{
    public static class MaterialCurveProperty
    {
        public static void DrawCurveProperty(Material material, MaterialProperty property, bool updateImmediate = false,
            bool allowSetWidth = false, int defaultWidth = 50)
        {
            if (material == null || material.shader == null)
                throw new ArgumentNullException("Try draw null p1roperty");

            var name = property.name;
            var path = AssetDatabase.GetAssetPath(material);
            var cuverDataObject = AssetDatabase.LoadAssetAtPath<MaterialCuverData>(path);

            if (cuverDataObject == null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(property.displayName);
                if (GUILayout.Button("Create curve data"))
                    MaterialCuverDataEditor.CreateDataForMaterial(material);
                GUILayout.EndHorizontal();
                return;
            }

            if (!cuverDataObject.Curves.TryGetValue(name, out var curveData))
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(property.displayName);
                if (GUILayout.Button("Create Cuver for this property"))
                    cuverDataObject.Curves[name] = new(name, defaultWidth);
                GUILayout.EndHorizontal();
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUI.BeginChangeCheck();
            
            EditorGUILayout.CurveField(property.displayName, curveData.curve);
            if (allowSetWidth)
                curveData.width = Math.Max(EditorGUILayout.IntField("", curveData.width, GUILayout.MaxWidth(30)), 0);
            
            if (EditorGUI.EndChangeCheck())
            {
                if(updateImmediate) curveData.DealCurveTexture(material, property);
                else curveData.dirty = true;
            }
            
            if (curveData.dirty && GUILayout.Button("Apply", GUILayout.MaxWidth(50)))
            {
                curveData.DealCurveTexture(material, property);
                curveData.dirty = false;
            }
            EditorGUILayout.EndHorizontal();

            // Draw context menu
            var rect = GUILayoutUtility.GetLastRect();
            if (Event.current.type == EventType.ContextClick && rect.Contains(Event.current.mousePosition))
                ShowContextMenu(material, cuverDataObject, property);
        }

        private static void ShowContextMenu(Material material, MaterialCuverData materialCuverData, MaterialProperty property)
        {
            var curveData = materialCuverData.Curves[property.name];
            var menu = new GenericMenu();
            
            menu.AddItem(new GUIContent("Select texture"), false, () =>
            {
                Selection.activeObject = curveData.texture;
            });
            
            menu.AddItem(new GUIContent("Delete this curve data"), false, () =>
            {
                curveData.DeleteCurveTestureFromMaterial();
                materialCuverData.Curves.Remove(property.name);
            });

            // TODO 
            // menu.AddItem(new GUIContent("Destroy all curve data"), false, () => { Object.DestroyImmediate(cuverData); });

            menu.AddItem(new GUIContent("Update texture"), false,
                () =>
                {
                    curveData.DealCurveTexture(material, property);
                    curveData.dirty = false;
                });

            menu.ShowAsContext();
        }
    }
}