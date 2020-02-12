Imports MATLAB
Module 主模块
	Sub Main()
		Dim c As New Array(Of Byte)({1})
		Dim a As Array(Of Byte) = Zeros(Of Byte)(3, 3)
		Dim b As Array(Of Decimal) = Ones(Of Decimal)(3, 3)
		Console.WriteLine(1 > a)
		a({4, 12, 1, 0}) = 1
		Console.WriteLine(a)
		Console.ReadLine()
	End Sub
End Module
