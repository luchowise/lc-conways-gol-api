namespace Lc.MicroService.ConwayGoL.Models.Request;

/// <summary>
/// Request model to retrieve the state of the board after a specified number of steps.
/// </summary>
public class MultipleStatesRequest
{
    /// <summary>
    /// Gets or sets the ID for the board.
    /// </summary>
    public int BoardId { get; set; }

    /// <summary>
    /// Gets or sets the number of steps to simulate.
    /// </summary>
    public int Steps { get; set; }
}