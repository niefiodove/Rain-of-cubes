using System;
using System.Collections;
using UnityEngine;

public class CubeSpawner : BaseSpawner<Cube>
{
    [SerializeField] private Collider _startPoint;

    public event Action<Collider> CubeDisable;

    protected override Cube CreateT()
    {
        Cube cube = Instantiate(_prefab);
        cube.WaitOver += ReturnToPool;
        _totalObjectsPool++;
        return cube;
    }
    protected override void InitializeT(Cube cube)
    {
        GetPosition(_startPoint);
        cube.transform.position = _position;
        cube.gameObject.SetActive(true);
        _totalObjectsScene++;
        _activeObjectsCount++;
        cube.ResetState();
    }

    protected override void GetPosition(Collider collider)
    {
        Vector3 min = collider.bounds.min;
        Vector3 max = collider.bounds.max;

        _position = new Vector3(
            UnityEngine.Random.Range(min.x, max.x),
            min.y,
            UnityEngine.Random.Range(min.z, max.z)
        );
    }

    protected override void ReleaseT(Cube cube)
    {
        base.ReleaseT(cube);

        if (cube.TryGetComponent<Collider>(out Collider collider))
            CubeDisable?.Invoke(collider);
    }

    protected override void DestroyT(Cube cube)
    {
        base.DestroyT(cube);
        cube.WaitOver -= ReturnToPool;
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
}
