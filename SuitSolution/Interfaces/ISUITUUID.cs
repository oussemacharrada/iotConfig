namespace SuitSolution.Interfaces;

public interface ISUITUUID
{
    Guid UUID { get; set; }
    byte[] ToSUIT();
    void FromSUIT(byte[] data);
}
