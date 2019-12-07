Imports System.Runtime.CompilerServices
''' <summary>
''' 类型规范：数值一律Long，长度一律UInteger，维度一律Byte
''' </summary>
Public Module Elmat
	Public Delegate Function Funbsx(A, B)
#Region "Private"
	Private Sub Reshape递归(原数组 As IEnumerator, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 新索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				新索引(当前维度) = a
				Reshape递归(原数组, 维度数, 各维长度, 新数组, 当前维度 + 1, 新索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				新索引(当前维度) = a
				原数组.MoveNext()
				新数组.SetValue(原数组.Current, 新索引)
			Next
		End If
	End Sub
	Private Sub Permute递归(原数组 As Array, 维度数 As Byte, 维度映射 As Byte(), 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 原索引 As Integer(), 新索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				原索引(维度映射(当前维度)) = a
				新索引(当前维度) = a
				Permute递归(原数组, 维度数, 维度映射, 各维长度, 新数组, 当前维度 + 1, 原索引, 新索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				原索引(维度映射(当前维度)) = a
				新索引(当前维度) = a
				新数组.SetValue(原数组.GetValue(原索引), 新索引)
			Next
		End If
	End Sub
	Private Sub Bsxfun递归(函数 As Funbsx, 数组1 As Array, 数组2 As Array, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				Bsxfun递归(函数, 数组1, 数组2, 维度数, 各维长度, 新数组, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				新数组.SetValue(函数.Invoke(数组1.GetValue(当前索引), 数组2.GetValue(当前索引)), 当前索引)
			Next
		End If
	End Sub
#End Region
#Region "Public"
	''' <summary>
	''' 创建全零数组
	''' </summary>
	''' <typeparam name="T">要创建的数据类型（类）</typeparam>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零数组</returns>
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array
		Return Array.CreateInstance(GetType(T), sz)
	End Function
	''' <summary>
	''' 创建全零向量
	''' </summary>
	''' <typeparam name="T">要创建的数据类型（类）</typeparam>
	''' <param name="sz">向量长度</param>
	''' <returns>全零向量</returns>
	Public Function Zeros(Of T)(sz As Integer) As T()
		Dim a(sz - 1) As T
		Return a
	End Function
	''' <summary>
	''' 对两个数组应用按元素运算（启用隐式扩展）
	''' </summary>
	''' <param name="fun">要应用的二元函数</param>
	''' <param name="A">输入数组</param>
	''' <param name="B">输入数组</param>
	''' <returns>返回数组</returns>
	Public Function Bsxfun(fun As Funbsx, A As Array, B As Array) As Array
		Dim c As Integer() = 适配(A, B), d As Array = Array.CreateInstance(A.Class, c)
		Bsxfun递归(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' 重构数组
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
		Dim d As Array = Array.CreateInstance(A.Class, Lengths)
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
		Dim d As Array = Array.CreateInstance(A.Class, b)
		Permute递归(A, e, dimorder, b, d, 0, Zeros(Of Integer)(e), Zeros(Of Integer)(e))
		Return d
	End Function
#End Region
End Module