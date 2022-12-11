using System;

namespace Html2UnityRich
{
    public class TagPropValStatus : TagPropStatus
    {
        public TagPropValStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                case '"':
                    machine.EnterStats (machine.tagPropValStartStatus);
                    break;

                default:
                    throw new Exception ($"TagPropValStartStatus语法错误，意料之外的字符{content}");
            }
        }
    }
}