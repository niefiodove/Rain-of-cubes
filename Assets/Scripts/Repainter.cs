using UnityEngine;

public class Repainter : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void RepaintToStartColor()
    {
        _renderer.material.color = _startColor;
    }

    public void Repaint()
    {
        _renderer.material.color = Color.black;
    }
}
