using Godot;

namespace BloonsTD5Rewritten.Tools.Workspace;

public partial class JsonTree : Tree
{
	[Export] public PackedScene? ContextMenu;
	public override void _Ready()
	{
		
	}

	public override void _GuiInput(InputEvent evt)
	{
		base._GuiInput(evt);

		if (evt is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Right && mouseButton.IsReleased())
			{
				if (GetSelected() == null)
				{
					ShowTreeContext();
				}
			}
		}
	}

	public void ShowTreeContext()
	{
		var menu = ContextMenu?.Instantiate<PopupMenu>();
		var windowPos = GetWindow().Position;
		var mousePos = GetGlobalMousePosition();
		var menuPos = new Vector2I((int)mousePos.X, (int)mousePos.Y);
		menu!.Position = windowPos + menuPos;
		AddChild(menu);
		menu?.Show();
	}
}