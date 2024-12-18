namespace BloonsTD5Rewritten.NewFramework.Scripts.Assets;

public interface IAssetSource
{
    public byte[] GetFileContent(IFileSystemEntry fileEntry);
    public string GetFileText(IFileSystemEntry fileEntry);
    public JsonWrapper GetJsonParsed(IFileSystemEntry fileEntry);
    public IFileSystemEntry GetRootEntry();
}