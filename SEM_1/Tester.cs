using System.Diagnostics;

namespace SEM_1;

public class Tester
{
    protected List<int> GetIntList(uint listSize)
    {
        Random rand = new Random();
        Dictionary<int, int> presentDataDic = new Dictionary<int, int>();
        List<int> presentDataList = new List<int>();

        while (presentDataDic.Count < listSize)
        {
            int insertValue = rand.Next(0, 200000000);
            if (!presentDataDic.ContainsKey(insertValue))
            {
                presentDataDic[insertValue] = insertValue;
                presentDataList.Add(insertValue);
            }
        }

        return presentDataList;
    }

    protected void fillTreeWithData(BinarySearchTree<int> bst, List<int> dataList)
    {
        foreach (int data in dataList)
        {
            bst.Insert(data);
        }
    }

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
                int insertValue = rand.Next(0, 200_000_000);
                while (presentDataDic.ContainsKey(insertValue))
                {
                    insertValue = rand.Next(0, 200_000_000);
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


    public virtual void InsertPerformanceTest(uint treeSize, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Insert performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;

            foreach (int j in dataList)
            {
                startTime = Stopwatch.GetTimestamp();
                bst.Insert(j);
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }

            foreach (int j in dataList)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.Insert(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"BST and AVL insert test for {treeSize} ints");
            Console.WriteLine($"BST total insert time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total insert time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }        
    }

    public virtual void FindPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            List<int> valuesToFind = new();
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);
            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;

            for (int j = 0; j < findCount; j++)
            {
                valuesToFind.Add(dataList[random.Next(0, dataList.Count)]);              
            }

            foreach (int j in valuesToFind)
            {
                startTime = Stopwatch.GetTimestamp();
                bst.Find(j);
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }

            foreach (int j in valuesToFind)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.Find(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"BST and AVL find test for {findCount} ints");
            Console.WriteLine($"BST total find time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total find time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public virtual void DeletePerformanceTest(uint treeSize, uint deleteCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Delete performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            int[] dataArray = dataList.ToArray();
            List<int> valuesToDelete = new();

            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();

            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);

            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;

            for (int j = 0; j < deleteCount; j++)
            {
                int index = random.Next(0, dataList.Count);
                while (dataArray[index] == -1)
                {
                    index = random.Next(0, dataList.Count);
                }
                valuesToDelete.Add(dataList[index]);
                dataArray[index] = -1;
            }

            foreach (int j in valuesToDelete)
            {
                startTime = Stopwatch.GetTimestamp();
                bst.Delete(j);
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }

            foreach (int j in valuesToDelete)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.Delete(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }
            Console.WriteLine($"BST and AVL delete test for {deleteCount} ints");
            Console.WriteLine($"BST total delete time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total delete time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public virtual void FindMinPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find min performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);
            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;
            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                bst.FindMin();
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }
            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.FindMin();
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }
            Console.WriteLine($"BST and AVL find min test for {findCount} calls");
            Console.WriteLine($"BST total FindMin time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total FindMin time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public virtual void FindMaxPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find max performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);
            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;
            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                bst.FindMax();
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }
            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.FindMax();
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }
            Console.WriteLine($"BST and AVL find max test for {findCount} calls");
            Console.WriteLine($"BST total FindMax time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total FindMax time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public virtual void IntervalSearchPerformanceTest(uint treeSize, uint searchCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Interval search performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            List<int> indexList = new();
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);
            TimeSpan totalBstTime = TimeSpan.Zero;
            TimeSpan totalAvlTime = TimeSpan.Zero;
            long startTime;
            dataList.Sort();

            for (int j = 0; j < searchCount; j++)
            {
                int minIndex = random.Next(0, dataList.Count);
                while (minIndex > dataList.Count - 501)
                {
                    minIndex = random.Next(0, dataList.Count);
                }
                indexList.Add(minIndex);
            }
            int max = dataList.Last();

            foreach (int j in indexList)
            {
                int min = dataList[j];
                startTime = Stopwatch.GetTimestamp();
                bst.IntervalSearch(min, max);
                totalBstTime += Stopwatch.GetElapsedTime(startTime);
            }
            foreach (int j in indexList)
            {
                int min = dataList[j];
                startTime = Stopwatch.GetTimestamp();
                avl.IntervalSearch(min, max);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"BST and AVL interval search test for {searchCount} searches");
            Console.WriteLine($"BST total interval search time: {totalBstTime.TotalMilliseconds} ms");
            Console.WriteLine($"AVL total interval search time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public void IntervalSearchTest(uint treeSize, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            List<int> dataList = GetIntList(treeSize);
            BinarySearchTree<int> bst = new BinarySearchTree<int>();
            AVLTree<int> avl = new AVLTree<int>();
            int min, max, minIndex;
            fillTreeWithData(bst, dataList);
            fillTreeWithData(avl, dataList);

            dataList.Sort();
            minIndex = random.Next(0, dataList.Count);
            max = dataList.Last();
            while (minIndex > dataList.Count - 501)
            {
                min = random.Next(0, dataList.Count);
            }
            min = dataList[minIndex];

            List<int> bstResult = bst.IntervalSearch(min, max);
            List<int> avlResult = avl.IntervalSearch(min, max);

            for (int j = 0; j < bstResult.Count; j++)
            {
                if (bstResult.ElementAt(j) != dataList[minIndex + j])
                {
                    Console.WriteLine("Interval search BST not correct.");
                    Console.WriteLine($"BST at {j} data at {minIndex + j}");
                    return;
                }
                if (avlResult.ElementAt(j) != dataList[minIndex + j])
                {
                    Console.WriteLine("Interval search AVL not correct.");
                    Console.WriteLine($"AVL at {j} data at {minIndex + j}");
                    return;
                }
            }
            Console.WriteLine($"Interval search test number {i} passed.");
        }
    }

    public void AVLTreeBalanceTest(uint treeSize, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"AVL tree balance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            AVLTree<int> avl = new AVLTree<int>();
            fillTreeWithData(avl, dataList);
            avl.InOrderBalanceCheck();
            Console.WriteLine("---------------------------------------------\n");
        }
    }
}