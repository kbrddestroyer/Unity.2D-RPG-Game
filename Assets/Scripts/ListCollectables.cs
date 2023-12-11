using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListCollectables : MonoBehaviour
{
    private static ListCollectables instance;
    public static ListCollectables Instance { get => instance; }

    [SerializeField] private CollectableBase[] collectables;

    public CollectableBase getCollectable()
    {
        return collectables[Random.Range(0, collectables.Length)]; 
    }

    private void Awake()
    {
        instance = this;
    }
}
