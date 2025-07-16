using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BannerDisplay : MonoBehaviour
{
    public Image image;
    public TextMeshProUGUI titleText;

    public void ShowMeme(MemeData meme) {
        titleText.text = "Who is this?";
        image.sprite = meme.image;
    }
}
