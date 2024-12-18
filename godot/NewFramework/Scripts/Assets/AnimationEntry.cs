using System.Collections.Generic;
using Godot;

namespace BloonsTD5Rewritten.NewFramework.Scripts.Assets;

public partial class AnimationEntry : Node
{
    private IFileSystemEntry? _fileEntry;
    private TextureQuality _quality;
    private readonly List<CellEntry> _cells = new();

    public string AnimationName;
    public FrameInfo Parent;
    
    public AnimationEntry(FrameInfo parent, IFileSystemEntry entry, TextureQuality quality, string animationName)
    {
        Parent = parent;
        _fileEntry = entry;
        _quality = quality;
        AnimationName = animationName;
    }

    public void AddCell(CellEntry entry)
    {
        _cells.Add(entry);
    }

    public CellEntry? GetCell(string name)
    {
        return _cells.Find(entry => entry.CellName == name);
    }

    public CellEntry? FindCell(string name)
    {
        return _cells.Find(cell => cell.CellName == name);
    }
}