﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Godot;

namespace BloonsTD5Rewritten.Godot.NewFramework.Scripts.Assets;

public partial class AsyncResourceLoader : Node
{
    private static AsyncResourceLoader _instance = null;
    public static AsyncResourceLoader Instance() => _instance;

    private readonly Dictionary<string, IResourcePromise> _promises = new();
    
    public override void _Ready()
    {
        _instance = this;
    }

    public override void _Process(double delta)
    {
        foreach (var data in _promises)
        {
            if (ResourceLoader.LoadThreadedGetStatus(data.Key) == ResourceLoader.ThreadLoadStatus.Loaded)
            {
                var resource = ResourceLoader.LoadThreadedGet(data.Key);
                data.Value.FullfillPromise(resource);
                _promises.Remove(data.Key);
            }

            if (ResourceLoader.LoadThreadedGetStatus(data.Key) is not (ResourceLoader.ThreadLoadStatus.Failed
                or ResourceLoader.ThreadLoadStatus.InvalidResource)) continue;
            
            var error = new Exception(data.Key + " was unable to be loaded!");
            data.Value.HandleError(error);
            _promises.Remove(data.Key);
        }
    }

    public ResourcePromise<T> Load<T>(string path) where T : class
    {
        Debug.Assert(Error.Ok == ResourceLoader.LoadThreadedRequest(path));
        _promises[path] = new ResourcePromise<T>();
        return _promises[path] as ResourcePromise<T>;
    }
    public ResourcePromise<Resource> Load(string path) => Load<Resource>(path);
}