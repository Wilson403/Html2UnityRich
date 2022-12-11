using System.Collections.Generic;

namespace Html2UnityRich
{
    /// <summary>
    /// 该状态机用于对一段Html文本进行分析（区分出标签，文本）
    /// </summary>
    public class HtmlMachine
    {
        private HtmlStatus _currentStatus;
        private readonly List<char> _characterList = new List<char> ();

        public readonly HtmlTagAnalyer htmlTagAnalyer;
        public readonly HtmlDefaultStatus htmlDefaultStatus;
        public readonly HtmlStartOrEndStatus htmlStartOrEndStatus;
        public readonly HtmlStartTagStatus htmlStartTagStatus;
        public readonly HtmlEndTagStatus htmlEndTagStatus;
        public readonly HtmlSingleTagStatus htmlSingleTagStatus;

        public HtmlStatus CurrentStatus
        {
            get
            {
                return _currentStatus;
            }
        }

        public HtmlMachine ()
        {
            htmlTagAnalyer = new HtmlTagAnalyer ();
            htmlDefaultStatus = new HtmlDefaultStatus (this);
            htmlStartOrEndStatus = new HtmlStartOrEndStatus (this);
            htmlStartTagStatus = new HtmlStartTagStatus (this);
            htmlEndTagStatus = new HtmlEndTagStatus (this);
            htmlSingleTagStatus = new HtmlSingleTagStatus (this);
            EnterStats (htmlDefaultStatus);
        }

        /// <summary>
        /// 进入一个状态
        /// </summary>
        /// <param name="htmlStatus"></param>
        public void EnterStats (HtmlStatus htmlStatus)
        {
            if ( htmlStatus == null && htmlStatus == _currentStatus )
            {
                return;
            }

            _currentStatus?.Exit ();
            _currentStatus = htmlStatus;
            _currentStatus.Enter ();
        }

        /// <summary>
        /// 添加字符
        /// </summary>
        /// <param name="character"></param>
        public void AddChar (char character) 
        {
            _characterList.Add (character);
        }

        /// <summary>
        /// 合并字符
        /// </summary>
        /// <returns></returns>
        public string MergeChar ()
        {
            var str = string.Join ("" , _characterList);
            _characterList.Clear ();
            return str;
        }
    }
}