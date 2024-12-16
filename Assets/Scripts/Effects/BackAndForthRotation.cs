using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackAndForthRotation : MonoBehaviour {

    [SerializeField] private float rotationSpeed = 50f;
    [SerializeField] private float minAngle = 5f;
    [SerializeField] private float maxAngle = 5f;

    private bool isRotatingForward = true;

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {
        float currentZ = transform.localEulerAngles.z;


        if (currentZ > 180) {
            currentZ -= 360;
        }

        if (isRotatingForward) {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);

            if (currentZ >= maxAngle) {
                isRotatingForward = false;
            }
        } else {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);

            if (currentZ <= minAngle) {
                isRotatingForward = true;
            }
        }
    }
}
