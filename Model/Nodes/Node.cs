namespace Model.Nodes
{
    public abstract class Node
    {

        public Node(string name){
            Name = name;
        }
        public long Size;
        public String Name;
        public virtual void Add(Node node)
        {
            throw new NotImplementedException();
        }

        public virtual List<Node> GetNodes()
        {
            throw new NotImplementedException();
        }
        
        public virtual bool IsComposite()
        {
            return true;
        }
    }
}