using UnityEngine;

public abstract class AData : ScriptableObject
{
    [SerializeField] private string id;

    public string Id => id;
}
