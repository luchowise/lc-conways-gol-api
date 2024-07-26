namespace Lc.MicroService.ConwayGoL.Factories.Errors;
public static class ErrorsConstants
{
    public const string BoardStateNullError = "Board state can't be null.";
    public const string InvalidBoardStateError = "Board state is invalid. All rows should have the same number of columns.";
    public const string BoardStateNotFoundError = "Board state was not found.";
}