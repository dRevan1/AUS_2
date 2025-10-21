namespace SEM_1;

public class Tester
{

    public void BSTBaseTest(uint operations, BinarySearchTree<int> bst)
    {
        Random rand = new Random();
        List<int> findInputs = new List<int>();
        List<int> findOutputs = new List<int>();
        Dictionary<int, int> presentDataDic = new Dictionary<int, int>();
        List<int> presentDataList = new List<int>();

        for (int i = 0; i < operations; i++)
        {
            double operationType = rand.NextDouble();
            if (operationType < 0.6)
            {
                int insertValue = rand.Next(0, 200000000);
                while (presentDataDic.ContainsKey(insertValue))
                {
                    insertValue = rand.Next(0, 200000000);
                }
                presentDataDic[insertValue] = insertValue;
                presentDataList.Add(insertValue);
                bst.Insert(insertValue);
            }
            else if (operationType < 0.8)
            {
                int findValue = rand.Next(0, presentDataList.Count);
                if (presentDataList.Count == 0)
                {
                    continue;
                }
                findInputs.Add(presentDataList[findValue]);
                int foundValue = bst.Find(presentDataList[findValue]);
                findOutputs.Add(foundValue);
            }
            else
            {
                int deleteValue = rand.Next(0, presentDataList.Count);
                if (presentDataList.Count == 0)
                {
                    continue;
                }
                bst.Delete(presentDataList[deleteValue]);
                presentDataDic.Remove(presentDataList[deleteValue]);
                presentDataList.RemoveAt(deleteValue);
            }
        }
        presentDataList.Sort();
        List<int> bstInOrder = bst.InOrderTraversal();
        for (int i = 0; i < presentDataList.Count; i++)
        {
            if (bstInOrder.ElementAt(i) != presentDataList[i])
            {
                Console.WriteLine("BST structure is incorrect according to in order traversal.");
                break;
            }
            if (i == presentDataList.Count - 1)
            {
                Console.WriteLine("BST structure is correct.");
            }
        }

        bool working = true;
        for (int i = 0; i < findInputs.Count; i++)
        {
            if (findInputs.ElementAt(i) != findOutputs.ElementAt(i))
            {
                Console.WriteLine($"Incorrect find - expected {findInputs.ElementAt(i)}, got {findOutputs.ElementAt(i)}.");
                working = false;
            }
        }
        if (working)
        {
            Console.WriteLine("All find values were successfully found.");
        }

        int min = bst.FindMin();
        int max = bst.FindMax();
        if (min == presentDataList[0])
        {
            Console.WriteLine($"Min correct - found {min}");
        }
        else
        {
            Console.WriteLine($"Min incorrect - expected {presentDataList[0]}, got {min}");
        }

        if (max == presentDataList[presentDataList.Count - 1])
        {
            Console.WriteLine($"Max correct - found {max}");
        }
        else
        {
            Console.WriteLine($"Max incorrect - expected {presentDataList[presentDataList.Count - 1]}, got {max}");
        }
    }

    public void AVLTestInsertDelete(int size)
    {
        Random rand = new Random();
        AVLTree<int> avl = new AVLTree<int>();
        Dictionary<int, int> presentDataDic = new Dictionary<int, int>();
        List<int> presentDataList = new List<int>();
        int insertValue = rand.Next(0, 20_000_000);

        while (presentDataDic.Count < size)
        {
            while (presentDataDic.ContainsKey(insertValue))
            {
                insertValue = rand.Next(0, 20_000_000);
            }
            presentDataDic[insertValue] = insertValue;
            presentDataList.Add(insertValue);
            avl.Insert(insertValue);
        }

        while (presentDataList.Count > 0)
        {
            int deleteValue = rand.Next(0, presentDataList.Count);
            while (!presentDataDic.ContainsKey(presentDataList[deleteValue]))
            {
                deleteValue = rand.Next(0, presentDataList.Count);
            }
            avl.Delete(presentDataList[deleteValue]);
            presentDataList.RemoveAt(deleteValue);
        }
    }

    public void AVLCrashTest()
    {
        AVLTree<int> avl = new AVLTree<int>();
        int[] insertDataList = { 22, 130, 56, 159, 24, 119, 60, 41, 8, 3,
                                 137, 62, 112, 47, 194, 101, 29, 21, 114, 38};
        int[] delteDataList = { 22, 8, 62, 38, 119, 29, 60, 21, 3};

        for (int i = 0; i < insertDataList.Length; i++)
        {
            Console.WriteLine($"Insert - {insertDataList[i]}");
            avl.Insert(insertDataList[i]);
        }

        for (int i = 0; i < delteDataList.Length; i++)
        {
            Console.WriteLine($"Delete - {delteDataList[i]}");
            avl.Delete(delteDataList[i]);
        }
    }
}
