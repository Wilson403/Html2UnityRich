using System.Collections.Generic;
using System.Text;
using LitJson;

namespace Html4UnityText
{
    public abstract class HtmlNode
    {
        /// <summary>
        /// 转化为支持prop的节点
        /// </summary>
        /// <returns></returns>
        public abstract HtmlNode ToPropNode ();

        /// <summary>
        /// 转化为支持UnityRich的节点
        /// </summary>
        /// <returns></returns>
        public abstract HtmlNode ToUnityRichNode ();

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <returns></returns>
        public abstract List<HtmlNode> GetChilds ();

        public override string ToString ()
        {
            StringBuilder sb = new StringBuilder ();
            JsonWriter jr = new JsonWriter (sb)
            {
                PrettyPrint = true ,
                IndentValue = 4
            };
            JsonMapper.ToJson (this , jr);
            return sb.ToString ();
        }
    }
}