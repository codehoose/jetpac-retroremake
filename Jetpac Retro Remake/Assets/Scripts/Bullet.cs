using System.Collections;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static float COOLDOWN_PERIOD = 0.25f;
    public static float BULLET_DEATH_AFTER = 0.1f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(BULLET_DEATH_AFTER);
        Destroy(gameObject);
    }
}
