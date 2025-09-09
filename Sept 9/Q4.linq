<Query Kind="Program" />

void Main()
{
	int number1 = 15;
	int number2 = 10;
	GetLargestIf(number1, number2).Dump("GetLargestIf");
	GetlargestTernary(number1, number2).Dump("GetlargestTernary");
}

public int GetLargestIf(int x, int y)
{
	if (x > y)
	{
		return x;
	}
	else
		if (x < y)
	{

		return y;
	}
	else
	{
		return 0;
	}
}

public string GetlargestTernary(int x, int y)
{
	return x > y ? "X Value"
				: y > x ? "Y Value"
				: "Same";
}