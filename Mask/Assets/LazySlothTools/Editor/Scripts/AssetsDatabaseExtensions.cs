namespace LazySloth.Utilities
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    
    public static class AssetsDatabaseExtensions
    {
        public static List<T> FindAssetsByType<T>() where T : Object
        {
            List<T> assets = new List<T>();
            string[] guids = AssetDatabase.FindAssets(string.Format("t:{0}", typeof(T)));
            for( int i = 0; i < guids.Length; i++ )
            {
                string assetPath = AssetDatabase.GUIDToAssetPath( guids[i] );
                T asset = AssetDatabase.LoadAssetAtPath<T>( assetPath );
                if( asset != null )
                {
                    assets.Add(asset);
                }
            }
            return assets;
        }

        public static T FindAssetByType<T>() where T : Object
        {
            var assets = FindAssetsByType<T>();
            if (assets.Count > 0)
            {
                Debug.LogWarning("found more than one matched type, returns first");
            }

            return assets[0];
        }
    }
}