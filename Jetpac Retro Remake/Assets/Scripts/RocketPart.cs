using System.Collections;
using UnityEngine;

public class RocketPart : MonoBehaviour
{
    public int partId;

    public SpriteRenderer whiteRocket;
    public SpriteRenderer fueledRocket;

    public int _fuelCellCount = 0;

    public void Init(Sprite sprite, int partId)
    {
        this.partId = partId;
        whiteRocket.sprite = sprite;
        fueledRocket.sprite = sprite;
    }

    IEnumerator Start()
    {
        var lastCellCount = _fuelCellCount;

        while (true)
        {
            if (lastCellCount != _fuelCellCount)
            {
                lastCellCount = _fuelCellCount;
                fueledRocket.enabled = _fuelCellCount != 0;
                fueledRocket.maskInteraction = _fuelCellCount == 1 ? SpriteMaskInteraction.VisibleInsideMask
                                                                   : SpriteMaskInteraction.None;
            }

            yield return null;
        }
    }
}
