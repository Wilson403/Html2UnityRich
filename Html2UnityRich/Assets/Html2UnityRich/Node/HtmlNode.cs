using System.Collections.Generic;
using System.Text;
using LitJson;

namespace Html2UnityRich
{
    public abstract class HtmlNode
    {
        protected int depth;
        /// <summary>
        /// 节点深度
        /// </summary>
        public int Depth 
        {
            get 
            {
                return depth;
            }
        }

        /// <summary>
        /// 获取属性数据
        /// </summary>
        /// <returns></returns>
        public abstract Dictionary<string , string> GetProp ();

        /// <summary>
        /// 获取子节点列表
        /// </summary>
        /// <returns></returns>
        public abstract List<HtmlNode> GetChilds ();

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
        ///转化为UGUI富文本
        /// </summary>
        /// <returns></returns>
        public abstract string ToUguiRichText ();

        /// <summary>
        ///转化为TextPro富文本
        /// </summary>
        /// <returns></returns>
        public abstract string ToTextProRichText ();

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