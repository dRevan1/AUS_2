using C5;
using System.Diagnostics;
namespace SEM_1.Core;

public class TesterThirdStructure : Tester
{
    public override void InsertPerformanceTest(uint treeSize, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Insert performance test number {i}:\n");
            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();
            List<int> dataList = GetIntList(treeSize);
            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;
            long startTime;

            foreach (int j in dataList)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.Insert(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            foreach (int j in dataList)
            {
                startTime = Stopwatch.GetTimestamp();
                treeSet.Add(j);
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"AVL and TreeSet (C5) insert test for {treeSize} ints");
            Console.WriteLine($"AVL total insert time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total insert time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public override void FindPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find performance test number {i}:\n");
            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();
            List<int> dataList = GetIntList(treeSize);
            List<int> valuesToFind = new();
            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;
            long startTime;

            fillTreeWithData(avl, dataList);
            foreach (int j in dataList)
            {
                treeSet.Add(j);
            }
            for (int j = 0; j < findCount; j++)
            {
                valuesToFind.Add(dataList[random.Next(0, dataList.Count)]);
            }

            foreach (int j in valuesToFind)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.Find(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }
            foreach (int j in valuesToFind)
            {
                startTime = Stopwatch.GetTimestamp();
                treeSet.Contains(j);
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"AVL and TreeSet (C5) find test for {findCount} ints");
            Console.WriteLine($"AVL total find time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total find time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public override void DeletePerformanceTest(uint treeSize, uint deleteCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Delete performance test number {i}:\n");
            List<int> dataList = GetIntList(treeSize);
            int[] dataArray = dataList.ToArray();
            List<int> valuesToDelete = new();

            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();

            
            fillTreeWithData(avl, dataList);
            foreach (int j in dataList)
            {
                treeSet.Add(j);
            }

            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;
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
                avl.Delete(j);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            foreach (int j in valuesToDelete)
            {
                startTime = Stopwatch.GetTimestamp();
                treeSet.Remove(j);
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }
            Console.WriteLine($"AVL and TreeSet (C5) delete test for {deleteCount} ints");
            Console.WriteLine($"AVL total delete time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total delete time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public override void FindMaxPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find max performance test number {i}:\n");
            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();
            List<int> dataList = GetIntList(treeSize);
            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;

            long startTime;
            fillTreeWithData(avl, dataList);
            foreach (int j in dataList)
            {
                treeSet.Add(j);
            }

            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.FindMax();
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                treeSet.FindMax();
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"AVL and TreeSet (C5) find max test for {findCount} calls");
            Console.WriteLine($"AVL total find max time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total find max time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public override void FindMinPerformanceTest(uint treeSize, uint findCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Find min performance test number {i}:\n");
            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();
            List<int> dataList = GetIntList(treeSize);
            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;

            long startTime;
            fillTreeWithData(avl, dataList);

            foreach (int j in dataList)
            {
                treeSet.Add(j);
            }

            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                avl.FindMin();
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }

            for (int j = 0; j < findCount; j++)
            {
                startTime = Stopwatch.GetTimestamp();
                treeSet.FindMin();
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"AVL and TreeSet (C5) find min test for {findCount} calls");
            Console.WriteLine($"AVL total find min time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total find min time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }

    public override void IntervalSearchPerformanceTest(uint treeSize, uint searchCount, uint replics)
    {
        for (int i = 1; i <= replics; i++)
        {
            Random random = new Random();
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"Interval search performance test number {i}:\n");
            AVLTree<int> avl = new AVLTree<int>();
            TreeSet<int> treeSet = new TreeSet<int>();
            List<int> dataList = GetIntList(treeSize);
            List<int> indexList = new();

            TimeSpan totalAvlTime = TimeSpan.Zero;
            TimeSpan totalTreeSetTime = TimeSpan.Zero;
            long startTime;
            fillTreeWithData(avl, dataList);
            foreach (int j in dataList)
            {
                treeSet.Add(j);
            }
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
            int min, max = dataList.Last();

            foreach (int j in indexList)
            {
                min = dataList[j];
                startTime = Stopwatch.GetTimestamp();
                avl.IntervalSearch(min, max);
                totalAvlTime += Stopwatch.GetElapsedTime(startTime);
            }
            foreach (int j in indexList)
            {
                min = dataList[j];
                startTime = Stopwatch.GetTimestamp();
                foreach (var x in treeSet.RangeFromTo(min, max)) 
                {
                   long result = x;
                }
                totalTreeSetTime += Stopwatch.GetElapsedTime(startTime);
            }

            Console.WriteLine($"AVL and TreeSet (C5) interval search test for {searchCount} searches");
            Console.WriteLine($"AVL total interval search time: {totalAvlTime.TotalMilliseconds} ms");
            Console.WriteLine($"TreeSet (C5) total interval search time: {totalTreeSetTime.TotalMilliseconds} ms");
            Console.WriteLine("---------------------------------------------\n");
        }
    }
}
