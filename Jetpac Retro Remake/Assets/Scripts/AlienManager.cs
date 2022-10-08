using System.Collections;
using UnityEngine;

public class AlienManager : MonoBehaviour
{
    public Color[] spectrumColours;
    public GameObject[] waveShapes;
    public int wave = 0;

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);

            GameObject copy = Instantiate(waveShapes[wave]);

            bool isRight = Random.Range(0f, 1f) > .5f;
            float x = isRight ? 180 : -180;
            float y = -88 + Random.Range(0f, 176);

            copy.GetComponent<AlienInit>().flipAlien = isRight;
            copy.GetComponent<AlienInit>().color = spectrumColours[Random.Range(0, spectrumColours.Length)];
            copy.transform.position = new Vector3(x, y, 0);

            switch (wave)
            {
                case 0:
                    SetupFireball(copy, isRight);
                    break;
            }
        }
    }

    private void SetupFireball(GameObject copy, bool isRight)
    {
        FireballAlien alien = copy.GetComponent<FireballAlien>();

        if (isRight)
        {
            alien.xDirection = isRight ? -1 : 1;
            alien.yGradient = -0.25f + Random.Range(0f, 0.5f);
            alien.speed = 16 + Random.Range(0, 4f);
        }
    }
}
