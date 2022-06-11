using UnityEngine;

public class Pickup : MonoBehaviour
{
    private bool _pickedUp;
    private DropManager _dropManager;

    public void SetDropManager(DropManager dropManager)
    {
        _dropManager = dropManager;
    }

    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && !_pickedUp)
        {
            _pickedUp = true;
            _dropManager.PickupObject(this.gameObject);
        }
    }
}
