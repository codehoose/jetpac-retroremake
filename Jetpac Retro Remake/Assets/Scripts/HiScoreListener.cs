using UnityEngine;

public class HiScoreListener : MonoBehaviour
{
    public TMPro.TMP_Text _scoreIndicator;
    int _hiScore;

    void Start()
    {
        ScoreManager.Instance.PropertyChanged += (o, propertyName) =>
        {
            if (propertyName == "Score")
            {
                if (ScoreManager.Instance.Score > _hiScore)
                {
                    _hiScore = ScoreManager.Instance.Score;
                    _scoreIndicator.text = $"{_hiScore:000000}";
                }
            }
        };
    }
}
