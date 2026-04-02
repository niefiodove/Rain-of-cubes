using UnityEngine;

public class Repainter : MonoBehaviour
{
    [SerializeField] private TriggerCollector _triggerCollector;
    [SerializeField] private Color _startColor;

    public void RepaintToStartColor(Renderer renderer)
    {
        renderer.material.color = _startColor;
    }

    private void Start()
    {
        foreach (TrigerControler trigerControler in _triggerCollector.TrigerControlers)
            trigerControler.TriggerActivated += Repaint;
    }

    private void OnDisable()
    {
        foreach (TrigerControler trigerControler in _triggerCollector.TrigerControlers)
            trigerControler.TriggerActivated -= Repaint;
    }

    private void Repaint(Collider collider)
    {
        if(collider.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = Color.black;
    }
}
