namespace Model
{
    public abstract class Component
    {
        public long size;
        public String name;
        public abstract string Operation();
        public virtual void Add(Component component)
        {
            throw new NotImplementedException();
        }

        public virtual void Remove(Component component)
        {
            throw new NotImplementedException();
        }
        public virtual bool IsComposite()
        {
            return true;
        }
    }

    public class Leaf : Component
    {
        public Leaf(string name, long size){
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

    // Класс Контейнер содержит сложные компоненты, которые могут иметь
    // вложенные компоненты. Обычно объекты Контейнеры делегируют фактическую
    // работу своим детям, а затем «суммируют» результат.
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

        // Контейнер выполняет свою основную логику особым образом. Он проходит
        // рекурсивно через всех своих детей, собирая и суммируя их результаты.
        // Поскольку потомки контейнера передают эти вызовы своим потомкам и так
        // далее,  в результате обходится всё дерево объектов.
        public override string Operation()
        {
            foreach (Component component in this._children)
            {

            }
            return "";
        }
    }
}