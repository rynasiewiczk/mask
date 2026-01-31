namespace LazySloth.Utilities
{
    using UnityEditor;

    public static class ConsoleExtension
    {
        [MenuItem("Tools/ClearConsole _F8")]
        private static void ClearConsole()
        {
            var logEntries = System.Type.GetType("UnityEditor.LogEntries,UnityEditor.dll");
            var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
            clearMethod.Invoke(null,null);
        }
    }
}