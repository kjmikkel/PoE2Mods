using System.IO;
using System.Linq;
using Patchwork;
using Patchwork.AutoPatching;

[assembly: PatchAssembly]
[PatchInfo]
public class CameraZoomPatchInfo : IPatchInfo
{
    public CameraZoomPatchInfo()
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

    public string PatchVersion => "1.0.0.0";
    public string Requirements => "None";
    public string PatchName => "Camera Zoom";
}
