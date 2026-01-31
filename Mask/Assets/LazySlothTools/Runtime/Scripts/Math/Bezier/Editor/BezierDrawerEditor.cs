namespace LazySloth.Utilities.Math
{
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(BezierDrawer))] public class BezierDrawerEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            var script = (BezierDrawer) target;
            script._startPoint = Handles.DoPositionHandle(script._startPoint, Quaternion.identity);
            script._controlPoint = Handles.DoPositionHandle(script._controlPoint, Quaternion.identity);
            script._endPoint = Handles.DoPositionHandle(script._endPoint, Quaternion.identity);
        }
    }
}