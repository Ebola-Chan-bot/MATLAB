Imports System.Reflection
''' <summary>
''' 本库的入口类，使用时直接将<see cref="Array"/>变量赋给<see cref="Array(Of T)"/>即可，然后即可享用本库强大的数组运算功能。
''' </summary>
''' <typeparam name="T">数据类型</typeparam>
Public NotInheritable Class Array(Of T)
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
	''' 这里发生了索引转换，(0,1)=2→(1,0)=2
	''' </summary>
	Private Sub 赋值递归(源数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As UInteger())
		Dim b As Byte = NDims() - 1
		If 当前维度 < b Then
			For a As UInteger = 0 To GetUpperBound(当前维度)
				源索引(b - 当前维度) = a
				目标索引(当前维度) = a
				赋值递归(源数组, 当前维度 + 1, 源索引, 目标索引)
			Next
		Else
			For a As UInteger = 0 To GetUpperBound(当前维度)
				源索引(b - 当前维度) = a
				目标索引(当前维度) = a
				SetValue(源数组.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 此转换会创建一个新数组。调用方有义务保证元素的类型是可正确转换的。
	''' </summary>
	''' <param name="本体">待转换的数组</param>
	''' <returns>尺寸相同的新数组，但元素类型可能改变</returns>
	Public Overloads Shared Widening Operator CType(数组 As Array) As Array(Of T)
		Dim e As Type = 数组.Class, f As Type = GetType(T), 转换器 As Func(Of Object, T)
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
		Dim a As T() = (From j In 数组.AsParallel.AsOrdered Select 转换器.Invoke(j)).ToArray
		If 数组.Rank = 1 Then
			Return New Array(Of T)(a, a.Length)
		Else
			Dim b As SByte
			For b = 数组.Rank - 1 To 0 Step -1
				If 数组.GetUpperBound(b) > 0 Then Exit For
			Next
			Dim c As Byte = Math.Max(b, 0), d(c) As UInteger
			For b = 0 To c
				d(b) = 数组.GetLength(b)
			Next
			Return New Array(Of T)(a, d)
		End If
	End Operator
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
	''' 数组加法，每个元素的位置对应相加产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了+运算符
	''' </summary>
	Public Shared Operator +(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CByte(c) + CByte(d), Object), A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSByte(c) + CSByte(d), Object), A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUShort(c) + CUShort(d), Object), A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CShort(c) + CShort(d), Object), A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUInt(c) + CUInt(d), Object), A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CInt(c) + CInt(d), Object), A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CULng(c) + CULng(d), Object), A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CLng(c) + CLng(d), Object), A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSng(c) + CSng(d), Object), A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDbl(c) + CDbl(d), Object), A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDec(c) + CDec(d), Object), A, B)
			Case Else
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) e.GetMethod("op_Addition").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组减法，每个元素的位置对应相减产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了-运算符
	''' </summary>
	Public Shared Operator -(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CByte(c) - CByte(d), Object), A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSByte(c) - CSByte(d), Object), A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUShort(c) - CUShort(d), Object), A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CShort(c) - CShort(d), Object), A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUInt(c) - CUInt(d), Object), A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CInt(c) - CInt(d), Object), A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CULng(c) - CULng(d), Object), A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CLng(c) - CLng(d), Object), A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSng(c) - CSng(d), Object), A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDbl(c) - CDbl(d), Object), A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDec(c) - CDec(d), Object), A, B)
			Case Else
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) e.GetMethod("op_Subtraction").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组点乘法，每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了*运算符
	''' </summary>
	Public Shared Operator *(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CByte(c) * CByte(d), Object), A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSByte(c) * CSByte(d), Object), A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUShort(c) * CUShort(d), Object), A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CShort(c) * CShort(d), Object), A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUInt(c) * CUInt(d), Object), A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CInt(c) * CInt(d), Object), A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CULng(c) * CULng(d), Object), A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CLng(c) * CLng(d), Object), A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSng(c) * CSng(d), Object), A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDbl(c) * CDbl(d), Object), A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDec(c) * CDec(d), Object), A, B)
			Case Else
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) e.GetMethod("op_Multiply").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	''' <summary>
	''' 数组右除,每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型或定义了/运算符
	''' </summary>
	Public Shared Operator /(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CByte(c) / CByte(d), Object), A, B)
			Case GetType(SByte)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSByte(c) / CSByte(d), Object), A, B)
			Case GetType(UShort)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUShort(c) / CUShort(d), Object), A, B)
			Case GetType(Short)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CShort(c) / CShort(d), Object), A, B)
			Case GetType(UInteger)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CUInt(c) / CUInt(d), Object), A, B)
			Case GetType(Integer)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CInt(c) / CInt(d), Object), A, B)
			Case GetType(ULong)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CULng(c) / CULng(d), Object), A, B)
			Case GetType(Long)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CLng(c) / CLng(d), Object), A, B)
			Case GetType(Single)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CSng(c) / CSng(d), Object), A, B)
			Case GetType(Double)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDbl(c) / CDbl(d), Object), A, B)
			Case GetType(Decimal)
				Return BsxFun(Of T, T, T)(Function(c As Object, d As Object) DirectCast(CDec(c) / CDec(d), Object), A, B)
			Case Else
				Return BsxFun(Of T, T, T)(Function(c As T, d As T) e.GetMethod("op_Division").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	''' <summary>
	''' 确定相等性。对两个数组对应位置判断是否相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了=运算符
	''' </summary>
	Public Shared Operator =(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As T, d As T) c.Equals(d), A, B)
	End Operator
	''' <summary>
	''' 确定不相等性。对两个数组对应位置判断是否不相等，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了<>运算符
	''' </summary>
	Public Shared Operator <>(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Return BsxFun(Function(c As T, d As T) Not c.Equals(d), A, B)
	End Operator
	''' <summary>
	''' 确定大于。对两个数组对应位置判断左边是否大于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&gt;运算符
	''' </summary>
	Public Shared Operator >(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Function(c As Object, d As Object) CByte(c) > CByte(d), A, B)
			Case GetType(SByte)
				Return BsxFun(Function(c As Object, d As Object) CSByte(c) > CSByte(d), A, B)
			Case GetType(UShort)
				Return BsxFun(Function(c As Object, d As Object) CUShort(c) > CUShort(d), A, B)
			Case GetType(Short)
				Return BsxFun(Function(c As Object, d As Object) CShort(c) > CShort(d), A, B)
			Case GetType(UInteger)
				Return BsxFun(Function(c As Object, d As Object) CUInt(c) > CUInt(d), A, B)
			Case GetType(Integer)
				Return BsxFun(Function(c As Object, d As Object) CInt(c) > CInt(d), A, B)
			Case GetType(ULong)
				Return BsxFun(Function(c As Object, d As Object) CULng(c) > CULng(d), A, B)
			Case GetType(Long)
				Return BsxFun(Function(c As Object, d As Object) CLng(c) > CLng(d), A, B)
			Case GetType(Single)
				Return BsxFun(Function(c As Object, d As Object) CSng(c) > CSng(d), A, B)
			Case GetType(Double)
				Return BsxFun(Function(c As Object, d As Object) CDbl(c) > CDbl(d), A, B)
			Case GetType(Decimal)
				Return BsxFun(Function(c As Object, d As Object) CDec(c) > CDec(d), A, B)
			Case Else
				Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) e.GetMethod("op_GreaterThan").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	''' <summary>
	''' 确定小于。对两个数组对应位置判断左边是否小于右边，返回<see cref="Boolean"/>结果存于新数组的对应位置。数组元素必须为基本数值类型或定义了&lt;运算符
	''' </summary>
	Public Shared Operator <(A As Array(Of T), B As Array(Of T)) As Array(Of Boolean)
		Dim e As Type = GetType(T)
		Select Case e
			Case GetType(Byte)
				Return BsxFun(Function(c As Object, d As Object) CByte(c) < CByte(d), A, B)
			Case GetType(SByte)
				Return BsxFun(Function(c As Object, d As Object) CSByte(c) < CSByte(d), A, B)
			Case GetType(UShort)
				Return BsxFun(Function(c As Object, d As Object) CUShort(c) < CUShort(d), A, B)
			Case GetType(Short)
				Return BsxFun(Function(c As Object, d As Object) CShort(c) < CShort(d), A, B)
			Case GetType(UInteger)
				Return BsxFun(Function(c As Object, d As Object) CUInt(c) < CUInt(d), A, B)
			Case GetType(Integer)
				Return BsxFun(Function(c As Object, d As Object) CInt(c) < CInt(d), A, B)
			Case GetType(ULong)
				Return BsxFun(Function(c As Object, d As Object) CULng(c) < CULng(d), A, B)
			Case GetType(Long)
				Return BsxFun(Function(c As Object, d As Object) CLng(c) < CLng(d), A, B)
			Case GetType(Single)
				Return BsxFun(Function(c As Object, d As Object) CSng(c) < CSng(d), A, B)
			Case GetType(Double)
				Return BsxFun(Function(c As Object, d As Object) CDbl(c) < CDbl(d), A, B)
			Case GetType(Decimal)
				Return BsxFun(Function(c As Object, d As Object) CDec(c) < CDec(d), A, B)
			Case Else
				Return BsxFun(Of T, T, Boolean)(Function(c As T, d As T) e.GetMethod("op_LessThan").Invoke(Nothing, {c, d}), A, B)
		End Select
	End Operator
	Private Sub SubsRAGet递归(目标数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 索引映射 As Integer()())
		Dim b As Integer() = 索引映射(当前维度)
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
	Private Sub SubsRASet递归(源数组 As Array(Of T), 当前维度 As Byte, 源索引 As UInteger(), 目标索引 As UInteger(), 索引映射 As Integer()())
		Dim b As Integer() = 索引映射(当前维度)
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
	''' 取多维数组的多个元素。此属性不具有健壮性，调用方应保证数组兼容此下标。
	''' </summary>
	''' <param name="subs">欲取元素的各维下标范围</param>
	''' <returns>所取元素拼接成数组</returns>
	Default Property SubsRA(subs As Integer()()) As Array(Of T)
		Get
			Dim b As Byte = subs.GetUpperBound(0), a(b) As UInteger
			For c As Byte = 0 To b
				a(c) = subs(c).Length
			Next
			Dim d As New Array(Of T)(a), e(NDims() - 1) As UInteger, f(d.NDims - 1) As UInteger
			SubsRAGet递归(d, 0, e, f, subs)
			Return d
		End Get
		Set(value As Array(Of T))
			Dim a(value.NDims - 1) As UInteger, b(NDims() - 1) As UInteger
			SubsRASet递归(value, 0, a, b, subs)
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
		e = e.Except(累积维度)
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
	Friend Sub New(本体 As T(), ParamArray 尺寸 As UInteger())
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
	Function Cast(Of TOut)(Optional [like] As Array(Of TOut) = Nothing) As Array(Of TOut)
		Return New Array(Of TOut)(本体.Select(Function(a As Object) CType(a, TOut)).ToArray, Size.ToArray)
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
End Class
