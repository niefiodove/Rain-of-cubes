using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;
    [SerializeField] private Collider _startPoint;
    [SerializeField] private TriggerCollector _triggerCollector;
    [SerializeField] private Repainter _repainter;
    [SerializeField] private float _reapetRate = 0.3f;
    [SerializeField] private int _poolCapacity = 5;
    [SerializeField] private int _poolMaxSize = 5;

    private ObjectPool<GameObject> _pool;
    private HashSet<GameObject> _activeObjects = new HashSet<GameObject>();

    private void OnEnable()
    {
        foreach(TrigerControler trigerControler in _triggerCollector.TrigerControlers)
            trigerControler.TriggerActivated += OnTrigger;
    }

    private void OnDisable()
    {
        foreach (TrigerControler trigerControler in _triggerCollector.TrigerControlers)
            trigerControler.TriggerActivated -= OnTrigger;
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
        if(obj.TryGetComponent<Renderer>(out Renderer renderer))
            _repainter.RepaintToStartColor(renderer);
            
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _reapetRate);
    }

    private void Update()
    {
        Debug.Log($"Общее количество объектов - {_pool.CountAll}, Количество взятых объектов - {_pool.CountActive},  Количество свободных объектов {_pool.CountInactive} ");
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void OnTrigger(Collider other)
    {
        if (_activeObjects.Add(other.gameObject))
            StartCoroutine(ReturnWithDelay(other));
    }

    private Vector3 GetRandomPosition(Collider collider)
    {
        Vector3 min = collider.bounds.min;
        Vector3 max = collider.bounds.max;

        return new Vector3(Random.Range(min.x, max.x), min.y, Random.Range(min.z, max.z));
    }

    IEnumerator ReturnWithDelay(Collider other)
    {
        yield return new WaitForSeconds(2f);

        _activeObjects.Remove(other.gameObject);

        if (other != null && other.gameObject.activeInHierarchy)
        {
            _pool.Release(other.gameObject);
        }
    }
}
