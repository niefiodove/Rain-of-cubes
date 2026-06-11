using UnityEngine;

public class UICubeHandler : UIHandler<CubeSpawner>
{
    [SerializeField] private CubeSpawner _cubeSpawner;

    protected override void UpdateText()
    {
        string newTextTotalObjectsScene = $"{_textTotalObjectsScene}: {_cubeSpawner.TotalObjectsScene}";
        string newTextTotalObjectsPool = $"{_textTotalObjectsPool}: {_cubeSpawner.TotalObjectsPool}";
        string newTextActiveObjectsCount = $"{_textActiveObjectsCount}: {_cubeSpawner.ActiveObjectsCount}";

        _name.text = _cubeSpawner.NameObject;

        _totalObjectsScene.text = newTextTotalObjectsScene;
        _totalObjectsPool.text = newTextTotalObjectsPool;
        _activeObjectsCount.text = newTextActiveObjectsCount;
    }
}
