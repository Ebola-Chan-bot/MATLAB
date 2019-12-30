Imports MATLAB
Module 主模块
	Sub Main()
		Dim Alpha As New Array(Of Byte)(1)
		Dim Color As Array(Of Byte) = ImRead("D:\OneDrive\文档\病魔少女草莓糖\赛尔号\辛.png", Alpha)
		Console.WriteLine(Alpha)
		Console.ReadLine()
	End Sub
End Module
