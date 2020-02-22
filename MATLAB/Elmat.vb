Public Module ElMat
	''' <summary>
	''' 这里不创建新数组，故用底层<see cref="Array"/>
	''' </summary>
	Private Sub Cat递归(Of T)(源数组 As Array(Of T), 目标数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 目标维度 As Byte, 目标维度累积数 As UInteger)
		If 当前维度 < 目标数组.NDims - 1 Then
			If 当前维度 = 目标维度 Then
				For a As UInteger = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = 目标维度累积数 + a
					Cat递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引, 目标维度, 目标维度累积数)
				Next
			Else
				For a As UInteger = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = a
					Cat递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引, 目标维度, 目标维度累积数)
				Next
			End If
		Else
			If 当前维度 = 目标维度 Then
				If 当前维度 < 源索引.Length Then
					For a As UInteger = 0 To 源数组.Size(当前维度) - 1
						源索引(当前维度) = a
						目标索引(当前维度) = 目标维度累积数 + a
						目标数组.SetValue(源数组.GetValue(源索引), 目标索引)
					Next
				Else
					For a As UInteger = 0 To 源数组.Size(当前维度) - 1
						目标索引(当前维度) = 目标维度累积数 + a
						目标数组.SetValue(源数组.GetValue(源索引), 目标索引)
					Next
				End If
			Else
				For a As UInteger = 0 To 源数组.Size(当前维度) - 1
					源索引(当前维度) = a
					目标索引(当前维度) = a
					目标数组.SetValue(源数组.GetValue(源索引), 目标索引)
				Next
			End If
		End If
	End Sub
	''' <summary>
	''' 返回由零组成并且数据类型为<c>typename</c>的 sz1×...×szN 数组，其中 sz1,...,szN 指示每个维度的大小。例如，<c>Zeros(2, 3)</c>将返回一个 2×3 矩阵。
	''' </summary>
	''' <typeparam name="typename">要创建的数据类型（类）</typeparam>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零数组</returns>
	Public Function Zeros(Of typename As INumeric)(ParamArray sz As UInteger()) As Array(Of typename)
		Return New Array(Of typename)(sz)
	End Function
	''' <summary>
	''' 返回由零组成并且数据类型为<see cref="MDouble"/>的 sz1×...×szN 数组，其中 sz1,...,szN 指示每个维度的大小。例如，<c>Zeros(2, 3)</c>将返回一个 2×3 矩阵。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全零<see cref="MDouble"/>数组</returns>
	Public Function Zeros(ParamArray sz As UInteger()) As Array(Of MDouble)
		Return Zeros(Of MDouble)(sz)
	End Function
	''' <summary>
	''' 返回一个由 1 组成并且数据类型为 classname 的 sz1×...×szN 数组。
	''' </summary>
	''' <typeparam name="classname">输出类</typeparam>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全1数组</returns>
	Public Function Ones(Of classname As {INumeric, New})(ParamArray sz As UInteger()) As Array(Of classname)
		Ones = New Array(Of classname)(sz)
		Dim a As New classname
		a.SetValue(1)
		Ones.本体 = Enumerable.Repeat(a, Ones.Numel).ToArray
	End Function
	''' <summary>
	''' 返回由 1 组成的 sz1×...×szN <see cref="MDouble"/>数组，其中 sz1,...,szN 指示每个维度的大小。例如，<c>Ones(2, 3)</c>返回由 1 组成的 2×3 数组。
	''' </summary>
	''' <param name="sz">每个维度的大小</param>
	''' <returns>全1<see cref="MDouble"/>数组</returns>
	Public Function Ones(ParamArray sz As UInteger()) As Array(Of MDouble)
		Return Ones(Of MDouble)(sz)
	End Function
	''' <summary>
	''' 这里不会创建新数组，因此可以用底层的<see cref="Array"/>
	''' </summary>
	Private Sub 适配递归(Of T1, T2)(原数组1 As Array(Of T1), 原数组2 As Array(Of T2), 各维长度 As UInteger(), 总维数 As Byte, 新数组1 As Array(Of T1), 新数组2 As Array(Of T2), 当前索引 As UInteger(), 当前维数 As Byte, 原索引1 As UInteger(), 原索引2 As UInteger())
		Dim b As UInteger = 原数组1.Size(当前维数), c As UInteger = 原数组2.Size(当前维数)
		If 当前维数 < 总维数 - 1 Then
			For a As UInteger = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.NDims Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.NDims Then 原索引2(当前维数) = a Mod c
				适配递归(原数组1, 原数组2, 各维长度, 总维数, 新数组1, 新数组2, 当前索引, 当前维数 + 1, 原索引1, 原索引2)
			Next
		Else
			For a As UInteger = 0 To 各维长度(当前维数) - 1
				当前索引(当前维数) = a
				If 当前维数 < 原数组1.NDims Then 原索引1(当前维数) = a Mod b
				If 当前维数 < 原数组2.NDims Then 原索引2(当前维数) = a Mod c
				新数组1.SetValue(原数组1.GetValue(原索引1), 当前索引)
				新数组2.SetValue(原数组2.GetValue(原索引2), 当前索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 对两个数组应用按元素运算（启用隐式扩展）。不同于MATLAB，这里的隐式扩展更加健壮，采用了循环填充方式，使得允许<c>Ones(2, 2) + Ones(4, 4) = Ones(4, 4) + Ones(4, 4)</c>
	''' </summary>
	Public Function BsxFun(Of TIn1, TIn2, TOut)(fun As Func(Of TIn1, TIn2, TOut), A As Array(Of TIn1), B As Array(Of TIn2)) As Array(Of TOut)
		If A.Numel = 1 Then
			Dim c As TIn1 = A.本体(0)
			Return New Array(Of TOut)(B.本体.Select(Function(d As TIn2) fun.Invoke(c, d)).ToArray, B.Size)
		ElseIf B.Numel = 1 Then
			Dim d As TIn2 = B.本体(0)
			Return New Array(Of TOut)(A.本体.Select(Function(c As TIn1) fun.Invoke(c, d)).ToArray, A.Size)
		Else
			Dim c As IArray() = 适配(A, B)
			A = c(0)
			B = c(1)
			Return New Array(Of TOut)(A.本体.AsParallel.AsOrdered.Zip(B.本体.AsParallel.AsOrdered, fun).ToArray, A.Size.ToArray)
		End If
	End Function
	''' <summary>
	''' 将 A 重构为一个 sz1×...×szN 数组，其中 sz1,...,szN 指示每个维度的大小。可以指定 Nothing 的单个维度大小，以便自动计算维度大小，以使 B 中的元素数与 A 中的元素数相匹配。例如，如果 A 是一个 10×10 矩阵，则<c>Reshape(A, 2, 2, Nothing)</c>将 A 的 100 个元素重构为一个 2×2×25 数组。
	''' </summary>
	''' <typeparam name="T">元素类型</typeparam>
	''' <param name="A">输入数组，必须具有确定的长度</param>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度</param>
	''' <returns>重构的数组</returns>
	Public Function Reshape(Of T)(A As Array(Of T), ParamArray sz As UInteger?()) As Array(Of T)
		Reshape = A.Clone
		Reshape.Reshape(sz)
	End Function
	''' <summary>
	''' 按照向量 dimorder 指定的顺序重新排列数组的维度。例如，<c>Permute(A, 1, 0)</c> 交换矩阵 A 的行和列维度。
	''' </summary>
	''' <typeparam name="T">元素类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <param name="dimorder">维度顺序。不同于MATLAB，从0开始</param>
	''' <returns>置换维度的数组</returns>
	Public Function Permute(Of T)(A As Array(Of T), ParamArray dimorder As Byte()) As Array(Of T)
		Return A.Permute(dimorder)
	End Function
	''' <summary>
	''' 返回一个向量，其元素包含 A 的相应维度的长度。例如，如果 A 是一个 3×4 矩阵，则<c>Size(A)</c>返回向量<c>{3, 4}</c>。sz 的长度为<c>NDims(A)</c>。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>数组各维尺寸构成的数组</returns>
	Public Function Size(Of T)(A As Array(Of T)) As UInteger()
		Return A.Size
	End Function
	''' <summary>
	''' 返回维度 dim 的长度。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As UInteger
		Return A.Size([dim])
	End Function
	<Runtime.CompilerServices.Extension> Public Function Size(A As Array) As UInteger()
		Dim c As Byte = A.Rank - 1, d(c) As UInteger
		For b As Byte = 0 To c
			d(b) = A.GetLength(b)
		Next
		Return d
	End Function
	''' <summary>
	''' 返回数组 A 的维数。函数会忽略<c>Size(A, [dim]) = 1</c>所针对的尾部单一维度。
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>维度数</returns>
	Public Function NDims(Of T)(A As Array(Of T)) As Byte
		Return A.NDims
	End Function
	''' <summary>
	''' 沿维度 dim 串联 A1、A2、…、An。调用方应保证所有输入在串联维度以外的维度等长，此函数不会进行检查。
	''' </summary>
	''' <typeparam name="T">元素类型，缺省<see cref="Object"/></typeparam>
	''' <param name="[dim]">沿其运算的维度</param>
	''' <param name="A">输入列表</param>
	''' <returns>串联的数组</returns>
	Public Function Cat(Of T)([dim] As Byte, ParamArray A As Array(Of T)()) As Array(Of T)
		Dim b As Array(Of UInteger) = A(0).Size.ToArray, c As UInteger
		For Each d As Array(Of T) In A
			c += d.Size([dim])
		Next
		'这一步可能会扩展数组
		b([dim]) = c
		Dim h As UInteger() = b
		c = h.Length - 1
		For e As Byte = 0 To c
			If h(e) = 0 Then h(e) = 1
		Next
		Dim g As New Array(Of T)(h), i(c) As UInteger
		Dim f As UInteger()
		For Each d As Array(Of T) In A
			ReDim f(d.NDims - 1)
			Cat递归(d, g, 0, f, i, [dim], i([dim]))
			i([dim]) += 1
		Next
		Return g
	End Function
	''' <summary>
	''' 返回数组 A 中的元素数目
	''' </summary>
	''' <typeparam name="T">数据类型</typeparam>
	''' <param name="A">输入数组</param>
	''' <returns>元素数目</returns>
	Public Function Numel(Of T)(A As Array(Of T)) As UInteger
		Return A.Numel
	End Function
End Module