using Html4UnityText;
using UnityEngine;
using UnityEngine.UI;

public class Start : MonoBehaviour
{
    public Text text;

    private void Awake ()
    {
        string content = Resources.Load<TextAsset> ("Text").text;
        text.text = Html4UnityTextMgr.CreateHtmlRootNode (content).ToPropNode ().ToUnityRichNode ().ToUnityRichText ();
    }
}