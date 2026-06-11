using UnityEngine;

public class UIBombHandler : UIHandler<BombSpawner>
{
    [SerializeField] private BombSpawner _bombSpawner;

    protected override void UpdateText()
    {
        string newTextTotalObjectsScene = $"{_textTotalObjectsScene}: {_bombSpawner.TotalObjectsScene}";
        string newTextTotalObjectsPool = $"{_textTotalObjectsPool}: {_bombSpawner.TotalObjectsPool}";
        string newTextActiveObjectsCount = $"{_textActiveObjectsCount}: {_bombSpawner.ActiveObjectsCount}";

        _name.text = _bombSpawner.NameObject;

        _totalObjectsScene.text = newTextTotalObjectsScene;
        _totalObjectsPool.text = newTextTotalObjectsPool;
        _activeObjectsCount.text = newTextActiveObjectsCount;
    }
}
