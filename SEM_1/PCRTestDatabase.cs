namespace SEM_1;

public class PCRTestDatabase
{
    public AVLTree<Person> People { get; set; } = new AVLTree<Person>();

    // #1
    public void AddPCRTest(PCRTest test)
    {
        Person? person = new Person { PersonID = test.PersonID };
        person = People.Find(person);
        if (person == null)
        {
            return;
        }
        person.Tests.Insert(test);
    }

    // #2
    public PCRTest? FindPCRTest(string personID, int testID)
    {
        Person? person = new Person { PersonID = personID };
        person = People.Find(person);
        if (person == null)
        {
            return null;
        }
        PCRTest? test = new PCRTest { TestID = testID, PersonID = personID };

        if (test == null)
        {
            return null;
        }
        return test;
    }

    // #19
    public void AddPerson(Person person)
    {
        People.Insert(person);
    }

    // #21
    public void DeletePerson(string personID)
    {
        Person person = new Person { PersonID = personID };
        People.Delete(person);
    }
}
