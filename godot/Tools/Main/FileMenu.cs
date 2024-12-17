using Godot;
using System;

public partial class FileMenu : PopupMenu
{
	[Export] public PackedScene? NewProjectPopup;
	[Export] public Node2D? SceneRoot;
	
	public override void _Ready()
	{
		IdPressed += OnIdPressed;
	}

	private void OnIdPressed(long id)
	{
		switch (id)
		{
			case 0:
				Toolkit.Instance.SetupWorkspace();
				break;
		}
	}
}
