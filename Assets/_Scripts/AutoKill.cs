using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoKill : MonoBehaviour
{
    [SerializeField] float LifeSpan = 2f;

    void Start()
    {
        Destroy(gameObject, LifeSpan);
    }
}
