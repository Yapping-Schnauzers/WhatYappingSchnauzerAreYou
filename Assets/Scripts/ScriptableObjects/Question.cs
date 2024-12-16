using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Question", menuName = "ScriptableObjects/Question")]
public class Question : ScriptableObject {
    public string question;
    public Answer[] answers;
}
