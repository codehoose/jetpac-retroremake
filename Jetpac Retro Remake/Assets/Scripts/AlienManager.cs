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
                case 0:
                    FireballAlien alien = copy.GetComponent<FireballAlien>();
                    alien.Init();
                    break;
            }
        }
    }
}
