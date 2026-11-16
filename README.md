# Jakar.Files

Files, directories, file watchers, embedded resources, INI configuration and preference files for Jakar.Extensions.

```sh
dotnet add package Jakar.Files
```

Split out of [Jakar.Extensions](https://github.com/Jakar510/Jakar.Extensions) in 10.12.0. It depends on [Jakar.Extensions](https://www.nuget.org/packages/Jakar.Extensions/). The types keep the `Jakar.Extensions` namespace, so code that used them from Jakar.Extensions only needs this package reference (and a recompile).

## What's in it

| Area | Types |
|---|---|
| Files and directories | `LocalFile` (read/write handlers, hashing, `FileData`/`FileMetaData`), `LocalDirectory`, `LocalFileWatcher`, `TempFile`, `FilePaths`, `PathExtensions`, `FileExtensions` |
| Embedded resources | `EmbeddedResources<TValue>` |
| INI and preferences | `IniConfig` (with the `JObjectNode` bridge), `PreferenceFile<TSelf>` (`{TSelf}.json`, imports an old `.ini`), `AppPreference` / `IAppPreferences` (the `"shared".GetPreference(...)` / `SetPreference(...)` string extensions) |
| App | `IAppInfo` (has `FilePaths Paths`), `AppSettings` / `IAppSettings<TViewPage>` / `IScreenshot`, `LanguageApi` (stores the selected language through `AppPreference`) |
| Security | Extension members that used to be instance members in Jakar.Extensions: `IDataProtector.Encrypt/Decrypt(LocalFile ...)`, `DataProtector.WithKey/WithKeyAsync(EmbeddedResources<T> ...)`, `DataProtector.WithKeyFile/WithKeyAsync(LocalFile ...)`, `PasswordRequirements.SetBlockedPasswords(LocalFile)`. The decryptor delegates are `DataProtectorFiles.Decryptor<TValue>` / `DecryptorAsync<TValue>` (were `IDataProtector.Decryptor<TValue>` / `DecryptorAsync<TValue>`). |

## Migrating from Jakar.Extensions 10.11

Add this package. Calls such as `protector.Decrypt(file)` or `protector.WithKeyFile(pem)` compile unchanged (extension members), but a class that *implements* `IDataProtector` no longer has to implement the `LocalFile` overloads, and code that named `IDataProtector.Decryptor<T>` uses `DataProtectorFiles.Decryptor<T>`.

## Versioning

Versions are `<.NET major>.<library major>.<library minor>`: a breaking change bumps the middle number (`10.1.0` → `10.2.0`), anything else the last one.

## Repository

| Path | What it is |
|---|---|
| `Jakar.Files/` | The package. |
| `Jakar.Files.Tests/` | NUnit tests. |
| `.github/workflows/ci.yml` | Build and test on Ubuntu and Windows, for every push and pull request. |
| `.github/workflows/publish.yml` | On `master`: CI, then pack and push to nuget.org (Trusted Publishing). |