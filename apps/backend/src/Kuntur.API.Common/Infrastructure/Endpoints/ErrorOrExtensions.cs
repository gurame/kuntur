using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Mapster;

namespace Kuntur.API.Common.Infrastructure.Endpoints;
public static class ErrorOrExtensions
{
    public static IResult MapResponse<TValue, TResponse>(this ErrorOr<TValue> errorOr)
    {
        return errorOr.Match(
                result => TypedResults.Ok(result.Adapt<TResponse>())
            ,
            Problem);
    }
    private static IResult Problem(List<Error> errors)
    {
        if (errors.Count is 0)
        {
            return TypedResults.Problem(
                statusCode: StatusCodes.Status500InternalServerError,
                title: "An unexpected error occurred.");
        }

        if (errors.All(error => error.Type == ErrorType.Validation))
        {
            return ValidationProblem(errors);
        }

        return Problem(errors[0]);
    }
    private static ProblemHttpResult Problem(Error error)
    {
        var statusCode = error.Type switch
        {
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Unauthorized => StatusCodes.Status403Forbidden,
            _ => StatusCodes.Status500InternalServerError,
        };

        return TypedResults.Problem(statusCode: statusCode, detail: error.Description);
    }
    private static ValidationProblem ValidationProblem(List<Error> errors)
    {
        var dict = new Dictionary<string, string[]>();
        foreach (var error in errors)
        {
            if (dict.TryGetValue(error.Code, out string[]? value))
            {
                dict[error.Code] = [.. value, error.Description];
            }
            else
            {
                dict.Add(error.Code, [error.Description]);
            }
        }
        return TypedResults.ValidationProblem(dict);
    }
}