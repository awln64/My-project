using TMPro;
using UnityEngine;

public class AnswerSign : MonoBehaviour {
    public TextMeshProUGUI textElement;

    private void Awake() {
        textElement = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetText(string value) {
        textElement.text = value;
    }
}
