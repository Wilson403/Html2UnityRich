using Html4UnityText;
using UnityEngine;

public class Start : MonoBehaviour
{
    private void Awake ()
    {
        Debug.LogWarning (Html4UnityTextMgr.CreateHtmlRootNode ("<p>abc<h1>d<h2>e</h1></h2>fg</p>"));
    }
}