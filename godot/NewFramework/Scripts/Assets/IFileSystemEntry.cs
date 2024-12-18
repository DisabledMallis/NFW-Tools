namespace BloonsTD5Rewritten.NewFramework.Scripts.Assets;

/// <summary>
/// File system entry interface
/// Always expected to use 'Assets' as the root directory
/// Should be used to inject/override assets
/// </summary>
public interface IFileSystemEntry
{
    public string Name { get; }
    public string Path { get; }
    public IFileSystemEntry? Parent { get; }
    public IFileSystemEntry[] GetChildren();
    public IFileSystemEntry? GetChild(string name);
    public bool IsDirectory { get; }
}