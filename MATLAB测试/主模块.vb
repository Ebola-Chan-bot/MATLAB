Imports MATLAB
Module 主模块
	Sub Main()
		Dim a As Array(Of Double) = Colon(1, -0.6, -1)
		Console.WriteLine(a)
		Dim b As Array(Of Double) = Rand(2, 2)
		Console.WriteLine(b)
		Dim c As Array(Of Byte) = Ones(Of Byte)(3, 3, 3)
		Console.WriteLine(c)
		Dim e As Array(Of Double) = ArrayFun(Function(f As Object()) CDbl(f(0)) + CDbl(f(1)) + CDbl(f(2)), a, b, c)
		Console.WriteLine(e)
		Console.ReadLine()
	End Sub
End Module
