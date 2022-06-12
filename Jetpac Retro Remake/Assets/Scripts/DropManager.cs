using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : MonoBehaviour
{
    private static DropManager _instance;

    private static readonly float LEFT_MOST_EDGE = -112f;
    private static readonly float DROP_WIDTH = 224f;

    private int fuelCount = 0;

    public float _dropRateSecs = 4f;

    public bool _isRunning = true;

    private List<GameObject> _droppedItems = new List<GameObject>();

    public GameObject[] pickups;
    public GameObject fuel;

    public static DropManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<DropManager>();
            }

            return _instance;
        }
    }


    // Rules:
    //      Fuel is only active if the rocket is complete
    //      There can only be x number of gems / pickups dropped
    //      If fuel is required and there is one slot left, it has to be fuel that drops


    IEnumerator Start()
    {
        List<GameObject> prefabs = new List<GameObject>();
        List<GameObject> prefabsNoFuel = new List<GameObject>();
        prefabsNoFuel.AddRange(pickups);
        prefabs.AddRange(pickups);
        prefabs.Add(fuel);

        while (true)
        {
            yield return new WaitForSeconds(_dropRateSecs);
            if (_isRunning && _droppedItems.Count < 4)
            {
                // Do the drop thing
                // TODO: Fuel will always drop. Stop it by checking on the state of the rocket
                // i.e. Fuel can only drop if the rocket is in one piece
                var prefabList = RocketManager.Instance._shipIsWhole ? prefabs : prefabsNoFuel;
                GameObject prefab = DropFuel() ? fuel : prefabList[Random.Range(0, prefabList.Count)];
                GameObject copy = Instantiate(prefab);
                float x = LEFT_MOST_EDGE + (DROP_WIDTH * Random.Range(0f, 1f));
                copy.transform.position = new Vector3(x, gameObject.transform.position.y);
                _droppedItems.Add(copy);
            }
        }
    }

    public void DropFuel(GameObject fuelPod)
    {
        StartCoroutine(DropFuelPod(fuelPod));
    }

    public void PickupObject(GameObject pickup)
    {
        _droppedItems.Remove(pickup);

        var p = pickup.GetComponent<Pickup>();
        if (!p._isFuel)
        {
            Destroy(pickup);
        }
    }

    private bool DropFuel()
    {
        bool fuelInPlay = false;
        for (int i = 0; i < _droppedItems.Count; i++)
        {
            if (_droppedItems[i].tag == "Fuel")
            {
                fuelInPlay = true;
                break;
            }
        }

        return RocketManager.Instance._shipIsWhole && !fuelInPlay && _droppedItems.Count == 3;
    }

    private IEnumerator DropFuelPod(GameObject fuelPod)
    {
        float endY = -88f + (fuelCount * 16f);
        Vector3 start = fuelPod.transform.position;
        Vector3 target = new Vector3(44, endY);

        float time = 0f;
        while (time < 1f)
        {
            fuelPod.transform.position = Vector3.Lerp(start, target, time);
            time += Time.deltaTime / 2f;
            yield return null;
        }

        fuelPod.transform.position = target;
        fuelCount++;
    }
}
