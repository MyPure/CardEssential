using System;
using System.IO;
using LitJson;

namespace CardEssential.Monitor.FileUtility;

[Serializable]
public abstract class BaseDataFile
{
    public abstract string DataName { get; }

    public virtual string DataFolderPath { get; } = "";

    public abstract BaseDataFile Default { get; }

    public abstract bool Valid { get; }

    public string DataFilePath => (string.IsNullOrEmpty(DataFolderPath) ? DataName : Path.Combine(DataFolderPath, DataName)) + ".dat";
}