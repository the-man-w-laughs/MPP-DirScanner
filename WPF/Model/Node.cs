using System.CodeDom;
using System.Collections.ObjectModel;
using System.Resources;
using System.Text.RegularExpressions;

namespace Presentation.Model
{
    public class Node
    {
        public string Name { get; }
        public long Length { get; }
        public double SizeInPercent { get; }
        public bool IsDirectory { get; }
        public ObservableCollection<Node>? Children { get; internal set; }
        public string IcoPath { get;  }
        public Node(string name, long length, double sizeInPercent, bool isDirectory = false, ObservableCollection<Node>? children = null)
        {
            Name = name;
            Length = length;
            SizeInPercent = sizeInPercent;
            IsDirectory = isDirectory;
            Children = children;            
            Regex regex = new Regex(@"\.([^\.]*)$");
            Match match = regex.Match(name);
            if (!isDirectory)
            switch (match.Groups[1].Value)
            {
                case "jpeg":
                case "jpg":
                    IcoPath = "Resources/jpg.png";
                    break;
                case "mp3":
                    IcoPath = "Resources/mp3-player.png";
                    break;
                default:
                    IcoPath = "Resources/google-docs.png";
                break;

            }
            else
            {
                IcoPath = "Resources/folder.png";
            }            
        }
    }
}
