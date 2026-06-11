using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]

public class Bomb : MonoBehaviour
{
    [SerializeField] private float _minimumLifetime = 2f;
    [SerializeField] private float _maximumLifetime = 5f;

    private Repainter _repainter;
    private WaitForSeconds _waitForSeconds;
    private Rigidbody _rigidbody;
    private float _lifetimeBomb;

    public event Action<Bomb> WaitOver;
    public event Action BombHasBeenPlanted;
    public float LifetimeBomb => _lifetimeBomb;

    public void ResetState()
    {
        _rigidbody.linearVelocity = Vector3.zero;
        _rigidbody.angularVelocity = Vector3.zero;
        _lifetimeBomb = SetLifetimeBomb();
        _waitForSeconds = new WaitForSeconds(_lifetimeBomb);
    }

    private void Awake()
    {
        _repainter = GetComponent<Repainter>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        if (_repainter != null)
        {
            StartCoroutine(ReturnWithDelay());
            _repainter.FadeOut();
        }
    }

    private IEnumerator ReturnWithDelay()
    {
        yield return _waitForSeconds;
        _repainter.RepaintToStartColor();

        BombHasBeenPlanted?.Invoke();
        WaitOver?.Invoke(this);
    }

    private float SetLifetimeBomb()
    {
        return UnityEngine.Random.Range(_minimumLifetime, _maximumLifetime);
    }
}