using UnityEngine;
using UnityEngine.SceneManagement;

public class TapToPlay : MonoBehaviour
{
    public string sceneName = "Game";

    void Start()
    {
        var button = GetComponent<UnityEngine.UI.Button>();
        button.onClick.AddListener(new UnityEngine.Events.UnityAction(() =>
        {
            // TODO: Set up player prefs to reset score and
            // to load the correct leve
            SceneManager.LoadScene(sceneName);
        }));
    }
}
