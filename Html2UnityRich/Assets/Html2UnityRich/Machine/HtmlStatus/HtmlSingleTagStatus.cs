namespace Html2UnityRich
{
    public class HtmlSingleTagStatus : HtmlStatus
    {
        public HtmlSingleTagStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            machine.AddChar (content);
            switch ( content )
            {
                case '>':
                    machine.htmlTagAnalyer.AddSingleNode (machine.MergeChar ());
                    machine.EnterStats (machine.htmlDefaultStatus);
                    break;
            }
        }
    }
}