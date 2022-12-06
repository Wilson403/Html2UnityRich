using System;

namespace Html4UnityText
{
    /// <summary>
    /// 默认状态
    /// </summary>
    public class TagPropDefaultStatus : TagPropStatus
    {
        public TagPropDefaultStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                case ' ':
                    break;

                case '<':
                    machine.EnterStats (machine.tagPropStartStatus);
                    break;

                default:
                    throw new Exception ($"语法错误,TagPropDefaultStatus遇到异常字符{content}");
            }
        }
    }
}