using UnityEngine;

public class Pickup : MonoBehaviour
{
    [Tooltip("Set this to true if the item is a fuel pod")]
    public bool _isFuel;

    public bool _pickedUp;

    [Tooltip("Set this to true if the item is a fuel pod and it's been dropped on the rocket")]
    public bool _falling; // For fuel items only
}
