using TMPro;
using UnityEngine;

public abstract class UIHandler<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _name;
    [SerializeField] protected TextMeshProUGUI _totalObjectsScene;
    [SerializeField] protected TextMeshProUGUI _totalObjectsPool;
    [SerializeField] protected TextMeshProUGUI _activeObjectsCount;

    [SerializeField] protected string _textTotalObjectsScene = "Total";
    [SerializeField] protected string _textTotalObjectsPool = "On stage";
    [SerializeField] protected string _textActiveObjectsCount = "Active on stage";

    private void Update()
    {
        UpdateText();
    }

    protected abstract void UpdateText();
}
