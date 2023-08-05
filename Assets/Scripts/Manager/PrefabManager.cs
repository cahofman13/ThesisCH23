using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager instance;
    public Dictionary<string, GameObject> prefabs = new Dictionary<string, GameObject>();

    [SerializeField] GameObject pMoveForward;

    private void Start()
    {
        if (instance) Destroy(this);
        else instance = this;

        prefabs.Add("MoveForward", pMoveForward);
    }
}
