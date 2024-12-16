using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PersonalityPoint", menuName = "ScriptableObjects/PersonalityPoint")]
public class PersonalityPoint : ScriptableObject {
    // Enum for schnauzer this point will apply to.
    public YappingSchnauzer schnauzer;
    // The point value this schnauzer will receive. Can be positive or negative.
    public int point;
}
