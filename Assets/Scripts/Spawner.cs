using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Collider _startPoint;
    [SerializeField] private Repainter _repainter;
    [SerializeField] private float _reapetRate = 0.3f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;

    private void OnEnable()
    {
        if (_prefab.TryGetComponent<Cube>(out Cube cube))
            cube.WaitOver += ReturnToPool;
    }

    private void OnDisable()
    {
        if (_prefab.TryGetComponent<Cube>(out Cube cube))
            cube.WaitOver -= ReturnToPool;
    }
    private void Awake()
    {
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomPosition(_startPoint);
        obj.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        obj.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            GetCube();
            yield return new WaitForSeconds(_reapetRate);
        }
    }


    private void GetCube()
    {
        _pool.Get();
    }

    private Vector3 GetRandomPosition(Collider collider)
    {
        Vector3 min = collider.bounds.min;
        Vector3 max = collider.bounds.max;

        return new Vector3(UnityEngine.Random.Range(min.x, max.x), min.y, UnityEngine.Random.Range(min.z, max.z));
    }

    private void ReturnToPool(GameObject gameObject)
    {
        _pool.Release(gameObject);
    }
}
