using System.Collections.Generic;

namespace Html4UnityText
{
    public class HtmlTagNode : HtmlNode
    {
        public readonly string tagStartName;
        public readonly string tagEndName;
        public readonly Dictionary<string , string> propKV;

        public HtmlTagNode (string tagStartName , string tagEndName , Dictionary<string , string> propKV)
        {
            this.tagStartName = tagStartName;
            this.tagEndName = tagEndName;
            this.propKV = propKV;
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

            //用状态机分析出来的数据重新创建HtmlTagNode
            return new HtmlTagNode (tagStartName: machine.tagName , tagEndName: machine.tagName , machine.propKV);
        }

        public override HtmlNode ToUnityRichNode ()
        {
            throw new System.NotImplementedException ();
        }
    }
}