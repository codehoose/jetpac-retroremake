using UnityEngine;

public class HiScoreListener : MonoBehaviour
{
    public TMPro.TMP_Text _scoreIndicator;

    void Start()
    {
        ScoreManager.Instance.PropertyChanged += (o, propertyName) =>
        {
            if (propertyName == "HiScore")
            {
                if (ScoreManager.Instance.Score >= ScoreManager.Instance.HiScore)
                {
                    int hiScore = ScoreManager.Instance.HiScore;
                    _scoreIndicator.text = $"{hiScore:000000}";
                }
            }
        };
    }
}
