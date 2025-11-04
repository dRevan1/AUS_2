namespace SEM_1.Core;

public class DistrictID : IComparable<DistrictID>
{
    public uint ID { get; set; }
    public DistrictID(uint id)
    {
        ID = id;
    }

    public override string ToString()
    {
        return ID.ToString() + "\n";
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
