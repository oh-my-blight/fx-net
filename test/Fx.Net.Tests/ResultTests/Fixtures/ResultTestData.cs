using Fx.Net.Types;

namespace Fx.Net.Tests.ResultTests.Fixtures;

internal record ResultTestData
{
    public Error Error { get; } = new Error("Code.Test", "Test error message");
    public string Value { get; } = "Hello, world!";
}