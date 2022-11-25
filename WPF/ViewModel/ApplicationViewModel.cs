using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Model.Services;
using Model.Interfaces;
using Presentation.Command;
using Model.Nodes;
using System.Windows.Forms;
using System.Windows.Controls;

namespace Presentation.ViewModel
{
    public class ApplicationViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private readonly IDirectoryScanner _scanner = new DirectoryScanner();

        public RelayCommand SetDirectoryCommand { get; }
        public RelayCommand StartScanningCommand { get; }
        public RelayCommand StopScanningCommand { get; }

        public ApplicationViewModel()
        {        
            StartScanningCommand = new RelayCommand(_ =>
            {
                Task.Run(() =>
                {
                    _isScanning = true;
                    Folder result = _scanner.Start(DirectoryPath, _maxThreadCount);
                    _isScanning = false;
                    Tree = new Model.FileTree(result);
                });
               
            }, _ => _directoryPath != null && !_isScanning);

            StopScanningCommand = new RelayCommand(_ =>
            {
                _scanner.Stop();
                _isScanning = false;
            }, _ => _isScanning);

            SetDirectoryCommand = new RelayCommand(_ =>
            {
                using var folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    DirectoryPath = folderBrowserDialog.SelectedPath;
                    Tree = null;
                }
            });
        }

        private string? _directoryPath;
        public string? DirectoryPath
        {
            get { return _directoryPath; }
            set
            {
                _directoryPath = value;
                OnPropertyChanged("DirectoryPath");
            }
        }

        private ushort _maxThreadCount = 32;        

        private Model.FileTree? _tree;
        public Model.FileTree? Tree
        {
            get { return _tree; }
            private set
            {
                _tree = value;
                OnPropertyChanged("Tree");
            }

        }

        private volatile bool _isScanning = false;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
            }
        }        
    }
}
