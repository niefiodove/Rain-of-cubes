using UnityEngine;
using UnityEngine.Pool;

public abstract class BaseSpawner<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected T _prefab;
    [SerializeField] protected float _reapetRate = 0.3f;
    [SerializeField] protected int _poolCapacity = 5;
    [SerializeField] protected int _poolMaxSize = 5;

    protected ObjectPool<T> _pool;
    protected Vector3 _position;
    protected int _totalObjectsScene;
    protected int _totalObjectsPool;
    protected int _activeObjectsCount;
    protected string _nameObject;

    public int TotalObjectsScene => _totalObjectsScene;
    public int TotalObjectsPool => _totalObjectsPool;
    public int ActiveObjectsCount => _activeObjectsCount;
    public string NameObject => _nameObject;

    private void Awake()
    {
        _pool = new ObjectPool<T>(
            createFunc: CreateT,
            actionOnGet: InitializeT,
            actionOnRelease: ReleaseT,
            actionOnDestroy: DestroyT,
            collectionCheck: true,
            defaultCapacity: _poolCapacity,
            maxSize: _poolMaxSize);

        _nameObject = _prefab.name;
    }

    protected abstract T CreateT();

    protected abstract void InitializeT(T objectT);
    protected abstract void GetPosition(Collider collider);

    protected virtual void ReleaseT(T objectT)
    {
        objectT.gameObject.SetActive(false);
        _activeObjectsCount--;
    }

    protected virtual void DestroyT(T objectT)
    {
        Destroy(objectT.gameObject);
        _totalObjectsPool--;
        _activeObjectsCount--;
    }

    protected virtual void ReturnToPool(T objectT)
    {
        _pool.Release(objectT);
    }
}