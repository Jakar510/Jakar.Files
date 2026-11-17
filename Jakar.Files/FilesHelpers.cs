// Jakar.Extensions :: Jakar.Files
// 11/16/2026

namespace Jakar.Extensions;


/// <summary> The Jakar.Extensions helpers the file types use, kept internal so they don't clash with Jakar.Extensions' public ones. </summary>
internal static class FilesHelpers
{
    /// <summary> Upper-case hex SHA-512 (as Jakar.Extensions' <c> Hash_SHA512 </c>). </summary>
    public static string Hash_SHA512( this ReadOnlySpan<byte> data ) => Convert.ToHexString(SHA512.HashData(data));
    public static string Hash_SHA512( this string data, Encoding? encoding = null ) => Convert.ToHexString(SHA512.HashData(( encoding ?? Encoding.Default ).GetBytes(data)));


    /// <summary> The bytes when <paramref name="value"/> is base64, otherwise the string itself. </summary>
    public static OneOf<byte[], string> TryGetData( this string value )
    {
        ArgumentNullException.ThrowIfNull(value);
        byte[] buffer = new byte[( value.Length / 4 + 1 ) * 3];

        return Convert.TryFromBase64String(value, buffer, out int written)
                   ? buffer.AsSpan(0, written).ToArray()
                   : value;
    }


    public static bool ShouldContinue( this CancellationToken token ) => !token.IsCancellationRequested;

    public static ReadOnlyMemory<byte> AsReadOnlyMemory( this MemoryStream stream ) => new(stream.GetBuffer(), 0, (int)stream.Length);

    public static async ValueTask<MemoryStream> ToMemoryStream( this Stream stream )
    {
        MemoryStream buffer = new((int)stream.Length);
        await stream.CopyToAsync(buffer).ConfigureAwait(false);
        buffer.Seek(0, SeekOrigin.Begin);
        return buffer;
    }


    /// <summary> Copies <paramref name="source"/> into <paramref name="destination"/> (creating it if needed), replacing existing keys; values are cloned. </summary>
    public static JObjectNode? Merge( JObjectNode? destination, JObjectNode? source )
    {
        if ( source is null || source.Count == 0 ) { return destination; }

        destination ??= [];
        foreach ( ( string key, JNode? value ) in source ) { destination[key] = value?.DeepClone(); }

        return destination;
    }
}



/// <summary> Jakar.Extensions' JSON helpers the INI/preference types use. </summary>
internal static class FilesJson
{
    /// <summary> As Jakar.Extensions' <c> Json.ReaderOptions </c>: comments and trailing commas allowed. </summary>
    public static readonly JsonReaderOptions ReaderOptions = new() { AllowComments = true, AllowTrailingCommas = true };


    public static string Serialize( scoped ReadOnlySpan<string> values )
    {
        JArrayNode array = new(values.Length);
        foreach ( string value in values ) { array.Add(value); }

        return array.ToJson(JsonWriterOptions.Default);
    }


    public static bool TryDeserialize<T, TConverter>( scoped ReadOnlySpan<char> json, [MaybeNullWhen(false)] out T value )
        where TConverter : IJsonConverter<T>
    {
        JsonReader<char> reader = new(json, ReaderOptions);

        try
        {
            if ( TConverter.TryRead(ref reader, out value) && reader.TryReadEnd() ) { return true; }

            value = default;
            return false;
        }
        finally { reader.Dispose(); }
    }
}
