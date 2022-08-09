using System;
using UnityEngine;

public class RocketEntryZone : MonoBehaviour
{
    public event EventHandler<GameObject> PlayerEntered;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerEntered?.Invoke(this, collision.gameObject);    
        }
    }
}
