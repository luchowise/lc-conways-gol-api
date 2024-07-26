namespace Lc.MicroService.ConwayGoL.Models.Request;

/// <summary>
/// Request model to retrieve the final state of the board.
/// </summary>
public class FinalStateRequest
{
    /// <summary>
    /// Gets or sets the ID for the board.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Gets or sets the max number of attempts to reach the final state.
    /// </summary>
    public int MaxAttempts { get; set; }
}
