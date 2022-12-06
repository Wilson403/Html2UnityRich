using System.Collections.Generic;

namespace Html4UnityText
{
    /// <summary>
    /// 该状态机用于对一个起始标签的属性进行分析
    /// </summary>
    public class TagPropMachine
    {
        private TagPropStatus _currentStatus;
        private readonly List<char> _characterList = new List<char> ();

        public readonly TagPropDefaultStatus tagPropDefaultStatus;
        public readonly TagPropStartStatus tagPropStartStatus;
        public readonly TagPropEndStatus tagPropEndStatus;
        public readonly TagPropSpaceStatus tagPropSpaceStatus;
        public readonly TagPropKeyStatus tagPropKeyStatus;
        public readonly TagPropValStatus tagPropValStatus;
        public readonly TagPropValStartStatus tagPropValStartStatus;
        public readonly TagPropValEndStatus tagPropValEndStatus;

        public TagPropMachine ()
        {
            tagPropDefaultStatus = new TagPropDefaultStatus (this);
            tagPropStartStatus = new TagPropStartStatus (this);
            tagPropSpaceStatus = new TagPropSpaceStatus (this);
            tagPropEndStatus = new TagPropEndStatus (this);
            tagPropKeyStatus = new TagPropKeyStatus (this);
            tagPropValStatus = new TagPropValStatus (this);
            tagPropValStartStatus = new TagPropValStartStatus (this);
            tagPropValEndStatus = new TagPropValEndStatus (this);
        }

        /// <summary>
        /// 进入一个状态
        /// </summary>
        /// <param name="htmlStatus"></param>
        public void EnterStats (TagPropStatus tagPropStatus)
        {
            if ( tagPropStatus == null && tagPropStatus == _currentStatus )
            {
                return;
            }

            _currentStatus?.Exit ();
            _currentStatus = tagPropStatus;
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


        public string tagName;
        public string propKey;
        public string propVal;
        public readonly List<KeyValuePair<string , string>> propKvList = new List<KeyValuePair<string , string>> ();

        /// <summary>
        /// 保存属性键值对
        /// </summary>
        public void SavePropKV ()
        {
            propKvList.Add (new KeyValuePair<string , string> (key: propKey , value: propVal));
        }
    }
}