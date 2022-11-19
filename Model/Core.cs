namespace Model{
    public class Core{
        public void ScanDir(Composite node, string path){
            if (Directory.Exists(path)){
                var dirInfo = new DirectoryInfo(path);
                node.name = dirInfo.Name;                
                foreach (FileInfo fi in dirInfo.GetFiles())
                {         
                    node.Add(new Leaf(fi.Name,fi.Length));          
                }                

                foreach (DirectoryInfo di in dirInfo.GetDirectories()){
                    node.Add(new Composite(di.Name));
                }
            }
        }
    }
}