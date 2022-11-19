namespace Tests;

using Model.Interfaces;
using Model.Nodes;
using Model.Services;

public class UnitTest1
{
    private IDirectoryScanner _directoryScanner;
    public UnitTest1()
    {
        _directoryScanner = new DirectoryScanner();
    }

    private static object GetEqualityComparer<T>()
    {
        throw new NotImplementedException();
    }

    [Theory]
    [InlineData(1)]    
    [InlineData(20)]
    public void ReturnCorrectNode(ushort threadCount)
    {
        Node root = _directoryScanner.Start(@"..\..\..\TestDir", threadCount);

        Assert.Equal("TestDir", root.Name);
        Assert.Equal(13, root.Size);        
        Assert.Equal(root.GetNodes().Count, 3);
        Assert.Equal(1, root.GetNodes().First(x => x.Name == "1.txt").Size);
        Assert.Equal(2, root.GetNodes().First(x => x.Name == "2.txt").Size);        

        Node innerFolder = root.GetNodes().First(x => x.Name == "10");
        Assert.Equal(10, root.GetNodes().First(x => x.Name == "10").Size);
        Assert.Equal(3 ,innerFolder.GetNodes().Count);
        Assert.Equal(0, innerFolder.GetNodes().First(x => x.Name == "0.txt").Size);
        Assert.Equal(4, innerFolder.GetNodes().First(x => x.Name == "4.txt").Size);  
        Assert.Equal(6, innerFolder.GetNodes().First(x => x.Name == "6.txt").Size);
    }    

    [Fact]
    public void SpeedTestAbsolute()    
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        _directoryScanner.Start(@"C:\Program Files\dotnet", 8);
        watch.Stop();        

        var elapsed8 = watch.Elapsed.TotalMilliseconds;
        Console.WriteLine($"elapsed8 = {elapsed8};");
        Assert.True(elapsed8 < 500);
    }

    [Fact]
    public void SpeedTestRelative()    
    {
        var watch = new System.Diagnostics.Stopwatch();
        watch.Start();
        _directoryScanner.Start(@"C:\Program Files\dotnet", 2);
        watch.Stop();        

        var elapsed2 = watch.Elapsed.TotalMilliseconds;        

        watch.Reset();
        watch.Start();
        _directoryScanner.Start(@"C:\Program Files\dotnet", 8);
        watch.Stop();
        var elapsed8 = watch.Elapsed.TotalMilliseconds;

        Console.WriteLine($"elapsed8 = {elapsed8.ToString()}, elapsed2 = {elapsed2.ToString()}");
        Assert.True(elapsed2 > elapsed8);
    }

    [Fact]
    public void SizeTest()    
    {
        var node = _directoryScanner.Start(@"C:\Program Files\dotnet", 32);
        Console.WriteLine($"dotnet size = {node.Size}");
        Assert.Equal(2427078694, node.Size);
    }
}