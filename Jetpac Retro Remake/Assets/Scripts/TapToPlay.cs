using UnityEngine;

public class TapToPlay : MonoBehaviour
{
    public string sceneName = "Game";

    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            LevelLoader.StartCleanGame();
        }));
    }
}
