using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuizUIManager : MonoBehaviour {
    // Basic singleton setup, this should be the only GM
    public static QuizUIManager Instance { get; private set; }

    public event EventHandler OnAnswerSelected;

    [SerializeField] private GameObject questionObject;
    [SerializeField] private GameObject choiceObjects;

    private TextMeshProUGUI questionText;
    private List<TextMeshProUGUI> choiceTexts;

    private Question currentQuestion;
    private GameObject selectedButton;

    private bool doUpdateUI;

    public Answer selectedAnswer { get; private set; }

    // Start is called before the first frame update
    void Awake() {
        ValidateStartState();

        Instance = this;
        doUpdateUI = true;

        questionText = questionObject.GetComponentInChildren<TextMeshProUGUI>();

        choiceTexts = new List<TextMeshProUGUI>();
        foreach (Transform child in choiceObjects.transform) {
            var textObject = child.GetComponentInChildren<TextMeshProUGUI>();

            if (textObject == null) {
                Debug.LogError("Couldn't find Text object for choices collection.");
            }

            choiceTexts.Add(textObject);
        }

        OnAnswerSelected += QuizUIManager_OnAnswerSelected;
    }

    // Update is called once per frame
    void Update() {
        if (doUpdateUI && GameManager.Instance.quizInProgress) {
            UpdateQuizUI();
            doUpdateUI = false;
        }
    }

    private void QuizUIManager_OnAnswerSelected(object sender, EventArgs e) {
        selectedButton = EventSystem.current.currentSelectedGameObject;
        Answer selectedAnswer = GetAnswerFromButton(selectedButton);
        Player.Instance.UpdateScore(selectedAnswer);
        doUpdateUI = true;
    }

    // Ensure all references needed in the editor are connected.
    private void ValidateStartState() {
        if (Instance != null) {
            Debug.LogError("Instance of QuizUIManager already exists");
            return;
        }

        if (questionObject == null) {
            Debug.LogError("Please add the Question object text to QuizUIManager");
        }

        if (choiceObjects== null) {
            Debug.LogError("Please connect the Choices object collection.");
        }
    }

    // Pretty fragile, unfortunately.
    private Answer GetAnswerFromButton(GameObject button) {
        string answerSelected = button.name;
        Answer result;
    
        switch(answerSelected) {
            case "Choice1":
                result = currentQuestion.answers[0];
                break;
            case "Choice2":
                result = currentQuestion.answers[1];
                break;
            case "Choice3":
                result = currentQuestion.answers[2];
                break;
            case "Choice4":
                result = currentQuestion.answers[3];
                break;
            default:
                throw new ArgumentException("Invalid option selected.");
        }

        return result;
    }

    // Public facing method for interacting with OnAnswerSelected EventHandler.
    public void AnswerHandler() {
        OnAnswerSelected?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateQuizUI() {
        GameManager gm = GameManager.Instance;

        // If game is over, don't process questions arr anymore. Will result in out of bounds error.
        if (!gm.quizInProgress || gm.questionNumber >= gm.questions.Count) {
            return;
        }

        currentQuestion = gm.questions[gm.questionNumber];

        questionText.text = currentQuestion.question;

        int choicesIndex = 0;
        foreach (Answer answer in currentQuestion.answers) {
            choiceTexts[choicesIndex++].text = answer.answer;
        }
    }
}
