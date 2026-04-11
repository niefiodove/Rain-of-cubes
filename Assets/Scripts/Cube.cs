using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    private bool _isFirstCollision = true;
    private Repainter _repainter;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);
    private Rigidbody _rigidbody;

    public event Action<Cube> WaitOver;

    private void Awake()
    {
        _repainter = GetComponent<Repainter>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _repainter.RepaintToStartColor();
    }

    private void OnEnable()
    {
        _isFirstCollision = true;
        _rigidbody.linearVelocity = Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_isFirstCollision && other.gameObject.TryGetComponent<Platform>(out Platform platform))
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

        WaitOver?.Invoke(this);
    }

    public void ResetState()
    {
        _isFirstCollision = true;
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
    }
}