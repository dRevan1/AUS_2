using SEM_1.Core;
using System.Windows;

namespace SEM_1_GUI
{
    public partial class DistrictCounts : Window
    {
        public List<District> Districts { get; set; } = new List<District>();
        public int DistrictCount { get; set; } = 0;
        public DistrictCounts(List<District> districts)
        {
            InitializeComponent();
            Districts = districts;
            DistrictCount = districts.Count;
            DataContext = this;
        }
    }
}
