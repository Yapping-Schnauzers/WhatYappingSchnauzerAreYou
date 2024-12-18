using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ResultManager : MonoBehaviour {
    // Basic singleton setup, this should be the only RM
    public static ResultManager Instance { get; private set; }

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject resultScreen;

    [SerializeField] private Sprite[] resultSprites;
    [SerializeField] private Image resultImage;

    [SerializeField] private ParticleSystem confetti;

    [SerializeField] private float loadingDuration = 3f;

    private int finalResultIndex;

    // Start is called before the first frame update
    void Awake() {
        if (Instance != null) {
            Debug.LogError("Instance of GameManager already exists");
            return;
        }

        // For when updating the result images, need to make sure to populate this field again
        if (resultSprites.Length == 0) {
            Debug.LogError("Please add results images to ResultManager.");
            return;
        }

        Instance = this;

        // Ensure these are not active during gameplay.
        loadingScreen.SetActive(false);
        resultScreen.SetActive(false);
    }

    public void ShowResults(YappingSchnauzer schnauzer) {
        switch(schnauzer) {
            case YappingSchnauzer.Bella:
                finalResultIndex = 0;
                break;
            case YappingSchnauzer.Selena:
                finalResultIndex = 1;
                break;
            case YappingSchnauzer.Lyla:
                finalResultIndex = 2;
                break;
            default:
                Debug.LogError("Invalid Final Result.");
                // If this still plays, just default to Bella.
                finalResultIndex = 0;
                break;
            
        }

        StartCoroutine(DisplayResults());
    }

    private IEnumerator DisplayResults() {
        loadingScreen.SetActive(true);

        yield return new WaitForSeconds(loadingDuration);

        // Hide loading screen and show result screen.
        loadingScreen.SetActive(false);
        resultScreen.SetActive(true);

        resultImage.sprite = resultSprites[finalResultIndex];

        // Play celebratory confetti particle system.
        if (confetti != null) {
            confetti.Play();
        }
    }
}
