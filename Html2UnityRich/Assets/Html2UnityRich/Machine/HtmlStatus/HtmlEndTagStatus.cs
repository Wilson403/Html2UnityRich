namespace Html2UnityRich
{
    public class HtmlEndTagStatus : HtmlStatus
    {
        public HtmlEndTagStatus (HtmlMachine machine) : base (machine)
        {

        }

        public override void ApendChar (char content)
        {
            machine.AddChar (content);
            switch ( content )
            {
                case '>':
                    machine.htmlTagAnalyer.AppendEnd (machine.MergeChar ());
                    machine.EnterStats (machine.htmlDefaultStatus);
                    break;
            }
        }
    }
}