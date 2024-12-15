using Godot;

namespace BloonsTD5Rewritten.Tools.CompoundSprite;

public partial class ReloadButton : Button
{
	[Export] public CsEditorZone? EditorZone;
	[Export] public SpeedAdjust? SpeedAdjust;
	[Export] public CodeEdit? JsonEdit;
	[Export] public Node2D? PreviewOwner;
	public override void _Ready()
	{
		Pressed += OnPressed;
	}

	private void OnPressed()
	{
		var tempPath = "user://compoundtooltemp.json";
		var access = FileAccess.Open(tempPath, FileAccess.ModeFlags.Write);
		if (access == null)
		{
			GD.PrintErr(FileAccess.GetOpenError());
			return;
		}
		access?.StoreString(JsonEdit?.Text);
		access?.Close();
		GD.Print("Reloading!");
		for (var i = 0; i < PreviewOwner?.GetChildCount(); i++)
		{
			PreviewOwner.GetChild(i).QueueFree();
		}
		Callable.From(() =>
		{
			var sprite = new NewFramework.Scripts.Compound.CompoundSprite();
			sprite.SpriteDefinitionRes = tempPath;
			sprite.LoadDefinitionFromJet = false;
			sprite.Position = Vector2.Zero; //Vector2.One * 640.0f * 0.5f;
			PreviewOwner?.AddChild(sprite);
			EditorZone!.PreviewSprite = sprite;
			sprite.PlaybackSpeed = SpeedAdjust?.AdjustedSpeed ?? 1.0f;
		}).CallDeferred();
	}
}