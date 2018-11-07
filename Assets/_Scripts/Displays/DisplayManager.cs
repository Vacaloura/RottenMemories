using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class DisplayManager : MonoBehaviour {

    public Text displayText;
    public GameObject interactText;

    public float displayTime;
    public float fadeTime;

    private IEnumerator fadeAlpha;

    private void Awake()
    {

        interactText = GameObject.Find(Names.interactText);
        interactText.SetActive(false);

    }

    private void Update() {
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
        ChangeScene();
    }

    private static DisplayManager displayManager;

    public static DisplayManager Instance() {
        if (!displayManager) {
            displayManager = FindObjectOfType(typeof(DisplayManager)) as DisplayManager;
            if (!displayManager)
                Debug.LogError("There needs to be one active DisplayManager script on a GameObject in your scene.");
        }

        return displayManager;
    }

    public void DisplayMessage(string message) {
        displayText.text = message;
        SetAlpha();
    }

    void SetAlpha() {
        if (fadeAlpha != null) {
            StopCoroutine(fadeAlpha);
        }
        fadeAlpha = FadeAlpha();
        StartCoroutine(fadeAlpha);
    }

    IEnumerator FadeAlpha() {
        Color resetColor = displayText.color;
        resetColor.a = 1;
        displayText.color = resetColor;

        yield return new WaitForSeconds(displayTime);

        while (displayText.color.a > 0) {
            Color displayColor = displayText.color;
            displayColor.a -= Time.deltaTime / fadeTime;
            displayText.color = displayColor;
            yield return null;
        }
        yield return null;
    }

    void ChangeScene() {
        if (Input.GetKeyDown(KeyCode.RightArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        if (Input.GetKeyDown(KeyCode.LeftArrow)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
