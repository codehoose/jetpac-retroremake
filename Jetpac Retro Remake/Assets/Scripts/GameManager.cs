using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameObject _currentPlayer;

    public GameObject player;

    public GameObject explosion;

    public void PlayerDied()
    {
        StartCoroutine(KillPlayer());
    }
    
    private void Start()
    {
        _currentPlayer = Instantiate(player);
    }

    private IEnumerator KillPlayer()
    {
        if (_currentPlayer != null)
        {
            var copy = Instantiate(explosion);
            copy.transform.position = _currentPlayer.transform.position;

            Destroy(_currentPlayer);
        }
        _currentPlayer = null;

        yield return new WaitForSeconds(1f);

        _currentPlayer = Instantiate(player);
    }
}
