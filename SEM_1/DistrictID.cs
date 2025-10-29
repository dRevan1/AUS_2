namespace SEM_1;

public class DistrictID : IComparable<DistrictID>
{
    public uint ID { get; set; }
    public DistrictID(uint id)
    {
        ID = id;
    }
    public int CompareTo(DistrictID? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
