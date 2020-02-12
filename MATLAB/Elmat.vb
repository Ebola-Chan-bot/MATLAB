Public Module ElMat
	Public Delegate Function Funbsx(A, B)
	''' <summary>
	''' 这里不会创建新数组，因此可以用底层的<see cref="Array"/>
	''' </summary>
	Private Sub 适配递归(Of T1, T2)(原数组1 As Array(Of T1), 原数组2 As Array(Of T2), 各维长度 As Integer(), 总维数 As Byte, 新数组1 As Array, 新数组2 As Array, 当前索引 As Integer(), 当前维数 As Byte, 原索引1 As Integer(), 原索引2 As Integer())
		Dim b As Integer = 原数组1.Size(当前维数), c As Integer = 原数组2.Size(当前维数)
		If 当前维数 < 总维数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.NDims Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.NDims Then 原索引2(当前维数) = a Mod c
				适配递归(原数组1, 原数组2, 各维长度, 总维数, 新数组1, 新数组2, 当前索引, 当前维数 + 1, 原索引1, 原索引2)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.NDims Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.NDims Then 原索引2(当前维数) = a Mod c
				新数组1.SetValue(原数组1(原索引1), 当前索引)
				新数组2.SetValue(原数组2(原索引2), 当前索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 判断两个数组尺寸是否匹配
	''' </summary>
	Private Function 匹配(Of T1, T2)(数组1 As Array(Of T1), 数组2 As Array(Of T2)) As Boolean
		Dim a As Byte = 数组1.NDims, b As Byte = 数组2.NDims
		If a = b Then
			Dim d As Integer() = 数组1.Size, e As Integer() = 数组2.Size
			For c As Byte = 0 To a - 1
				If d(c) <> e(c) Then
					Return False
				End If
			Next
			Return True
		Else
			Return False
		End If
	End Function
	''' <summary>
	''' 由于适配一定会创建新数组，即使用户输入的是<see cref="Array"/>也一律改成<see cref="Array(Of T)"/>
	''' </summary>
	Friend Function 适配(Of T1, T2)(ByRef 数组1 As Array(Of T1), ByRef 数组2 As Array(Of T2)) As Integer()
		If 匹配(数组1, 数组2) Then Return 数组1.Size
		Dim b As Byte = Math.Max(数组1.NDims, 数组2.NDims), c(b - 1) As Integer, h As Integer, i As Integer
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
		Dim d As New Array(Of T1)(c), e As New Array(Of T2)(c)
		适配递归(数组1, 数组2, c, b, d, e, Zeros(Of Integer)(b), 0, Zeros(Of Integer)(数组1.NDims), Zeros(Of Integer)(数组2.NDims))
		数组1 = d
		数组2 = e
		Return c
	End Function
	''' <summary>
	''' 这里带类型参数是委托的要求
	''' </summary>
	Friend Sub BsxFun递归(函数 As Funbsx, 数组1 As Array, 数组2 As Array, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				BsxFun递归(函数, 数组1, 数组2, 维度数, 各维长度, 新数组, 当前维度 + 1, 当前索引)
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
	''' <summary>
	''' 这里不创建新数组，故用底层<see cref="Array"/>
	''' </summary>
	Private Sub Cat递归(Of T)(源数组 As Array(Of T), 目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer(), 目标维度 As Byte, 目标维度累积数 As Integer)
		If 当前维度 < 目标数组.Rank - 1 Then
			If 当前维度 = 目标维度 Then
				For a As Integer = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = 目标维度累积数 + a
					Cat递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引, 目标维度, 目标维度累积数)
				Next
			Else
				For a As Integer = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = a
					Cat递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引, 目标维度, 目标维度累积数)
				Next
			End If
		Else
			If 当前维度 = 目标维度 Then
				For a As Integer = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = 目标维度累积数 + a
					目标数组.SetValue(源数组(源索引), 目标索引)
				Next
			Else
				For a As Integer = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = a
					目标数组.SetValue(源数组(源索引), 目标索引)
				Next
			End If
		End If
	End Sub
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
	''' 创建全零<see cref="Double"/>数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零<see cref="Double"/>数组</returns>
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
	''' 创建全部为 1 的<see cref="Double"/>数组。不同于MATLAB，如果只有一个大小参数，将返回第1维向量，而不是正方矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全1<see cref="Double"/>数组</returns>
	Public Function Ones(ParamArray sz As Integer()) As Array(Of Double)
		Return Ones(Of Double)(sz)
	End Function
	''' <summary>
	''' 对两个数组应用按元素运算（启用隐式扩展）。不同于MATLAB，这里的隐式扩展更加健壮，采用了循环填充方式，使得允许<c>Ones(2,2)+Ones(4,4)=Ones(4,4)+Ones(4,4)</c>
	''' </summary>
	''' <typeparam name="T">输出元素类型</typeparam>
	''' <param name="fun">要应用的二元函数</param>
	''' <param name="A">输入数组</param>
	''' <param name="B">输入数组</param>
	''' <returns>返回数组</returns>
	Public Function BsxFun(Of TIn1, TIn2, TOut)(fun As Funbsx, A As Array(Of TIn1), B As Array(Of TIn2)) As Array(Of TOut)
		Dim c As Integer() = 适配(A, B)
		Dim d As New Array(Of TOut)(c)
		BsxFun递归(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' 重构数组
	''' </summary>
	''' <typeparam name="T">元素类型</typeparam>
	''' <param name="A">输入数组，必须具有确定的长度</param>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度</param>
	''' <returns>重构的数组</returns>
	Public Function Reshape(Of T)(A As Array(Of T), ParamArray sz As UInteger?()) As Array(Of T)
		Return A.Reshape(sz)
	End Function
	''' <summary>
	''' 置换数组维度，维度顺序从0开始。
	''' </summary>
	''' <typeparam name="T">元素类型，缺省<see cref="Object"/></typeparam>
	''' <param name="A">输入数组</param>
	''' <param name="dimorder">维度顺序。不同于MATLAB，从0开始</param>
	''' <returns>置换维度的数组</returns>
	Public Function Permute(Of T)(A As Array(Of T), ParamArray dimorder As Byte()) As Array(Of T)
		Return A.Permute(dimorder)
	End Function
#Region "Size"
	''' <summary>
	''' 数组大小。末尾的单一维度会忽略。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>数组各维尺寸构成的数组</returns>
	Public Function Size(Of T)(A As Array(Of T)) As Integer()
		Return A.Size
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As Integer
		Return A.Size([dim])
	End Function
#End Region
	''' <summary>
	''' 数组的维度数目，忽略较高的单一维度。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>维度数</returns>
	Public Function NDims(Of T)(A As Array(Of T)) As Byte
		Return A.NDims
	End Function
	''' <summary>
	''' 串联数组。所有输入在串联维度以外的维度必须匹配，此函数不会检查是否匹配，不匹配的数组串联可能会报错或产生未知结果。
	''' </summary>
	''' <typeparam name="T">元素类型，缺省<see cref="Object"/></typeparam>
	''' <param name="[dim]">沿其运算的维度</param>
	''' <param name="A">输入列表</param>
	''' <returns>串联的数组</returns>
	Public Function Cat(Of T)([dim] As Byte, ParamArray A As Array(Of T)()) As Array(Of T)
		Dim b As Array(Of Integer) = A(0).Size, c As Integer, d As Array(Of T)
		For Each d In A
			c += d.Size([dim])
		Next
		b([dim]) = c
		c = b.Numel - 1
		For e As Byte = 0 To c
			If b(e) = 0 Then b(e) = 1
		Next
		Dim g As New Array(Of T)(CType(b, Integer()))
		For e As Byte = 0 To c
			b(e) = 0
		Next
		b([dim]) = -1
		Dim f(c) As Integer
		For Each d In A
			Cat递归(d, g, 0, f, b, [dim], b([dim]) + 1)
		Next
		Return g
	End Function
	''' <summary>
	''' 返回数组 A 中的元素数目 n 等同于 prod(size(A))。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>元素数目</returns>
	Function Numel(Of T)(A As Array(Of T)) As Integer
		Return A.Numel
	End Function
#End Region
End Module