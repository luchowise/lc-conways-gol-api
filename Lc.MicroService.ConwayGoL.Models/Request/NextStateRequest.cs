namespace Lc.MicroService.ConwayGoL.Models.Request;

/// <summary>
/// Request model to retrieve the next state of the board.
/// </summary>
public class NextStateRequest
{
    /// <summary>
    /// Gets or sets the ID for the board.
    /// </summary>
    public int BoardId { get; set; }
}