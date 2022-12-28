using System.Collections;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private GameObject _currentPlayer;

    private GameState _gameState;

    public GameObject player;

    public GameObject explosion;

    public void PlayerDied()
    {
        StartCoroutine(KillPlayer());
    }

    public GameState GameState
    {
        get
        {
            if (_gameState.type == GameStateType.Invalid)
            {
                _gameState = LevelLoader.GetGameState();
            }

            return _gameState;
        }
    }
    
    private void Start()
    {
        _gameState = new GameState() { type = GameStateType.Invalid };
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
