Imports MATLAB
Module 主模块
	Sub Main()
		Dim a As Array(Of Double) = Rand(4, 5)
		Dim e As Array(Of Byte) = ArrayFun(Function(d As Double) As Byte
											   Return d + 1
										   End Function, a)
		Console.WriteLine(a)
		Console.WriteLine(e)
		Console.ReadLine()
	End Sub
End Module
