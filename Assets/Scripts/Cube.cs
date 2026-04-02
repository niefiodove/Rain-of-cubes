using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private Repainter _repainter;
    private bool _isFirstCollision = true;
    private WaitForSeconds _waitForSeconds = new WaitForSeconds(2f);

    public event Action<GameObject> WaitOver;

    private void Start()
    {
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            _repainter.RepaintToStartColor(renderer);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer <= gameObject.layer && _isFirstCollision)
        {
            _isFirstCollision = false;
            Collider collider = GetComponent<Collider>();

            if (collider != null)
            {
                _repainter.Repaint(collider);
                StartCoroutine(ReturnWithDelay(collider));
            }
        }
    }

    private IEnumerator ReturnWithDelay(Collider collider)
    {
        yield return _waitForSeconds;
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
            _repainter.RepaintToStartColor(renderer);

        collider.gameObject.SetActive(false);
        WaitOver?.Invoke(gameObject);
    }
}
