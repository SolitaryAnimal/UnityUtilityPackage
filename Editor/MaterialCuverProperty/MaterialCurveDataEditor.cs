using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using ObjectField = UnityEditor.Search.ObjectField;

namespace Sanimal.Editor
{
    [CustomEditor(typeof(MaterialCuverData))]
    public class MaterialCurveDataEditor : UnityEditor.Editor
    {
        public static MaterialCuverData CreateDataForMaterial(Material material)
        {
            var curveData = CreateInstance<MaterialCuverData>();
            curveData.name = "CurveData";
            AssetDatabase.AddObjectToAsset(curveData, material);
            AssetDatabase.SaveAssets();
            return curveData;
        }

        private MaterialCuverData Target => target as MaterialCuverData;
        
        public override VisualElement CreateInspectorGUI()
        {
            
            var root = new VisualElement() { name = "root" };
            
            root.Add(new Button(() =>
            {
                if(EditorUtility.DisplayDialog("Warning", "Are you sure you want to remove all sub texture from material?", "Yes", "No"))
                    RemoveAllSubTextureFromMaterial();
            }) {text = "REMOVE ALL SUB TEXTURE"});
            root.Add(new Button(() =>
            {
                if(EditorUtility.DisplayDialog("Warning", "Are you sure you want to remove curve data and all sub texture from material?", "Yes", "No"))
                    RemoverCurveDataFromMaterial();
            }) {text = "REMOVE CURVE DATA AND ALL SUB TEXTURE"});

            // var dataProperty = serializedObject.FindProperty("cuverDatas");
            // var listView = new ListView();
            // listView.showBorder = true;
            // listView.dataSource = Target;
            // listView.itemsSource = Target.Curves.Values.ToList();
            // listView.BindProperty(dataProperty);
            //
            // listView.makeItem = () => new PropertyField();
            // listView.bindItem = (element, i) => ((PropertyField)element).BindProperty(dataProperty.GetArrayElementAtIndex(i));
            // root.Add(listView);
            
            return root;
        }

        public void RemoverCurveDataFromMaterial()
        {
            RemoveAllSubTextureFromMaterial();
            AssetDatabase.RemoveObjectFromAsset(Target);
            AssetDatabase.SaveAssets();
        }
        
        public void RemoveAllSubTextureFromMaterial()
        {
            var path = AssetDatabase.GetAssetPath(Target);
            foreach (var tex in AssetDatabase.LoadAllAssetRepresentationsAtPath(path).Where(o => o is Texture))
            {
                AssetDatabase.RemoveObjectFromAsset(tex);
                DestroyImmediate(tex);
            }
            AssetDatabase.SaveAssets();
        }
    }
}