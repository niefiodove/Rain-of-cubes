using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    private Repainter _repainter;
    private bool _isFirstCollision = true;

    public event Action<GameObject> WaitOver;

    private void Awake()
    {
        _repainter = FindFirstObjectByType<Repainter>();
    }

    private void Start()
    {
        if (gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            _repainter.RepaintToStartColor(renderer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer <= gameObject.layer && _isFirstCollision)
        {
            _isFirstCollision = false;

            if (TryGetComponent<Collider>(out Collider collider))
            {
                _repainter.Repaint(collider);
                StartCoroutine(ReturnWithDelay(collider));
            }
        }
    }

    IEnumerator ReturnWithDelay(Collider collider)
    {
        yield return new WaitForSeconds(2f);
        if (collider.gameObject.TryGetComponent<Renderer>(out Renderer renderer))
            _repainter.RepaintToStartColor(renderer);

        collider.gameObject.SetActive(false);
        WaitOver?.Invoke(gameObject);
    }
}
