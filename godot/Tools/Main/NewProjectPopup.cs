using Godot;
using System;

public partial class NewProjectPopup : Window
{
	[Export] public Button? CancelButton;
	
	public override void _Ready()
	{
		CloseRequested += OnCloseRequested;
		CancelButton!.Pressed += CancelButtonOnPressed;
	}

	private void OnCloseRequested()
	{
		QueueFree();
	}

	private void CancelButtonOnPressed()
	{
		QueueFree();
	}
}
