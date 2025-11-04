using System.Text;

namespace SEM_1.Core;

public class RegionID : IComparable<RegionID>
{
    public uint ID { get; set; }
    public AVLTree<DistrictID> DistrictIDs { get; set; } = new AVLTree<DistrictID>();
    public RegionID(uint id)
    {
        ID = id;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"{ID}");
        List<string> districtIDs = DistrictIDs.LevelOrderTraversal();
        sb.AppendLine($";{districtIDs.Count}");

        foreach (string districtID in districtIDs)
        {
            sb.Append(districtID);
        }

        return sb.ToString();
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
