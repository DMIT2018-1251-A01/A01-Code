<Query Kind="Statements" />

List<int> numbersA = [1,2,3,4];
List<int> numbersB = [3,4,5,6];

numbersA.Union(numbersB).Dump();
numbersA.Intersect(numbersB).Dump();
numbersA.Except(numbersB).Dump();
numbersA.Concat(numbersB).Dump();