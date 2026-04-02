using System;
using UnityEngine;

public class TrigerControler : MonoBehaviour
{
    public event Action<Collider> TriggerActivated;

    private void OnTriggerEnter(Collider other)
    {
        TriggerActivated?.Invoke(other);
    }
}
