namespace Lc.MicroService.ConwayGoL.Models.Dto;

/// <summary>
/// Data transfer object representing the state of the board in the Game of Life.
/// </summary>
public class BoardStateDto : IEquatable<BoardStateDto>
{
    /// <summary>
    /// Gets or sets the unique identifier for the board state.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the board, that's represented as a list of lists of integers.
    /// Each list represents a row on the board.
    /// </summary>
    public List<List<int>> Board { get; set; }

    /// <summary>
    /// Default Constructor
    /// Initializes the board property.
    /// </summary>
    public BoardStateDto()
    {
        Board = new List<List<int>>();
    }

    /// <summary>
    /// Determines if the specified <see cref="BoardStateDto"/> is equal to the current <see cref="BoardStateDto"/>.
    /// </summary>
    /// <param name="other">The <see cref="BoardStateDto"/> to compare with the current <see cref="BoardStateDto"/>.</param>
    /// <returns>true or false</returns>
    public bool Equals(BoardStateDto other)
    {
        if (other == null) return false;
        if (Id != other.Id) return false;

        if (Board.Count != other.Board.Count) return false;
        for (var i = 0; i < Board.Count; i++)
        {
            if (!Board[i].SequenceEqual(other.Board[i])) return false;
        }
        return true;
    }

    /// <summary>
    /// Determines if the specified object is equal to the current <see cref="BoardStateDto"/>.
    /// </summary>
    /// <param name="obj">The object to compare with the current <see cref="BoardStateDto"/>.</param>
    /// <returns>true or false</returns>
    public override bool Equals(object obj)
    {
        return Equals(obj as BoardStateDto);
    }

    /// <summary>
    /// Default hash function.
    /// </summary>
    /// <returns>A hash code for the <see cref="BoardStateDto"/> instance.</returns>
    public override int GetHashCode()
    {
        var hash = Id.GetHashCode();
        foreach (var row in Board)
        {
            foreach (var cell in row)
            {
                hash = hash * 31 + cell.GetHashCode();
            }
        }
        return hash;
    }
}
