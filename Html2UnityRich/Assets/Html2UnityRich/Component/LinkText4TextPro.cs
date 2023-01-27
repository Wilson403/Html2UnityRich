﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LinkText4TextPro : TextMeshProUGUI, IPointerClickHandler
{
    /// <summary>
    /// 超链接信息类
    /// </summary>
    private class HyperlinkInfo
    {
        public int startIndex;

        public int endIndex;

        public string name;

        public readonly List<Rect> boxes = new List<Rect> ();
    }

    public event Action<string> evt;

    /// <summary>
    /// 解析完最终的文本
    /// </summary>
    private string m_OutputText;

    /// <summary>
    /// 超链接信息列表
    /// </summary>
    private readonly List<HyperlinkInfo> m_HrefInfos = new List<HyperlinkInfo> ();

    /// <summary>
    /// 文本构造器
    /// </summary>
    protected static readonly StringBuilder s_TextBuilder = new StringBuilder ();

    /// <summary>
    /// 超链接色号  只能统一 默认蓝色
    /// </summary>
    public string richColorString = "blue";

    [Serializable]
    public class HrefClickEvent : UnityEvent<string> { }

    [SerializeField]
    private HrefClickEvent m_OnHrefClick = new HrefClickEvent ();

    /// <summary>
    /// 超链接点击事件
    /// </summary>
    public HrefClickEvent onHrefClick
    {
        get { return m_OnHrefClick; }
        set { m_OnHrefClick = value; }
    }


    /// <summary>
    /// 超链接正则
    /// </summary>
    private static readonly Regex s_HrefRegex = new Regex (@"<a href=([^>\n\s]+)>(.*?)(</a>)" , RegexOptions.Singleline);

    protected override void OnEnable ()
    {
        base.OnEnable ();
        onHrefClick.AddListener (OnHyperlinkTextInfo);
    }

    protected override void OnDisable ()
    {
        base.OnDisable ();
        onHrefClick.RemoveListener (OnHyperlinkTextInfo);
    }

    public override void SetVerticesDirty ()
    {
        base.SetVerticesDirty ();
        m_OutputText = GetOutputText (text);
    }

    protected override void OnPopulateMesh (VertexHelper toFill)
    {
        string orignText = text;
        text = m_OutputText;
        base.OnPopulateMesh (toFill);
        text = orignText;
        UIVertex vert = new UIVertex ();

        // 处理超链接包围框
        foreach ( HyperlinkInfo hrefInfo in m_HrefInfos )
        {
            hrefInfo.boxes.Clear ();
            if ( hrefInfo.startIndex >= toFill.currentVertCount )
            {
                continue;
            }

            // 将超链接里面的文本顶点索引坐标加入到包围框
            toFill.PopulateUIVertex (ref vert , hrefInfo.startIndex);
            var pos = vert.position;
            var bounds = new Bounds (pos , Vector3.zero);
            for ( int i = hrefInfo.startIndex, m = hrefInfo.endIndex ; i < m ; i++ )
            {
                if ( i >= toFill.currentVertCount )
                {
                    break;
                }

                toFill.PopulateUIVertex (ref vert , i);
                pos = vert.position;
                if ( pos.x < bounds.min.x ) // 换行重新添加包围框
                {
                    hrefInfo.boxes.Add (new Rect (bounds.min , bounds.size));
                    bounds = new Bounds (pos , Vector3.zero);
                }
                else
                {
                    bounds.Encapsulate (pos); // 扩展包围框
                }
            }
            hrefInfo.boxes.Add (new Rect (bounds.min , bounds.size));
        }
    }

    /// <summary>
    /// 获取超链接解析后的最后输出文本
    /// </summary>
    /// <returns></returns>
    protected virtual string GetOutputText (string outputText)
    {
        s_TextBuilder.Length = 0;
        m_HrefInfos.Clear ();
        var indexText = 0;
        foreach ( Match match in s_HrefRegex.Matches (outputText) )
        {
            s_TextBuilder.Append (outputText.Substring (indexText , match.Index - indexText));
            s_TextBuilder.Append ($"<color={richColorString}>");  // 超链接颜色

            var group = match.Groups [1];
            var hrefInfo = new HyperlinkInfo
            {
                startIndex = s_TextBuilder.Length * 4 , // 超链接里的文本起始顶点索引
                endIndex = ( s_TextBuilder.Length + match.Groups [2].Length - 1 ) * 4 + 3 ,
                name = group.Value
            };
            m_HrefInfos.Add (hrefInfo);

            s_TextBuilder.Append (match.Groups [2].Value);
            s_TextBuilder.Append ("</color>");
            indexText = match.Index + match.Length;
        }
        s_TextBuilder.Append (outputText.Substring (indexText , outputText.Length - indexText));
        return s_TextBuilder.ToString ();
    }

    /// <summary>
    /// 点击事件检测是否点击到超链接文本
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick (PointerEventData eventData)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle (rectTransform , eventData.position , eventData.pressEventCamera , out var lp);

        for ( int i1 = 0 ; i1 < m_HrefInfos.Count ; i1++ )
        {
            HyperlinkInfo hrefInfo = m_HrefInfos [i1];
            var boxes = hrefInfo.boxes;
            for ( var i = 0 ; i < boxes.Count ; ++i )
            {
                if ( boxes [i].Contains (lp) )
                {
                    m_OnHrefClick.Invoke (hrefInfo.name);
                    return;
                }
            }
        }
    }

    /// <summary>
    /// 当前点击超链接回调
    /// </summary>
    /// <param name="info">回调信息</param>
    private void OnHyperlinkTextInfo (string info)
    {
        evt?.Invoke (info);
    }

    public void ClearAction ()
    {
        evt = null;
    }

    public void SetHyperTextColor (string color)
    {
        richColorString = color;
    }
}