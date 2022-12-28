using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject regularMenu;
    public GameObject debugMenu;
    public Button[] levelButtons;
    public Sprite[] waveGraphic;
    public GameObject debugMenuButton;
    public Transform panel;

    void Start()
    {
        for(int index = 0;index < 16; index++)
        {
            GameObject menuButton = Instantiate(debugMenuButton, panel);
            Button button = menuButton.GetComponent<Button>();
            TMPro.TextMeshProUGUI txt = button.GetComponentInChildren<TMPro.TextMeshProUGUI>();
            Image img = menuButton.GetComponentInChildren<Image>();
            txt.text = $"Level {index + 1}";
            img.sprite = waveGraphic[index % 8]; // Only 8 different aliens, but 16 screens
            int levelIndex = index;
            button.onClick.AddListener(() =>
            {
                LevelLoader.LoadLevel(levelIndex, 0, 0, LevelLoader.DefaultLives);
            });
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            debugMenu.SetActive(!debugMenu.activeInHierarchy);
            regularMenu.SetActive(!debugMenu.activeInHierarchy);
        }
    }
}
