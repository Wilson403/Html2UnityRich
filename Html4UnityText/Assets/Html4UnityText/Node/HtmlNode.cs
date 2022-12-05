using System.Collections.Generic;
using System.Text;
using LitJson;

namespace Html4UnityText
{
    public abstract class HtmlNode
    {
        public List<HtmlNode> childNodeList = new List<HtmlNode> (0);

        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder ();
            JsonWriter jr = new JsonWriter (sb);
            jr.PrettyPrint = true;
            jr.IndentValue = 4;
            JsonMapper.ToJson (this , jr);
            return sb.ToString ();
        }
    }
}