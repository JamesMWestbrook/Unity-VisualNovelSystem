using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.Serialization;
using Sirenix.OdinInspector;

[System.Serializable]
//[InlineProperty]
public class ResourcePathAsset<T> where T : UnityEngine.Object
{
    //    [HideInInspector]
    [SerializeField] private string path;

    [ShowInInspector, OnValueChanged(nameof(OnSet))]
    [HideLabel]
#if UNITY_EDITOR
    [SerializeField]
#endif
    private T asset;
    private void OnSet()
    {
#if UNITY_EDITOR
        if (asset == null)
        {
            path = "";
            return;
        }
        path = UnityEditor.AssetDatabase.GetAssetPath(asset);
        int start = path.IndexOf("Resources");
        path = path.Substring(start + 10); // add 10 to remove the 'Resources/' part as well
        path = System.IO.Path.ChangeExtension(path, null);
#endif
    }


    public T Asset
    {
        get
        {
            if (asset == null)
            {
                asset = Resources.Load<T>(path);
            }
            return asset;
        }
    }

    public T Get()
    {
        if (asset == null)
        {
            asset = Resources.Load<T>(path);
        }
        return asset;
    }

}