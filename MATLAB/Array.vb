Imports System.Reflection
''' <summary>
''' 本库的入口类，使用时直接将<see cref="Array"/>变量赋给<see cref="Array(Of T)"/>即可，然后即可享用本库强大的数组运算功能。
''' </summary>
''' <typeparam name="T">数据类型</typeparam>
Public Class Array(Of T)
	Implements IEnumerable(Of T), IArray
	Friend 本体 As T()
	Private Sizes As UInteger()
	Private Sub ToString递归(字符串 As Text.StringBuilder, 当前维度 As Byte, 当前索引 As UInteger())
		If 当前维度 > 1 Then
			For a As UInteger = 0 To GetUpperBound(当前维度)
				当前索引(当前维度) = a
				ToString递归(字符串, 当前维度 - 1, 当前索引)
			Next
		Else
			字符串.Append("(:,:")
			For c As Byte = 2 To 当前索引.GetUpperBound(0)
				字符串.Append(",").Append(当前索引(c))
			Next
			字符串.AppendLine(")")
			For a As UInteger = 0 To GetUpperBound(0)
				当前索引(0) = a
				For b As UInteger = 0 To GetUpperBound(1)
					当前索引(1) = b
					字符串.Append(GetValue(当前索引)).Append(" ")
				Next
				字符串.AppendLine()
			Next
		End If
	End Sub
	Private Sub SubsAsgn递归(目标数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger())
		If 当前维度 < NDims() - 1 Then
			For a As UInteger = 0 To GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				SubsAsgn递归(目标数组, 当前维度 + 1, 源索引, 目标索引)
			Next
		Else
			For a As UInteger = 0 To GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				目标数组.SetValue(GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	Private Sub Permute递归(维度映射 As Byte(), 新数组 As Array(Of T), 当前维度 As Byte, 原索引 As UInteger(), 新索引 As UInteger())
		Dim b As Byte
		If 当前维度 < 新数组.NDims - 1 Then
			For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				Permute递归(维度映射, 新数组, 当前维度 + 1, 原索引, 新索引)
			Next
		Else
			For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				新数组.SetValue(GetValue(原索引), 新索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 此转换创建一个只有一个元素的数组
	''' </summary>
	''' <param name="元素">唯一元素</param>
	''' <returns>单元素数组</returns>
	Public Overloads Shared Widening Operator CType(元素 As T) As Array(Of T)
		Return New Array(Of T)({元素}, 1)
	End Operator
	''' <summary>
	''' 此转换不创建新数组，只改变表示形式
	''' </summary>
	''' <param name="数组">源数组</param>
	''' <returns>改变了表示形式的源数组</returns>
	Public Overloads Shared Widening Operator CType(数组 As T()) As Array(Of T)
		Return New Array(Of T)(数组, 数组.Length)
	End Operator
	''' <summary>
	''' 此处按照(1,0)=1规则赋值
	''' </summary>
	Private Shared Sub 反转递归(枚举器 As IEnumerator(Of T), 目标数组 As Array, 当前维度 As Byte, 目标索引 As Integer())
		If 当前维度 > 0 Then
			For a As UInteger = 0 To 目标数组.GetUpperBound(当前维度)
				目标索引(当前维度) = a
				反转递归(枚举器, 目标数组, 当前维度 - 1, 目标索引)
			Next
		Else
			For a As UInteger = 0 To 目标数组.GetUpperBound(当前维度)
				目标索引(当前维度) = a
				枚举器.MoveNext()
				目标数组.SetValue(枚举器.Current, 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 此转换将会创建新数组
	''' </summary>
	Overloads Shared Narrowing Operator CType(数组 As Array(Of T)) As Array
		Dim a As Array = Array.CreateInstance(GetType(T), 数组.Size.Select(Function(c As Object) CInt(c)).ToArray), b(a.Rank - 1) As Integer
		反转递归(数组.GetEnumerator, a, a.Rank - 1, b)
		Return a
	End Operator
	''' <summary>
	''' 此转换不会创建新数组，只是转换表示形式
	''' </summary>
	Overloads Shared Narrowing Operator CType(数组 As Array(Of T)) As T()
		Return 数组.本体
	End Operator
	''' <summary>
	''' 数组数加法，每个元素加上常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator +(A As Array(Of T), B As T) As Array(Of T)
		Return New Array(Of T)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) CType(c.Plus(B), T)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 数组加法，每个元素的位置对应相加产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator +(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As INumeric, d As INumeric) DirectCast(c.Plus(d), T), A, B)
	End Operator
	''' <summary>
	''' 数组数减法，每个元素减去常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator -(A As Array(Of T), B As T) As Array(Of T)
		Return New Array(Of T)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) CType(c.Minus(B), T)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 数组减法，每个元素的位置对应相减产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator -(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As INumeric, d As INumeric) DirectCast(c.Minus(d), T), A, B)
	End Operator
	''' <summary>
	''' 数组点乘法，每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator *(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As INumeric, d As INumeric) DirectCast(c.Times(d), T), A, B)
	End Operator
	''' <summary>
	''' 数组数乘法，每个元素乘上常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator *(A As Array(Of T), B As T) As Array(Of T)
		Return New Array(Of T)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) CType(c.Times(B), T)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 数组数除法，每个元素除去常数得到新数组，数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator /(A As Array(Of T), B As T) As Array(Of T)
		Return New Array(Of T)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) CType(c.RDivide(B), T)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 数组右除,每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator /(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return BsxFun(Function(c As INumeric, d As INumeric) DirectCast(c.RDivide(d), T), A, B)
	End Operator
	''' <summary>
	''' 确定相等性，每个元素判断是否等于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符
	''' </summary>
	Public Shared Operator =(A As Array(Of T), B As T) As Array(Of Boolean)
		Return New Array(Of Boolean)(A.本体.AsParallel.AsOrdered.Select(Function(c As T) c.Equals(B)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 确定相等性。对两个数组对应位置判断是否相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符
	''' </summary>
	Public Shared Operator =(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As T, d As T) c.Equals(d), A, B)
	End Operator
	''' <summary>
	''' 确定不相等性，每个元素判断是否不等于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;&gt;运算符
	''' </summary>
	Public Shared Operator <>(A As Array(Of T), B As T) As Array(Of Boolean)
		Return New Array(Of Boolean)(A.本体.AsParallel.AsOrdered.Select(Function(c As T) c.Equals(B)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 确定不相等性。对两个数组对应位置判断是否不相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;&gt;运算符
	''' </summary>
	Public Shared Operator <>(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As T, d As T) Not c.Equals(d), A, B)
	End Operator
	''' <summary>
	''' 确定大于。每个元素判断是否大于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator >(A As Array(Of T), B As T) As Array(Of Boolean)
		Return New Array(Of Boolean)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) c.Gt(B)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 确定大于。对两个数组对应位置判断左边是否大于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator >(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As INumeric, d As INumeric) c.Gt(d), A, B)
	End Operator
	''' <summary>
	''' 确定小于。每个元素判断是否小于常数，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator <(A As Array(Of T), B As T) As Array(Of Boolean)
		Return New Array(Of Boolean)(A.本体.AsParallel.AsOrdered.Select(Function(c As INumeric) c.Lt(B)).ToArray, A.Size)
	End Operator
	''' <summary>
	''' 确定小于。对两个数组对应位置判断左边是否小于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须实现<see cref="INumeric"/>
	''' </summary>
	Public Shared Operator <(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As INumeric, d As INumeric) c.Lt(d), A, B)
	End Operator
	Private Sub SubsRAGet递归(目标数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 索引映射 As UInteger()())
		Dim b As UInteger() = 索引映射(当前维度)
		If 当前维度 < 目标数组.NDims - 1 Then
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = b(a)
				目标索引(当前维度) = a
				SubsRAGet递归(目标数组, 当前维度 + 1, 源索引, 目标索引, 索引映射)
			Next
		Else
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = b(a)
				目标索引(当前维度) = a
				目标数组.SetValue(GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	Private Sub SubsRASet递归(源数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 索引映射 As UInteger()())
		Dim b As UInteger() = 索引映射(当前维度)
		If 当前维度 < 源数组.NDims - 1 Then
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = b(a)
				SubsRASet递归(源数组, 当前维度 + 1, 源索引, 目标索引, 索引映射)
			Next
		Else
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = b(a)
				SetValue(源数组.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 此重载用于兼容字面常量，为了减少类型转换的性能消耗请尽可能使用<see cref="SubsRA(UInteger()())"/>重载。取多维数组的多个元素。此属性不具有健壮性，调用方应保证数组兼容此下标。
	''' </summary>
	''' <param name="subs">欲取元素的各维下标范围</param>
	''' <returns>所取元素拼接成数组</returns>
	Default Property SubsRA(subs As Integer()()) As Array(Of T)
		Get
			Return SubsRA(subs.Select(Function(a As Integer()) a.Select(Function(b As Integer) CUInt(b)).ToArray).ToArray)
		End Get
		Set(value As Array(Of T))
			SubsRA(subs.Select(Function(a As Integer()) a.Select(Function(b As Integer) CUInt(b)).ToArray).ToArray) = value
		End Set
	End Property
	''' <summary>
	''' 取多维数组的多个元素。此属性不具有健壮性，调用方应保证数组兼容此下标。
	''' </summary>
	''' <param name="subs">欲取元素的各维下标范围</param>
	''' <returns>所取元素拼接成数组</returns>
	Default Property SubsRA(subs As UInteger()()) As Array(Of T)
		Get
			Dim b As Byte = subs.GetUpperBound(0), a(b) As UInteger, e(b) As UInteger
			For c As Byte = 0 To b
				a(c) = subs(c).Length
				e(c) = subs(c)(0)
			Next
			Dim d As New Array(Of T)(a), f(d.NDims - 1) As UInteger
			SubsRAGet递归(d, 0, e, f, subs)
			Return d
		End Get
		Set(value As Array(Of T))
			Dim a(value.NDims - 1) As UInteger, b As UInteger() = subs.Select(Function(c As UInteger()) c(0)).ToArray
			SubsRASet递归(value, 0, a, b, subs)
		End Set
	End Property
	''' <summary>
	''' 取多维数组的多个元素，使用<see cref="ColonExpression"/>以支持<see cref="[End]"/>索引
	''' </summary>
	''' <param name="subs">欲取元素的各维下标范围</param>
	''' <returns>所取元素拼接成数组</returns>
	Default Property SubsRA(subs As ColonExpression()) As Array(Of T)
		Get
			Dim b As Byte = subs.Length - 1, c(b)() As UInteger
			For a As Byte = 0 To subs.Length - 1
				c(a) = subs(a).ToIndex(Size(a) - 1)
			Next
			Return SubsRA(c)
		End Get
		Set(value As Array(Of T))
			Dim b As Byte = subs.Length - 1, c(b)() As UInteger
			For a As Byte = 0 To subs.Length - 1
				c(a) = subs(a).ToIndex(Size(a) - 1)
			Next
			SubsRA(c) = value
		End Set
	End Property
	''' <summary>
	''' 取向量的多个元素，使用<see cref="ColonExpression"/>以支持<see cref="[End]"/>索引
	''' </summary>
	''' <param name="subs">欲取元素的线性索引</param>
	''' <returns>所取元素拼接成向量</returns>
	Default Property SubsRA(subs As ColonExpression) As Array(Of T)
		Get
			Return subs.ToIndex(Numel - 1).Select(Function(a As UInteger) 本体(a)).ToArray
		End Get
		Set(value As Array(Of T))
			Dim a As UInteger() = subs.ToIndex(Numel - 1)
			For b As UInteger = 0 To a.Length - 1
				本体(a(b)) = value.本体(b)
			Next
		End Set
	End Property
	''' <summary>
	''' 取多维数组的单个元素。如果取值时，下标数与数组维度不匹配，将补零或截尾至匹配。如果赋值时，提供的下标数大于数组维度且多出的下标不全为0，或者某维下标超过该维上限，数组将被扩展，0填充。
	''' </summary>
	''' <param name="subs">元素的各维下标</param>
	''' <returns>所取元素</returns>
	Default Property SubsRA(subs As UInteger()) As T
		Get
			If subs.Length <> NDims() Then ReDim Preserve subs(NDims() - 1)
			Return GetValue(subs)
		End Get
		Set(value As T)
			Dim a As SByte, i As Byte = NDims()
			If subs.Length < i Then ReDim Preserve subs(i - 1)
			For a = subs.GetUpperBound(0) To 0 Step -1
				If subs(a) > 0 Then Exit For
				If a < i Then Exit For
			Next
			If a < subs.GetUpperBound(0) Then ReDim Preserve subs(a)
			Dim c(a) As UInteger, d As Boolean = False, e As UInteger
			For b As Byte = 0 To a
				e = Size(b)
				If subs(b) < e Then
					c(b) = e
				Else
					c(b) = subs(b) + 1
					d = True
				End If
			Next
			If d Then
				Dim f As New Array(Of T)(c), g(i - 1) As UInteger, h(a) As UInteger
				SubsAsgn递归(f, 0, g, h)
				本体 = f.本体
				Reshape(c)
			End If
			SetValue(value, subs)
		End Set
	End Property
	''' <summary>
	''' 取一维数组的单个元素。如果是多维数组，将先展开成一维。此处线性索引规则是(1,0)=1
	''' </summary>
	''' <param name="subs">元素的下标</param>
	''' <returns>所取元素</returns>
	Default Property SubsRA(subs As UInteger) As T
		Get
			Dim n As Byte = NDims(), o As UInteger, m(n - 1) As UInteger
			For d As Byte = 0 To n - 1
				o = Size(d)
				m(d) = subs Mod o
				If subs = m(d) Then
					Exit For
				Else
					subs = (subs - m(d)) / o
				End If
			Next
			Return GetValue(m)
		End Get
		Set(value As T)
			If subs > Numel - 1 AndAlso NDims() = 1 Then
				ReDim Preserve 本体(subs)
				Reshape(CUInt(本体.Length))
			End If
			Dim o As UInteger, m(NDims() - 1) As UInteger
			For d As Byte = 0 To NDims() - 1
				o = Size(d)
				m(d) = subs Mod o
				If subs = m(d) Then
					Exit For
				Else
					subs = (subs - m(d)) / o
				End If
			Next
			SetValue(value, m)
		End Set
	End Property
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <returns>类</returns>
	ReadOnly Property [Class] As Type = GetType(T)
	Public Overrides Function ToString() As String
		Dim a As New Text.StringBuilder
		Select Case NDims()
			Case 1
				For Each b As T In 本体
					a.Append(b.ToString).Append(" ")
				Next
			Case 2
				a.AppendLine("列为第0维，行为第1维")
				For b As Integer = 0 To GetUpperBound(0)
					For c As Integer = 0 To GetUpperBound(1)
						a.Append(GetValue(b, c).ToString).Append(" ")
					Next
					a.AppendLine()
				Next
			Case Else
				a.AppendLine("列为第0维，行为第1维")
				Dim d(NDims() - 1) As UInteger
				ToString递归(a, NDims() - 1, d)
		End Select
		Return a.ToString
	End Function
	''' <summary>
	''' 创建该数组的浅表副本
	''' </summary>
	''' <returns>浅表副本</returns>
	Function Clone() As Array(Of T)
		Return New Array(Of T)(本体.Clone, Size.ToArray)
	End Function
	''' <summary>
	''' 置换数组维度，维度顺序从0开始。
	''' </summary>
	''' <param name="dimorder">维度顺序。不同于MATLAB，从0开始</param>
	''' <returns>置换维度的数组</returns>
	Function Permute(ParamArray dimorder As Byte()) As Array(Of T)
		Dim e As Byte = dimorder.Length, b(e - 1) As UInteger
		For c As Byte = 0 To e - 1
			b(c) = Size(dimorder(c))
		Next
		Dim d As New Array(Of T)(b), a(NDims() - 1) As UInteger, f(d.NDims - 1) As UInteger
		Permute递归(dimorder, d, 0, a, f)
		Return d
	End Function
	Private Function 累积递归(Of TOut)(当前维度 As Byte, 当前索引 As UInteger(), 累积维度 As Byte(), 累积器 As I累积器(Of T, TOut)) As TOut
		Dim b As Byte = 累积维度(当前维度)
		If 当前维度 < 累积维度.GetUpperBound(0) Then
			For a As UInteger = 0 To GetUpperBound(b)
				当前索引(b) = a
				累积递归(当前维度 + 1, 当前索引, 累积维度, 累积器)
			Next
		Else
			For a As UInteger = 0 To GetUpperBound(b)
				当前索引(b) = a
				累积器.累积(GetValue(当前索引))
			Next
		End If
		Return 累积器.结果()
	End Function
	Private Sub 拆分递归(Of TOut)(目标数组 As Array(Of TOut), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 拆分维度 As Byte(), 累积维度 As Byte(), 累积器 As I累积器(Of T, TOut))
		Dim b As Byte = 拆分维度(当前维度)
		If 当前维度 < 拆分维度.GetUpperBound(0) Then
			For a As UInteger = 0 To 目标数组.GetUpperBound(b)
				源索引(b) = a
				目标索引(b) = a
				拆分递归(目标数组, 当前维度 + 1, 源索引, 目标索引, 拆分维度, 累积维度, 累积器)
			Next
		Else
			For a As UInteger = 0 To 目标数组.GetUpperBound(b)
				源索引(b) = a
				目标索引(b) = a
				目标数组.SetValue(累积递归(0, 源索引, 累积维度, 累积器), 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 对多维数组的某些维度进行累积，运算得到单一值，使得这些维度变成单一维度，其它维度保留。不应当假设此函数会按照一定顺序进行累积。累积器应当在取结果时自动重置，本函数不负责重置。
	''' </summary>
	''' <typeparam name="TOut">运算输出值类型</typeparam>
	''' <param name="累积器">整个累积过程重复使用一个累积器</param>
	''' <param name="维度">进行累积的维度</param>
	''' <returns>累积结果数组，在累积维度上长度为1</returns>
	Function 累积降维(Of TOut)(累积器 As I累积器(Of T, TOut), ParamArray 累积维度 As Byte()) As Array(Of TOut)
		Dim b As UInteger() = Size()
		For Each a As Byte In 累积维度
			b(a) = 1
		Next
		Dim g As Byte = NDims() - 1, e(g) As Byte
		For a As Byte = 0 To g
			e(a) = a
		Next
		e = e.Except(累积维度).ToArray
		Dim c As New Array(Of TOut)(b), d(NDims() - 1) As UInteger, f(c.NDims - 1) As UInteger
		拆分递归(c, 0, d, f, e, 累积维度, 累积器)
		Return c
	End Function
	Private Sub 割补递归(新数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger())
		If 当前维度 < 新数组.NDims - 1 Then
			If 当前维度 < 源索引.Length Then
				For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
					源索引(当前维度) = a Mod Size(当前维度)
					目标索引(当前维度) = a
					割补递归(新数组, 当前维度 + 1, 源索引, 目标索引)
				Next
			Else
				For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
					目标索引(当前维度) = a
					割补递归(新数组, 当前维度 + 1, 源索引, 目标索引)
				Next
			End If
		Else
			If 当前维度 < 源索引.Length Then
				For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
					源索引(当前维度) = a Mod Size(当前维度)
					目标索引(当前维度) = a
					新数组.SetValue(GetValue(源索引), 目标索引)
				Next
			Else
				For a As UInteger = 0 To 新数组.GetUpperBound(当前维度)
					目标索引(当前维度) = a
					新数组.SetValue(GetValue(源索引), 目标索引)
				Next
			End If
		End If
	End Sub
	''' <summary>
	''' 此函数按照新维度对数组进行暴力割补，多出部分循环填充。如果新尺寸和
	''' </summary>
	Public Function 循环割补(新尺寸() As UInteger) As Array(Of T)
		If Size.SequenceEqual(新尺寸) Then Return Me
		Dim a As New Array(Of T)(新尺寸), b(NDims() - 1) As UInteger, c(a.NDims - 1) As UInteger
		割补递归(a, 0, b, c)
		Return a
	End Function

	Private Function IEnumerable_GetEnumerator() As IEnumerator(Of T) Implements IEnumerable(Of T).GetEnumerator
		Return GetEnumerator()
	End Function
	Function GetValue(ParamArray index As UInteger()) As T
		If index.Length = 1 Then
			Return 本体(index(0))
		Else
			Return 本体(下标投影(index))
		End If
	End Function

	ReadOnly Property Numel As UInteger
		Get
			Return 本体.Length
		End Get
	End Property
	Sub SetValue(value As T, ParamArray index As UInteger())
		If index.Length = 1 Then
			本体(index(0)) = value
		Else
			本体(下标投影(index)) = value
		End If
	End Sub
	''' <summary>
	''' 索引转换：(1,0)=1
	''' </summary>
	Private Function 下标投影(ParamArray index As UInteger()) As UInteger
		下标投影 = 0
		For a As SByte = index.Length - 1 To 0 Step -1
			下标投影 = 下标投影 * Sizes(a) + index(a)
		Next
	End Function
	Function GetUpperBound(dimension As Byte) As UInteger
		Return Sizes(dimension) - 1
	End Function
	Sub New(本体 As T(), ParamArray 尺寸 As UInteger())
		Reshape(尺寸)
		Me.本体 = 本体
	End Sub
	Sub New(ParamArray 尺寸 As UInteger())
		Reshape(尺寸)
		ReDim 本体(Sizes.AsParallel.AsUnordered.Aggregate(Function(累积数 As UInteger, 积入数 As UInteger) As UInteger
															Return 累积数 * 积入数
														End Function) - 1)
	End Sub
	''' <summary>
	''' 数组的维度数目，忽略较高的单一维度。
	''' </summary>
	''' <returns>维度数</returns>
	ReadOnly Property NDims As Byte Implements IArray.NDims
		Get
			Return Sizes.Length
		End Get
	End Property

	Private Function 排除小尾数(尾数 As UInteger, ParamArray 索引 As UInteger()) As UInteger()
		Dim a As SByte
		For a = 索引.GetUpperBound(0) To 0 Step -1
			If 索引(a) > 尾数 Then Exit For
		Next
		a = Math.Max(a, 0)
		ReDim Preserve 索引(a)
		Return 索引
	End Function
	''' <summary>
	''' 将本数组原地重构。如果需要新数组请先<see cref="Clone"/>。调用方应保证各维长度之积等于元素总数，此函数不会检查。如果不匹配将产生未知后果。
	''' </summary>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度。多个Nothing将导致未知结果</param>
	Sub Reshape(ParamArray sz As UInteger?())
		Dim f As Byte = sz.Length, b As UInteger = 1, c As UInteger? = Nothing
		ReDim Sizes(f - 1)
		For e As Byte = 0 To f - 1
			If sz(e) Is Nothing Then
				c = e
			Else
				b *= sz(e)
				Sizes(e) = sz(e)
			End If
		Next
		If c IsNot Nothing Then Sizes(c) = Numel() / b
		Sizes = 排除小尾数(1, Sizes)
	End Sub
	''' <summary>
	''' 将本数组原地重构。如果需要新数组请先<see cref="Clone"/>。调用方应保证各维长度之积等于元素总数，此函数不会检查。如果不匹配将产生未知后果。
	''' </summary>
	''' <param name="sz">各维长度</param>
	Sub Reshape(sz As UInteger())
		Sizes = 排除小尾数(1, sz)
	End Sub

	Public Function GetEnumerator() As IEnumerator Implements IEnumerable.GetEnumerator
		Return 本体.GetEnumerator
	End Function
	''' <summary>
	''' 如果转换器为空，则此函数将自动查找转换器。查找顺序：1、若<see cref="T"/>继承自或实现TOut，不转换；2、在<see cref="T"/>中定义的向TOut的扩大转换；3、在TOut中定义的来自<see cref="T"/>的扩大转换；3、在<see cref="T"/>中定义的向TOut的收缩转换；4、在TOut中定义的来自<see cref="T"/>的收缩转换。5、如果没找到，可能会出错。
	''' </summary>
	''' <typeparam name="TOut">输出类型</typeparam>
	''' <param name="转换器">转换函数</param>
	''' <returns>返回值</returns>
	Function Cast(Of TOut)() As IArray
		Select Case GetType(TOut)
			Case GetType(Decimal)
				Return NDecimal()
			Case GetType(Double)
				Return NDouble()
			Case GetType(Single)
				Return NSingle()
			Case GetType(SByte)
				Return NSByte()
			Case GetType(Short)
				Return NShort()
			Case GetType(Integer)
				Return NInteger()
			Case GetType(Long)
				Return NLong()
			Case GetType(Byte)
				Return NByte()
			Case GetType(UShort)
				Return NUShort()
			Case GetType(UInteger)
				Return NUInteger()
			Case GetType(ULong)
				Return NULong()
			Case GetType(MDecimal)
				Return [Decimal]()
			Case GetType(MDouble)
				Return [Double]()
			Case GetType(MSingle)
				Return [Single]()
			Case GetType(MInt8)
				Return Int8()
			Case GetType(MInt16)
				Return Int16()
			Case GetType(MInt32)
				Return Int32()
			Case GetType(MInt64)
				Return Int64()
			Case GetType(MUInt8)
				Return UInt8()
			Case GetType(MUInt16)
				Return UInt16()
			Case GetType(MUInt32)
				Return UInt32()
			Case GetType(MUInt64)
				Return UInt64()
			Case Else
				Return New Array(Of TOut)(本体.Select(Function(a As Object) CType(a, TOut)).ToArray, Size)
		End Select
	End Function

	Private Function IArray_循环割补(ParamArray 尺寸() As UInteger) As IArray Implements IArray.循环割补
		Return 循环割补(尺寸)
	End Function

	Private Function IArray_GetValue(ParamArray index() As UInteger) As Object Implements IArray.GetValue
		Return GetValue(index)
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <returns>数组大小</returns>
	Public Function Size() As UInteger() Implements IArray.Size
		Return Sizes.Clone
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	Public Function Size([dim] As Byte) As UInteger Implements IArray.Size
		If [dim] < NDims Then
			Return Sizes([dim])
		Else
			Return 1
		End If
	End Function
	Public Function Min() As T
		Return 本体.AsParallel.AsUnordered.Aggregate(Function(a As INumeric, b As INumeric) a.Min(b))
	End Function
	Public Function Min(ParamArray vecdim As Byte()) As Array(Of T)
		Return 累积降维(New Min累积器(Of T), vecdim)
	End Function
	Public Function Max() As T
		Return 本体.AsParallel.AsUnordered.Aggregate(Function(a As INumeric, b As INumeric) a.Max(b))
	End Function
	Public Function Max(ParamArray vecdim As Byte()) As Array(Of T)
		Return 累积降维(New Max累积器(Of T), vecdim)
	End Function
	Public Function [Decimal]() As Array(Of MDecimal) Implements IArray.Decimal
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MDecimal)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MDecimal(CDec(a))).ToArray, Size)
		Else
			Return New Array(Of MDecimal)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MDecimal(a)).ToArray, Size)
		End If
	End Function
	Public Function [Double]() As Array(Of MDouble) Implements IArray.Double
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MDouble)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MDouble(CDbl(a))).ToArray, Size)
		Else
			Return New Array(Of MDouble)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MDouble(a)).ToArray, Size)
		End If
	End Function
	Public Function [Single]() As Array(Of MSingle) Implements IArray.Single
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MSingle)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MSingle(CSng(a))).ToArray, Size)
		Else
			Return New Array(Of MSingle)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MSingle(a)).ToArray, Size)
		End If
	End Function
	Public Function Int8() As Array(Of MInt8) Implements IArray.Int8
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt8)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MInt8(CSByte(a))).ToArray, Size)
		Else
			Return New Array(Of MInt8)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MInt8(a)).ToArray, Size)
		End If
	End Function
	Public Function Int16() As Array(Of MInt16) Implements IArray.Int16
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt16)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MInt16(CShort(a))).ToArray, Size)
		Else
			Return New Array(Of MInt16)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MInt16(a)).ToArray, Size)
		End If
	End Function
	Public Function Int32() As Array(Of MInt32) Implements IArray.Int32
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt32)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MInt32(CInt(a))).ToArray, Size)
		Else
			Return New Array(Of MInt32)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MInt32(a)).ToArray, Size)
		End If
	End Function
	Public Function Int64() As Array(Of MInt64) Implements IArray.Int64
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MInt64)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MInt64(CLng(a))).ToArray, Size)
		Else
			Return New Array(Of MInt64)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MInt64(a)).ToArray, Size)
		End If
	End Function
	Public Function UInt8() As Array(Of MUInt8) Implements IArray.UInt8
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt8)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MUInt8(CByte(a))).ToArray, Size)
		Else
			Return New Array(Of MUInt8)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MUInt8(a)).ToArray, Size)
		End If
	End Function
	Public Function UInt16() As Array(Of MUInt16) Implements IArray.UInt16
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt16)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MUInt16(CUShort(a))).ToArray, Size)
		Else
			Return New Array(Of MUInt16)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MUInt16(a)).ToArray, Size)
		End If
	End Function
	Public Function UInt32() As Array(Of MUInt32) Implements IArray.UInt32
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt32)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MUInt32(CUInt(a))).ToArray, Size)
		Else
			Return New Array(Of MUInt32)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MUInt32(a)).ToArray, Size)
		End If
	End Function
	Public Function UInt64() As Array(Of MUInt64) Implements IArray.UInt64
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of MUInt64)(本体.AsParallel.AsOrdered.Select(Function(a As Object) New MUInt64(CULng(a))).ToArray, Size)
		Else
			Return New Array(Of MUInt64)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) New MUInt64(a)).ToArray, Size)
		End If
	End Function

	Public Function NDecimal() As Array(Of Decimal) Implements IArray.NDecimal
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Decimal)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CDec(a)).ToArray, Size)
		Else
			Return New Array(Of Decimal)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CDec(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NDouble() As Array(Of Double) Implements IArray.NDouble
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Double)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CDbl(a)).ToArray, Size)
		Else
			Return New Array(Of Double)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CDbl(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NSingle() As Array(Of Single) Implements IArray.NSingle
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Single)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CSng(a)).ToArray, Size)
		Else
			Return New Array(Of Single)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CSng(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NSByte() As Array(Of SByte) Implements IArray.NSByte
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of SByte)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CSByte(a)).ToArray, Size)
		Else
			Return New Array(Of SByte)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CSByte(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NShort() As Array(Of Short) Implements IArray.NShort
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Short)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CShort(a)).ToArray, Size)
		Else
			Return New Array(Of Short)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CShort(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NInteger() As Array(Of Integer) Implements IArray.NInteger
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Integer)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CInt(a)).ToArray, Size)
		Else
			Return New Array(Of Integer)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CInt(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NLong() As Array(Of Long) Implements IArray.NLong
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Long)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CLng(a)).ToArray, Size)
		Else
			Return New Array(Of Long)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CLng(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NByte() As Array(Of Byte) Implements IArray.NByte
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of Byte)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CByte(a)).ToArray, Size)
		Else
			Return New Array(Of Byte)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CByte(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NUShort() As Array(Of UShort) Implements IArray.NUShort
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of UShort)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CUShort(a)).ToArray, Size)
		Else
			Return New Array(Of UShort)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CUShort(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NUInteger() As Array(Of UInteger) Implements IArray.NUInteger
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of UInteger)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CUInt(a)).ToArray, Size)
		Else
			Return New Array(Of UInteger)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CUInt(a.RawData)).ToArray, Size)
		End If
	End Function

	Public Function NULong() As Array(Of ULong) Implements IArray.NULong
		If GetType(T).GetInterface("INumeric") Is Nothing Then
			Return New Array(Of ULong)(本体.AsParallel.AsOrdered.Select(Function(a As Object) CULng(a)).ToArray, Size)
		Else
			Return New Array(Of ULong)(本体.AsParallel.AsOrdered.Select(Function(a As INumeric) CULng(a.RawData)).ToArray, Size)
		End If
	End Function
End Class
