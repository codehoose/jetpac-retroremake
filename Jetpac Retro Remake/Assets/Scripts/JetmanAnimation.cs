using System.Collections;
using UnityEngine;

public class JetmanAnimation : MonoBehaviour
{
    private SpriteRenderer _renderer;
    public Sprite[] _sprites;
    public int _idleFrame;
    public bool _isAnimating;
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
                _renderer.sprite = _sprites[currentFrame];
                currentFrame++;
                currentFrame %= _sprites.Length;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    public void Idle()
    {
        _renderer.sprite = _sprites[_idleFrame];
        _isAnimating = false;
    }
}
