Imports System.Runtime.CompilerServices, MathNet.Numerics.LinearAlgebra
Public Module 通用工具
	''' <summary>
	''' 返回一系列数组各维长度的最大值
	''' </summary>
	Function 尺寸适配(ParamArray 数组 As BaseArray()) As Integer()
		Dim a(数组.Max(Function(b As BaseArray) b.NDims) - 1) As Integer, e As Integer()
		For Each c As BaseArray In 数组
			e = c.各维长度
			For d As Byte = 0 To c.NDims - 1
				a(d) = Math.Max(e(d), a(d))
			Next
		Next
		Return a
	End Function
	Friend Sub BsxFun递归(Of TIn1, TIn2, TOut)(fun As Func(Of TIn1, TIn2, TOut), A As TypedArray(Of TIn1), B As TypedArray(Of TIn2), 目标数组 As IList(Of TOut), 当前维度 As Byte, A索引 As Integer, B索引 As Integer, 目标索引 As Integer, 各维长度 As Integer())
		Dim d As Integer = A.Size(当前维度), e As Integer = B.Size(当前维度), f As Integer = 各维长度(当前维度)
		A索引 *= d
		B索引 *= e
		目标索引 *= f
		If 当前维度 > 0 Then
			If d < f OrElse e < f Then
				For c = 0 To f - 1
					BsxFun递归(fun, A, B, 目标数组, 当前维度 - 1, A索引 + (c Mod d), B索引 + (c Mod e), 目标索引 + c, 各维长度)
				Next
			Else
				For c = 0 To f - 1
					BsxFun递归(fun, A, B, 目标数组, 当前维度 - 1, A索引 + c, B索引 + c, 目标索引 + c, 各维长度)
				Next
			End If
		Else
			If d < f OrElse e < f Then
				For c = 0 To f - 1
					目标数组(目标索引 + c) = fun.Invoke(A.本体(A索引 + (c Mod d)), B.本体(B索引 + (c Mod e)))
				Next
			Else
				For c = 0 To f - 1
					目标数组(目标索引 + c) = fun.Invoke(A.本体(A索引 + c), B.本体(B索引 + c))
				Next
			End If
		End If
	End Sub
	Private Sub 累积递归(Of TIn, TOut)(源数组 As IList(Of TIn), 当前维度 As Byte, 累积维长 As Integer(), 当前索引 As Integer, 累积维权 As Integer(), 累积器 As I累积器(Of TIn, TOut))
		Dim b As Integer = 累积维权(当前维度)
		If 当前维度 > 0 Then
			For a As Integer = 0 To 累积维长(当前维度) - 1
				累积递归(源数组, 当前维度 - 1, 累积维长, 当前索引, 累积维权, 累积器)
				当前索引 += b
			Next
		Else
			For a As Integer = 0 To 累积维长(当前维度) - 1
				累积器.累积(源数组(当前索引))
				当前索引 += b
			Next
		End If
	End Sub
	Private Sub 拆分递归(Of TIn, TOut)(源数组 As IList(Of TIn), 目标数组 As TOut(), 当前维度 As Byte, 累积维长 As Integer(), 累积维权 As Integer(), 拆分维长 As Integer(), 源拆分维权 As Integer(), 目标拆分维权 As Integer(), 源索引 As Integer, 目标索引 As Integer, 累积器 As I累积器(Of TIn, TOut))
		Dim b As Integer = 源拆分维权(当前维度), c As Integer = 目标拆分维权(当前维度)
		If 当前维度 > 0 Then
			For a As Integer = 0 To 拆分维长(当前维度) - 1
				拆分递归(源数组, 目标数组, 当前维度 - 1, 累积维长, 累积维权, 拆分维长, 源拆分维权, 目标拆分维权, 源索引, 目标索引, 累积器)
				源索引 += b
				目标索引 += c
			Next
		Else
			For a As Integer = 0 To 拆分维长(当前维度) - 1
				累积递归(源数组, 累积维长.Length - 1, 累积维长, 源索引, 累积维权, 累积器)
				目标数组(目标索引) = 累积器.结果
				源索引 += b
				目标索引 += c
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
	<Extension> Function 累积降维(Of TIn, TOut)(源数组 As TypedArray(Of TIn), 累积器 As I累积器(Of TIn, TOut), ParamArray 累积维度 As Byte()) As (Integer(), TOut())
		Dim a As Integer() = 源数组.各维长度, b As Byte = a.Length - 1, c(b) As Integer, d As SByte, g As Integer() = a.Clone
		c(0) = 1
		For d = 0 To b - 1
			c(d + 1) = c(d) * a(d)
		Next
		Dim f(b) As Byte, m As Integer = c(b) * a(b)
		For d = 0 To b
			f(d) = d
		Next
		f = f.Except(累积维度).ToArray
		b = 累积维度.Length - 1
		Dim h(b) As Integer, i(b) As Integer
		For d = 0 To b
			h(d) = a(累积维度(d))
			i(d) = c(累积维度(d))
			g(累积维度(d)) = 1
			m /= h(d)
		Next
		b = f.Length - 1
		Dim k(b) As Integer, l(b) As Integer, j(b) As Integer
		j(0) = 1
		For d = 0 To b - 1
			j(d + 1) = j(d) * g(f(d))
		Next
		For d = 0 To b
			k(d) = a(f(d))
			l(d) = c(f(d))
		Next
		Dim e(m - 1) As TOut
		拆分递归(源数组.本体, e, b, h, i, k, l, j, 0, 0, 累积器)
		Return (g, e)
	End Function
	Friend Function 核心Cat(Of T)(维度 As SByte, A As TypedArray(Of T)()) As (Integer(), T())
		Dim b As TypedArray(Of T) = A(0), c As Byte = b.NDims - 1, d As Byte = Math.Max(维度, c), e(d) As Integer, f As SByte
		b.各维长度.CopyTo(e, 0)
		For f = c + 1 To d
			e(f) = 1
		Next
		Dim g As Integer = 1
		For f = 0 To 维度 - 1
			g *= e(f)
		Next
		c = A.Length - 1
		Dim i(c) As Integer, j As Integer
		e(维度) = 0
		For f = 0 To c
			j = A(f).Size(维度)
			i(f) = g * j
			e(维度) += j
		Next
		j = 1
		For f = 维度 + 1 To d
			j *= e(f)
		Next
		Dim k As Integer = g * e(维度), l(g * e(维度) * j - 1) As T, m As IList(Of T), p As Integer, q As Integer, r As Integer, h As Integer, n As Integer, o As Integer
		j -= 1
		For f = 0 To c
			m = A(f).本体
			r = 0
			g = p
			h = i(f)
			o = h - 1
			For n = 0 To j
				For q = 0 To o
					l(g + q) = m(r + q)
				Next
				g += k
				r += h
			Next
			p += h
		Next
		Return (e, l)
	End Function
	Friend Function 核心BsxFun(Of TIn1, TIn2, TOut)(fun As Func(Of TIn1, TIn2, TOut), A As TypedArray(Of TIn1), B As TypedArray(Of TIn2)) As (Integer(), TOut())
		Dim c As Integer() = A.Size.ToArray
		If c.SequenceEqual(B.Size) Then
			Return (c, A.本体.Zip(B.本体, fun).ToArray)
		Else
			c = 尺寸适配(A, B)
			Dim d(c.Aggregate(Function(e As Integer, f As Integer) e * f)) As TOut
			BsxFun递归(fun, A, B, d, c.Length - 1, 0, 0, 0, c)
			Return (c, d)
		End If
	End Function
	Friend Function 核心BsxFun(Of T As {Structure, IEquatable(Of T), IFormattable})(fun As Func(Of T, T, T), A As NumericArray(Of T), B As NumericArray(Of T)) As (Integer(), Vector(Of T))
		Dim c As Integer() = A.Size.ToArray
		If c.SequenceEqual(B.Size) Then
			Return (c, A.o本体.Map2(fun, B.o本体))
		Else
			c = 尺寸适配(A, B)
			Dim d As Vector(Of T) = CreateVector.Dense(Of T)(c.Aggregate(Function(e As Integer, f As Integer) e * f))
			BsxFun递归(fun, A, B, d, c.Length - 1, 0, 0, 0, c)
			Return (c, d)
		End If
	End Function
End Module
