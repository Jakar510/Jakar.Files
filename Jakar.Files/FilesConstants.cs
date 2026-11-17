// Jakar.Extensions :: Jakar.Files
// 11/16/2026

namespace Jakar.Extensions;


/// <summary> The Jakar.Extensions <c> Constants </c> the file types use (same values; Jakar.Extensions keeps its public copies). </summary>
internal static class FilesConstants
{
    public const int    DESCRIPTION = 2048;
    public const string EMPTY       = "";
    public const int    HASH        = 512;
    public const int    MAX_SIZE    = 0x3FFFFFDF; // string.MaxLength = 2,147,483,647 (2^31-1) - overhead
    public const int    NAME        = 512;
    public const int    TYPE        = 512;
    public const string ACCOUNTS_FILE = "Accounts.json";
    public const string APP_CACHE_ZIP_FILE = "App.Cache.zip";
    public const string APP_DATA_DIRECTORY = "AppData";
    public const string APP_DATA_ZIP_FILE = "App.Data.zip";
    public const string APP_STATE_FILE = "AppState.json";
    public const string CACHE_DIRECTORY = "Cache";
    public const string CLOSE = " ]";
    public const string CRASH_DATA = "Crash.dat";
    public const int DEFAULT_CAPACITY = 64;
    public const string EQUALS_SPACE = " = ";
    public const string FALSE = "false";
    public const string FEEDBACK_FILE = "Feedback.txt";
    public const string INCOMING_FILE = "Incoming.json";
    public const string LOGS_DIRECTORY = "Logs";
    public const string LOGS_FILE = "App.Logs";
    public const string LOGS_ZIP_FILE_NAME = "App.logs.zip";
    public const string NULL = "null";
    public const string OPEN = "[ ";
    public const string OUTGOING_FILE = "Outgoing.json";
    public const string SCREEN_SHOT_FILE = "ScreenShot.png";
    public const string TRUE = "true";
}
