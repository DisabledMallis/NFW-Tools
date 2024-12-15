using Godot;

namespace BloonsTD5Rewritten.Tools.CompoundSprite;

public partial class FileSelecter : MenuButton
{
	[Export] public CsEditorZone? EditorZone;
	[Export] public LineEdit? PathEdit;
	[Export] public CodeEdit? JsonEdit;
	[Export] public Node2D? PreviewOwner;
	
	public override void _Ready()
	{
		base._Ready();
		
		GetPopup().IdPressed += OnIdPressed;
	}

	private void OnIdPressed(long id)
	{
		switch (id)
		{
			case 0:
				OpenFromFile();
				break;
			case 1:
				OpenFromJet();
				break;
			default:
				return;
		}
	}

	private void OpenFromFile()
	{
		var assetImporterConfig = GetNode<Node>("/root/AssetImporterConfig");
		
		var dialog = new FileDialog();
		dialog.Title = "Open File";
		dialog.Access = FileDialog.AccessEnum.Filesystem;
		dialog.CurrentDir = assetImporterConfig.Get("game_dir").AsString();
		dialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		dialog.Filters = new []
		{
			"*.*;*.*;All files",
			"*.json;*.json;JSON files"
		};
		dialog.UseNativeDialog = true;
		dialog.FileSelected += OpenSpriteFile;
		dialog.Show();
	}

	private void OpenSpriteFile(string path)
	{
		GD.Print("Opening sprite file: " + path);
		for (var i = 0; i < PreviewOwner?.GetChildCount(); i++)
		{
			PreviewOwner.GetChild(i).QueueFree();
		}
		Callable.From(() =>
		{
			var sprite = new NewFramework.Scripts.Compound.CompoundSprite();
			sprite.SpriteDefinitionRes = path;
			sprite.LoadDefinitionFromJet = false;
			sprite.Position = Vector2.Zero;//Vector2.One * 640.0f * 0.5f;
			PreviewOwner?.AddChild(sprite);
			EditorZone!.PreviewSprite = sprite;
		}).CallDeferred();
		PathEdit!.Text = path;
		JsonEdit!.Text = FileAccess.GetFileAsString(path);
	}

	private void OpenFromJet()
	{
		var assetImporterConfig = GetNode<Node>("/root/AssetImporterConfig");
		
		var dialog = new FileDialog();
		dialog.Title = "Open File";
		dialog.Access = FileDialog.AccessEnum.Filesystem;
		dialog.CurrentDir = assetImporterConfig.Get("game_dir").AsString();
		dialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		dialog.Filters = new []
		{
			"*.*,*.*;All files",
			"*.json;*.tower;JET files"
		};
		dialog.UseNativeDialog = false;
		dialog.FileSelected += OpenSpriteFile;
		dialog.Show();
	}
}