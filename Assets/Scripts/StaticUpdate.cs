using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticUpdate : MonoBehaviour
{
    public static System.Action OnUpdate;

    static StaticUpdate instance;

    private void Awake()
    {
        if (instance != null) {
            Destroy(this);
            return;
        }
        instance = this;
    }

    void Update()
    {
        OnUpdate?.Invoke();
    }
}
