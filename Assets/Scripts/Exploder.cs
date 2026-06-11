using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Bomb))]
public class Exploder : MonoBehaviour
{
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private float _explosionRadius = 2f;
    [SerializeField] private float _explosionForce = 500f;

    private Bomb _bomb;

    private void Awake()
    {
        _bomb = GetComponent<Bomb>();
    }

    private void OnEnable()
    {
        _bomb.BombHasBeenPlanted += ScatterExplosion;
    }

    private void OnDisable()
    {
        _bomb.BombHasBeenPlanted -= ScatterExplosion;
    }

    public void ScatterExplosion()
    {
        foreach (Rigidbody explotableObject in GetExplodableObject())
            explotableObject.AddExplosionForce(_explosionForce, gameObject.transform.position, _explosionRadius);

        Instantiate(_effect, gameObject.transform.position, Quaternion.identity);
    }

    private List<Rigidbody> GetExplodableObject()
    {
        Collider[] hits = Physics.OverlapSphere(gameObject.transform.position, _explosionRadius);

        List<Rigidbody> objects = new();

        foreach (Collider hit in hits)
            if (hit.attachedRigidbody != null)
                objects.Add(hit.attachedRigidbody);

        return objects;
    }
}
