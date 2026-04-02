using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class TriggerCollector : MonoBehaviour
{
    public TrigerControler[] TrigerControlers { get; private set; }
    private void Awake()
    {
        TrigerControlers = GetComponentsInChildren<TrigerControler>();
    }
}
