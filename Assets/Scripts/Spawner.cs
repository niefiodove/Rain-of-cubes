using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private Collider _startPoint;
    [SerializeField] private float _reapetRate = 0.3f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<Cube> _pool;

    private void Awake()
    {
        _pool = new ObjectPool<Cube>(
            createFunc: CreateCube,
            actionOnGet: InitializeCube,
            actionOnRelease: ReleaseCube,
            actionOnDestroy: DestroyCube,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);
    }

    private Cube CreateCube()
    {
        Cube cube = Instantiate(_prefab);
        cube.WaitOver += ReturnToPool;
        return cube;
    }

    private void InitializeCube(Cube cube)
    {
        cube.transform.position = GetRandomPosition(_startPoint);
        cube.gameObject.SetActive(true);
        cube.ResetState();
    }

    private void ReleaseCube(Cube cube)
    {
        cube.gameObject.SetActive(false);
    }

    private void DestroyCube(Cube cube)
    {
        cube.WaitOver -= ReturnToPool;
        Destroy(cube.gameObject);
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
            _pool.Get();
            yield return waitForSeconds;
        }
    }

    private Vector3 GetRandomPosition(Collider collider)
    {
        Vector3 min = collider.bounds.min;
        Vector3 max = collider.bounds.max;

        return new Vector3(
            Random.Range(min.x, max.x),
            min.y,
            Random.Range(min.z, max.z)
        );
    }

    private void ReturnToPool(Cube cube)
    {
        _pool.Release(cube);
    }
}