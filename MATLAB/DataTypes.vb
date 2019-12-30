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
	''' <summary>
	''' 将数组转换为不同的数据类型
	''' </summary>
	''' <typeparam name="newclass">目标类</typeparam>
	''' <param name="A">要转换的数组</param>
	''' <returns>转换后的数组</returns>
	Public Function Cast(Of newclass)(A As Array) As Array(Of newclass)
		Return New Array(Of newclass)(A)
	End Function
	''' <summary>
	''' 将变量转换为不同的数据类型
	''' </summary>
	''' <typeparam name="newclass">目标类</typeparam>
	''' <param name="A">要转换的变量</param>
	''' <returns>转换后的变量</returns>
	Public Function Cast(Of newclass)(A As Object) As newclass
		Return A
	End Function
End Module
