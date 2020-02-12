Imports System.Reflection
''' <summary>
''' 本库的入口类，使用时直接将<see cref="Array"/>变量赋给<see cref="Array(Of T)"/>即可，然后即可享用本库强大的数组运算功能。
''' </summary>
''' <typeparam name="T">数据类型</typeparam>
Public Class Array(Of T)
	Private 本体 As Array
	Private Delegate Function Caster(输入) As T
	Private Shared Sub CType递归(源数组 As Array, 目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer(), 转换器 As Caster)
		If 当前维度 < 目标数组.Rank - 1 Then
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				CType递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引, 转换器)
			Next
		Else
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				目标数组.SetValue(转换器.Invoke(源数组.GetValue(源索引)), 目标索引)
			Next
		End If
	End Sub
	Private Sub ToString递归(字符串 As Text.StringBuilder, 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 > 1 Then
			For a As Integer = 0 To 本体.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				ToString递归(字符串, 当前维度 - 1, 当前索引)
			Next
		Else
			字符串.Append("(:,:")
			For c As Byte = 2 To 当前索引.GetUpperBound(0)
				字符串.Append(",").Append(当前索引(c))
			Next
			字符串.AppendLine(")")
			For a As Integer = 0 To 本体.GetUpperBound(0)
				当前索引(0) = a
				For b As Integer = 0 To 本体.GetUpperBound(1)
					当前索引(1) = b
					字符串.Append(本体.GetValue(当前索引)).Append(" ")
				Next
				字符串.AppendLine()
			Next
		End If
	End Sub
	Private Shared Sub Reshape递归(原数组 As IEnumerator, 新数组 As Array, 当前维度 As Byte, 新索引 As Integer())
		If 当前维度 > 0 Then
			For a As Integer = 0 To 新数组.GetUpperBound(当前维度)
				新索引(当前维度) = a
				Reshape递归(原数组, 新数组, 当前维度 - 1, 新索引)
			Next
		Else
			For a As Integer = 0 To 新数组.GetUpperBound(当前维度)
				新索引(当前维度) = a
				原数组.MoveNext()
				新数组.SetValue(原数组.Current, 新索引)
			Next
		End If
	End Sub
	Private Sub SubsAsgn递归(目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer())
		If 当前维度 < 本体.Rank - 1 Then
			For a As Integer = 0 To 本体.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				SubsAsgn递归(目标数组, 当前维度 + 1, 源索引, 目标索引)
			Next
		Else
			For a As Integer = 0 To 本体.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				目标数组.SetValue(本体.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	Private Sub Permute递归(维度映射 As Byte(), 新数组 As Array, 当前维度 As Byte, 原索引 As Integer(), 新索引 As Integer())
		Dim b As Integer
		If 当前维度 < 新数组.Rank - 1 Then
			For a As Integer = 0 To 新数组.GetUpperBound(当前维度)
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				Permute递归(维度映射, 新数组, 当前维度 + 1, 原索引, 新索引)
			Next
		Else
			For a As Integer = 0 To 新数组.GetUpperBound(当前维度)
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				新数组.SetValue(本体.GetValue(原索引), 新索引)
			Next
		End If
	End Sub
	Private Shared Sub 赋值递归(源数组 As Array, 目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer())
		If 当前维度 < 目标数组.Rank - 1 Then
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				赋值递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引)
			Next
		Else
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				目标数组.SetValue(源数组.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 必须在构造函数入口处封禁单一高维度
	''' </summary>
	''' <param name="数组">这个数组若无单一高维度，会被直接赋值给本体；否则将创建新数组</param>
	Private Sub New(数组 As Array)
		Dim a As SByte
		For a = 数组.Rank - 1 To 0 Step -1
			If 数组.GetUpperBound(a) > 0 Then Exit For
		Next
		a = Math.Max(a, 0)
		If a < 数组.Rank - 1 Then
			Dim b(a) As Integer
			For c As Byte = 0 To a
				b(c) = 数组.GetLength(c)
			Next
			Dim d As Array = Array.CreateInstance(GetType(T), b), e(数组.Rank - 1) As Integer, f(a) As Integer
			赋值递归(数组, d, 0, e, f)
			数组 = d
		End If
		本体 = 数组
	End Sub
	Private Sub New(标量 As T)
		本体 = {标量}
	End Sub
	''' <summary>
	''' 必须在构造函数入口处封禁单一高维度
	''' </summary>
	''' <param name="尺寸">如果存在单一高维度将被截尾</param>
	Sub New(ParamArray 尺寸 As Integer())
		Dim a As SByte
		For a = 尺寸.GetUpperBound(0) To 0 Step -1
			If 尺寸(a) > 1 Then Exit For
		Next
		If a < 尺寸.GetUpperBound(0) Then ReDim Preserve 尺寸(Math.Max(a, 0))
		本体 = Array.CreateInstance(GetType(T), 尺寸)
	End Sub
	''' <summary>
	''' 此转换会创建一个新数组。调用方有义务保证元素的类型是可正确转换的。
	''' </summary>
	''' <param name="本体">待转换的数组</param>
	''' <returns>尺寸相同的新数组，但元素类型可能改变</returns>
	Public Overloads Shared Widening Operator CType(数组 As Array) As Array(Of T)
		Dim e As Type = 数组.Class, f As Type = GetType(T), 转换器 As Caster
		If f.IsAssignableFrom(e) Then
			转换器 = Function(输入) As T
					  Return 输入
				  End Function
		Else
			Dim g As MethodInfo() = e.GetMethods(), i As MethodInfo() = f.GetMethods
			For Each h As MethodInfo In g
				If h.Name = "op_Implicit" AndAlso f.IsAssignableFrom(h.ReturnType) Then
					转换器 = Function(输入) As T
							  Return h.Invoke(Nothing, {输入})
						  End Function
					Exit For
				End If
			Next
			If 转换器 Is Nothing Then
				For Each h As MethodInfo In i
					If h.Name = "op_Implicit" AndAlso h.GetParameters(0).ParameterType.IsAssignableFrom(e) Then
						转换器 = Function(输入) As T
								  Return h.Invoke(Nothing, {输入})
							  End Function
						Exit For
					End If
				Next
			End If
			If 转换器 Is Nothing Then
				For Each h As MethodInfo In g
					If h.Name = "op_Explicit" AndAlso f.IsAssignableFrom(h.ReturnType) Then
						转换器 = Function(输入) As T
								  Return h.Invoke(Nothing, {输入})
							  End Function
						Exit For
					End If
				Next
			End If
			If 转换器 Is Nothing Then
				For Each h As MethodInfo In i
					If h.Name = "op_Explicit" AndAlso h.GetParameters(0).ParameterType.IsAssignableFrom(e) Then
						转换器 = Function(输入) As T
								  Return h.Invoke(Nothing, {输入})
							  End Function
						Exit For
					End If
				Next
			End If
			If 转换器 Is Nothing Then
				转换器 = Function(输入) As T
						  Return 输入
					  End Function
			End If
		End If
		Dim d As Byte = 数组.Rank - 1, c(d) As Integer
		For b As Byte = 0 To d
			c(b) = 数组.GetLength(b)
		Next
		Dim a As New Array(Of T)(c), j(d) As Integer, k(a.本体.Rank - 1) As Integer
		CType递归(数组, a.本体, 0, j, k, 转换器)
		Return a
	End Operator
	''' <summary>
	''' 此转换创建一个只有本体一个元素的数组
	''' </summary>
	''' <param name="本体">唯一元素</param>
	''' <returns>单元素数组</returns>
	Public Overloads Shared Widening Operator CType(本体 As T) As Array(Of T)
		Return New Array(Of T)(本体)
	End Operator
	''' <summary>
	''' 此转换不创建新数组，只改变表示形式
	''' </summary>
	''' <param name="本体">源数组</param>
	''' <returns>改变了表示形式的源数组</returns>
	Public Overloads Shared Widening Operator CType(本体 As T()) As Array(Of T)
		Return New Array(Of T)(本体)
	End Operator
	''' <summary>
	''' 此转换不会创建新数组，只会改变表示形式
	''' </summary>
	''' <param name="AT">待转换的数组</param>
	''' <returns>和源数组是同一个数组，只是表示形式不同</returns>
	Overloads Shared Narrowing Operator CType(AT As Array(Of T)) As Array
		Return AT.本体
	End Operator

	Overloads Shared Narrowing Operator CType(AT As Array(Of T)) As T()
		If AT.本体.Rank = 1 Then
			Return AT.本体
		Else
			Dim a(AT.本体.Length - 1) As T, b As IEnumerator = AT.本体.GetEnumerator
			For c As Integer = 0 To a.GetUpperBound(0)
				b.MoveNext()
				a(c) = b.Current
			Next
			Return a
		End If
	End Operator
	''' <summary>
	''' 数组加法，每个元素的位置对应相加产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了+运算符
	''' </summary>
	Public Shared Operator +(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Byte, d As Byte) As Byte
											  Return c + d
										  End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As SByte, d As SByte) As SByte
											  Return c + d
										  End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As UShort, d As UShort) As UShort
											  Return c + d
										  End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Short, d As Short) As Short
											  Return c + d
										  End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As UInteger, d As UInteger) As UInteger
											  Return c + d
										  End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Integer, d As Integer) As Integer
											  Return c + d
										  End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As ULong, d As ULong) As ULong
											  Return c + d
										  End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Long, d As Long) As Long
											  Return c + d
										  End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Single, d As Single) As Single
											  Return c + d
										  End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Decimal, d As Decimal) As Decimal
											  Return c + d
										  End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
											  Return c + d
										  End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_Addition")
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) As T
											  Return f.Invoke(Nothing, {c, d})
										  End Function, A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组减法，每个元素的位置对应相减产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了-运算符
	''' </summary>
	Public Shared Operator -(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Byte, d As Byte) As Byte
											  Return c - d
										  End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As SByte, d As SByte) As SByte
											  Return c - d
										  End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As UShort, d As UShort) As UShort
											  Return c - d
										  End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Short, d As Short) As Short
											  Return c - d
										  End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As UInteger, d As UInteger) As UInteger
											  Return c - d
										  End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Integer, d As Integer) As Integer
											  Return c - d
										  End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As ULong, d As ULong) As ULong
											  Return c - d
										  End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Long, d As Long) As Long
											  Return c - d
										  End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Single, d As Single) As Single
											  Return c - d
										  End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Decimal, d As Decimal) As Decimal
											  Return c - d
										  End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
											  Return c - d
										  End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_Subtraction")
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) As T
											  Return f.Invoke(Nothing, {c, d})
										  End Function, A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组点乘法，每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了*运算符
	''' </summary>
	Public Shared Operator *(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Byte, d As Byte) As Byte
											  Return c * d
										  End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As SByte, d As SByte) As SByte
											  Return c * d
										  End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As UShort, d As UShort) As UShort
											  Return c * d
										  End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Short, d As Short) As Short
											  Return c * d
										  End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As UInteger, d As UInteger) As UInteger
											  Return c * d
										  End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Integer, d As Integer) As Integer
											  Return c * d
										  End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As ULong, d As ULong) As ULong
											  Return c * d
										  End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Long, d As Long) As Long
											  Return c * d
										  End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Single, d As Single) As Single
											  Return c * d
										  End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Decimal, d As Decimal) As Decimal
											  Return c * d
										  End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
											  Return c * d
										  End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_Multiply")
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) As T
											  Return f.Invoke(Nothing, {c, d})
										  End Function, A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组右除,每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了/运算符
	''' </summary>
	Public Shared Operator /(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Byte, d As Byte) As Byte
											  Return c / d
										  End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As SByte, d As SByte) As SByte
											  Return c / d
										  End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As UShort, d As UShort) As UShort
											  Return c / d
										  End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Short, d As Short) As Short
											  Return c / d
										  End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As UInteger, d As UInteger) As UInteger
											  Return c / d
										  End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Integer, d As Integer) As Integer
											  Return c / d
										  End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As ULong, d As ULong) As ULong
											  Return c / d
										  End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Long, d As Long) As Long
											  Return c / d
										  End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Single, d As Single) As Single
											  Return c / d
										  End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Decimal, d As Decimal) As Decimal
											  Return c / d
										  End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
											  Return c / d
										  End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_Division")
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) As T
											  Return f.Invoke(Nothing, {c, d})
										  End Function, A, B)
		End Select
	End Operator
	''' <summary>
	''' 确定相等性。对两个数组对应位置判断是否相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符
	''' </summary>
	Shared Operator =(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) As Boolean
											Return c.Equals(d)
										End Function, A, B)
	End Operator
	''' <summary>
	''' 确定不相等性。对两个数组对应位置判断是否不相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了<>运算符
	''' </summary>
	Shared Operator <>(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) As Boolean
											Return Not c.Equals(d)
										End Function, A, B)
	End Operator
	''' <summary>
	''' 确定大于。对两个数组对应位置判断左边是否大于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&gt;运算符
	''' </summary>
	Public Shared Operator >(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, Boolean)(Function(c As Byte, d As Byte) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, Boolean)(Function(c As SByte, d As SByte) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, Boolean)(Function(c As UShort, d As UShort) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, Boolean)(Function(c As Short, d As Short) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, Boolean)(Function(c As UInteger, d As UInteger) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, Boolean)(Function(c As Integer, d As Integer) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, Boolean)(Function(c As ULong, d As ULong) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, Boolean)(Function(c As Long, d As Long) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, Boolean)(Function(c As Single, d As Single) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, Boolean)(Function(c As Decimal, d As Decimal) As Boolean
													Return c > d
												End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, Boolean)(Function(c As Double, d As Double) As Boolean
													Return c > d
												End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_GreaterThan")
				Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) As Boolean
													Return f.Invoke(Nothing, {c, d})
												End Function, A, B)
		End Select
	End Operator
	''' <summary>
	''' 确定小于。对两个数组对应位置判断左边是否小于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;运算符
	''' </summary>
	Public Shared Operator <(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, Boolean)(Function(c As Byte, d As Byte) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, Boolean)(Function(c As SByte, d As SByte) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, Boolean)(Function(c As UShort, d As UShort) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, Boolean)(Function(c As Short, d As Short) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, Boolean)(Function(c As UInteger, d As UInteger) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, Boolean)(Function(c As Integer, d As Integer) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, Boolean)(Function(c As ULong, d As ULong) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, Boolean)(Function(c As Long, d As Long) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, Boolean)(Function(c As Single, d As Single) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, Boolean)(Function(c As Decimal, d As Decimal) As Boolean
													Return c < d
												End Function, A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, Boolean)(Function(c As Double, d As Double) As Boolean
													Return c < d
												End Function, A, B)
			Case Else
				Dim f As MethodInfo = e.GetMethod("op_LessThan")
				Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) As Boolean
													Return f.Invoke(Nothing, {c, d})
												End Function, A, B)
		End Select
	End Operator
	Private Sub SubsRAGet递归(目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer(), 索引映射 As Integer()())
		Dim b As Integer() = 索引映射(当前维度)
		If 当前维度 < 目标数组.Rank - 1 Then
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = b(a)
				目标索引(当前维度) = a
				SubsRAGet递归(目标数组, 当前维度 + 1, 源索引, 目标索引, 索引映射)
			Next
		Else
			For a As Integer = 0 To 目标数组.GetUpperBound(当前维度)
				源索引(当前维度) = b(a)
				目标索引(当前维度) = a
				目标数组.SetValue(本体.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	Private Sub SubsRASet递归(源数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer(), 索引映射 As Integer()())
		Dim b As Integer() = 索引映射(当前维度)
		If 当前维度 < 源数组.Rank - 1 Then
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = b(a)
				SubsRASet递归(源数组, 当前维度 + 1, 源索引, 目标索引, 索引映射)
			Next
		Else
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = b(a)
				本体.SetValue(源数组.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 取多维数组的多个元素。此属性不具有健壮性，调用方应保证数组兼容此下标。
	''' </summary>
	''' <param name="subs">欲取元素的各维下标范围</param>
	''' <returns>所取元素拼接成数组</returns>
	Default Property SubsRA(subs As Integer()()) As Array(Of T)
		Get
			Dim b As Byte = subs.GetUpperBound(0), a(b) As Integer
			For c As Byte = 0 To b
				a(c) = subs(c).Length
			Next
			Dim d As New Array(Of T)(a), e(本体.Rank - 1) As Integer, f(d.本体.Rank - 1) As Integer
			SubsRAGet递归(d.本体, 0, e, f, subs)
			Return d
		End Get
		Set(value As Array(Of T))
			Dim a(value.本体.Rank - 1) As Integer, b(本体.Rank - 1) As Integer
			SubsRASet递归(value, 0, a, b, subs)
		End Set
	End Property
	''' <summary>
	''' 取多维数组的单个元素。如果取值时，下标数与数组维度不匹配，将补零或截尾至匹配。如果赋值时，提供的下标数大于数组维度且多出的下标不全为0，或者某维下标超过该维上限，数组将被扩展，0填充。
	''' </summary>
	''' <param name="subs">元素的各维下标</param>
	''' <returns>所取元素</returns>
	Default Property SubsRA(subs As Integer()) As T
		Get
			If subs.Length <> 本体.Rank Then ReDim Preserve subs(本体.Rank - 1)
			Return 本体.GetValue(subs)
		End Get
		Set(value As T)
			Dim a As SByte, i As Byte = 本体.Rank
			If subs.Length < i Then ReDim Preserve subs(i - 1)
			For a = subs.GetUpperBound(0) To 0 Step -1
				If subs(a) > 0 Then Exit For
				If a < i Then Exit For
			Next
			If a < subs.GetUpperBound(0) Then ReDim Preserve subs(a)
			Dim c(a) As Integer, d As Boolean = False, e As Integer
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
				Dim f As Array = Array.CreateInstance(GetType(T), c), g(i - 1) As Integer, h(a) As Integer
				SubsAsgn递归(f, 0, g, h)
				本体 = f
			End If
			本体.SetValue(value, subs)
		End Set
	End Property
	''' <summary>
	''' 取一维数组的单个元素。如果是多维数组，将先展开成一维。如果超出元素个数，将先扩展成足够长的向量，空余处0填充。
	''' </summary>
	''' <param name="subs">元素的下标</param>
	''' <returns>所取元素</returns>
	Default Property SubsRA(subs As Integer) As T
		Get
			Dim n As Byte = 本体.Rank, o As Integer, m(n - 1) As Integer
			For d As Byte = 0 To n - 1
				o = Size(d)
				m(d) = subs Mod o
				If subs = m(d) Then
					Exit For
				Else
					subs = (subs - m(d)) / o
				End If
			Next
			Return 本体.GetValue(m)
		End Get
		Set(value As T)
			If subs > 本体.Length - 1 Then
				Dim a(subs) As T, b = 本体.GetEnumerator, c As Integer = 0
				While b.MoveNext
					a(c) = b.Current
					c += 1
				End While
				a(subs) = value
				本体 = a
			Else
				Dim o As Integer, m(本体.Rank - 1) As Integer
				For d As Byte = 0 To 本体.Rank - 1
					o = 本体.GetLength(d)
					m(d) = subs Mod o
					If subs = m(d) Then
						Exit For
					Else
						subs = (subs - m(d)) / o
					End If
				Next
				本体.SetValue(value, m)
			End If
		End Set
	End Property
	''' <summary>
	''' 数组的维度数目，忽略较高的单一维度。
	''' </summary>
	''' <returns>维度数</returns>
	ReadOnly Property NDims As Byte
		Get
			Return 本体.Rank
		End Get
	End Property
	''' <summary>
	''' 返回数组 A 中的元素数目 n 等同于 prod(size(A))。
	''' </summary>
	''' <returns>元素数目</returns>
	ReadOnly Property Numel As Integer
		Get
			Return 本体.Length
		End Get
	End Property
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <returns>类</returns>
	ReadOnly Property [Class] As Type = GetType(T)
	''' <summary>
	''' 数组大小。末尾的单一维度会忽略。
	''' </summary>
	''' <returns>数组各维尺寸构成的数组</returns>
	Function Size() As Integer()
		Dim b As Byte = 本体.Rank - 1, c(b) As Integer
		For a = 0 To b
			c(a) = 本体.GetLength(a)
		Next
		Return c
	End Function
	''' <summary>
	''' 数组大小
	''' </summary>
	''' <param name="[dim]">查询的维度</param>
	''' <returns>数组大小</returns>
	Function Size([dim] As Byte) As Integer
		If [dim] < NDims Then
			Return 本体.GetLength([dim])
		Else
			Return 1
		End If
	End Function
	Public Overrides Function ToString() As String
		Dim a As New Text.StringBuilder
		Select Case 本体.Rank
			Case 1
				For Each b As T In 本体
					a.Append(b.ToString).Append(" ")
				Next
			Case 2
				a.AppendLine("列为第0维，行为第1维")
				For b As Integer = 0 To 本体.GetUpperBound(0)
					For c As Integer = 0 To 本体.GetUpperBound(1)
						a.Append(本体.GetValue(b, c).ToString).Append(" ")
					Next
					a.AppendLine()
				Next
			Case Else
				a.AppendLine("列为第0维，行为第1维")
				Dim d(本体.Rank - 1) As Integer
				ToString递归(a, 本体.Rank - 1, d)
		End Select
		Return a.ToString
	End Function
	''' <summary>
	''' 如果转换器为空，则此函数将自动查找转换器。查找顺序：1、若<see cref="T"/>继承自或实现TOut，不转换；2、在<see cref="T"/>中定义的向TOut的扩大转换；3、在TOut中定义的来自<see cref="T"/>的扩大转换；3、在<see cref="T"/>中定义的向TOut的收缩转换；4、在TOut中定义的来自<see cref="T"/>的收缩转换。5、如果没找到，可能会出错。
	''' </summary>
	''' <typeparam name="TOut">输出类型</typeparam>
	''' <param name="转换器">转换函数</param>
	''' <returns>返回值</returns>
	Function Cast(Of TOut)() As Array(Of TOut)
		Return 本体
	End Function
	''' <summary>
	''' 重构数组
	''' </summary>
	''' <param name="sz">各维长度，可以使用一个Nothing表示自动推断该维长度</param>
	''' <returns>重构的数组</returns>
	Function Reshape(ParamArray sz As UInteger?()) As Array(Of T)
		Dim f As Byte = sz.Length, b As UInteger = 1, c As UInteger? = Nothing, Lengths(f - 1) As Integer
		For e As Byte = 0 To f - 1
			If sz(e) Is Nothing Then
				c = e
			Else
				b *= sz(e)
				Lengths(e) = sz(e)
			End If
		Next
		If c IsNot Nothing Then Lengths(c) = 本体.Length / b
		Dim d As New Array(Of T)(Lengths), a(f - 1) As Integer
		Reshape递归(本体.GetEnumerator, d.本体, f - 1, a)
		Return d
	End Function
	''' <summary>
	''' 创建该数组的浅表副本
	''' </summary>
	''' <returns>浅表副本</returns>
	Function Clone() As Array(Of T)
		Return New Array(Of T)(本体)
	End Function
	''' <summary>
	''' 置换数组维度，维度顺序从0开始。
	''' </summary>
	''' <param name="dimorder">维度顺序。不同于MATLAB，从0开始</param>
	''' <returns>置换维度的数组</returns>
	Function Permute(ParamArray dimorder As Byte()) As Array(Of T)
		Dim e As Byte = dimorder.Length, b(e - 1) As Integer
		For c As Byte = 0 To e - 1
			b(c) = Size(dimorder(c))
		Next
		Dim d As New Array(Of T)(b), a(本体.Rank - 1) As Integer, f(d.本体.Rank - 1) As Integer
		Permute递归(dimorder, d.本体, 0, a, f)
		Return d
	End Function
End Class
