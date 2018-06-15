﻿using Patchwork;
using Patchwork.AutoPatching;
using System.IO;
using System.Linq;

[assembly: PatchAssembly]
[PatchInfo]
public class CombatLogLoggerPatchInfo : IPatchInfo
{
    public CombatLogLoggerPatchInfo()
    {
        // Left blank on purpose
    }

    private static string Combine(params string[] paths)
    {
        return paths.Aggregate(@"", Path.Combine);
    }

    public FileInfo GetTargetFile(AppInfo app)
    {
        var file = Combine(app.BaseDirectory.FullName, "PillarsOfEternityII_Data", "Managed", "Assembly-CSharp.dll");
        return new FileInfo(file);
    }

    public string CanPatch(AppInfo app)
    {
        return null;
    }

    public string PatchVersion => "1.0.0.000";
    public string Requirements => "None";
    public string PatchName => "Combat Log Exporter";
}
