using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    IEnumerator Start()
    {
        var animator = GetComponent<Animator>();
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1)
        {
            yield return null;
        }

        Destroy(gameObject);
    }
}
