using Model.Nodes;
using System.Collections.ObjectModel;

namespace Presentation.Model
{
    public class FileTree
    {
        public Node Root { get; }

        // initial method 
        public FileTree(Folder tree)
        {
            Root = new Node(tree.Name, tree.Size, 0, tree.IsComposite());            
            if (tree.GetNodes() != null)
            {
                SetChilds(tree, Root);
            }
        }

        // node is a source struct containing information about node
        // dtoNode is a local struct representing node is TreeView
        private void SetChilds(Folder node, Node dtoNode)
        {
            if (node.GetNodes() != null)
            {
                dtoNode.Children = new ObservableCollection<Node>();
                foreach (var child in node.GetNodes())
                {
                    double sizeInPercent = node.Size == 0? 0 : (float)child.Size/ (float)node.Size * 100;

                    Node newNode = new Node(child.Name, child.Size, sizeInPercent, child.IsComposite());
                    if (child.IsComposite() && child.GetNodes() != null)                    
                    {
                        SetChilds((Folder)child, newNode);
                    }
                    dtoNode.Children.Add(newNode);
                }
            }            
        }
    }
}
