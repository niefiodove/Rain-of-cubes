using System.Collections;
using UnityEngine;

public class Repainter : MonoBehaviour
{
    [SerializeField] private Color _startColor;
    private Renderer _renderer;
    private Bomb _bomb;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        TryGetComponent<Bomb>(out _bomb);

        _renderer.material.SetFloat("_Mode", 3);
        _renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        _renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        _renderer.material.SetInt("_ZWrite", 0);
        _renderer.material.DisableKeyword("_ALPHATEST_ON");
        _renderer.material.EnableKeyword("_ALPHABLEND_ON");
        _renderer.material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        _renderer.material.renderQueue = 3000;
    }

    public void RepaintToStartColor()
    {
        _renderer.material.color = _startColor;
    }
    public void RepaintToStartAlpha()
    {
        Color newColor = _renderer.material.color;
        newColor.a = 1f;
        _renderer.material.color = newColor;
    }

    public void Repaint()
    {
        _renderer.material.color = Color.black;
    }

    public void FadeOut()
    {
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        if (_bomb == null)
        {
            yield break;
        }

        Color color = _renderer.material.color;
        float lifetimeBomb = _bomb.LifetimeBomb;
        float elapsed = 0f;

        while (elapsed < lifetimeBomb)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / lifetimeBomb);
            _renderer.material.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        _renderer.material.color = new Color(color.r, color.g, color.b, 0f);
    }
}
