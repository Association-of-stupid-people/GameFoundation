#if !BESTHTTP_DISABLE_ALTERNATE_SSL && (!UNITY_WEBGL || UNITY_EDITOR)
#pragma warning disable
namespace BestHTTP.SecureProtocol.Org.BouncyCastle.Crypto.Tls
{
    using System;
    using BestHTTP.SecureProtocol.Org.BouncyCastle.Utilities.Date;

    internal class Timeout
    {
        private long durationMillis;
        private long startMillis;

        internal Timeout(long durationMillis)
            : this(durationMillis, DateTimeUtilities.CurrentUnixMs())
        {
        }

        internal Timeout(long durationMillis, long currentTimeMillis)
        {
            this.durationMillis = Math.Max(0, durationMillis);
            this.startMillis    = Math.Max(0, currentTimeMillis);
        }

        //internal long RemainingMillis()
        //{
        //    return RemainingMillis(DateTimeUtilities.CurrentUnixMs());
        //}

        internal long RemainingMillis(long currentTimeMillis)
        {
            lock (this)
            {
                // If clock jumped backwards, reset start time 
                if (this.startMillis > currentTimeMillis)
                {
                    this.startMillis = currentTimeMillis;
                    return this.durationMillis;
                }

                var elapsed   = currentTimeMillis - this.startMillis;
                var remaining = this.durationMillis - elapsed;

                // Once timeout reached, lock it in
                if (remaining <= 0) return this.durationMillis = 0L;

                return remaining;
            }
        }

        //internal static int ConstrainWaitMillis(int waitMillis, Timeout timeout)
        //{
        //    return ConstrainWaitMillis(waitMillis, timeout, DateTimeUtilities.CurrentUnixMs());
        //}

        internal static int ConstrainWaitMillis(int waitMillis, Timeout timeout, long currentTimeMillis)
        {
            if (waitMillis < 0)
                return -1;

            var timeoutMillis = GetWaitMillis(timeout, currentTimeMillis);
            if (timeoutMillis < 0)
                return -1;

            if (waitMillis == 0)
                return timeoutMillis;

            if (timeoutMillis == 0)
                return waitMillis;

            return Math.Min(waitMillis, timeoutMillis);
        }

        internal static Timeout ForWaitMillis(int waitMillis) { return ForWaitMillis(waitMillis, DateTimeUtilities.CurrentUnixMs()); }

        internal static Timeout ForWaitMillis(int waitMillis, long currentTimeMillis)
        {
            if (waitMillis < 0)
                throw new ArgumentException("cannot be negative", "waitMillis");

            if (waitMillis > 0)
                return new Timeout(waitMillis, currentTimeMillis);

            return null;
        }

        //internal static int GetWaitMillis(Timeout timeout)
        //{
        //    return GetWaitMillis(timeout, DateTimeUtilities.CurrentUnixMs());
        //}

        internal static int GetWaitMillis(Timeout timeout, long currentTimeMillis)
        {
            if (null == timeout)
                return 0;

            var remainingMillis = timeout.RemainingMillis(currentTimeMillis);
            if (remainingMillis < 1L)
                return -1;

            if (remainingMillis > int.MaxValue)
                return int.MaxValue;

            return (int)remainingMillis;
        }

        //internal static bool HasExpired(Timeout timeout)
        //{
        //    return HasExpired(timeout, DateTimeUtilities.CurrentUnixMs());
        //}

        internal static bool HasExpired(Timeout timeout, long currentTimeMillis) { return null != timeout && timeout.RemainingMillis(currentTimeMillis) < 1L; }
    }
}
#pragma warning restore
#endif