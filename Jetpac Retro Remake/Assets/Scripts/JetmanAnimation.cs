using System.Collections;
using UnityEngine;

public class JetmanAnimation : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public Sprite[] _sprites;
    public Sprite[] _inFlightSprites;
    public int _idleFrame;
    public bool _isAnimating;
    public bool _isInflight;
    public int currentFrame = 0;

    public bool FlipHorizontal
    {
        get { return _renderer.flipX; }
        set { _renderer.flipX = value; }
    }

    void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (_isAnimating)
            {
                var sprites = _isInflight ? _inFlightSprites : _sprites;
                _renderer.sprite = sprites[currentFrame];
                currentFrame++;
                currentFrame %= sprites.Length;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Idle()
    {
        if (_isInflight || _renderer == null) return;

        _renderer.sprite = _sprites[_idleFrame];
        _isAnimating = false;
    }
}
