using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace BloonsTD5Rewritten.NewFramework.Scripts.Assets;

public partial class TextureLoader : Node
{
	private static TextureLoader? _instance;

	public static TextureLoader? Instance()
	{
		return _instance;
	}
	public IAssetSource? FileSource;

	private List<SpriteInfo>? _spritesRoot;
	public List<SpriteInfo>? SpritesRoot => _spritesRoot;
	private readonly Dictionary<string, Task<ImageTexture>> _thumbLoadTasks = new();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Debug.Assert(_instance == null, "Why are there two texture loaders?");
		_instance = this;
		
		//Get the instance of the gdscript from the root
		_spritesRoot = LoadSpriteInfo();
	}

	public ImageTexture? GetTrackThumb(string trackName)
	{
		if (_thumbLoadTasks.TryGetValue(trackName, out var task))
		{
			return task.IsCompleted ? task.Result : null;
		}

		_thumbLoadTasks[trackName] = Task.Run(() =>
		{
			var texturesDir = "Assets/Textures";
			var imageFile = texturesDir + "/Ultra/track_thumbs/" + trackName + "_thumb.jpg";
			var image = Image.LoadFromFile(imageFile);
			var texture = ImageTexture.CreateFromImage(image);
			return texture;
		});
		//Call again to check if it completed
		return GetTrackThumb(trackName);
	}
	
	public SpriteInfo GetSpriteInfo(string name)
	{
		return _spritesRoot!.FirstOrDefault(info => info.Path.EndsWith(name + ".xml"))!;
	}

	public Variant FindCell(string name, string texture)
	{
		var result = _spritesRoot!.Select(info => info.FindCell(name, texture))
			.FirstOrDefault(result => result != null);
		if (result != null)
			return Variant.From(result);

		if (texture == "error")
			throw new Exception("Failed to find error texture...");
		if (texture != "" && FindFrame(texture) == null)
			return FindCell("texture_not_found", "error");
		
		return FindCell("sprite_not_found", "error");
	}
	public Variant FindCell(string name) => FindCell(name, "");

	public FrameInfo? FindFrame(string name)
	{
		return _spritesRoot!.Select(info => info.FindFrame(name)).FirstOrDefault(result => result != null);
	}
		
	private List<SpriteInfo>? LoadSpriteInfo()
	{
		var rootEntry = FileSource?.GetRootEntry();
		var texturesDir = rootEntry?.GetChild("Textures");

		/*texturesDir.ListDirBegin();
		var filename = texturesDir.GetNext();
		while (!string.IsNullOrEmpty(filename))
		{
			if (!texturesDir.CurrentIsDir())
			{
				results.Add(new SpriteInfo(filename.Replace(".xml", ""), dirPath, dirPath + "/" + filename));
			}
			filename = texturesDir.GetNext();
		}*/

		return (from entry in texturesDir!.GetChildren() where !entry.IsDirectory select new SpriteInfo(entry)).ToList();
	}
}