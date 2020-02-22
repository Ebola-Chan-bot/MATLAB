Imports MATLAB
Module 主模块
	Sub Main()
		Dim a As Array(Of MUInt16) = UInt16(Rand(3, 3, 3) * 255)
		Console.WriteLine(a)
		Console.WriteLine(Mean(a, 2))
		Console.WriteLine(Mean(a))
		Console.ReadLine()
	End Sub
End Module
