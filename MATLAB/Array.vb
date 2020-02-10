Public Class Array(Of T)
	Private 本体 As Array
	Private Shared Sub 赋值递归(源数组 As Array, 目标数组 As Array, 各维长度 As Integer(), 总维数 As Byte, 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 总维数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				赋值递归(源数组, 目标数组, 各维长度, 总维数, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				目标数组.SetValue(源数组.GetValue(当前索引), 当前索引)
			Next
		End If
	End Sub
	Private Sub ToString递归(字符串 As Text.StringBuilder, 各维长度 As Integer(), 总维数 As Byte, 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 > 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				当前索引(当前维度) = a
				ToString递归(字符串, 各维长度, 总维数, 当前维度 - 1, 当前索引)
			Next
		Else
			字符串.Append("(:,:")
			For c As Byte = 2 To 当前索引.GetUpperBound(0)
				字符串.Append(",").Append(当前索引(c))
			Next
			字符串.AppendLine(")")
			For a As Integer = 0 To 各维长度(0) - 1
				当前索引(0) = a
				For b As Integer = 0 To 各维长度(1) - 1
					当前索引(1) = b
					字符串.Append(SubsRA(当前索引)).Append(" ")
				Next
				字符串.AppendLine()
			Next
		End If
	End Sub
	Private Shared Sub Reshape递归(原数组 As IEnumerator, 维度数 As Byte, 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 新索引 As Integer())
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
	Private Shared Sub SubsAsgn递归(源数组 As Array, 目标数组 As Array, 当前维度 As Byte, 源索引 As Integer(), 目标索引 As Integer())
		If 当前维度 < 源数组.Rank - 1 Then
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				SubsAsgn递归(源数组, 目标数组, 当前维度 + 1, 源索引, 目标索引)
			Next
		Else
			For a As Integer = 0 To 源数组.GetUpperBound(当前维度)
				源索引(当前维度) = a
				目标索引(当前维度) = a
				目标数组.SetValue(源数组.GetValue(源索引), 目标索引)
			Next
		End If
	End Sub
	Private Shared Sub Permute递归(原数组 As Array, 维度数 As Byte, 维度映射 As Byte(), 各维长度 As Integer(), 新数组 As Array, 当前维度 As Byte, 原索引 As Integer(), 新索引 As Integer())
		Dim b As Integer
		If 当前维度 < 维度数 - 1 Then
			For a As Integer = 0 To 各维长度(当前维度) - 1
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				Permute递归(原数组, 维度数, 维度映射, 各维长度, 新数组, 当前维度 + 1, 原索引, 新索引)
			Next
		Else
			For a As Integer = 0 To 各维长度(当前维度) - 1
				b = 维度映射(当前维度)
				If b < 原索引.Length Then 原索引(b) = a
				新索引(当前维度) = a
				新数组.SetValue(原数组.GetValue(原索引), 新索引)
			Next
		End If
	End Sub
	Private Sub New(数组 As Array)
		本体 = 数组
	End Sub
	Private Sub New(标量 As T)
		本体 = {标量}
	End Sub
	Friend Sub New(ParamArray Size As Integer())
		本体 = Array.CreateInstance(GetType(T), Size)
	End Sub
	''' <summary>
	''' 此转换会创建一个新数组
	''' </summary>
	''' <param name="本体">待转换的数组</param>
	''' <returns>尺寸相同的新数组，但元素类型可能改变</returns>
	Public Overloads Shared Widening Operator CType(数组 As Array) As Array(Of T)
		Dim b As SByte
		For b = 数组.Rank - 1 To 0 Step -1
			If 数组.GetLength(b) > 1 Then Exit For
		Next
		Dim c(b) As Integer
		For d As Byte = 0 To b
			c(d) = 数组.GetLength(d)
		Next
		Dim 本体 As Array = Array.CreateInstance(GetType(T), c)
		赋值递归(数组, 本体, c, 本体.Rank, 0, Zeros(Of Integer)(本体.Rank))
		Return New Array(Of T)(本体)
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
	''' <param name="TA">待转换的数组</param>
	''' <returns>和源数组是同一个数组，只是表示形式不同</returns>
	Overloads Shared Narrowing Operator CType(AT As Array(Of T)) As Array
		Return AT.本体
	End Operator

	Public Shared Operator +(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		If e.GetInterfaces.Contains(GetType(IPlusable)) Then
			Return BsxFun(Of T, T, T)(Function(c As IPlusable, d As Object) As T
										  Return c.Plus(d)
									  End Function, A, B)
		Else
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
				Case Else
					Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
												  Return c + d
											  End Function, A, B)
			End Select
		End If
	End Operator

	Public Shared Operator -(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		If e.GetInterfaces.Contains(GetType(IMinusable)) Then
			Return BsxFun(Of T, T, T)(Function(c As IMinusable, d As Object) As T
										  Return c.Minus(d)
									  End Function, A.本体, B.本体)
		Else
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
				Case Else
					Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
												  Return c - d
											  End Function, A, B)
			End Select
		End If
	End Operator

	Public Shared Operator *(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		If e.GetInterfaces.Contains(GetType(ITimesable)) Then
			Return BsxFun(Of T, T, T)(Function(c As ITimesable, d As Object) As T
										  Return c.Times(d)
									  End Function, A.本体, B.本体)
		Else
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
				Case Else
					Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
												  Return c * d
											  End Function, A, B)
			End Select
		End If
	End Operator

	Public Shared Operator /(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Dim e As Type = GetType(T)
		If e.GetInterfaces.Contains(GetType(IRDividable)) Then
			Return BsxFun(Of T, T, T)(Function(c As IRDividable, d As Object) As T
										  Return c.RDivide(d)
									  End Function, A.本体, B.本体)
		Else
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
				Case Else
					Return BsxFun(Of T, T, T)(Function(c As Double, d As Double) As Double
												  Return c / d
											  End Function, A, B)
			End Select
		End If
	End Operator
	Default Property SubsRA(subs As Integer()) As T
		Get
			Dim k As Integer = subs.Length
			If k = 1 AndAlso subs(0) < Numel Then
				Dim m As Integer = subs(0), n As Byte = NDims, o As Integer
				ReDim subs(n - 1)
				For d As Byte = 0 To n - 1
					o = Size(d)
					subs(d) = m Mod o
					If m = subs(d) Then
						Exit For
					Else
						m = (m - subs(d)) / o
					End If
				Next
			ElseIf k <> NDims Then
				ReDim Preserve subs(NDims - 1)
			End If
			Return 本体.GetValue(subs)
		End Get
		Set(value As T)
			Dim k As Integer = subs.Length
			If k = 1 AndAlso subs(0) < Numel Then
				Dim m As Integer = subs(0), n As Byte = NDims, o As Integer
				ReDim subs(n - 1)
				For d As Byte = 0 To n - 1
					o = Size(d)
					subs(d) = m Mod o
					If m = subs(d) Then
						Exit For
					Else
						m = (m - subs(d)) / o
					End If
				Next
				本体.SetValue(value, subs)
			Else
				Dim c As Byte = Math.Max(NDims, k), e(c - 1) As Integer, f(c - 1) As Integer, g As Integer, h As Integer, i As Boolean = False
				If c <> NDims Then i = True
				For d As Byte = 0 To c - 1
					g = Size(d)
					If d < k Then
						h = subs(d)
					Else
						h = 0
					End If
					If g > h Then
						e(d) = g
						f(d) = h
					Else
						e(d) = h + 1
						f(d) = h
						i = True
					End If
				Next
				If i Then
					Dim l As Array = Array.CreateInstance(GetType(T), e)
					SubsAsgn递归(本体, l, 0, Zeros(Of Integer)(NDims), Zeros(Of Integer)(c))
					本体 = l
				End If
				本体.SetValue(value, f)
			End If
		End Set
	End Property
	Default Property SubsRA(subs As Integer) As T
		Get
			Return SubsRA({subs})
		End Get
		Set(value As T)
			SubsRA({subs}) = value
		End Set
	End Property
	ReadOnly Property NDims As Byte
		Get
			Return 本体.Rank
		End Get
	End Property
	ReadOnly Property Numel As Integer
		Get
			Return 本体.Length
		End Get
	End Property
	ReadOnly Property [Class] As Type = GetType(T)
	Function Size() As Integer()
		Dim b As Byte = 本体.Rank - 1, c(b) As Integer
		For a = 0 To b
			c(a) = 本体.GetLength(a)
		Next
		Return c
	End Function
	Function Size([dim] As Byte) As Integer
		If [dim] < NDims Then
			Return 本体.GetLength([dim])
		Else
			Return 1
		End If
	End Function
	Public Overrides Function ToString() As String
		Dim a As New Text.StringBuilder
		Select Case NDims
			Case 1
				For Each b As T In 本体
					a.Append(b.ToString).Append(" ")
				Next
			Case 2
				a.AppendLine("列为第0维，行为第1维")
				For b As Integer = 0 To 本体.GetUpperBound(0)
					For c As Integer = 0 To 本体.GetUpperBound(1)
						a.Append(SubsRA({b, c}).ToString).Append(" ")
					Next
					a.AppendLine()
				Next
			Case Else
				a.AppendLine("列为第0维，行为第1维")
				ToString递归(a, Size, NDims, NDims - 1, Zeros(Of Integer)(NDims))
		End Select
		Return a.ToString
	End Function
	Function Cast(Of TOut)() As Array(Of TOut)
		Return 本体
	End Function
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
		If c IsNot Nothing Then Lengths(c) = Numel / b
		Dim d As New Array(Of T)(Lengths)
		Reshape递归(本体.GetEnumerator, f, Lengths, d, f - 1, Zeros(Of Integer)(f))
		Return d
	End Function
	Function Clone() As Array(Of T)
		Return New Array(Of T)(本体)
	End Function
	Function Permute(ParamArray dimorder As Byte()) As Array(Of T)
		Dim e As Byte = dimorder.Length, b(e - 1) As Integer
		For c As Byte = 0 To e - 1
			b(c) = Size(dimorder(c))
		Next
		Dim d As New Array(Of T)(b)
		Permute递归(本体, e, dimorder, b, d, 0, Zeros(Of Integer)(NDims), Zeros(Of Integer)(e))
		Return d
	End Function
End Class
