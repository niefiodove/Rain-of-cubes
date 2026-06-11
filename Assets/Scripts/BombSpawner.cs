using UnityEngine;

public class BombSpawner : BaseSpawner<Bomb>
{
    [SerializeField] private CubeSpawner cubeSpawner;
    protected override void InitializeT(Bomb bomb)
    {
        bomb.transform.position = _position;
        bomb.gameObject.SetActive(true);
        _totalObjectsScene++;
        _activeObjectsCount++;
        bomb.ResetState();
    }

    protected override void GetPosition(Collider collider)
    {
        _position = collider.transform.position;
        _pool.Get();
    }

    protected override Bomb CreateT()
    {
        Bomb bomb = Instantiate(_prefab);
        bomb.WaitOver += ReturnToPool;
        _totalObjectsPool++;
        return bomb;
    }

    protected override void DestroyT(Bomb bomb)
    {
        base.DestroyT(bomb);
        bomb.WaitOver -= ReturnToPool;
    }

    private void OnEnable()
    {
        cubeSpawner.CubeDisable += GetPosition;
    }

    private void OnDisable()
    {
        cubeSpawner.CubeDisable -= GetPosition;
    }
}
