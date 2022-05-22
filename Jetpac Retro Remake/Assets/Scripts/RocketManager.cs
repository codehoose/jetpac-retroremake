using System.Collections;
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

    public RocketAsset[] rockets;
    public GameObject rocketPartPrefab;

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

    void Start()
    {
        InitRocketPositions(0);
    }

    public void InitRocketPositions(int rocketId, bool splitRocket = true)
    {
        Vector3[] positions = splitRocket ? _startPositions : _startStationaryPositions;
        ShowRocketParts(rockets[rocketId], positions, splitRocket);
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
    }

    public void ShowRocketParts(RocketAsset rocket, Vector3[] positions, bool splitRocket)
    {
        var parts = new Sprite[] { rocket.top, rocket.middle, rocket.bottom };
        for (int i = 0; i < parts.Length; i++)
        {
            var go = Instantiate(rocketPartPrefab, positions[i], Quaternion.identity);
            go.GetComponent<SpriteRenderer>().sprite = parts[i];
            go.GetComponent<RocketPart>().partId = i;
            go.GetComponent<BoxCollider2D>().enabled = splitRocket;
        }
    }
}
