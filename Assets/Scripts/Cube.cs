using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]

public class Cube : MonoBehaviour
{
    [SerializeField] private Repainter _repainter;
    private bool _isFirstCollision = true;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);

    public event Action<Cube> WaitOver;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        _repainter.RepaintToStartColor(renderer);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer < gameObject.layer && _isFirstCollision)
        {
            _isFirstCollision = false;
            Collider collider = GetComponent<Collider>();
            Renderer renderer = GetComponent<Renderer>();

            _repainter.Repaint(renderer);
            StartCoroutine(ReturnWithDelay(collider));
        }
    }

    private IEnumerator ReturnWithDelay(Collider collider)
    {
        yield return _waitForSeconds;
        Renderer renderer = GetComponent<Renderer>();
        Rigidbody body = GetComponent<Rigidbody>();

        if (renderer != null)
            _repainter.RepaintToStartColor(renderer);

        collider.gameObject.SetActive(false);
        body.linearVelocity = Vector3.zero;
        WaitOver?.Invoke(gameObject.GetComponent<Cube>());
    }
}
