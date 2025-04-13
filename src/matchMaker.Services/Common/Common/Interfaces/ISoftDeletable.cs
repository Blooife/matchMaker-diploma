namespace Common.Interfaces;

public interface ISoftDeletable
{
    DateTime? DeletedAt { get; set; }
}
