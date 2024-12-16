using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour {
    // Basic singleton setup.
    public static Player Instance { get; private set; }

    // Contains KVPs of schnauzer and their scores, added and subtracted based on player answers.
    public Dictionary<YappingSchnauzer, int> personalityScore { get; private set; }

    // Start is called before the first frame update
    void Start() {
        if (Instance != null) {
            Debug.LogError("Somehow... there are two players instantiated.");
            return;
        }
        Instance = this;
    
        personalityScore = new Dictionary<YappingSchnauzer, int>();

        QuizUIManager.Instance.OnAnswerSelected += Player_OnAnswerSelected;
    }

    // Update is called once per frame
    void Update() {}

    private void Player_OnAnswerSelected(object sender, EventArgs e) {
        return;
    }

    // Add or update to overall Personality Score.
    public void UpdateScore(Answer answer) {
        foreach (PersonalityPoint point in answer.points) {
            YappingSchnauzer doggo = point.schnauzer;
            if (personalityScore.TryGetValue(doggo, out int currentScore)) {
                personalityScore[doggo] = currentScore + point.point;
            } else {
                personalityScore[doggo] = point.point;
            }
        }
    }

    // Get the YappingSchnauzer enum with highest score. Handles ties by selecting randomly.
    public YappingSchnauzer GetPersonality() {
        int maxValue = personalityScore.Max(pair => pair.Value);
        List<YappingSchnauzer> highestKeys = personalityScore
                                        .Where(pair => pair.Value == maxValue)
                                        .Select(pair => pair.Key)
                                        .ToList();

        // There are ties, so randomly settle it.
        if (highestKeys.Count > 1) {
            var random = new System.Random();
            return highestKeys[random.Next(highestKeys.Count)];
        }

        return highestKeys.FirstOrDefault();
    }
}
