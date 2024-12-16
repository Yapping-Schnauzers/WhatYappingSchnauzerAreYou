using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Answer", menuName = "ScriptableObjects/Answer")]
public class Answer : ScriptableObject {
    // The text of the answer.
    public string answer;
    // The points and their associated schnauzer they apply to.
    public PersonalityPoint[] points;
}
