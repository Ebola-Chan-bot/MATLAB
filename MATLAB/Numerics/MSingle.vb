Public Structure MSingle
	Implements INumeric
	Private 数值 As Single
	Shared ReadOnly Property NaN As MSingle = New MSingle(Single.NaN)
	Sub New(数值 As Single)
		Me.数值 = 数值
	End Sub
	Sub New(数值 As INumeric)
		Me.数值 = 数值.RawData
	End Sub
	Public ReadOnly Property RawData As Object Implements INumeric.RawData
		Get
			Return 数值
		End Get
	End Property

	Shared Widening Operator CType(数值 As Single) As MSingle
		Return New MSingle(数值)
	End Operator

	Shared Widening Operator CType(数值 As MSingle) As Single
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MSingle) As MDecimal
		Return 数值.数值
	End Operator

	Shared Widening Operator CType(数值 As MSingle) As MDouble
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MInt8
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MInt16
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MInt32
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MInt64
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MUInt8
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MUInt16
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MUInt32
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MSingle) As MUInt64
		Return 数值.数值
	End Operator

	Shared Operator +(A As MSingle, B As INumeric) As MSingle
		Return A.数值 + CSng(B.RawData)
	End Operator

	Shared Operator -(A As MSingle, B As INumeric) As MSingle
		Return A.数值 - CSng(B.RawData)
	End Operator

	Shared Operator *(A As MSingle, B As INumeric) As MSingle
		Return A.数值 * CSng(B.RawData)
	End Operator

	Shared Operator /(A As MSingle, B As INumeric) As MSingle
		Return A.数值 / CSng(B.RawData)
	End Operator

	Shared Operator >(A As MSingle, B As INumeric) As Boolean
		Return A.数值 > CSng(B.RawData)
	End Operator

	Shared Operator <(A As MSingle, B As INumeric) As Boolean
		Return A.数值 < CSng(B.RawData)
	End Operator

	Public Function Plus(B As INumeric) As INumeric Implements INumeric.Plus
		Return Me + B
	End Function

	Public Function Minus(B As INumeric) As INumeric Implements INumeric.Minus
		Return Me - B
	End Function

	Public Function Times(B As INumeric) As INumeric Implements INumeric.Times
		Return Me * B
	End Function

	Public Function RDivide(B As INumeric) As INumeric Implements INumeric.RDivide
		Return Me / B
	End Function

	Public Function Gt(B As INumeric) As Boolean Implements INumeric.Gt
		Return Me > B
	End Function

	Public Function Lt(B As INumeric) As Boolean Implements INumeric.Lt
		Return Me < B
	End Function

	Public Function Min(B As INumeric) As INumeric Implements INumeric.Min
		Return New MSingle(Math.Min(数值, CSng(B.RawData)))
	End Function

	Public Function Max(B As INumeric) As INumeric Implements INumeric.Max
		Return New MSingle(Math.Max(数值, CSng(B.RawData)))
	End Function

	Public Overrides Function Equals(obj As Object) As Boolean
		If obj.GetType.GetInterface("INumeric") Is Nothing Then
			Return 数值.Equals(CSng(obj))
		Else
			Return 数值.Equals(CSng(DirectCast(obj, INumeric).RawData))
		End If
	End Function

	Public Shared Operator =(left As MSingle, right As INumeric) As Boolean
		Return left.数值.Equals(CSng(right.RawData))
	End Operator

	Public Shared Operator <>(left As MSingle, right As INumeric) As Boolean
		Return left.数值.Equals(CSng(right.RawData))
	End Operator

	Public Sub SetValue(value As INumeric) Implements INumeric.SetValue
		数值 = value.RawData
	End Sub

	Public Sub SetValue(value As Object) Implements INumeric.SetValue
		数值 = value
	End Sub
	Shared Function CastFrom(ParamArray 数组 As Single()) As MSingle()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As Single) New MSingle(a)).ToArray
	End Function
	Shared Function CastBack(ParamArray 数组 As MSingle()) As Single()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As MSingle) a.数值).ToArray
	End Function
	Public Overrides Function ToString() As String
		Return 数值
	End Function
End Structure
