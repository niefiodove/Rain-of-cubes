using UnityEngine;

public class Repainter : MonoBehaviour
{
    [SerializeField] private Color _startColor;

    public void RepaintToStartColor(Renderer renderer)
    {
        renderer.material.color = _startColor;
    }

    public void Repaint(Collider collider)
    {
        if(collider.TryGetComponent<Renderer>(out Renderer renderer))
            renderer.material.color = Color.black;
    }
}
