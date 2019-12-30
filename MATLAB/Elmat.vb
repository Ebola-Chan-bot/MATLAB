Imports System.Runtime.CompilerServices
Public Module Elmat
	Public Delegate Function Funbsx(Of Out T)(A, B) As T
	Private Sub 适配递归(原数组1 As Array, 原数组2 As Array, 各维长度 As Integer(), 总维数 As Byte, 新数组1 As Array, 新数组2 As Array, 当前索引 As Integer(), 当前维数 As Byte, 原索引1 As Integer(), 原索引2 As Integer())
		Dim b As Integer = 原数组1.Size(当前维数), c As Integer = 原数组2.Size(当前维数)
		If 当前维数 < 总维数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.Rank Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.Rank Then 原索引2(当前维数) = a Mod c
				适配递归(原数组1, 原数组2, 各维长度, 总维数, 新数组1, 新数组2, 当前索引, 当前维数 + 1, 原索引1, 原索引2)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.Rank Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.Rank Then 原索引2(当前维数) = a Mod c
				新数组1.SetValue(原数组1.GetValue(原索引1), 当前索引)
				新数组2.SetValue(原数组2.GetValue(原索引2), 当前索引)
			Next
		End If
	End Sub
	Private Function 适配(ByRef 数组1 As Array, ByRef 数组2 As Array) As Integer()
		Dim b As Byte = Math.Max(数组1.Rank, 数组2.Rank), c(b - 1) As Integer, h As Integer, i As Integer
		For a As Byte = 0 To b - 1
			h = 数组1.Size(a)
			i = 数组2.Size(a)
			If h = i Then
				c(a) = h
			ElseIf h < i Then
				c(a) = i
			Else
				c(a) = h
			End If
		Next
		Dim d As Array = Array.CreateInstance(数组1.Class, c), e As Array = Array.CreateInstance(数组2.Class, c)
		适配递归(数组1, 数组2, c, b, d, e, Zeros(Of Integer)(b), 0, Zeros(Of Integer)(数组1.Rank), Zeros(Of Integer)(数组2.Rank))
		数组1 = d
		数组2 = e
		Return c
	End Function
#Region "Private"
	Private Sub Reshape递归(原数组 As IEnumerator, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 新索引 As Integer())
		If 当前维度 > 0 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				新索引(当前维度) = a
				Reshape递归(原数组, 维度数, 各维长度, 新数组, 当前维度 - 1, 新索引)
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
	Private Sub Bsxfun递归(Of T)(函数 As Funbsx(Of T), 数组1 As Array, 数组2 As Array, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array(Of T), 当前维度 As Byte, 当前索引 As Integer())
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
	Private Sub Ones递归(Of T)(数组 As Array, 类型1 As T, 维度数 As Byte, 各维长度 As Integer(), 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				Ones递归(数组, 类型1, 维度数, 各维长度, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				数组.SetValue(类型1, 当前索引)
			Next
		End If
	End Sub
#End Region
#Region "Public"
	''' <summary>
	''' 创建全零数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <typeparam name="T">要创建的数据类型（类）</typeparam>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零数组</returns>
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Return New Array(Of T)(sz)
	End Function
	''' <summary>
	''' 创建全零Double数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零Double数组</returns>
	Public Function Zeros(ParamArray sz As Integer()) As Array(Of Double)
		Return Zeros(Of Double)(sz)
	End Function
	''' <summary>
	''' 创建全部为 1 的数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <typeparam name="T">输出类</typeparam>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全1数组</returns>
	Public Function Ones(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Dim a As New Array(Of T)(sz)
		Ones递归(a, CType(DirectCast(1, Object), T), sz.Length, sz, 0, Zeros(Of Integer)(sz.Length))
		Return a
	End Function
	''' <summary>
	''' 创建全部为 1 的Double数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全1Double数组</returns>
	Public Function Ones(ParamArray sz As Integer()) As Array(Of Double)
		Return Ones(Of Double)(sz)
	End Function
	''' <summary>
	''' 对两个数组应用按元素运算（启用隐式扩展）。不同于MATLAB，这里的隐式扩展更加健壮，采用了循环填充方式，使得允许Ones(2,2)+Ones(4,4)=Ones(4,4)+Ones(4,4)
	''' </summary>
	''' <param name="fun">要应用的二元函数</param>
	''' <param name="A">输入数组</param>
	''' <param name="B">输入数组</param>
	''' <returns>返回数组</returns>
	Public Function Bsxfun(Of T)(fun As Funbsx(Of T), A As Array, B As Array) As Array(Of T)
		Dim c As Integer() = 适配(A, B)
		Dim d As New Array(Of T)(c)
		Bsxfun递归(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' 重构数组。
	''' </summary>
	''' <param name="A">输入数组，必须具有确定的长度</param>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度</param>
	''' <returns>重构的数组</returns>
	<Extension> Public Function Reshape(A As ICollection, ParamArray sz As UInteger?()) As Array
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
			Lengths(c) = A.Count / b
		End If
		Dim d As Array = Array.CreateInstance(A.Class, Lengths)
		Reshape递归(A.GetEnumerator, f, Lengths, d, f - 1, Zeros(Of Integer)(f))
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
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <returns>数组各维尺寸构成的数组</returns>
	<Extension> Public Function Size(A As Array) As Integer()
		Dim d As Byte = A.Rank
		If d = 1 Then
			Return {A.Length}
		Else
			Dim c(d - 1) As Integer
			For b As Byte = 0 To d - 1
				c(b) = A.GetLength(b)
			Next
			Return c
		End If
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	<Extension> Public Function Size(A As Array, [dim] As Byte) As Integer
		If [dim] < A.Rank Then
			Return A.GetLength([dim])
		Else
			Return 1
		End If
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <returns>数组各维尺寸构成的数组</returns>
	Public Function Size(Of T)(A As Array(Of T)) As Integer()
		Return A.Size
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="A">输入数组</param>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As Integer
		Return A.Size([dim])
	End Function
#End Region
End Module