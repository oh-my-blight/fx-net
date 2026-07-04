using Fx.Net.Errors;
using Fx.Net.Result;
using Fx.Net.Types;

using Tests.Fx.Net.Result.Fixtures;

namespace Tests.Fx.Net.Result;

public class ResultConversionTests
{
    private readonly ResultTestData _resultTestData = new();

    [Fact]
    public void Deconstruct_WhenResultIsSuccess_OutputsTrueValidValueAndNoneError()
    {
        var result = global::Fx.Net.Result.Result.Success(_resultTestData.Value);

        var (isSuccess, value, error) = result;

        Assert.True(isSuccess);
        Assert.Equal(_resultTestData.Value, value);
        Assert.Equal(Error.None, error);
    }

    [Fact]
    public void Deconstruct_WhenResultIsFailure_OutputsFalseDefaultValueAndCorrectError()
    {
        Result<string> result = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        var (isSuccess, value, error) = result;

        Assert.False(isSuccess);
        Assert.Equal(_resultTestData.Error, error);
        Assert.Equal(null!, value);
    }

    [Fact]
    public void Deconstruct_WhenDefaultInstance_OutputsFalseDefaultValueAndDefaultNullFailureError()
    {
        Result<int> resultValueType = default;

        var (isSuccess, value, error) = resultValueType;

        Assert.False(isSuccess);
        Assert.Equal(0, value);
        Assert.Equal(ResultErrors.DefaultNullFailure, error);

        Result<string> resultRefType = default;
        var (isSuccessRefType, valueRefType, errorRefType) = resultRefType;

        Assert.False(isSuccessRefType);
        Assert.Equal(null!, valueRefType);
        Assert.Equal(ResultErrors.DefaultNullFailure, errorRefType);
    }


    [Fact]
    public void ImplicitOperator_ConvertsFailedResultToGenericResult()
    {
        var failedResult = global::Fx.Net.Result.Result.Failure(_resultTestData.Error);

        Result<int> resultInt = failedResult;
        Result<string> resultString = failedResult;

        Assert.Multiple(
            () => Assert.False(resultInt.IsSuccess),
            () => Assert.Equal(_resultTestData.Error, resultInt.Error),
            () => Assert.False(resultString.IsSuccess),
            () => Assert.Equal(_resultTestData.Error, resultString.Error)
        );
    }

    [Fact]
    public void ImplicitOperator_FromDefaultFailedResult_ResilientToEmptyState()
    {
        FailedResult defaultFailedResult = default;
        Result<int> result = defaultFailedResult;

        Assert.Multiple(
            () => Assert.False(result.IsSuccess),
            () => Assert.True(result.IsFailure),
            () => Assert.Equal(ResultErrors.DefaultNullFailure, result.Error)
        );
    }
}