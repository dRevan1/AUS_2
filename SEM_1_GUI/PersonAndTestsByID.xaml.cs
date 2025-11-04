using SEM_1.Core;
using System.Windows;

namespace SEM_1_GUI
{

    public partial class PersonAndTestsByID : Window
    {
        public List<Person> People { get; set; } = new List<Person>();
        public List<PCRTestByID> PCRTests { get; set; } = new List<PCRTestByID>();
        public int PeopleCount { get; set; } = 0;
        public int TestsCount { get; set; } = 0;
        public PersonAndTestsByID(List<Person> people, List<PCRTestByID> tests)
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
