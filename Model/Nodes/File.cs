namespace Model.Nodes
{
    public class File : Node
    {
        public File(string name, long size): base(name)
        {
            this.Size = size;
        }

        public override bool IsComposite()
        {
            return false;
        }
    }
}