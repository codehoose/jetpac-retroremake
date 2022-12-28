using UnityEngine;

public class FuelDropTrigger : MonoBehaviour
{
    public DropManager _dropManager;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Dropped Fuel")
        {
            _dropManager.AddFuel(collision.gameObject);
        }
    }
}
