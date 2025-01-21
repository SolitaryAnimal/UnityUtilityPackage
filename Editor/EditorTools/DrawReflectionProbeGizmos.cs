using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Overlays;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Sanimal.Editor
{
    [Overlay(typeof(SceneView), "Draw Reflection Probe Gizmos")]

    public class DrawReflectionProbeGizmos : Overlay
    {
        private static bool drawGizmos = false;

        private static Color selectedColor = Color.green,
            selectedBlendColor = Color.red,
            unSelectedColor = Color.yellow,
            unSelectedBlendColor = new Color(1, 1, 0, 0.5f);

        private static HashSet<GameObject> reflectionProbes;
        private static FieldInfo[] colorFields;


        static DrawReflectionProbeGizmos()
        {
            reflectionProbes = new();
            UpdateReflectionProbes();
            colorFields = typeof(DrawReflectionProbeGizmos).GetFields(BindingFlags.Static | BindingFlags.NonPublic)
                .Where(f => f.FieldType == typeof(Color)).ToArray();
            EditorApplication.hierarchyChanged += UpdateReflectionProbes;
            EditorApplication.projectChanged += UpdateReflectionProbes;
        }

        private static void UpdateReflectionProbes()
        {
            reflectionProbes.Clear();
            reflectionProbes.AddRange(Object.FindObjectsByType<ReflectionProbe>(FindObjectsSortMode.None)
                .Select(r => r.gameObject));
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NotInSelectionHierarchy)]
        private static void DrawSelectedGizmos(ReflectionProbe reflectionProbe, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) > 0)
                DrawGizmos(reflectionProbe, selectedColor, selectedBlendColor);
            if ((gizmoType & GizmoType.NotInSelectionHierarchy) > 0 &&
                reflectionProbes.Contains(Selection.activeGameObject))
                DrawGizmos(reflectionProbe, unSelectedColor, unSelectedBlendColor);
        }

        private static void DrawGizmos(ReflectionProbe reflectionProbe, Color borderColor, Color blendColor)
        {
            if (!drawGizmos) return;
            Gizmos.color = borderColor;
            var transform = reflectionProbe.transform;
            Gizmos.DrawWireCube(transform.position + reflectionProbe.center, reflectionProbe.size);
            Gizmos.color = blendColor;
            Gizmos.DrawWireCube(transform.position + reflectionProbe.center,
                reflectionProbe.size - Vector3.one * reflectionProbe.blendDistance);
        }

        public override VisualElement CreatePanelContent()
        {
            var root = new VisualElement { name = "root", dataSource = this };
            root.style.minWidth = 200;

            var toggle = new Toggle("Enable") { value = drawGizmos };
            toggle.RegisterValueChangedCallback((e) => drawGizmos = e.newValue);
            root.Add(toggle);

            foreach (var colorField in colorFields)
            {
                var selectColorField = new ColorField(colorField.Name) { value = (Color)colorField.GetValue(null) };
                selectColorField.RegisterValueChangedCallback(e => colorField.SetValue(null, e.newValue));
                root.Add(selectColorField);
            }

            return root;
        }
    }
}
