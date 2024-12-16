using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopupDoggo : MonoBehaviour {
[SerializeField] private Image[] images;
    [SerializeField] private float minInterval = 2f;
    [SerializeField] private float maxInterval = 5f;
    [SerializeField] private Vector2 offscreenDistance = new Vector2(500, 250);
    [SerializeField] private float moveInSpeed = 5f;
    [SerializeField] private float moveOutSpeed = 2f;
    [SerializeField] private float popInDuration = 0.5f;
    [SerializeField] private float edgeMarginY = 160f;
    [SerializeField] private float edgeMarginX = 375f;

    private RectTransform[] imageTransforms;

    void Start() {
        // Get RectTransforms of all images
        imageTransforms = new RectTransform[images.Length];
        for (int i = 0; i < images.Length; i++) {
            imageTransforms[i] = images[i].GetComponent<RectTransform>();
        }

        // Start the coroutine to trigger the effect at random intervals
        StartCoroutine(TriggerPopupEffect());
    }

    private IEnumerator TriggerPopupEffect() {
        while (true) {
            float interval = Random.Range(minInterval, maxInterval);
            yield return new WaitForSeconds(interval);

            int randomImageIndex = Random.Range(0, images.Length);
            RectTransform selectedImage = imageTransforms[randomImageIndex];
            selectedImage.gameObject.SetActive(true);
            Vector2 offscreenPosition, nearEdgePosition;
            float rotation = GetRandomOffscreenPositionAndRotation(out offscreenPosition, out nearEdgePosition);

            selectedImage.anchoredPosition = offscreenPosition;
            selectedImage.rotation = Quaternion.Euler(0, 0, rotation);

            selectedImage.gameObject.SetActive(true);
            yield return StartCoroutine(EaseInAndOut(selectedImage, offscreenPosition, nearEdgePosition));
        }
    }

    private IEnumerator EaseInAndOut(RectTransform image, Vector2 offscreenPosition, Vector2 nearEdgePosition) {

        while (Vector2.Distance(image.anchoredPosition, nearEdgePosition) > 1f) {
            image.anchoredPosition = Vector2.Lerp(
                image.anchoredPosition, 
                nearEdgePosition, 
                Time.deltaTime * moveInSpeed
            );
            yield return null;
        }

        // Pause briefly at the center
        yield return new WaitForSeconds(popInDuration);

        while (Vector2.Distance(image.anchoredPosition, offscreenPosition) > 1f) {
            image.anchoredPosition = Vector2.Lerp(
                image.anchoredPosition, 
                offscreenPosition, 
                Time.deltaTime * moveOutSpeed
            );
            yield return null;
        }

        image.gameObject.SetActive(false);
    }

    private float GetRandomOffscreenPositionAndRotation(out Vector2 offscreenPosition,
                                                        out Vector2 nearEdgePosition) {
        int randomDirection = Random.Range(0, 4);
        Vector2 screenDimensions = new Vector2(Screen.width, Screen.height);
        float randomPosition;
        float rotation = 0f;

        switch (randomDirection) {
            // Offscreen Left
            case 0:
                randomPosition = Random.Range(-screenDimensions.y / 2, screenDimensions.y / 2);
                offscreenPosition = new Vector2(-offscreenDistance.x, randomPosition);
                nearEdgePosition = offscreenPosition + new Vector2(offscreenDistance.x - edgeMarginX, 0);
                rotation = -15f;
                break;
            // Offscreen Right
            case 1:
                randomPosition = Random.Range(-screenDimensions.y / 2, screenDimensions.y / 2);
                offscreenPosition = new Vector2(offscreenDistance.x, randomPosition);
                nearEdgePosition = offscreenPosition - new Vector2(offscreenDistance.x - edgeMarginX, 0);
                rotation = 15f;
                break;
            // Offscreen Up
            case 2:
                randomPosition = Random.Range(-screenDimensions.x / 2, screenDimensions.x / 2);
                offscreenPosition = new Vector2(randomPosition, offscreenDistance.y);
                nearEdgePosition = offscreenPosition - new Vector2(0, offscreenDistance.y - edgeMarginY);
                rotation = 180f;
                break;
            // Offscreen Down
            case 3:
                randomPosition = Random.Range(-screenDimensions.x / 2, screenDimensions.x / 2);
                offscreenPosition = new Vector2(randomPosition, -offscreenDistance.y);
                nearEdgePosition = offscreenPosition + new Vector2(0, offscreenDistance.y - edgeMarginY);
                rotation = 0f;
                break;
            // Incase something goes horribly wrong.
            default:
                offscreenPosition = Vector2.zero;
                nearEdgePosition = Vector2.zero;
                rotation = 0f;
                break;
        }

        return rotation;
    }
}
