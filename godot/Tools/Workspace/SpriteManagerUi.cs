using System;
using System.Collections.Generic;
using BloonsTD5Rewritten.NewFramework.Scripts.Assets;
using Godot;

namespace BloonsTD5Rewritten.Tools.Workspace;

public partial class SpriteManagerUi : VBoxContainer
{
	[Export] public Button? RootButton;
	[Export] public FlowContainer? SpritesContainer;

	private List<SpriteInfo>? _currentRoot = new();
	
	public override void _Ready()
	{
		RootButton!.Pressed += RootButtonOnPressed;
	}

	private void RootButtonOnPressed()
	{
		_currentRoot = TextureLoader.Instance()?.SpritesRoot;
	}

	private void UpdateSprites()
	{
		for (var i = 0; i < SpritesContainer?.GetChildCount(); i++)
		{
			var child = SpritesContainer.GetChild(i);
			child?.QueueFree();
		}

		foreach (var spriteInfo in _currentRoot?.ToArray() ?? Array.Empty<SpriteInfo>())
		{
			var sheetButton = new Button();
			sheetButton.Text = spriteInfo.Name;
			if (spriteInfo.Frames.Count == 1)
			{
				sheetButton.Icon = spriteInfo.Frames[0].GetTexture();
			}
			SpritesContainer?.AddChild(sheetButton);
		}
	}
}