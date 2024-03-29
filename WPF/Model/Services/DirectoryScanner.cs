﻿using Model.Interfaces;
using Model.Nodes;
using Model.Services;
using Prism.Mvvm;
using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using File = Model.Nodes.File;

namespace WPF.Services
{
    public class DirectoryScanner : BindableBase, IDirectoryScanner
    {
        private CancellationTokenSource _tokenSource;
        private TaskQueue? _taskQueue;
        public bool IsRunning { get; private set; }

        public DirectoryScanner()
        {            
        }
        private long _dynamicSize;

        public long DynamicSize {
            get {
                return _dynamicSize;
            }
            set
            {
                _dynamicSize = value;
                RaisePropertyChanged("DynamicSize");
            }
        }

        public Folder Start(string path, ushort maxThreadCount)
        {
            DynamicSize = 0;
            _tokenSource = new CancellationTokenSource();
            if (!Directory.Exists(path))
            {
                throw new ArgumentException($"Directory {path} does not exist");
            }

            if (maxThreadCount == 0)
            {
                throw new ArgumentException($"Max thread count should be greater than 0");
            }

            IsRunning = true;

            _taskQueue = new TaskQueue(maxThreadCount, _tokenSource);

            
            var directoryInfo = new DirectoryInfo(path);
            var root = new Folder(directoryInfo.FullName, directoryInfo.Name);
            var token = _tokenSource.Token;
            var rootTask = new Task(() => ScanDirectory(root), token);
            _taskQueue.Enqueue(rootTask);

            _taskQueue.StartAndWaitAll();
            IsRunning = false;

            calculateSize(root);    
            return root;
        }

        public void Stop()
        {
            _tokenSource.Cancel();
            IsRunning = false;
        }

        private void ScanDirectory(Folder folder)
        {
            var outerDirectoryInfo = new DirectoryInfo(folder.FullName);
            var token = _tokenSource.Token;

            DirectoryInfo[]? directoriesInfo;
            try
            {
                directoriesInfo = outerDirectoryInfo.GetDirectories().
                    Where(info => info.LinkTarget == null).ToArray();
            }
            catch (Exception)
            {
                directoriesInfo = null;
            }

            if (directoriesInfo != null)
            {
                foreach (var directoryInfo in directoriesInfo)
                {
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    var innerFolder = new Folder(directoryInfo.FullName, directoryInfo.Name);
                    folder.Add(innerFolder);
                    var task = new Task(() => ScanDirectory(innerFolder), token);
                    _taskQueue!.Enqueue(task);
                }
            }

            FileInfo[]? filesInfo;
            try
            {
                filesInfo = outerDirectoryInfo.GetFiles()
                    .Where(info => info.LinkTarget == null).ToArray();
            }
            catch
            {
                filesInfo = null;
            }

            if (filesInfo != null)
            {
                foreach (var fileInfo in filesInfo)
                {                    
                    if (token.IsCancellationRequested)
                    {
                        return;
                    }
                    var file = new File(fileInfo.Name, fileInfo.Length);                    
                    folder.Add(file);
                    DynamicSize += fileInfo.Length;
                }
            }   
        }

        private long calculateSize(Node node)
        {
            long size = 0;
            if (node.IsComposite())
            {
                foreach (Node innerNode in node.GetNodes())
                {
                    size += calculateSize(innerNode);
                }
                node.Size = size;
            }
            else
            {
                return node.Size;
            }
            DynamicSize = size;
            return size;
        }
    }
}
