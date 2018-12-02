using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DisplayManager : MonoBehaviour {

    public Text displayText;
    [HideInInspector] public GameObject interactText;
    public GameObject displayTextPanel;


    public float displayTime;
    public float fadeTime;

    private IEnumerator fadeAlpha;

    [HideInInspector] public static DisplayManager displayManagerInstance;

    private void Awake()
    {

        interactText = GameObject.Find(Names.interactText);
        interactText.SetActive(false);
        if (displayManagerInstance == null)
            displayManagerInstance = this;
        else Debug.LogError("Tried to create a second DisplayManager");
    }


    /*public static DisplayManager Instance() {
        if (!displayManagerInstance) {
            displayManagerInstance = FindObjectOfType(typeof(DisplayManager)) as DisplayManager;
            if (!displayManagerInstance)
                Debug.LogError("There needs to be one active DisplayManager script on a GameObject in your scene.");
        }

        return displayManagerInstance;
    }*/

    public void DisplayMessage(string message, float time) {
        displayTextPanel.SetActive(true);
        displayTime = time;
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
        Color resetTextColor = displayText.color;
        Color resetPanelColor = displayTextPanel.GetComponent<Image>().color;
        resetTextColor.a = 1;
        resetPanelColor.a = 0.67f;
        displayText.color = resetTextColor;
        displayTextPanel.GetComponent<Image>().color = resetPanelColor;

        yield return new WaitForSeconds(displayTime);

        while (displayText.color.a > 0) {
            Color textColor = displayText.color;
            Color panelColor = displayTextPanel.GetComponent<Image>().color;
            textColor.a -= Time.deltaTime / fadeTime;
            panelColor.a -= Time.deltaTime / fadeTime;

            displayText.color = textColor;
            displayTextPanel.GetComponent<Image>().color = panelColor;
            yield return null;
        }
        displayTextPanel.SetActive(false);
        yield return null;
    }

 
}
