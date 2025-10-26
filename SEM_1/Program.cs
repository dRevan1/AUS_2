using SEM_1;

Tester tester = new Tester();
TesterThirdStructure avlTreeSetTester = new TesterThirdStructure();
BinarySearchTree<int> bst = new BinarySearchTree<int>();
AVLTree<int> avl = new AVLTree<int>();

tester.BSTBaseTest(10000, avl);
