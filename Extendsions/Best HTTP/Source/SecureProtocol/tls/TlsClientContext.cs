#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Tls
{
    /// <summary>Marker interface to distinguish a TLS client context.</summary>
    public interface TlsClientContext
        : TlsContext
    {
    }
}
#pragma warning restore
#endif