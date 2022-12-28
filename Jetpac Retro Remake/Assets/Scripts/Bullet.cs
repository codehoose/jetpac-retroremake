using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float COOLDOWN_PERIOD = 0.1f;
    public static float BULLET_DEATH_AFTER = 0.15f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(BULLET_DEATH_AFTER);
        Destroy(gameObject);
    }
}
