Imports System.Runtime.CompilerServices
''' <summary>
''' 类型规范：数值一律Long，长度一律UInteger，维度一律Byte
''' </summary>
Public Module Elmat
#Region "Private"
	Private Sub Reshape递归(原数组 As IEnumerator, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 新索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a = 0 To 各维长度(当前维度) - 1
				新索引(当前维度) = a
				Reshape递归(原数组, 维度数, 各维长度, 新数组, 当前维度 + 1, 新索引)
			Next
		Else
			For a = 0 To 各维长度(当前维度) - 1
				新索引(当前维度) = a
				原数组.MoveNext()
				新数组.SetValue(原数组.Current, 新索引)
			Next
		End If
	End Sub
	Private Sub Permute递归(原数组 As Array, 维度数 As Byte, 维度映射 As Byte(), 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 原索引 As Integer(), 新索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a = 0 To 各维长度(当前维度) - 1
				原索引(维度映射(当前维度)) = a
				新索引(当前维度) = a
				Permute递归(原数组, 维度数, 维度映射, 各维长度, 新数组, 当前维度 + 1, 原索引, 新索引)
			Next
		Else
			For a = 0 To 各维长度(当前维度) - 1
				原索引(维度映射(当前维度)) = a
				新索引(当前维度) = a
				新数组.SetValue(原数组.GetValue(原索引), 新索引)
			Next
		End If
	End Sub
	<Extension> Private Function ElementType(数组 As Array) As Type
		Return 数组.GetValue(Zeros(Of Integer)(数组.Rank)).GetType
	End Function
#End Region
#Region "Public"
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array
		Return Array.CreateInstance(GetType(T), sz)
	End Function
	Public Function Zeros(Of T)(sz As Integer) As T()
		Dim a(sz - 1) As T
		Return a
	End Function
	''' <summary>
	''' 重构数组，与MATLAB用法相同
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度</param>
	''' <returns>重构的数组</returns>
	<Extension> Public Function Reshape(A As Array, ParamArray sz As UInteger?()) As Array
		Dim f As Byte = sz.Length, b As UInteger = 1, c As UInteger? = Nothing, Lengths(f - 1) As Integer
		For e As Byte = 0 To f - 1
			If sz(e) Is Nothing Then
				c = e
			Else
				b *= sz(e)
				Lengths(e) = sz(e)
			End If
		Next
		If c IsNot Nothing Then
			Lengths(c) = A.Length / b
		End If
		Dim d As Array = Array.CreateInstance(A.ElementType, Lengths)
		Reshape递归(A.GetEnumerator, f, Lengths, d, 0, Zeros(Of Integer)(f))
		Return d
	End Function
	''' <summary>
	''' 置换数组维度，维度顺序与MATLAB略有不同
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <param name="dimorder">维度顺序。不同于MATLAB，从0开始</param>
	''' <returns>置换维度的数组</returns>
	<Extension> Public Function Permute(A As Array, ParamArray dimorder As Byte()) As Array
		Dim e As Byte = dimorder.Length, b(e - 1) As Integer
		For c As Byte = 0 To e - 1
			b(c) = A.GetLength(dimorder(c))
		Next
		Dim d As Array = Array.CreateInstance(A.ElementType, b)
		Permute递归(A, e, dimorder, b, d, 0, Zeros(Of Integer)(e), Zeros(Of Integer)(e))
		Return d
	End Function
#End Region
End Module