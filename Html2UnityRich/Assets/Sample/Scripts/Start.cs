using System.Collections.Generic;
using Html2UnityRich;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Start : MonoBehaviour
{
    public LinkText4UGUI preTextUGUI;
    public TextMeshProUGUI textMeshPro;
    public Button btnShowUGUIText;
    public Button btnShowTextMeshPro;

    private void Awake ()
    {
        string content = Resources.Load<TextAsset> ("Text").text;
        var rootNode = Html2UnityRichMgr.CreateHtmlRootNode (content).ToPropNode ().ToUnityRichNode ();

        #region 由于TextPro组件支持对齐标签，ToTextProRichText直接赋值即可
        textMeshPro.alignment = TextAlignmentOptions.MidlineLeft;
        textMeshPro.text = rootNode.ToTextProRichText ();
        #endregion

        #region 由于UGUI不支持对齐标签，拆分为多个Text显示，每个Text单独设置对齐
        List<HtmlNode> htmlTagNodes = rootNode.GetChilds ();
        for ( int i = 0 ; i < htmlTagNodes.Count ; i++ )
        {
            LinkText4UGUI textItem = GameObject.Instantiate<LinkText4UGUI> (preTextUGUI , preTextUGUI.transform.parent);
            textItem.text = htmlTagNodes [i].ToUguiRichText ();
            textItem.gameObject.SetActive (true);
           
            if ( htmlTagNodes [i].GetProp ().ContainsKey (HtmlTagName.HTML_CLASS_ALIGN_LEFT) )
            {
                textItem.alignment = TextAnchor.MiddleLeft;
            }
            else if ( htmlTagNodes [i].GetProp ().ContainsKey (HtmlTagName.HTML_CLASS_ALIGN_RIGHT) )
            {
                textItem.alignment = TextAnchor.MiddleRight;
            }
            else if ( htmlTagNodes [i].GetProp ().ContainsKey (HtmlTagName.HTML_CLASS_ALIGN_CENTER) )
            {
                textItem.alignment = TextAnchor.MiddleCenter;
            }
        }
        #endregion

        preTextUGUI.transform.parent.gameObject.SetActive (false);
        textMeshPro.gameObject.SetActive (true);

        btnShowUGUIText.onClick.AddListener (() =>
        {
            preTextUGUI.transform.parent.gameObject.SetActive (true);
            textMeshPro.gameObject.SetActive (false);
        });

        btnShowTextMeshPro.onClick.AddListener (() =>
        {
            preTextUGUI.transform.parent.gameObject.SetActive (false);
            textMeshPro.gameObject.SetActive (true);
        });

        Debug.Log ($"根节点：{rootNode}");
    }
}