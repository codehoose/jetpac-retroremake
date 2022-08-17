using UnityEngine;

public class ScoreListener : MonoBehaviour
{
    public TMPro.TMP_Text _scoreIndicator;

    void Start()
    {
        ScoreManager.Instance.PropertyChanged += (o, propertyName) =>
          {
              if (propertyName == "Score")
              {
                  _scoreIndicator.text = $"{ScoreManager.Instance.Score:000000}";
              }
          };
    }
}
