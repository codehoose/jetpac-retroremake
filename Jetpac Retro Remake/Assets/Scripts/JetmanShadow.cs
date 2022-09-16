using System.Collections;
using UnityEngine;

public class JetmanShadow : MonoBehaviour
{
    private Locomotion _player;
    private Vector3 _offset;
    public bool isRunning;

    public void SetPlayer(Locomotion player, Vector3 offset)
    {
        _player = player;
        _offset = offset;
        transform.localPosition = _offset;

        if (_player == null)
        {
            GetComponent<SpriteRenderer>().sprite = null;
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            if (_player != null)
            {
                Sprite sprite = _player.jetmanShape._sprites[_player.jetmanShape.currentFrame];
                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                renderer.sprite = sprite;
                renderer.flipX = _player.jetmanShape.FlipHorizontal;
                
            }

            yield return null;
        }
    }



    //public void SetShape(Sprite sprite, Vector3 pos)
    //{
    //    if (sprite == null)
    //    {
    //        gameObject.SetActive(false);
    //        return;
    //    }

    //    transform.position = pos;
    //    GetComponent<SpriteRenderer>().sprite = sprite;
    //}
}
