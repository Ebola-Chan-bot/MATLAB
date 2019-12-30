Imports System.Runtime.CompilerServices
Public Module DataTypes
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <param name="数组">数组</param>
	''' <returns>类</returns>
	<Extension> Public Function [Class](数组 As IEnumerable) As Type
		Dim a As IEnumerator = 数组.GetEnumerator
		a.MoveNext()
		Return a.Current.GetType
	End Function
	Public Function Cast(Of T)(A As Array) As Array(Of T)
		Return New Array(Of T)(A)
	End Function
	Public Function Cast(Of T)(A As Object) As T
		Return A
	End Function
End Module
