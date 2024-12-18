using UnityEngine;

public class Rotate : MonoBehaviour {
    [SerializeField] private float rotationSpeed = 300f;

    void Update() {
        transform.Rotate(0, 0, -(rotationSpeed * Time.deltaTime));
    }
}
