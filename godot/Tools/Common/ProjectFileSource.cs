using BloonsTD5Rewritten.NewFramework.Scripts;
using BloonsTD5Rewritten.NewFramework.Scripts.Assets;

namespace BloonsTD5Rewritten.Tools.Common;

public class ProjectFileSource : IAssetSource
{
    public string ProjectPath = string.Empty;
    
    public byte[] GetFileContent(string path)
    {
        
    }

    public string GetFileText(string path)
    {
    }

    public JsonWrapper GetJsonParsed(string path)
    {
    }
}