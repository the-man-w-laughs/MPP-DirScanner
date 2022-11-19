namespace Model
{


    public class File : Component
    {
        public File(string name, long size){
            this.name = name;
            this.size = size;
        }    
        public override string Operation()
        {
            return "Leaf";
        }

        public override bool IsComposite()
        {
            return false;
        }
    }

    public class Composite : Component
    {
        private List<Component> _children = new List<Component>();
        
        public Composite(string name){
            this.name = name;
        }
        public override void Add(Component component)
        {
            this._children.Add(component);
        }

        public override void Remove(Component component)
        {
            this._children.Remove(component);
        }
        public override string Operation()
        {
            foreach (Component component in this._children)
            {

            }
            return "";
        }
    }
}