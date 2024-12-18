using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    // Basic singleton setup, this should be the only GM
    public static GameManager Instance { get; private set; }

    public event EventHandler OnQuizComplete;


    [SerializeField] private Question[] questionsSetInInspector;

    public List<Question> questions { get; private set; }

    public int questionNumber { get; private set; }

    public bool quizInProgress { get; private set; }

    private void GameManager_OnAnswerSelected(object sender, EventArgs e) {
        questionNumber++;
        Debug.Log("We are on question number: " + questionNumber);
    }


    // Start is called before the first frame update
    void Awake() {
        if (Instance != null) {
            Debug.LogError("Instance of GameManager already exists");
            return;
        }

        if (questionsSetInInspector.Length == 0) {
            Debug.LogError("Please populate questions field.");
        }

        Instance = this;
        quizInProgress = true;
        questionNumber = 0;
        questions = new List<Question>(questionsSetInInspector);

        QuizUIManager.Instance.OnAnswerSelected += GameManager_OnAnswerSelected;
    }

    // Update is called once per frame
    void Update() {
        // Check to see if quiz is over. If it is, set up appropriate flag.
        if (questionNumber >= questions.Count && quizInProgress) {
            quizInProgress = false;
            OnQuizComplete?.Invoke(this, EventArgs.Empty);
            Debug.Log("Done!");
            foreach (var score in Player.Instance.personalityScore) {
                Debug.Log($"Schnauzer: {score.Key}, Score {score.Value}");
            }

            ResultManager.Instance.ShowResults(Player.Instance.GetPersonality());
        }
    }
}
