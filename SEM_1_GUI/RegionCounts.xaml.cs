using SEM_1.Core;
using System.Windows;

namespace SEM_1_GUI
{
    public partial class RegionCounts : Window
    {
        public List<Region> Regions { get; set; } = new List<Region>();
        public int RegionCount { get; set; } = 0;
        public RegionCounts(List<Region> regions)
        {
            InitializeComponent();
            Regions = regions;
            RegionCount = regions.Count;
            DataContext = this;
        }
    }
}
