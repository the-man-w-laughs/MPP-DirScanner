namespace Model.Nodes
{
    public class Folder : Node
    {
        private List<Node> _children = new List<Node>();

        public string FullName;
        public Folder(string fullName, string name): base(name)
        {
            this.FullName = fullName;
        }
        public override void Add(Node component)
        {
            this._children.Add(component);
        }

        public override List<Node> GetNodes(){
            return _children;
        }
    }
}