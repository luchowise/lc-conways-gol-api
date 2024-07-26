using Lc.MicroService.ConwayGoL.Models.Dto;

namespace Lc.MicroService.ConwayGoL.Factories.Errors;

public static class ErrorDtoFactory
{
    public static ErrorDto Create(int statusCode, string message) => new()
    {
        StatusCode = statusCode,
        Message = message
    };
}
