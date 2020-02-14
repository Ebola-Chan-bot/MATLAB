Imports System.Runtime.CompilerServices
Public Module DataTypes
	Private Sub ArrayFun递归(Of TIn, TOut)(输入数组 As Array, 输出数组 As Array, 函数 As Func(Of TIn, TOut), 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 输出数组.Rank - 1 Then
			For a As Integer = 0 To 输出数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				ArrayFun递归(输入数组, 输出数组, 函数, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 输出数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				输出数组.SetValue(函数.Invoke(输入数组.GetValue(当前索引)), 当前索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <param name="数组">数组</param>
	''' <returns>类</returns>
	<Extension> Public Function [Class](数组 As IEnumerable) As Type
		Return 数组.GetType.GetElementType
	End Function
	''' <summary>
	''' 将 A 转换为类 newclass，其中 newclass 是与 A 兼容的数据类型
	''' </summary>
	''' <typeparam name="TIn">原来类</typeparam>
	''' <typeparam name="newclass">目标类</typeparam>
	''' <param name="A">要转换的数组</param>
	''' <param name="[like]">将 A 转换为与此相同的数据类型</param>
	Public Function Cast(Of TIn, newclass)(A As Array(Of TIn), Optional [like] As Array(Of newclass) = Nothing) As Array(Of newclass)
		Return A.Cast([like])
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
	''' 判断两个数组尺寸是否匹配，即各维长度均相等
	''' </summary>
	Function 匹配(ParamArray 数组 As IArray()) As Boolean
		Dim a As IEnumerable(Of UInteger) = 数组(0).Size
		Return New ArraySegment(Of IArray)(数组, 1, 数组.Length - 1).AsParallel.AsUnordered.All(Function(b As IArray) b.Size.SequenceEqual(a))
	End Function
	''' <summary>
	''' 先判断两个数组是否<see cref="匹配"/>，若匹配则原样返回。若不匹配，则对两个数组进行必要的扩展，使两个数组各维长度均相等，扩展采用循环填充（而非补0）。扩展操作会创建新的数组替换原来的数组。
	''' </summary>
	Function 适配(ParamArray 数组 As IArray()) As IArray()
		If 匹配(数组) Then Return 数组
		Dim b(数组.AsParallel.AsUnordered.Aggregate(1, Function(维数 As Byte, a As IArray) Math.Max(维数, a.NDims)) - 1) As UInteger
		Parallel.For(0, b.Length, Sub(c As Integer)
									  b(c) = 数组.Aggregate(Of UInteger)(1, Function(长度 As UInteger, a As IArray) Math.Max(长度, a.Size(c)))
								  End Sub)
		Return 数组.AsParallel.AsOrdered.Select(Function(a As IArray) a.循环割补(b)).ToArray
	End Function
	''' <summary>
	''' 将 func 应用于数组 A1,...,An 的元素，因此 B(i) = func(A1(i),...,An(i))。函数 func 必须接受 n 个输入参数并返回一个标量。数组 A1,...,An 的大小如果不同，较大的数组会被保留，较小的数组则会被循环填补空缺。
	''' </summary>
	''' <typeparam name="TIn">输入数组元素类型</typeparam>
	''' <typeparam name="TOut">输出数组元素类型</typeparam>
	''' <param name="func">要执行的函数</param>
	''' <param name="A">输入数组</param>
	''' <param name="ErrorHandler">错误处理方法</param>
	''' <returns>输出数组</returns>
	Public Function ArrayFun(Of T)(func As Func(Of Object(), T), ParamArray A As IArray()) As Array(Of T)
		Dim b As IArray() = 适配(A), d As New Array(Of T)(b(0).Size.ToArray), e As T() = d.本体
		Parallel.For(0, e.Length, Sub(c As Integer) e(c) = func.Invoke(b.Select(Function(f As IArray) f.GetValue(c)).ToArray))
		Return d
	End Function
End Module
