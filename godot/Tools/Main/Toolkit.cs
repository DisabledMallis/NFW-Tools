using Godot;
using System;

public partial class Toolkit : Node2D
{
	[Export] public CanvasLayer? ToolLayer;
	[Export] public Control? Workspace;
	[Export] public PackedScene? WorkspaceScene;

	public static Toolkit Instance = null!;

	public override void _Ready()
	{
		base._Ready();
		
		Instance = this;
	}

	public void OpenProject(string path)
	{
		SetupWorkspace();
	}

	public void SetupWorkspace()
	{
		Workspace?.QueueFree();
		Workspace = WorkspaceScene?.Instantiate<Control>();
		ToolLayer?.AddChild(Workspace);
	}
}
