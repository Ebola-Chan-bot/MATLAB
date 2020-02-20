# MATLAB
这个项目试图将经典常用的MATLAB函数在.Net中实现。源码在MATLAB目录中，MATLAB测试是一个用于测试功能的控制台应用。将发布在NuGet。
## 在.Net环境中使用强大的MATLAB函数！
```VB
'MATLAB经典的rand函数，数组和常数直接相乘，转换uint64！
Dim b As Array(Of MUInt64) = UInt64(Rand(3, 3, 3) * 255)
Console.WriteLine("原数组：")
'数组直接输出全部元素！
Console.WriteLine(b)
Console.WriteLine("线性索引10号元素，对应三维下标(1,0,1)：")
'明明是多维数组却支持线性索引！
Console.WriteLine(b(10))
Console.WriteLine("三维下标(0,1,2)：")
'当然也支持多维索引！
Console.WriteLine(b({0, 1, 2}))
Console.WriteLine("切取3×3×3立方体的中间一层，并多次输出其中某些行列：")
'强大的数组切片操作！
Console.WriteLine(b({({0, 0, 1, 1, 2, 2}), ({0, 0, 2, 2, 1, 2}), ({1})}))
Console.WriteLine("使用冒号表达式的索引：0:end, end:-1:0,end:end")
'MATLAB冒号表达式，支持end参数！
Console.WriteLine(b({Colon(0, [End]), Colon([End], -1, 0), Colon([End], [End])}))
'数组之间直接做乘法、加法，尺寸不相符的数组自动补齐！
Dim a As Array(Of MUInt64) = UInt64(Rand(2, 2, 1) * 127) + UInt64(Rand(3, 2, 3) * 127)
Console.WriteLine("对切片随机赋值：")
Console.WriteLine(a)
'数组级赋值！将小数组赋值给大数组的一部分切片！
b({Colon([End], -1, 0), Colon(0, 2, [End]), Colon(0, 2)}) = a
Console.WriteLine("随机赋值以后：")
Console.WriteLine(b)
```

详细文档：<https://github.com/Silver-Fang/MATLAB/blob/master/MATLAB/%E6%96%87%E6%A1%A3.md>
## NuGet说明
MATLAB一些数组操作的.Net实现，详见项目URL。 目前已实现的MATLAB函数：class zeros ones bsxfun reshape permute size colon plus minus cast imread rand arrayfun cat numel times rdivide eq ne gt lt max min decimal double single uint8 uint16 uint32 uint64 int8 int16 int32 int64。 此外将System.Array类增强为Array(Of T)，各种基本数据类型也结构化以实现统一的接口，以支持数组之间直接进行运算符操作，以及自定义类运算的数组化支持。详细文档参见项目URL：https://github.com/Silver-Fang/MATLAB/blob/master/MATLAB/文档.md

欢迎在项目URL提交Issues。

新版本不保证兼容旧版本，请谨慎升级。
## 1.0.1

功能改进：Array(Of T), class, reshape, bsxfun, size, colon

新增函数：cast, imread

## 1.1.0

功能改进：Array(Of T), cast, bsxfun, reshape, permute, size, imread, plus, minus

新增函数：rand, arrayfun, cat, numel, times, rdivide

移除函数：subsref, subsasgn

## 1.1.1

功能改进：Array(Of T) class colon

新增函数：eq ne gt lt

## 1.1.2

对Array(Of T)的底层实现进行了重大改进，各函数也因此有较大改动。另外特别撰写了对所有公开函数的详细文档，帮助没有MATLAB使用经验的开发者。

## 1.2.0

新增模块：ColonExpression DataFun IArray INumeric MDecimal MDouble MSingle MUInt8 MUInt16 MUInt32 MUInt64 MInt8 MInt16 MInt32 MInt64

新增函数：max min decimal double single uint8 uint16 uint32 uint64 int8 int16 int32 int64

所有模块所有函数都有重大改动。为了支持用户自己编写类实现数组运算重载，数组运算现在仅支持实现INumeric结构/类，.Net数据类型可以轻松转换为INumeric结构系列进行运算，自定义类只要实现INumeric就可以支持数组化运算。详见文档。