using Html4UnityText;
using UnityEngine;

public class Start : MonoBehaviour
{
    private void Awake ()
    {
        var root = Html4UnityTextMgr.CreateHtmlRootNode ("<h1 class=\"ql-align-center\">123<p><font size=\"1\"><strong class=\"ql-size-large\" style=\"color: rgb(102, 185, 102);\"> 露娜物語新服：<h5>奈拉</h5>城鎮開啟啦！</strong></font></p></h1>");
        Debug.LogWarning (root);
        Debug.LogWarning (root.ToPropNode ());
    }
}