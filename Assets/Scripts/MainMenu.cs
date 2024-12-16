using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    private bool showSettings;

    public void GoToScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ToggleSettings() {
        showSettings = !showSettings;
    }

    void Start() {
        showSettings = false;
    }
}
