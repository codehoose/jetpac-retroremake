using System;
using System.Runtime.CompilerServices;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private int _score;
    private int _hiScore;

    public event EventHandler<string> PropertyChanged;

    public int Score
    {
        get { return _score; }
        set
        {
            _score = value;
            OnPropertyChanged();
        }
    }

    public int HiScore
    {
        get { return _hiScore; }
        set
        {
            _hiScore = value;
            OnPropertyChanged();
        }
    }

    public void PickUpFuel()
    {
        Score += 100;
    }

    public void PickUpGem()
    {
        Score += 250;
    }

    public void KillAlien()
    {
        Score += 25;
    }

    protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
    {
        PropertyChanged?.Invoke(this, propertyName);

        if (propertyName == "Score")
        {
            if (Score > _hiScore)
            {
                HiScore = Score;
            }
        }
    }

    public void Start()
    {
        Score = GameManager.Instance.GameState.score;
        HiScore = GameManager.Instance.GameState.hiscore;
    }
}
