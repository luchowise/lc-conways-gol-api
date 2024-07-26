namespace Lc.MicroService.ConwayGoL.Models.Dto;

/// <summary>
/// Data transfer object for error details.
/// </summary>
public class ErrorDto
{
    /// <summary>
    /// Gets or sets the status code of the error.
    /// </summary>
    public int StatusCode { get; set; }

    /// <summary>
    /// Gets or sets the error message.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Gets or sets the details of the error.
    /// </summary>
    public string Detail { get; set; }
}

