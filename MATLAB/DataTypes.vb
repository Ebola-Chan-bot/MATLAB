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
	Public Function Cast(Of TIn, newclass)(A As Array(Of TIn)) As Array(Of newclass)
		Return A.Cast(Of newclass)
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
		Dim b(数组.Aggregate(1, Function(维数 As Byte, a As IArray) Math.Max(维数, a.NDims)) - 1) As UInteger
		Parallel.For(0, b.Length, Sub(c As Integer)
									  b(c) = 数组.Aggregate(Of UInteger)(1, Function(长度 As UInteger, a As IArray) Math.Max(长度, a.Size(c)))
								  End Sub)
		Return 数组.AsParallel.AsOrdered.Select(Function(a As IArray) a.循环割补(b)).ToArray
	End Function
	''' <summary>
	''' 将 func 应用于数组 A1,...,An 的元素，因此 B(i) = func(A1(i),...,An(i))。函数 func 必须接受 n 个输入参数并返回一个标量。数组 A1,...,An 的大小如果不同，较大的数组会被保留，较小的数组则会被循环填补空缺。这个函数不擅长处理对大规模数组每个元素进行的少量运算，例如数组之间的简单算术运算请勿使用此函数。
	''' </summary>
	''' <typeparam name="T">输出数组元素类型</typeparam>
	''' <param name="func">要执行的函数</param>
	''' <param name="A">输入数组</param>
	''' <returns>输出数组</returns>
	Public Function ArrayFun(Of T)(func As Func(Of Object(), T), ParamArray A As IArray()) As Array(Of T)
		Dim b As IArray() = 适配(A), d As New Array(Of T)(b(0).Size.ToArray), e As T() = d.本体
		Parallel.For(0, e.Length, Sub(c As Integer) e(c) = func.Invoke(b.Select(Function(f As IArray) f.GetValue(c)).ToArray))
		Return d
	End Function
	''' <summary>
	''' 将函数 func 应用于 A 的元素，一次一个元素。然后 ArrayFun 将 func 的输出串联成输出数组 B，因此，对于 A 的第 i 个元素来说，B(i) = func(A(i))。输入参数 func 是一个函数的函数句柄，此函数接受一个输入参数并返回一个标量。func 的输出可以是任何数据类型，只要该类型的对象可以串联即可。数组 A 和 B 必须具有相同的大小。<br/>
	''' 您不能指定 arrayfun 计算 B 的各元素的顺序，也不能指望它们按任何特定的顺序完成计算。<br/>
	''' 当参与一系列运算的数组只有一个，其它均为标量时，使用<see cref="ArrayFun(Of T)(Func(Of T, Single), TypedArray(Of T))"/>通常可以有效提高性能。
	''' </summary>
	''' <param name="func">要对输入数组的元素应用的函数，指定为函数句柄。</param>
	''' <param name="A">输入数组。</param>
	''' <returns>输出数组</returns>
	Public Function ArrayFun(Of T)(func As Func(Of T, Single), A As TypedArray(Of T)) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From b As T In A.本体 Select func.Invoke(b)).ToArray)
	End Function
	''' <summary>
	''' 将 X 中的值转换为十进位。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Decimal](X As Decimal) As Array(Of MDecimal)
		Return New MDecimal(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为十进位。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Decimal](Of T As INumeric)(X As T) As Array(Of MDecimal)
		Return New MDecimal(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为十进位。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Decimal](X As IArray) As Array(Of MDecimal)
		Return X.Decimal
	End Function
	''' <summary>
	''' 将 X 中的值转换为十进位。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function [Decimal](X As Array) As Array(Of MDecimal)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MDecimal)((From a In X.AsParallel.AsOrdered Select New MDecimal(CDec(a))).ToArray, X.Size)
		Else
			Return New Array(Of MDecimal)((From a As INumeric In X.AsParallel.AsOrdered Select New MDecimal(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为十进位。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Decimal](X As IToVector) As Array(Of MDecimal)
		Return X.ToMDecimal
	End Function
	''' <summary>
	''' 将 X 中的值转换为双精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Double](X As Double) As Array(Of MDouble)
		Return New MDouble(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为双精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Double](Of T As INumeric)(X As T) As Array(Of MDouble)
		Return New MDouble(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为双精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Double](X As IArray) As Array(Of MDouble)
		Return X.Double
	End Function
	''' <summary>
	''' 将 X 中的值转换为双精度。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function [Double](X As Array) As Array(Of MDouble)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MDouble)((From a In X.AsParallel.AsOrdered Select New MDouble(CDbl(a))).ToArray, X.Size)
		Else
			Return New Array(Of MDouble)((From a As INumeric In X.AsParallel.AsOrdered Select New MDouble(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为双精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Double](X As IToVector) As Array(Of MDouble)
		Return X.ToMDouble
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Single](X As Single) As Array(Of MSingle)
		Return New MSingle(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Single](Of T As INumeric)(X As T) As Array(Of MSingle)
		Return New MSingle(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Single](X As IArray) As Array(Of MSingle)
		Return X.Single
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	<Extension> Public Function ToSingle(X As ByteArray) As SingleArray
		Return New SingleArray(X.Size.ToArray, (From a As Byte In X.本体 Select CSng(a)).ToArray)
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function [Single](X As Array) As Array(Of MSingle)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MSingle)((From a In X.AsParallel.AsOrdered Select New MSingle(CSng(a))).ToArray, X.Size)
		Else
			Return New Array(Of MSingle)((From a As INumeric In X.AsParallel.AsOrdered Select New MSingle(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为单精度。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Single](X As IToVector) As Array(Of MSingle)
		Return X.ToMSingle
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int8(X As SByte) As Array(Of MInt8)
		Return New MInt8(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int8(Of T As INumeric)(X As T) As Array(Of MInt8)
		Return New MInt8(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int8(X As IArray) As Array(Of MInt8)
		Return X.Int8
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function Int8(X As Array) As Array(Of MInt8)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt8)((From a In X.AsParallel.AsOrdered Select New MInt8(CSByte(a))).ToArray, X.Size)
		Else
			Return New Array(Of MInt8)((From a As INumeric In X.AsParallel.AsOrdered Select New MInt8(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Int8](X As IToVector) As Array(Of MInt8)
		Return X.ToMInt8
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int16(X As Short) As Array(Of MInt16)
		Return New MInt16(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int16(Of T As INumeric)(X As T) As Array(Of MInt16)
		Return New MInt16(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int16(X As IArray) As Array(Of MInt16)
		Return X.Int16
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function Int16(X As Array) As Array(Of MInt16)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt16)((From a In X.AsParallel.AsOrdered Select New MInt16(CShort(a))).ToArray, X.Size)
		Else
			Return New Array(Of MInt16)((From a As INumeric In X.AsParallel.AsOrdered Select New MInt16(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Int16](X As IToVector) As Array(Of MInt16)
		Return X.ToMInt16
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int32(X As Integer) As Array(Of MInt32)
		Return New MInt32(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int32(Of T As INumeric)(X As T) As Array(Of MInt32)
		Return New MInt32(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int32(X As IArray) As Array(Of MInt32)
		Return X.Int32
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function Int32(X As Array) As Array(Of MInt32)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt32)((From a In X.AsParallel.AsOrdered Select New MInt32(CInt(a))).ToArray, X.Size)
		Else
			Return New Array(Of MInt32)((From a As INumeric In X.AsParallel.AsOrdered Select New MInt32(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Int32](X As IToVector) As Array(Of MInt32)
		Return X.ToMInt32
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int64(X As Long) As Array(Of MInt64)
		Return New MInt64(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int64(Of T As INumeric)(X As T) As Array(Of MInt64)
		Return New MInt64(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function Int64(X As IArray) As Array(Of MInt64)
		Return X.Int64
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function Int64(X As Array) As Array(Of MInt64)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt64)((From a In X.AsParallel.AsOrdered Select New MInt64(CLng(a))).ToArray, X.Size)
		Else
			Return New Array(Of MInt64)((From a As INumeric In X.AsParallel.AsOrdered Select New MInt64(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [Int64](X As IToVector) As Array(Of MInt64)
		Return X.ToMInt64
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt8(X As Byte) As Array(Of MUInt8)
		Return New MUInt8(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt8(Of T As INumeric)(X As T) As Array(Of MUInt8)
		Return New MUInt8(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt8(X As IArray) As Array(Of MUInt8)
		Return X.UInt8
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function UInt8(X As Array) As Array(Of MUInt8)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt8)((From a In X.AsParallel.AsOrdered Select New MUInt8(CByte(a))).ToArray, X.Size)
		Else
			Return New Array(Of MUInt8)((From a As INumeric In X.AsParallel.AsOrdered Select New MUInt8(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt8"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [UInt8](X As IToVector) As Array(Of MUInt8)
		Return X.ToMUInt8
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="Byte"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function ToByte(X As TypedArray(Of Single)) As ByteArray
		Return New ByteArray(X.Size.ToArray, (From a As Single In X.本体 Select CByte(a)).ToArray)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt16(X As UShort) As Array(Of MUInt16)
		Return New MUInt16(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt16(Of T As INumeric)(X As T) As Array(Of MUInt16)
		Return New MUInt16(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt16(X As IArray) As Array(Of MUInt16)
		Return X.UInt16
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function UInt16(X As Array) As Array(Of MUInt16)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt16)((From a In X.AsParallel.AsOrdered Select New MUInt16(CUShort(a))).ToArray, X.Size)
		Else
			Return New Array(Of MUInt16)((From a As INumeric In X.AsParallel.AsOrdered Select New MUInt16(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt16"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [UInt16](X As IToVector) As Array(Of MUInt16)
		Return X.ToMUInt16
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt32(X As UInteger) As Array(Of MUInt32)
		Return New MUInt32(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt32(Of T As INumeric)(X As T) As Array(Of MUInt32)
		Return New MUInt32(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt32(X As IArray) As Array(Of MUInt32)
		Return X.UInt32
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function UInt32(X As Array) As Array(Of MUInt32)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt32)((From a In X.AsParallel.AsOrdered Select New MUInt32(CUInt(a))).ToArray, X.Size)
		Else
			Return New Array(Of MUInt32)((From a As INumeric In X.AsParallel.AsOrdered Select New MUInt32(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt32"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function [UInt32](X As IToVector) As Array(Of MUInt32)
		Return X.ToMUInt32
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt64(X As ULong) As Array(Of MUInt64)
		Return New MUInt64(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt64(Of T As INumeric)(X As T) As Array(Of MUInt64)
		Return New MUInt64(X)
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt64(X As IArray) As Array(Of MUInt64)
		Return X.UInt64
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组</param>
	Public Function UInt64(X As Array) As Array(Of MUInt64)
		If X.Class.GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt64)((From a In X.AsParallel.AsOrdered Select New MUInt64(CULng(a))).ToArray, X.Size)
		Else
			Return New Array(Of MUInt64)((From a As INumeric In X.AsParallel.AsOrdered Select New MUInt64(a)).ToArray, X.Size)
		End If
	End Function
	''' <summary>
	''' 将 X 中的值转换为<see cref="MUInt64"/>类型。
	''' </summary>
	''' <param name="X">输入数组，指定为标量、向量、矩阵或多维数组。</param>
	Public Function UInt64(X As IToVector) As Array(Of MUInt64)
		Return X.ToMUInt64
	End Function
End Module
