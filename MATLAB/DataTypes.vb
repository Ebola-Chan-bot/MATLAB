Imports System.Runtime.CompilerServices
Public Module DataTypes
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <param name="数组">数组</param>
	''' <returns>类</returns>
	<Extension> Public Function [Class](数组 As Array) As Type
		Return 数组.GetValue(Zeros(Of Integer)(数组.Rank).Cast(Of Integer).ToArray).GetType
	End Function
End Module
