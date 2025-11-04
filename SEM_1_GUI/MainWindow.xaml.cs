using SEM_1.Core;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace SEM_1_GUI
{
    public partial class MainWindow : Window
    {
        PCRTestDatabase pcrTestDatabase = new PCRTestDatabase();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PopulateBtn_Click(object sender, RoutedEventArgs e)
        {
            pcrTestDatabase.PopulateDatabase(10, 100, 1000, 5000);
            MessageBox.Show("Databse has been populated");
        }

        private void ImportBtn_Click(object sender, RoutedEventArgs e)
        {
            pcrTestDatabase.ImportDatabase();
            MessageBox.Show("Import complete");
        }

        private void ExportBtn_Click(object sender, RoutedEventArgs e)
        {
            pcrTestDatabase.ExportDatabase();
            MessageBox.Show("Export complete");
        }

         // # 1
        private void AddPCRBtn_Click(object sender, RoutedEventArgs e)
        {
            byte minuteOfTest = byte.Parse(MinuteUpper.Text);
            byte hourOfTest = byte.Parse(HourUpper.Text);
            byte dayOfTest = byte.Parse(DayUpper.Text);
            byte monthOfTest = byte.Parse(MonthUpper.Text);
            ushort yearOfTest = ushort.Parse(YearUpper.Text);
            string personID = TestPersonID.Text;
            uint testSite = uint.Parse(TestSite.Text);
            uint region = uint.Parse(Region.Text);
            uint district = uint.Parse(District.Text);
            double testValue = double.Parse(TestValue.Text);
            string note = Note.Text;
            PCRTestData testData = new PCRTestData(
                minuteOfTest,
                hourOfTest,
                dayOfTest,
                monthOfTest,
                yearOfTest,
                personID,
                0,
                testSite,
                region,
                district,
                false,
                testValue,
                note
            );

            string result = pcrTestDatabase.AddPCRTest(testData);
            if (result != "")
            {
                MessageBox.Show(result);
            }
        }

        // # 2
        private void FindPCRBtn_Click(object sender, RoutedEventArgs e)
        {
            Person? patient = pcrTestDatabase.FindPerson(PersonID.Text);
            PCRTestByID? test = pcrTestDatabase.FindPCRTest(PersonID.Text, uint.Parse(TestID.Text));
            if (patient == null || test == null) 
            {
                MessageBox.Show("This person or test isn't in the database!");
            }
            else
            {
                List<Person> people = new List<Person>();
                people.Add(patient);
                List<PCRTestByID> tests = new List<PCRTestByID>();
                tests.Add(test);
                var window = new PersonAndTestsByID(people, tests);
                window.ShowDialog();
            }

        }

        // # 3
        private void FindPersonsTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            Person? patient = pcrTestDatabase.FindPerson(PersonID.Text);
            List<PCRTest> tests = pcrTestDatabase.GetAllTestsByPerson(PersonID.Text);
            if (patient == null)
            {
                MessageBox.Show("This person is not in the databse!");
            }
            else
            {
                List<Person> people = new List<Person>();
                people.Add(patient);
                var window = new PersonAndTests(people, tests);
                window.ShowDialog();
            }
        }

        // # 4
        private void FindDistrictPositivesBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest>? tests = pcrTestDatabase.GetAllPositiveTestsByDistrict(uint.Parse(District.Text), min, max);
            if (tests == null)
            {
                MessageBox.Show("This district isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 5
        private void FindDistrictAllBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest>? tests = pcrTestDatabase.GetAllTestsByDistrict(uint.Parse(District.Text), min, max);
            if (tests == null)
            {
                MessageBox.Show("This district isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 6
        private void FindRegionPositivesBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest>? tests = pcrTestDatabase.GetAllPositiveTestsByRegion(uint.Parse(Region.Text), min, max);
            if (tests == null)
            {
                MessageBox.Show("This region isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 7
        private void FindRegionAllBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest>? tests = pcrTestDatabase.GetAllTestsByRegion(uint.Parse(Region.Text), min, max);
            if (tests == null)
            {
                MessageBox.Show("This region isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 8
        private void FindAllPositiveTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest> tests = pcrTestDatabase.GetAllPositiveTests(min, max);
            var window = new PersonAndTests(new List<Person>(), tests);
            window.ShowDialog();
        }

        // # 9
        private void FindAllTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest> tests = pcrTestDatabase.GetAllTests(min, max);
            var window = new PersonAndTests(new List<Person>(), tests);
            window.ShowDialog();
        }

        // # 10
        private void FindDistrictSickBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Person>? people = pcrTestDatabase.GetSickPeopleByDistrict(uint.Parse(District.Text), max, int.Parse(Days.Text));
            if (people == null)
            {
                MessageBox.Show("This district isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(people, new List<PCRTest>());
                window.ShowDialog();
            }
        }

        // # 11
        private void FindDistrictSickSortedBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Person>? people = pcrTestDatabase.GetSickPeopleByDistrictSorted(uint.Parse(District.Text), max, int.Parse(Days.Text));
            if (people == null)
            {
                MessageBox.Show("This district isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(people, new List<PCRTest>());
                window.ShowDialog();
            }
        }

        // # 12
        private void FindRegionSickBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Person>? people = pcrTestDatabase.GetSickPeopleByRegion(uint.Parse(Region.Text), max, int.Parse(Days.Text));
            if (people == null)
            {
                MessageBox.Show("This region isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(people, new List<PCRTest>());
                window.ShowDialog();
            }
        }

        // # 13
        private void FindAllSickBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Person> people = pcrTestDatabase.GetAllSickPeople(max, int.Parse(Days.Text));
            var window = new PersonAndTests(people, new List<PCRTest>());
            window.ShowDialog();
        }

        // # 14
        private void FindSickEachDistrictBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Person> people = pcrTestDatabase.GetSickPersonFromEveryDistrict(max, int.Parse(Days.Text));
            var window = new PersonAndTests(people, new List<PCRTest>());
            window.ShowDialog();
        }

        // # 15
        private void FindDistrictsBySickBtn_Click(object sender, RoutedEventArgs e) 
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<District> districts = pcrTestDatabase.GetDistrictsBySickPeople(max, int.Parse(Days.Text));
            var window = new DistrictCounts(districts);
            window.ShowDialog();
        }

        // # 16
        private void FindRegionsBySickBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<Region> regions = pcrTestDatabase.GetRegionBySickPeople(max, int.Parse(Days.Text));
            var window = new RegionCounts(regions);
            window.ShowDialog();
        }

        // # 17
        private void FindSiteTestsBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest max = new PCRTest(new PCRTestData(59,
                23,
                byte.Parse(DayUpper.Text),
                byte.Parse(MonthUpper.Text),
                ushort.Parse(YearUpper.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            PCRTest min = new PCRTest(new PCRTestData(0,
                0,
                byte.Parse(DayLower.Text),
                byte.Parse(MonthLower.Text),
                ushort.Parse(YearLower.Text),
                "",
                0,
                0,
                0,
                0,
                false,
                0.0,
                ""));

            List<PCRTest>? tests = pcrTestDatabase.GetAllTestsBySite(uint.Parse(TestSite.Text), min, max);
            if (tests == null)
            {
                MessageBox.Show("This test site isn't in the database!");
            }
            else
            {
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 18
        private void FindTestByIDBtn_Click(object sender, RoutedEventArgs e)
        {
            PCRTest? test = pcrTestDatabase.GetPCRTest(uint.Parse(TestID.Text));
            if (test == null)
            {
                MessageBox.Show("Test with this ID wasn't found in the databse!");
            }
            else
            {
                List<PCRTest> tests = new List<PCRTest>();
                tests.Add(test);
                var window = new PersonAndTests(new List<Person>(), tests);
                window.ShowDialog();
            }
        }

        // # 19
        private void AddPersonBtn_Click(object sender, RoutedEventArgs e)
        {
            Person person = new Person(PersonID.Text, Name.Text, Surname.Text, byte.Parse(DayOfBirth.Text), byte.Parse(MonthOfBirth.Text), ushort.Parse(YearOfBirth.Text));
            string result = pcrTestDatabase.AddPerson(person);
            MessageBox.Show(result);
        }

        // # 20
        private void DeletePCRBtn_Click(object sender, RoutedEventArgs e)
        {
            string result = pcrTestDatabase.DeletePCRTestByID(uint.Parse(TestID.Text));
            MessageBox.Show(result);
        }

        // # 21
        private void DeletePersonBtn_Click(object sender, RoutedEventArgs e)
        {
            string result = pcrTestDatabase.DeletePerson(PersonID.Text);
            MessageBox.Show(result);
        }
    }
}