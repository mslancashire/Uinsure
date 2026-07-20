using Microsoft.Extensions.Time.Testing;
using System.Net.NetworkInformation;

namespace Tests.Uinsure.Core;

public class Settings
{
    public readonly static DateTimeOffset ReferenceDateTime = new(2026, 5, 1, 10, 0, 0, 0, TimeSpan.Zero);
    public readonly static DateOnly ReferenceDate = DateOnly.FromDateTime(ReferenceDateTime.Date);

    public readonly static DateOnly DateForRenewal = new(2025, 06, 15);

    public static TimeProvider TimeProvider
    {
        get
        {
            var fakeProvider = new FakeTimeProvider();
            fakeProvider.SetUtcNow(ReferenceDateTime);

            return fakeProvider;
        }
    }
}
