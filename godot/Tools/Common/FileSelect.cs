using System;
using Godot;

namespace BloonsTD5Rewritten.Tools.Common;

public partial class FileSelect : HBoxContainer
{
	[Export] public string DialogTitle = "Open File";
	[Export] public string StartingDirectory = "";
	[Export] public string[]? FileFilter;
	[Export] public FileDialog.FileModeEnum FileMode = FileDialog.FileModeEnum.OpenFile;
	
	[Export] public LineEdit? FilePathEdit;
	[Export] public Button? FileSelectButton;

	public event EventHandler<string>? FileSelected;
	public override void _Ready()
	{
		FilePathEdit!.TextSubmitted += OnFileSelected;
		FileSelectButton!.Pressed += OpenFileDialog;
	}

	private void OpenFileDialog()
	{
		var dialog = new FileDialog();
		dialog.Title = DialogTitle;
		dialog.Access = FileDialog.AccessEnum.Filesystem;
		dialog.CurrentDir = StartingDirectory;
		dialog.FileMode = FileMode;
		dialog.Filters = FileFilter;
		dialog.UseNativeDialog = true;
		dialog.FileSelected += OnFileSelected;
		dialog.Show();
	}

	private void OnFileSelected(string path)
	{
		FilePathEdit!.Text = path;
		FileSelected?.Invoke(this, path);
		GD.Print($"FileSelected: {path}");
	}
}