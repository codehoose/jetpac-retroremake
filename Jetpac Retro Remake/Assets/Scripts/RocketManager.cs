using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketManager : MonoBehaviour
{
    private static RocketManager _instance;

    public static RocketManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<RocketManager>();
            }

            return _instance;
        }
    }

    private List<RocketPart> _rocketParts;
    public RocketAsset[] rockets;
    public GameObject rocketPartPrefab;

    public int _fuelLevel;

    public bool _shipIsWhole;

    private Vector3[] _startPositions = new Vector3[]
    {
        new Vector3(-72f, 32f),
        new Vector3(8,8),
        new Vector3(44,-80)
    };

    private Vector3[] _startStationaryPositions = new Vector3[]
    {
        new Vector3(44, -48),
        new Vector3(44, -64),
        new Vector3(44,-80)
    };

    IEnumerator Start()
    {
        InitRocketPositions(0);
        var lastFuelLevel = _fuelLevel;
        while (true)
        {
            if (lastFuelLevel != _fuelLevel)
            {
                lastFuelLevel = _fuelLevel;
                foreach (var part in _rocketParts) part._fuelCellCount = 0;

                if (_fuelLevel != 0)
                {
                    /*
                     * 0        - Empty
                     * 1        - R11   0  0
                     * 2        - R12   0  1
                     * 3        - R21   1  2
                     * 4        - R22   1  3
                     * 5        - R31   2  4
                     * 6        - R32   2  5
                     */

                    var rocketId = (_fuelLevel - 1) / 2; // _fuelLevel / 3;      // 1, 2 or 3
                    var level = (_fuelLevel % 2);   // Needs to be 1 or 2
                    if (level == 0) level = 2;

                    for (int r = 0; r <= rocketId; r++)
                    {
                        _rocketParts[r]._fuelCellCount = 2;
                    }

                    _rocketParts[rocketId]._fuelCellCount = level;
                }
            }

            yield return null;
        }
    }

    public void InitRocketPositions(int rocketId, bool splitRocket = true)
    {
        Vector3[] positions = splitRocket ? _startPositions : _startStationaryPositions;
        ShowRocketParts(rockets[rocketId], positions, splitRocket);
        _shipIsWhole = !splitRocket;
    }

    public void DropPart(GameObject rocketPart, int rocketPartId)
    {
        StartCoroutine(DropThePart(rocketPart, rocketPartId));
    }

    private IEnumerator DropThePart(GameObject rocketPart, int rocketPartId)
    {
        float endY = rocketPartId == 1 ? -64f : -48f;
        Vector3 start = rocketPart.transform.position;
        Vector3 target = new Vector3(44, endY);

        float time = 0f;
        while (time < 1f)
        {
            rocketPart.transform.position = Vector3.Lerp(start, target, time);
            time += Time.deltaTime / 2f;
            yield return null;
        }

        rocketPart.transform.position = target;
        _shipIsWhole = rocketPartId == 0;
    }

    public void ShowRocketParts(RocketAsset rocket, Vector3[] positions, bool splitRocket)
    {
        _rocketParts = new List<RocketPart>();

        var parts = new Sprite[] { rocket.top, rocket.middle, rocket.bottom };
        for (int i = 0; i < parts.Length; i++)
        {
            var go = Instantiate(rocketPartPrefab, positions[i], Quaternion.identity);
            var part = go.GetComponent<RocketPart>();
            part.Init(parts[i], i);
            go.GetComponent<BoxCollider2D>().enabled = splitRocket;
            _rocketParts.Insert(0, part);
        }
    }
}
