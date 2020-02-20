Imports MATLAB
Module 主模块
	Sub Main()
		Dim a As Array(Of MUInt64) = Ones(Of MUInt64)(2, 2)
		Console.WriteLine(a)
		Dim b As Array(Of MUInt64) = Ones(Of MUInt64)(3, 3)
		Console.WriteLine(b)
		Console.WriteLine(a = b)
		Console.WriteLine(a = 1)
		Console.ReadLine()
	End Sub
End Module
