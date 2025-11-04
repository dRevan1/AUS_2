using SEM_1.Core;
using System.Collections.ObjectModel;
using System.Windows;

namespace SEM_1_GUI
{
    public partial class PersonAndTests : Window
    {
        public List<Person> People {  get; set; } = new List<Person>();
        public List<PCRTest> PCRTests { get; set; } = new List<PCRTest>();
        public int PeopleCount { get; set; } = 0;
        public int TestsCount { get; set; } = 0;
        public PersonAndTests(List<Person> people, List<PCRTest> tests)
        {
            InitializeComponent();
            People = people;
            PCRTests = tests;
            PeopleCount = People.Count;
            TestsCount = PCRTests.Count;
            DataContext = this;
        }
    }
}
