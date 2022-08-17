using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropManager : SingletonMonoBehaviour<DropManager>
{
    private static readonly int MAX_ITEMS = 2;
    private static readonly float LEFT_MOST_EDGE = -112f;
    private static readonly float DROP_WIDTH = 224f;

    private bool _dropFuel = false;

    public float _dropRateSecs = 4f;

    public bool _isRunning = true;
    

    private List<GameObject> _droppedItems = new List<GameObject>();

    public GameObject[] pickups;
    public GameObject fuel;

    // Rules:
    //      Fuel is only active if the rocket is complete
    //      There can only be x number of gems / pickups dropped
    //      If fuel is required and there is one slot left, it has to be fuel that drops


    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(_dropRateSecs);
            if (_isRunning && _droppedItems.Count < MAX_ITEMS)
            {
                if (RocketManager.Instance.State == RocketState.Fuelling)
                {
                    GameObject prefab  = _dropFuel ? fuel : pickups[Random.Range(0, pickups.Length)];
                    GameObject copy = Instantiate(prefab);
                    float x = LEFT_MOST_EDGE + (DROP_WIDTH * Random.Range(0f, 1f));
                    copy.transform.position = new Vector3(x, gameObject.transform.position.y);
                    _droppedItems.Add(copy);
                    _dropFuel = !_dropFuel;
                }
                else if (RocketManager.Instance.State == RocketState.ReadyForTakeOff)
                {
                    GameObject prefab = pickups[Random.Range(0, pickups.Length)];
                    GameObject copy = Instantiate(prefab);
                    float x = LEFT_MOST_EDGE + (DROP_WIDTH * Random.Range(0f, 1f));
                    copy.transform.position = new Vector3(x, gameObject.transform.position.y);
                    _droppedItems.Add(copy);
                }
            }
        }
    }

    //public void DropFuel(GameObject fuelPod)
    //{
    //    if (_dropFuelCoroutine != null) return;
    //    _dropFuelCoroutine = DropFuelPod(fuelPod);
    //    StartCoroutine(_dropFuelCoroutine);
    //}

    public void PickupObject(GameObject pickup)
    {
        _droppedItems.Remove(pickup);

        var p = pickup.GetComponent<Pickup>();
        if (!p._isFuel)
        {
            Destroy(pickup);
        }
    }

    //private IEnumerator DropFuelPod(GameObject fuelPod)
    //{
    //    Vector3 start = fuelPod.transform.position;
    //    Vector3 target = new Vector3(44, -80);

    //    float time = 0f;
    //    while (time < 1f)
    //    {
    //        fuelPod.transform.position = Vector3.Lerp(start, target, time);
    //        time += Time.deltaTime / 2f;
    //        yield return null;
    //    }
    //}

    public void AddFuel(GameObject fuelPod)
    {
        _droppedItems.Remove(fuelPod);
        Destroy(fuelPod);

        RocketManager.Instance._fuelLevel++;
    }
}
