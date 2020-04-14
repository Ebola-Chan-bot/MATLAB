Public MustInherit Class TypedArray(Of T)
	Inherits BaseArray
	Friend 本体 As T()
	ReadOnly Property 原型 As T()
		Get
			Return 本体
		End Get
	End Property
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
		ReDim 本体(各维长度.Aggregate(Function(乘积 As Integer, 乘数 As Integer) 乘积 * 乘数) - 1)
	End Sub
	Sub New(各维长度 As Integer(), 原型 As T())
		MyBase.New(各维长度)
		本体 = 原型
	End Sub
	Public Overrides ReadOnly Property NumEl As Integer
		Get
			Return 本体.Length
		End Get
	End Property
	Protected Sub SubsRef递归(目标数组 As T(), 当前维度 As Byte, 源索引 As Integer, ByRef 目标索引 As Integer, 索引映射 As Integer()())
		Dim a As Integer() = 索引映射(当前维度)
		源索引 *= 各维长度(当前维度)
		If 当前维度 > 0 Then
			For b As Integer = 0 To a.Length - 1
				SubsRef递归(目标数组, 当前维度 - 1, 源索引 + a(b), 目标索引, 索引映射)
			Next
		Else
			For b As Integer = 0 To a.Length - 1
				目标数组(目标索引) = 本体(源索引 + a(b))
				目标索引 += 1
			Next
		End If
	End Sub
	Private Sub SubsAsgn递归(源数组 As T(), 当前维度 As Byte, ByRef 源索引 As Integer, 目标索引 As Integer, 索引映射 As Integer()())
		Dim a As Integer() = 索引映射(当前维度)
		目标索引 *= 各维长度(当前维度)
		If 当前维度 > 0 Then
			For b As Integer = 0 To a.Length - 1
				SubsAsgn递归(源数组, 当前维度 - 1, 源索引, 目标索引 + a(b), 索引映射)
			Next
		Else
			For b As Integer = 0 To a.Length - 1
				本体(目标索引 + a(b)) = 源数组(源索引)
				源索引 += 1
			Next
		End If
	End Sub
	''' <summary>
	''' 用双层数组进行索引，第一层排列不同的维度，第二层是在该维度内要提取的切片
	''' </summary>
	Default WriteOnly Property SubsRA(subs As Integer()()) As TypedArray(Of T)
		Set(value As TypedArray(Of T))
			SubsAsgn递归(value.本体, Math.Min(NDims, subs.Length) - 1, 0, 0, subs)
		End Set
	End Property
	''' <summary>
	''' 用冒号表达式进行索引，用Nothing表示该维度下标上限
	''' </summary>
	Default WriteOnly Property SubsRA(subs As IntegerColon()) As TypedArray(Of T)
		Set(value As TypedArray(Of T))
			Dim b As Byte = Math.Min(NDims, subs.Length) - 1, c(b)() As Integer
			For a As Byte = 0 To b
				c(a) = subs(a).ToIndex(各维长度(a) - 1)
			Next
			SubsRA(c) = value
		End Set
	End Property
	''' <summary>
	''' 用逻辑数组进行索引，将对应位置为True的值修改为特定标量
	''' </summary>
	Default WriteOnly Property SubsRA(subs As TypedArray(Of Boolean)) As T
		Set(value As T)
			本体 = 本体.Zip(subs.本体, Function(a As T, b As Boolean) If(b, value, a)).ToArray
		End Set
	End Property
End Class
