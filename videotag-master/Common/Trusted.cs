namespace Common;

public readonly record struct Trusted<T>
{
    public required T Value { get; init; }

    public static implicit operator T(Trusted<T> trusted) => trusted.Value;

    public static Trusted<T> From(T value) => new() { Value = value };
}