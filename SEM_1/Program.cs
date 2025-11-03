using SEM_1;

Tester tester = new Tester();
TesterThirdStructure avlTreeSetTester = new TesterThirdStructure();
BinarySearchTree<int> bst = new BinarySearchTree<int>();
AVLTree<int> avl = new AVLTree<int>();

PCRTestDatabase app = new PCRTestDatabase();
//app.PopulateDatabase(20, 100, 20, 20);
app.ImportDatabase();
app.ExportDatabase();