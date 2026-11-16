// Jakar.Extensions :: Jakar.Files.Tests
// 10/02/2026

using System.IO;
using System.Threading.Tasks;



namespace Jakar.Extensions.Tests;


/// <summary> File types through Jakar.Json: LocalFile / LocalDirectory, preference files, the IniConfig bridge. </summary>
[TestFixture]
public sealed class FilesJson_Tests
{
    [Test] public void LocalFile_And_LocalDirectory_SerializeAsTheirPath()
    {
        string         directory = Path.Combine(Path.GetTempPath(), $"jakar-{Guid.NewGuid():N}");
        LocalDirectory dir       = new(directory);

        try
        {
            LocalFile file = dir.Join("missing.txt"); // doesn't exist: the 10.x contract threw on Length
            string    json = file.ToJson();

            Assert.That(json,                                           Does.Contain("FullPath").And.Contain("FileEncoding").And.Not.Contain("Length"));
            Assert.That(LocalFile.FromJson(json).FullPath,              Is.EqualTo(file.FullPath));
            Assert.That(LocalDirectory.FromJson(dir.ToJson()).FullPath, Is.EqualTo(dir.FullPath));
            Assert.That(dir.ToJson(),                                   Does.Not.Contain("CreationTimeUtc").And.Not.Contain("Files"));
        }
        finally { Directory.Delete(directory, true); }
    }


    // ─── PreferenceFile ───────────────────────────────────────────────────────

    [Test] public async Task Preferences_SaveLoad_KeepUnknownKeys()
    {
        string directory = Path.Combine(Path.GetTempPath(), $"jakar-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);

        try
        {
            LocalFile file = new(Path.Combine(directory, "prefs.json"));
            await File.WriteAllTextAsync(file.FullPath, "{\"General\":{\"Theme\":\"Light\",\"Enabled\":true},\"Server\":{\"Host\":\"h\",\"Port\":\"8080\"},\"FromNewerVersion\":{\"A\":1}}");

            await using ( TestPreferences prefs = await TestPreferences.CreateAsync(file) )
            {
                Assert.That(prefs.General.Theme,                Is.EqualTo("Light"));
                Assert.That(prefs.Server.Port,                  Is.EqualTo(8080));
                Assert.That(prefs.Contains("FromNewerVersion"), Is.True, "unknown keys are kept");
                prefs.General.Theme = "Blue";
            }

            string saved = await File.ReadAllTextAsync(file.FullPath);
            Assert.That(saved, Does.Contain("\"Blue\"").And.Contain("FromNewerVersion"));
        }
        finally { Directory.Delete(directory, true); }
    }


    [Test] public async Task Preferences_ImportLegacyIni()
    {
        string directory = Path.Combine(Path.GetTempPath(), $"jakar-{Guid.NewGuid():N}");
        Directory.CreateDirectory(directory);

        try
        {
            LocalFile file = new(Path.Combine(directory, "prefs.json"));
            await File.WriteAllTextAsync(Path.Combine(directory, "prefs.ini"), "[General]\nTheme=Green\nEnabled=true\n\n[Server]\nHost=example\nPort=9000\n");

            TestPreferences prefs = await TestPreferences.CreateAsync(file);
            Assert.That(prefs.General.Theme,                               Is.EqualTo("Green"));
            Assert.That(prefs.General.Enabled,                             Is.True);
            Assert.That(prefs.Server.Port,                                 Is.EqualTo(9000));
            Assert.That(file.Exists,                                       Is.True, "imported preferences are saved as JSON");
            Assert.That(File.Exists(Path.Combine(directory, "prefs.ini")), Is.True, "the .ini is kept as a backup");
        }
        finally { Directory.Delete(directory, true); }
    }


    [Test] public void IniConfig_JsonBridge_RoundTrips()
    {
        IniConfig   config = IniConfig.Parse("[A]\nx=1\ny=true\n[B]\nz=text\n");
        JObjectNode json   = config.ToJsonObject();

        Assert.That((bool)json["A"]!["y"],     Is.True);
        Assert.That((string?)json["A"]!["x"], Is.EqualTo("1"));

        IniConfig back = IniConfig.FromJsonObject(json);
        Assert.That(back["B"]["z"], Is.EqualTo("text"));
        Assert.That(back["A"]["y"], Is.EqualTo("true"));
    }
}