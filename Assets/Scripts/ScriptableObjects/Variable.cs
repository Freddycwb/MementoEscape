using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Variable<T> : ScriptableObject
{
    [Multiline]
    public string DeveloperDescription = "";
    public T Value;

    public bool PersistBetweenScenes = false;

    private void OnEnable()
    {
        if (PersistBetweenScenes)
        {
            hideFlags = HideFlags.DontUnloadUnusedAsset;
        }
    }

    public void SetValue(T value)
    {
        Value = value;
    }

    public void SetValue(Variable<T> value)
    {
        Value = value.Value;
    }
}
