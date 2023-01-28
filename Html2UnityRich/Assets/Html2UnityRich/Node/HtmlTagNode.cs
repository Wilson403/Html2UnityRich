using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Html2UnityRich
{
    public class HtmlTagNode : HtmlNode
    {
        public readonly string tagStartName;
        public readonly string tagEndName;
        public readonly Dictionary<string , string> propKV;
        public readonly List<HtmlNode> childs;

        public HtmlTagNode (string tagStartName , string tagEndName , int depth , Dictionary<string , string> propKV = null , List<HtmlNode> childs = null)
        {
            this.tagStartName = tagStartName;
            this.tagEndName = tagEndName;
            this.propKV = propKV ?? new Dictionary<string , string> (0);
            this.childs = childs ?? new List<HtmlNode> (0);
            this.depth = depth;
        }

        public override Dictionary<string , string> GetProp ()
        {
            return propKV;
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
            return new HtmlTagNode (tagStartName: machine.tagName , tagEndName: machine.tagName , depth: depth , machine.propKV , newChilds);
        }

        public override HtmlNode ToUnityRichNode ()
        {
            var propsDict = new Dictionary<string , string> ();

            //对Html标签进行换算
            switch ( tagStartName )
            {
                case HtmlTagName.HTML_TAG_P:
                    propsDict [HtmlTagName.HTML_TAG_P] = "\n\n";
                    break;

                case HtmlTagName.HTML_TAG_BR:
                    propsDict [HtmlTagName.HTML_TAG_BR] = "\n";
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

                case HtmlTagName.HTML_TAG_STRONG:
                    propsDict [HtmlTagName.UNITY_RICH_TEXT_TAG_B] = "";
                    break;

                case HtmlTagName.HTML_TAG_A:
                case HtmlTagName.HTML_TAG_SPAN:
                case "":
                case null:
                    //Nothing
                    break;

                default:
                    Debug.LogError ($"Not support html tag name [{tagStartName}]");
                    break;
            }

            //对Class进行换算
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

            //对Style进行换算
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

            //对HRFT换算
            if ( propKV.ContainsKey (HtmlTagName.HTML_HREF) )
            {
                propsDict [HtmlTagName.HTML_HREF] = propKV [HtmlTagName.HTML_HREF];
            }

            List<HtmlNode> newChilds = new List<HtmlNode> ();
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                newChilds.Add (childs [i].ToUnityRichNode ());
            }

            return new HtmlTagNode (tagStartName: tagStartName , tagEndName: tagEndName , depth: depth , childs: newChilds , propKV: propsDict);
        }

        public override string ToUguiRichText ()
        {
            string result = "";
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                result += childs [i].ToUguiRichText ();
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

                    case HtmlTagName.HTML_TAG_P:
                        result = depth == 1 ? $"\n{result}" : $"{item.Value}{result}"; //由于UGUI是使用多Text来实现对齐，所以第一层的节点显示本身就有换行，这里就只有一个\n
                        break;

                    case HtmlTagName.HTML_TAG_BR:
                        result = $"{item.Value}{result}";
                        break;

                    case HtmlTagName.HTML_HREF:
                        result = $"<{HtmlTagName.HTML_TAG_A} {HtmlTagName.HTML_HREF}=\"{item.Value}\"><color=blue>{result}</color></{HtmlTagName.HTML_TAG_A}>";
                        break;

                    case HtmlTagName.HTML_CLASS_ALIGN_LEFT:
                    case HtmlTagName.HTML_CLASS_ALIGN_RIGHT:
                    case HtmlTagName.HTML_CLASS_ALIGN_CENTER:
                        Debug.LogWarning ("UGUI不支持富文本对齐标签,可以使用多Text来实现对齐");
                        break;

                    default:
                        Debug.LogError ($"Not support ugui tag name [{item.Key}]");
                        break;
                }
            }

            return result;
        }

        public override string ToTextProRichText ()
        {
            string result = "";
            for ( int i = 0 ; i < childs.Count ; i++ )
            {
                result += childs [i].ToTextProRichText ();
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

                    case HtmlTagName.HTML_TAG_P:
                    case HtmlTagName.HTML_TAG_BR:
                        result = $"{item.Value}{result}";
                        break;

                    case HtmlTagName.HTML_CLASS_ALIGN_LEFT:
                        result = $"<{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}=left>{result}</{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}>";
                        break;

                    case HtmlTagName.HTML_CLASS_ALIGN_RIGHT:
                        result = $"<{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}=right>{result}</{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}>";
                        break;

                    case HtmlTagName.HTML_CLASS_ALIGN_CENTER:
                        result = $"<{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}=center>{result}</{HtmlTagName.TEXT_PRO_RICH_TEXT_TAG_ALIGN}>";
                        break;

                    case HtmlTagName.HTML_HREF:
                        result = $"<color=blue><u><link={item.Value}>{result}</link></u></color>";
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