using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Html4UnityText
{
    public class HtmlTagNode : HtmlNode
    {
        public readonly string tagStartName;
        public readonly string tagEndName;
        public readonly Dictionary<string , string> propKV;
        public readonly List<HtmlNode> childs;

        public HtmlTagNode (string tagStartName , string tagEndName , Dictionary<string , string> propKV = null , List<HtmlNode> childs = null)
        {
            this.tagStartName = tagStartName;
            this.tagEndName = tagEndName;
            this.propKV = propKV ?? new Dictionary<string , string> (0);
            this.childs = childs ?? new List<HtmlNode> (0);
        }

        public override List<HtmlNode> GetChilds ()
        {
            return childs;
        }

        public override HtmlNode ToPropNode ()
        {
            //先对起始标签进行分析
            TagPropMachine machine = new TagPropMachine ();
            char [] charArr = tagStartName.ToCharArray ();
            for ( int i = 0 ; i < charArr.Length ; i++ )
            {
                machine.CurrentStatus.ApendChar (charArr [i]);
            }
            machine.CurrentStatus.EndApend ();

            List<HtmlNode> newChilds = new List<HtmlNode> (childs.Count);
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                newChilds.Add (childs [i].ToPropNode ());
            }

            //用状态机分析出来的数据重新创建HtmlTagNode
            return new HtmlTagNode (tagStartName: machine.tagName , tagEndName: machine.tagName , machine.propKV , newChilds);
        }

        public override HtmlNode ToUnityRichNode ()
        {
            var propsDict = new Dictionary<string , string> ();

            //转化Html标签为UnityRichText标签
            switch ( tagStartName )
            {
                case HtmlTagName.HTML_TAG_P:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeP;
                    break;

                case HtmlTagName.HTML_TAG_H1:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH1;
                    break;

                case HtmlTagName.HTML_TAG_H2:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH2;
                    break;

                case HtmlTagName.HTML_TAG_H3:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH3;
                    break;

                case HtmlTagName.HTML_TAG_H4:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH4;
                    break;

                case HtmlTagName.HTML_TAG_H5:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH5;
                    break;

                case HtmlTagName.HTML_TAG_H6:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeH6;
                    break;

                case HtmlTagName.HTML_TAG_EM:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_I] = "";
                    break;

                default:
                    Debug.LogError ($"Not support html tag name [{tagStartName}]");
                    break;
            }

            //对Class属性进行分析
            if ( propKV.ContainsKey (HtmlTagName.HTML_CLASS) )
            {
                string [] classList = propKV [HtmlTagName.HTML_CLASS].Split (new char [] { ';' });

                for ( int i = 0 ; i < classList.Length ; i++ )
                {
                    if ( string.IsNullOrEmpty (classList [i]) )
                    {
                        continue;
                    }

                    switch ( classList [i] )
                    {
                        case HtmlTagName.HTML_CLASS_SIZE_SMALL:
                            propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeQlSizeSmall;
                            break;

                        case HtmlTagName.HTML_CLASS_LARGE_NAME:
                            propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeQlSizeLarge;
                            break;

                        case HtmlTagName.HTML_CLASS_HUGE_NAME:
                            propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE] = HtmlTagName.htmlSizeQlSizeHuge;
                            break;

                        case HtmlTagName.HTML_CLASS_ALIGN_LEFT:
                            propsDict [HtmlTagName.HTML_CLASS_ALIGN_LEFT] = "";
                            break;

                        case HtmlTagName.HTML_CLASS_ALIGN_CENTER:
                            propsDict [HtmlTagName.HTML_CLASS_ALIGN_CENTER] = "";
                            break;

                        case HtmlTagName.HTML_CLASS_ALIGN_RIGHT:
                            propsDict [HtmlTagName.HTML_CLASS_ALIGN_RIGHT] = "";
                            break;

                        default:
                            Debug.LogError ($"Not support class name [{classList [i]}]");
                            break;
                    }
                }
            }

            //对Style属性进行分析
            if ( propKV.ContainsKey (HtmlTagName.HTML_STYLE) )
            {
                var kvList = propKV [HtmlTagName.HTML_STYLE].Replace (" " , "").Split (new char [] { ';' });
                var styleList = new List<KeyValuePair<string , string>> ();

                for ( int i = 0 ; i < kvList.Length ; i++ )
                {
                    if ( string.IsNullOrEmpty (kvList [i]) )
                    {
                        continue;
                    }

                    var kvSplit = kvList [i].Split (new char [] { ':' });
                    if ( kvSplit.Length == 2 )
                    {
                        styleList.Add (new KeyValuePair<string , string> (kvSplit [0] , kvSplit [1]));
                    }
                    else
                    {
                        Debug.LogError ($"Style format error [{kvList [i]}]");
                    }
                }

                for ( int i = 0 ; i < styleList.Count ; i++ )
                {
                    switch ( styleList [i].Key )
                    {
                        case HtmlTagName.UNITY_RICH_TEXT_TAG_COLOR:
                            {
                                var match = Regex.Matches (styleList [i].Value , @"\d+");
                                if ( match == default || match.Count != 3 )
                                {
                                    Debug.LogError ($"Color format error [{styleList [i].Value}]");
                                    break;
                                }

                                var r = Convert.ToString (int.Parse (match [0].Value) , 16).ToUpper ();
                                var g = Convert.ToString (int.Parse (match [1].Value) , 16).ToUpper ();
                                var b = Convert.ToString (int.Parse (match [2].Value) , 16).ToUpper ();
                                propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_COLOR] = $"#{r}{g}{b}";
                                break;
                            };

                        default:
                            Debug.LogError ($"Not support style name [{styleList [i].Key}]");
                            break;
                    }
                }
            }

            List<HtmlNode> newChilds = new List<HtmlNode> ();
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                newChilds.Add (childs [i].ToUnityRichNode ());
            }

            return new HtmlTagNode (tagStartName: tagStartName , tagEndName: tagEndName , childs: newChilds , propKV: propsDict);
        }

        public override string ToUnityRichText ()
        {
            string result = "";
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                result += childs [i].ToUnityRichText ();
            }

            foreach ( var item in propKV )
            {
                switch ( item.Key )
                {
                    case HtmlTagName.UNITY_RICH_TEXT_TAG_B:
                        result = $"<{HtmlTagName.UNITY_RICH_TEXT_TAG_B}>{result}</{HtmlTagName.UNITY_RICH_TEXT_TAG_B}>";
                        break;

                    case HtmlTagName.UNITY_RICH_TEXT_TAG_I:
                        result = $"<{HtmlTagName.UNITY_RICH_TEXT_TAG_I}>{result}</{HtmlTagName.UNITY_RICH_TEXT_TAG_I}>";
                        break;

                    case HtmlTagName.UNITY_RICH_TEXT_TAG_COLOR:
                        result = $"<{HtmlTagName.UNITY_RICH_TEXT_TAG_COLOR}={item.Value}>{result}</{HtmlTagName.UNITY_RICH_TEXT_TAG_COLOR}>";
                        break;

                    case HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE:
                        result = $"<{HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE}={item.Value}>{result}</{HtmlTagName.UNITY_RICH_TEXT_TAG_SIZE}>";
                        break;

                    default:
                        Debug.LogError ($"Not support unity tag name [{item.Key}]");
                        break;
                }
            }

            return result;
        }
    }
}