using System.Collections;
using UnityEngine;

public class AlienInit : MonoBehaviour
{
    public bool flipAlien = false;

    public Color color
    {
        get { return GetComponent<SpriteRenderer>().color; }
        set
        {
            GetComponent<SpriteRenderer>().color = value;
        }
    }
  
    void Start()
    {
        GetComponent<SpriteRenderer>().flipX = flipAlien;
    }
}
