using System;
using System.IO;
using Godot;
using FileAccess = Godot.FileAccess;

namespace BloonsTD5Rewritten.Tools.Main;

public partial class NewProjectPopup : Window
{
	[Export] public LineEdit? ProjectLocationEdit;
	[Export] public Label? ProjectLocationError;
	[Export] public LineEdit? GameLocationEdit;
	[Export] public Label? GameLocationError;
	[Export] public Button? CancelButton;
	[Export] public Button? CreateButton;
	
	public override void _Ready()
	{
		CloseRequested += OnCloseRequested;
		CancelButton!.Pressed += CancelButtonOnPressed;
		CreateButton!.Pressed += CreateButtonPressed;
	}

	private void CreateButtonPressed()
	{
		var error = 0;
		if (!DirAccess.DirExistsAbsolute(ProjectLocationEdit?.Text))
		{
			ProjectLocationError!.Text = "The project location does not exist.";
			ProjectLocationError?.Show();
			error = 1;
		}
		else if (DirAccess.GetFilesAt(ProjectLocationEdit?.Text).Length > 0)
		{
			ProjectLocationError!.Text = "The project location already contains files.";
			ProjectLocationError?.Show();
			error = 2;
		}
		if (!FileAccess.FileExists(GameLocationEdit?.Text) || !DirAccess.DirExistsAbsolute(Path.GetDirectoryName(GameLocationEdit?.Text)))
		{
			GameLocationError!.Text = "The game location doesn't exist.";
			GameLocationError?.Show();
			error = 3;
		}

		switch (error)
		{
			case 2:
			{
				var confirmDialog = new ConfirmationDialog();
				confirmDialog.Title = "Create anyway?";
				confirmDialog.DialogText = "The selected project folder already has files inside, create anyway? This may overwrite existing files and is irreversible.";
				confirmDialog.Exclusive = true;
				confirmDialog.ForceNative = true;
				confirmDialog.Confirmed += CommitCreateProject;
				AddChild(confirmDialog);
				confirmDialog.CallDeferred("show");
				return;
			}
			case > 0:
				return;
		}

		CommitCreateProject();
	}

	private void CreateProjectFiles()
	{
		GD.Print("Creating project files...");
		Toolkit.Instance.OpenProject(ProjectLocationEdit?.Text ?? string.Empty);
	}

	private void CommitCreateProject()
	{
		CreateProjectFiles();
		QueueFree();
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