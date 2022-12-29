using System.Collections;
using UnityEngine;

public class AlienManager : MonoBehaviour
{
    public Color[] spectrumColours;
    public GameObject[] waveShapes;
    public int wave = 0;

    IEnumerator Start()
    {
        wave = GameManager.Instance.GameState.wave % 8; // There are only 8 alien types

        while (true)
        {
            yield return new WaitForSeconds(3f);

            GameObject copy = Instantiate(waveShapes[wave]);
            copy.GetComponent<AlienInit>().color = spectrumColours[Random.Range(0, spectrumColours.Length)];

            switch (wave)
            {
                case 0: // Fireballs
                    FireballAlien fireball = copy.GetComponent<FireballAlien>();
                    fireball.Init();
                    break;
                case 2: // Balls
                case 4: // Ufos
                    FollowAlien follow = copy.GetComponent<FollowAlien>();
                    follow.Init();
                    break;
                case 3: // Planes
                    PlaneBehaviour plane = copy.GetComponent<PlaneBehaviour>();
                    plane.Init();
                    break;
                case 1: // Fuzzies
                case 5: // Crosses
                case 7: // Frogs
                    BouncingAlien bouncing = copy.GetComponent<BouncingAlien>();
                    bouncing.Init();
                    break;
            }
        }
    }
}
