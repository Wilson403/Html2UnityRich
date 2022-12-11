namespace Html2UnityRich
{
    public class TagPropValEndStatus : TagPropStatus
    {
        public TagPropValEndStatus (TagPropMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            switch ( content )
            {
                case ' ':
                    machine.EnterStats (machine.tagPropSpaceStatus);
                    break;

                case '/':
                case '>':
                    machine.EnterStats (machine.tagPropEndStatus);
                    break;
            }
        }
    }
}