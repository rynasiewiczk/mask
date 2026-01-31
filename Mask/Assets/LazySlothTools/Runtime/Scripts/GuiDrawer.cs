namespace LazySloth.Utilities
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;

    public class GuiDrawer : MonoBehaviour
    {
        private static HashSet<Action> _onGui = new HashSet<Action>();

        public static void RegisterOnGui(Action action)
        {
            _onGui.Add(action);
        }

        public static void UnRegisterOnGui(Action action)
        {
            _onGui.Remove(action);
        }

        private void OnGUI()
        {
            _onGui.ForEach(x => x.Invoke());
        }
    }
}