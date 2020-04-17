Imports MathNet.Numerics.LinearAlgebra
Public Class NumericArray(Of T As {Structure, IEquatable(Of T), IFormattable})
	Inherits TypedArray(Of T)
	Protected Friend Overridable ReadOnly Property o本体 As Vector(Of T)
	Public NotOverridable Overrides ReadOnly Property 本体 As IList(Of T)
		Get
			Return o本体
		End Get
	End Property
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
	End Sub
	Sub New(各维长度 As Integer(), 本体 As T())
		MyBase.New(各维长度)
		o本体 = CreateVector.DenseOfArray(本体)
	End Sub
	Sub New(各维长度 As Integer(), 本体 As Vector(Of T))
		MyBase.New(各维长度)
		o本体 = 本体
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New NumericArray(Of T)(各维长度.Clone, o本体)
	End Function
	''' <summary>
	''' 用双层数组进行索引，第一层排列不同的维度，第二层是在该维度内要赋值的切片
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As Integer()()) As NumericArray(Of T)
		Get
			Dim a As (Integer(), T()) = SubsRef(subs)
			Return New NumericArray(Of T)(a.Item1, a.Item2)
		End Get
	End Property
	''' <summary>
	''' 用冒号表达式进行索引，用Nothing表示该维度下标上限
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As IntegerColon()) As NumericArray(Of T)
		Get
			Dim a As (Integer(), T()) = SubsRef(subs)
			Return New NumericArray(Of T)(a.Item1, a.Item2)
		End Get
	End Property

	Shared Operator +(A As NumericArray(Of T), B As T) As NumericArray(Of T)
		Return New NumericArray(Of T)(A.Size.ToArray, A.o本体 + B)
	End Operator

	Shared Operator -(A As T, B As NumericArray(Of T)) As NumericArray(Of T)
		Return New NumericArray(Of T)(B.Size.ToArray, A - B.o本体)
	End Operator

	Shared Operator -(A As NumericArray(Of T), B As T) As NumericArray(Of T)
		Return New NumericArray(Of T)(A.Size.ToArray, A.o本体 - B)
	End Operator

	Shared Operator *(A As T, B As NumericArray(Of T)) As NumericArray(Of T)
		Return New NumericArray(Of T)(B.Size.ToArray, A * B.o本体)
	End Operator

	Shared Operator /(A As T, B As NumericArray(Of T)) As NumericArray(Of T)
		Return New NumericArray(Of T)(B.Size.ToArray, A / B.o本体)
	End Operator

	Shared Operator /(A As NumericArray(Of T), B As T) As NumericArray(Of T)
		Return New NumericArray(Of T)(A.Size.ToArray, A.o本体 / B)
	End Operator

	Shared Operator ^(A As NumericArray(Of T), B As T) As NumericArray(Of T)
		Return New NumericArray(Of T)(A.Size.ToArray, A.o本体.PointwisePower(B))
	End Operator
End Class
