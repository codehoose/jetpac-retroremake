using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RocketManager : SingletonMonoBehaviour<RocketManager>
{
    private static readonly int MAX_FUEL_PODS = 6;
    private static readonly float ROCKET_CEILING = 136;
    private static readonly float ROCKET_TAKE_OFF_SPEED = 48f;

    private List<RocketPart> _rocketParts;
    List<RocketPartLandData> _rocketPartAdditional;
    public RocketAsset[] rockets;
    public GameObject rocketPartPrefab;
    public GameObject rocketEntryZone;

    public bool _rocketInitialSplit = true;
    public int _fuelLevel;
    
    public RocketState State { get; set; }

    private Vector3[] _startPositions = new Vector3[]
    {
        new Vector3(-72f, 32f),
        new Vector3(8,8),
        new Vector3(44,-80)
    };

    private Vector3[] _startStationaryPositions = new Vector3[]
    {
        new Vector3(44, 136),
        new Vector3(44, 120),
        new Vector3(44,104)
    };

    private Vector3[] _endStationaryPositions = new Vector3[]
    {
        new Vector3(44, -48),
        new Vector3(44, -64),
        new Vector3(44,-80)
    };

    IEnumerator Start()
    {
        rocketEntryZone.GetComponent<RocketEntryZone>().PlayerEntered += RocketManager_PlayerEntered;

        GameState gameState = GameManager.Instance.GameState;
        _rocketInitialSplit = (gameState.wave % 4) == 0;

        InitRocketPositions(gameState.wave / 4, _rocketInitialSplit);
        var lastFuelLevel = _fuelLevel;
        bool flash = false;
        while (true)
        {
            switch (State)
            {
                case RocketState.InPieces:
                    break;
                case RocketState.Fuelling:
                    lastFuelLevel = DoFuel(lastFuelLevel);
                    break;
                case RocketState.ReadyForTakeOff:
                    flash = !flash;
                    var level = flash ? 2 : 0;
                    foreach (var part in _rocketParts) part._fuelCellCount = level;
                    yield return new WaitForSeconds(0.5f);
                    break;
                case RocketState.Landing:

                    bool isDone = true;

                    foreach (var part in _rocketPartAdditional)
                    {
                        part.part.transform.position -= new Vector3(0, ROCKET_TAKE_OFF_SPEED, 0) * Time.deltaTime;
                        if (part.part.transform.position.y <= part.end.y)
                        {
                            part.part.transform.position = part.end;
                            part.done = true;
                        }
                    }

                    foreach (var part in _rocketPartAdditional)
                    {
                        isDone &= part.done;
                    }

                    if (isDone)
                    {
                        State = RocketState.Fuelling;
                    }

                    break;
                case RocketState.TakeOff:
                    foreach (var part in _rocketParts)
                    {
                        part.transform.position += new Vector3(0, ROCKET_TAKE_OFF_SPEED, 0) * Time.deltaTime;
                        if (part.transform.position.y >= ROCKET_CEILING)
                        {
                            int wave = gameState.wave + 1;
                            LevelLoader.LoadLevel(wave + 1, ScoreManager.Instance.Score, ScoreManager.Instance.Score, 0);
                        }
                    }
                    break;
            }

            yield return null;
        }
    }

    private int DoFuel(int lastFuelLevel)
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

                if (_fuelLevel == MAX_FUEL_PODS)
                {
                    State = RocketState.ReadyForTakeOff;
                    rocketEntryZone.SetActive(true);
                }
            }
        }

        return lastFuelLevel;
    }

    private void RocketManager_PlayerEntered(object sender, GameObject e)
    {
        Destroy(e); // Kill the player
        State = RocketState.TakeOff;
    }

    public void InitRocketPositions(int rocketId, bool splitRocket = true)
    {
        Vector3[] positions = splitRocket ? _startPositions : _startStationaryPositions;
        ShowRocketParts(rockets[rocketId], positions, splitRocket);
        State = splitRocket ? RocketState.InPieces : RocketState.Landing; // Was Fuelling
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
        State = rocketPartId == 0 ? RocketState.Fuelling : State;
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

        _rocketPartAdditional = new List<RocketPartLandData>();
        _rocketPartAdditional.Add(new RocketPartLandData { end = _endStationaryPositions[2], start = _startPositions[2], part = _rocketParts[0] });
        _rocketPartAdditional.Add(new RocketPartLandData { end = _endStationaryPositions[1], start = _startPositions[1], part = _rocketParts[1] });
        _rocketPartAdditional.Add(new RocketPartLandData { end = _endStationaryPositions[0], start = _startPositions[0], part = _rocketParts[2] });
    }
}
