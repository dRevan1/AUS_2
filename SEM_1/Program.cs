using SEM_1;

Tester tester = new Tester();
TesterThirdStructure avlTreeSetTester = new TesterThirdStructure();
BinarySearchTree<int> bst = new BinarySearchTree<int>();
AVLTree<int> avl = new AVLTree<int>();


avlTreeSetTester.IntervalSearchPerformanceTest(10_000, 1_000_000, 10);