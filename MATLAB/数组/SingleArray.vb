Public Class SingleArray
	Inherits TypedArray(Of Single)
	''' <summary>
	''' 创建一个各维为指定长度的全零数组
	''' </summary>
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
	End Sub
	''' <summary>
	''' 创建一个各维为指定长度，具有给定的一维原型的数组
	''' </summary>
	Sub New(各维长度 As Integer(), 原型 As Single())
		MyBase.New(各维长度, 原型)
	End Sub
	''' <summary>
	''' 创建一个具有给定原型的一维数组
	''' </summary>
	Sub New(原型 As Single())
		MyBase.New({原型.Length}, 原型)
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New SingleArray(各维长度.Clone, 本体.Clone)
	End Function
	''' <summary>
	''' 用双层数组进行索引，第一层排列不同的维度，第二层是在该维度内要赋值的切片
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As Integer()()) As SingleArray
		Get
			Dim b As New SingleArray((From a As Integer() In subs Select a.Length).ToArray)
			SubsRef递归(b.本体, b.NDims - 1, 0, 0, subs)
			Return b
		End Get
	End Property
	''' <summary>
	''' 用冒号表达式进行索引，用Nothing表示该维度下标上限
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As IntegerColon()) As SingleArray
		Get
			Dim b As Byte = Math.Min(NDims, subs.Length) - 1, c(b)() As Integer
			For a As Byte = 0 To b
				c(a) = subs(a).ToIndex(各维长度(a) - 1)
			Next
			Return SubsRA(c)
		End Get
	End Property

	Shared Operator +(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From c As Single In A.本体 Select c + B).ToArray)
	End Operator

	Shared Operator +(A As SingleArray, B As TypedArray(Of Single)) As SingleArray
		Return BsxFun(Function(c As Single, d As Single) c + d, A, B)
	End Operator

	Shared Operator -(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, (From c As Single In B.本体 Select A - c).ToArray)
	End Operator

	Shared Operator -(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From c As Single In A.本体 Select c - B).ToArray)
	End Operator

	Shared Operator -(A As SingleArray, B As TypedArray(Of Single)) As SingleArray
		Return BsxFun(Function(c As Single, d As Single) c - d, A, B)
	End Operator

	Shared Operator *(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, (From c As Single In B.本体 Select A * c).ToArray)
	End Operator

	Shared Operator *(A As SingleArray, B As TypedArray(Of Single)) As SingleArray
		Return BsxFun(Function(c As Single, d As Single) c * d, A, B)
	End Operator

	Shared Operator /(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, (From c As Single In B.本体 Select A / c).ToArray)
	End Operator

	Shared Operator /(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From c As Single In A.本体 Select c / B).ToArray)
	End Operator

	Shared Operator /(A As SingleArray, B As SingleArray) As SingleArray
		Return BsxFun(Function(c As Single, d As Single) c / d, A, B)
	End Operator

	Shared Operator ^(A As SingleArray, B As Double) As SingleArray
		Return New SingleArray(A.Size.ToArray, (From c As Single In A.本体 Select CSng(c ^ B)).ToArray)
	End Operator
End Class
