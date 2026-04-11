using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private bool _isFirstCollision = true;
    private Repainter _repainter;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);
    private Collider _collider;
    private Rigidbody _rigidbody;

    public event Action<Cube> WaitOver;

    private void Awake()
    {
        _repainter = GetComponent<Repainter>();
        _collider = GetComponent<Collider>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _repainter.RepaintToStartColor();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent<Platform>(out Platform platform) && _isFirstCollision)
        {
            _isFirstCollision = false;

            _repainter.Repaint();
            StartCoroutine(ReturnWithDelay());
        }
    }

    private IEnumerator ReturnWithDelay()
    {
        yield return _waitForSeconds;
        _repainter.RepaintToStartColor();
        _collider.gameObject.SetActive(false);
        _rigidbody.linearVelocity = Vector3.zero;
        WaitOver?.Invoke(this);
    }
}
