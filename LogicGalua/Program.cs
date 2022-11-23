// See https://aka.ms/new-console-template for more information

const int m = 3;
// порождающий полином галуа для m=3 - x3+x+1
const int p = 11; // x3+x+1, делитель всегда порождающий полином
const int a = 7;
const int b = 5;

int[] ap = new int[m]; // Массив значений полинома для переменной a, где каждый элмент - x^i, где i от 0 до m
int[] bp = new int[m];

// Разбиваем въожные данные на полином
for (int i=0; i<m; i++) {
    int t = (int)Math.Pow(2, i);
    ap[i] = a & t ; // a[i] = x^i
    bp[i] = b & t; // b[i] = x^i
}

// умножение полиномов не в поле галуа
int sum = 0;
for (int i =0; i< m; i++)
{
    for (int j = 0; j < m; j++)
    {
        sum ^= ap[i] * bp[j]; // ксорим перемноженные иксы между собой
    }
}

Console.WriteLine(sum);


// Деление в поле галуа
var ost = sum; // принимаем делимое за первый остаток
while (ost > p) // Если остаток меньше делителя, то деление окончено
{
    var firstNum = ost/p; // берем первую цифру остатка
    var newOst = firstNum * p; // вычисляем новый остаток = умножение перврой цифры на порождающий полином (делитель)
    ost = ost ^ newOst; // ксорим прошлый остаток с получившимя остатком
}

// Остаток - результат деления.
Console.WriteLine($"{ost}");

//Console.WriteLine(27 ^ 22);

//Console.WriteLine(13 ^ 11);
