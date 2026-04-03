using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Collider _startPoint;
    [SerializeField] private Repainter _repainter;
    [SerializeField] private float _reapetRate = 0.3f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;

    private void OnEnable()
    {
            _prefab.WaitOver += ReturnToPool;
    }

    private void OnDisable()
    {
            _prefab.WaitOver -= ReturnToPool;
    }
    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => InitializeCube(obj),
            actionOnRelease: (obj) => obj.gameObject.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private void InitializeCube(Cube obj)
    {
        obj.transform.position = GetRandomPosition(_startPoint);
        obj.gameObject.SetActive(true);
    }

    private void Start()
    {
        StartCoroutine(SpawnCoroutine());
    }

    private IEnumerator SpawnCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(_reapetRate);

        while (enabled)
        {
            GetCube();
            yield return waitForSeconds;
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

    private void ReturnToPool(Cube cube)
    {
        _pool.Release(cube);
    }
}
