Imports MathNet.Numerics.LinearAlgebra
Public Class SingleArray
	Inherits NumericArray(Of Single)
	Friend i本体 As [Single].DenseVector
	Protected Friend Overrides ReadOnly Property o本体 As Vector(Of Single)
		Get
			Return i本体
		End Get
	End Property
	''' <summary>
	''' 创建一个各维为指定长度的全零数组
	''' </summary>
	Sub New(各维长度 As Integer())
		MyBase.New(各维长度)
		i本体 = New [Single].DenseVector(NumEl)
	End Sub
	''' <summary>
	''' 创建一个各维为指定长度，具有给定的一维原型的数组
	''' </summary>
	Sub New(各维长度 As Integer(), 原型 As Single())
		MyBase.New(各维长度)
		i本体 = New [Single].DenseVector(原型)
	End Sub
	''' <summary>
	''' 创建一个具有给定原型的一维数组
	''' </summary>
	Sub New(原型 As Single())
		MyBase.New({原型.Length})
		i本体 = New [Single].DenseVector(原型)
	End Sub
	Friend Sub New(各维长度 As Integer(), 原型 As Vector(Of Single))
		MyBase.New(各维长度)
		i本体 = 原型
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New SingleArray(各维长度.Clone, i本体)
	End Function
	''' <summary>
	''' 用双层数组进行索引，第一层排列不同的维度，第二层是在该维度内要赋值的切片
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As Integer()()) As SingleArray
		Get
			Dim a As (Integer(), Single()) = SubsRef(subs)
			Return New SingleArray(a.Item1, a.Item2)
		End Get
	End Property
	''' <summary>
	''' 用冒号表达式进行索引，用Nothing表示该维度下标上限
	''' </summary>
	Default Overloads ReadOnly Property SubsRA(subs As IntegerColon()) As SingleArray
		Get
			Dim a As (Integer(), Single()) = SubsRef(subs)
			Return New SingleArray(a.Item1, a.Item2)
		End Get
	End Property

	Overloads Shared Operator +(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, A.i本体 + B)
	End Operator

	Overloads Shared Operator +(A As SingleArray, B As NumericArray(Of Single)) As SingleArray
		If A.Size.SequenceEqual(B.Size) Then
			Return New SingleArray(A.Size.ToArray, A.i本体 + B.o本体)
		Else
			Return BsxFun(Function(c As Single, d As Single) c + d, A, B)
		End If
	End Operator

	Overloads Shared Operator -(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, A - B.i本体)
	End Operator

	Overloads Shared Operator -(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, A.i本体 - B)
	End Operator

	Overloads Shared Operator -(A As SingleArray, B As NumericArray(Of Single)) As SingleArray
		If A.Size.SequenceEqual(B.Size) Then
			Return New SingleArray(A.Size.ToArray, A.i本体 - B.o本体)
		Else
			Return BsxFun(Function(c As Single, d As Single) c - d, A, B)
		End If
	End Operator

	Overloads Shared Operator *(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, A * B.o本体)
	End Operator

	Overloads Shared Operator *(A As SingleArray, B As NumericArray(Of Single)) As SingleArray
		If A.Size.SequenceEqual(B.Size) Then
			Return New SingleArray(A.Size.ToArray, A.i本体.PointwiseMultiply(B.o本体))
		Else
			Return BsxFun(Function(c As Single, d As Single) c * d, A, B)
		End If
	End Operator

	Overloads Shared Operator /(A As Single, B As SingleArray) As SingleArray
		Return New SingleArray(B.Size.ToArray, A / B.o本体)
	End Operator

	Overloads Shared Operator /(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, A.i本体 / B)
	End Operator

	Overloads Shared Operator /(A As SingleArray, B As SingleArray) As SingleArray
		If A.Size.SequenceEqual(B.Size) Then
			Return New SingleArray(A.Size.ToArray, A.i本体.PointwiseDivide(B.o本体))
		Else
			Return BsxFun(Function(c As Single, d As Single) c / d, A, B)
		End If
	End Operator

	Overloads Shared Operator ^(A As SingleArray, B As Single) As SingleArray
		Return New SingleArray(A.Size.ToArray, A.i本体.PointwisePower(B))
	End Operator
End Class
