namespace Uinsure.Core.Models;

public class PolicyHolder
{
    public static PolicyHolder Basic(string firstName, string lastName, DateOnly dob)
        => new() { FirstName = firstName, LastName = lastName, DateOfBirth = dob };

    public string FirstName { get; init; }

    public string LastName { get; init; }

    public DateOnly DateOfBirth { get; init; }
}
