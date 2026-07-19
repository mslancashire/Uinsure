using Microsoft.Extensions.Time.Testing;

namespace Tests.Uinsure.Core;

public class Settings
{
    public readonly static DateTimeOffset ReferenceDateTime = new(2026, 5, 1, 10, 0, 0, 0, TimeSpan.Zero);

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
