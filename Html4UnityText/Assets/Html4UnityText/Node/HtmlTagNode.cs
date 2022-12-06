using System.Collections.Generic;

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
            throw new System.NotImplementedException ();
        }
    }
}