using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Godot;
using Ionic.Zip;

namespace BloonsTD5Rewritten.NewFramework.Scripts.Assets;

public partial class JetFileImporter : Node, IAssetSource
{
	private static JetFileImporter? _instance;
	public static JetFileImporter Instance() => _instance!;
	
	private Node? _assetImporterConfig;
	private ZipFile? _jetFile;
	public ZipFile? JetFile => _jetFile;
	public string JetPassword = "Q%_{6#Px]]";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_assetImporterConfig = GetNode<Node>("/root/AssetImporterConfig");
		var jetFile = _assetImporterConfig.Get("jet_file").ToString();
		
		var zipFile = new ZipFile(jetFile);
		zipFile.Password = JetPassword;
		_jetFile = zipFile;

		_instance = this;
	}

	public ZipEntry? GetFileEntry(IFileSystemEntry fileEntry)
	{
		return _jetFile?.Entries.FirstOrDefault(entry => entry.FileName == fileEntry.Path);
	}

	public MemoryStream GetFileStream(IFileSystemEntry fileEntry)
	{
		var entry = GetFileEntry(fileEntry);
		var stream = new MemoryStream();
		entry?.Extract(stream);
		stream.Seek(0, SeekOrigin.Begin);
		GD.Print("Read file: " + fileEntry.Name);
		return stream;
	}

	public byte[] GetFileContent(IFileSystemEntry fileEntry)
	{
		var stream = GetFileStream(fileEntry);
		return stream.ToArray();
	}

	public string GetFileText(IFileSystemEntry fileEntry)
	{
		var data = GetFileContent(fileEntry);
		return Encoding.ASCII.GetString(data);
	}
	
	public Variant GetJsonEntry(IFileSystemEntry fileEntry)
	{
		var data = GetFileText(fileEntry);
		return Json.ParseString(data);
	}

	public JsonWrapper GetJsonParsed(IFileSystemEntry fileEntry)
	{
		return new JsonWrapper(GetJsonEntry(fileEntry));
		/*var data = GetFileText(path);

		return new JsonWrapper(JsonSerializer.Deserialize<JsonElement>(data));*/
	}

	public class JetEntry : IFileSystemEntry
	{
		public JetEntry(ZipFile? zipFile)
		{
			_zipFile = zipFile;
		}
		
		private string? _name = "Assets";
		public string Name => _name;
		
		private string? _path = "Assets/";
		public string Path => _path;
		
		private IFileSystemEntry? _parent;
		public IFileSystemEntry? Parent => _parent;

		public IFileSystemEntry[] GetChildren()
		{
			List<IFileSystemEntry> childEntries = new();
			foreach (var entry in _zipFile?.Entries ?? Enumerable.Empty<ZipEntry>())
			{
				if (!entry.FileName.StartsWith(Path)) continue;
				
				var childEntry = new JetEntry(_zipFile);
				childEntry._name = entry.FileName;
				childEntry._path = entry.FileName;
				childEntry._parent = this;
				childEntry._isDirectory = entry.IsDirectory;
				childEntries.Add(childEntry);
			}
			return childEntries.ToArray();
		}

		public IFileSystemEntry? GetChild(string name)
		{
			var fullPath = _path + name;
			if (!_zipFile?.ContainsEntry(fullPath) ?? true) return null;
			
			var zipEntry = _zipFile.Entries.FirstOrDefault(e => e.FileName == fullPath);
			var childEntry = new JetEntry(_zipFile);
			childEntry._name = zipEntry?.FileName;
			childEntry._path = zipEntry?.FileName + (zipEntry?.IsDirectory ?? false ? "/" : "");
			childEntry._parent = this;
			childEntry._isDirectory = zipEntry?.IsDirectory ?? false;
			return childEntry;
		}

		private bool _isDirectory = false;
		public bool IsDirectory => _isDirectory;
		
		private readonly ZipFile? _zipFile;
	};
	public IFileSystemEntry GetRootEntry()
	{
		var rootEntry = new JetEntry(_jetFile);
		return rootEntry;
	}
}
