Public Class SingleArray
	Inherits TypedArray(Of Single)
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
	End Sub
	Sub New(各维长度 As Integer(), 原型 As Single())
		MyBase.New(各维长度, 原型)
	End Sub
	Sub New(原型 As Single())
		MyBase.New({原型.Length}, 原型)
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New SingleArray(各维长度.Clone, 本体.Clone)
	End Function
	Default Overloads ReadOnly Property SubsRA(subs As Integer()()) As SingleArray
		Get
			Dim b As New SingleArray((From a As Integer() In subs Select a.Length).ToArray)
			SubsRef递归(b.本体, b.NDims - 1, 0, 0, subs)
			Return b
		End Get
	End Property
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
