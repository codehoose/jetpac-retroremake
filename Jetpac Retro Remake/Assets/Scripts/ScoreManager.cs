using System;
using System.Runtime.CompilerServices;

public class ScoreManager : SingletonMonoBehaviour<ScoreManager>
{
    private int _score;

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
    }
}
