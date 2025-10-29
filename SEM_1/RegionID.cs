namespace SEM_1;

public class RegionID : IComparable<RegionID>
{
    public uint ID { get; set; }
    public AVLTree<DistrictID> RegionDistrictIDs { get; set; } = new AVLTree<DistrictID>();
    public RegionID(uint id)
    {
        ID = id;
    }
    public int CompareTo(RegionID? other)
    {
        if (other == null)
        {
            return 1;
        }
        return ID.CompareTo(other.ID);
    }
}
