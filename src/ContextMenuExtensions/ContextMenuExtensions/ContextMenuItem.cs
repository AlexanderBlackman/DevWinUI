﻿using Windows.Data.Json;
using Windows.Storage;

namespace WinUICommunity;

enum MultipleFilesFlag
{
    OFF, EACH, JOIN
}

public class ContextMenuItem : ContextMenuBaseModel
{
    public StorageFile File { get; set; }

    private string _title;
    private string _exe;
    private string _param;
    private string _icon;
    private string _acceptExts;
    private bool _acceptDirectory;
    private bool _acceptFile;
    private string _pathDelimiter;
    private string _paramForMultipleFiles;
    private int _acceptMultipleFilesFlag;
    private int _index;

    public string Title { get => _title; set => SetProperty(ref _title, value); }
    public string Exe { get => _exe; set => SetProperty(ref _exe, value); }
    public string Param { get => _param; set => SetProperty(ref _param, value); }
    public string Icon { get => _icon; set => SetProperty(ref _icon, value); }
    public string AcceptExts { get => _acceptExts; set => SetProperty(ref _acceptExts, string.IsNullOrEmpty(value) ? value : value.ToLower()); }// to lower for match
    public bool AcceptDirectory { get => _acceptDirectory; set => SetProperty(ref _acceptDirectory, value); }
    public bool AcceptFile { get => _acceptFile; set => SetProperty(ref _acceptFile, value); }
    public int AcceptMultipleFilesFlag { get => _acceptMultipleFilesFlag; set => SetProperty(ref _acceptMultipleFilesFlag, value); }
    public string PathDelimiter { get => _pathDelimiter; set => SetProperty(ref _pathDelimiter, value); }
    public string ParamForMultipleFiles { get => _paramForMultipleFiles; set => SetProperty(ref _paramForMultipleFiles, value); }
    public int Index { get => _index; set => SetProperty(ref _index, value); }
    private static string NameToJsonKey(string name)
    {
        return name[0].ToString().ToLower() + name.Substring(1);
    }

    public static ContextMenuItem ReadFromJson(string content)
    {
        var json = JsonObject.Parse(content);
        var menu = new ContextMenuItem
        {
            Title = json.GetNamedString(NameToJsonKey(nameof(Title)), "menu"),
            Exe = json.GetNamedString(NameToJsonKey(nameof(Exe)), string.Empty),
            Param = json.GetNamedString(NameToJsonKey(nameof(Param)), string.Empty),
            Icon = json.GetNamedString(NameToJsonKey(nameof(Icon)), string.Empty),
            AcceptExts = json.GetNamedString(NameToJsonKey(nameof(AcceptExts)), string.Empty),
            AcceptDirectory = json.GetNamedBoolean(NameToJsonKey(nameof(AcceptDirectory)), false),
            AcceptFile = json.GetNamedBoolean(NameToJsonKey(nameof(AcceptFile)), true),
            AcceptMultipleFilesFlag = (int)json.GetNamedNumber(NameToJsonKey(nameof(AcceptMultipleFilesFlag)), (int)MultipleFilesFlag.OFF),
            PathDelimiter = json.GetNamedString(NameToJsonKey(nameof(PathDelimiter)), string.Empty),
            ParamForMultipleFiles = json.GetNamedString(NameToJsonKey(nameof(ParamForMultipleFiles)), string.Empty),
            Index = (int)json.GetNamedNumber(NameToJsonKey(nameof(Index)), 0),
        };
        return menu;
    }


    public static string WriteToJson(ContextMenuItem content)
    {
        var json = new JsonObject
        {
            [NameToJsonKey(nameof(Title))] = JsonValue.CreateStringValue(content.Title),
            [NameToJsonKey(nameof(Exe))] = JsonValue.CreateStringValue(content.Exe ?? string.Empty),
            [NameToJsonKey(nameof(Param))] = JsonValue.CreateStringValue(content.Param ?? string.Empty),
            [NameToJsonKey(nameof(Icon))] = JsonValue.CreateStringValue(content.Icon ?? string.Empty),
            [NameToJsonKey(nameof(AcceptExts))] = JsonValue.CreateStringValue(content.AcceptExts ?? string.Empty),
            [NameToJsonKey(nameof(AcceptDirectory))] = JsonValue.CreateBooleanValue(content.AcceptDirectory),
            [NameToJsonKey(nameof(AcceptFile))] = JsonValue.CreateBooleanValue(content.AcceptFile),
            [NameToJsonKey(nameof(AcceptMultipleFilesFlag))] = JsonValue.CreateNumberValue(content.AcceptMultipleFilesFlag),
            [NameToJsonKey(nameof(PathDelimiter))] = JsonValue.CreateStringValue(content.PathDelimiter ?? string.Empty),
            [NameToJsonKey(nameof(ParamForMultipleFiles))] = JsonValue.CreateStringValue(content.ParamForMultipleFiles ?? string.Empty),
            [NameToJsonKey(nameof(Index))] = JsonValue.CreateNumberValue(content.Index),
        };
        return json.Stringify();
    }

    public static (bool, string) Check(ContextMenuItem content)
    {
        if (string.IsNullOrEmpty(content.Title))
        {
            return (false, nameof(content.Title));
        }

        return string.IsNullOrEmpty(content.Exe)
            ? (false, nameof(content.Exe))
            : string.IsNullOrEmpty(content.Param) ? (false, nameof(content.Param)) : (true, string.Empty);
    }
}