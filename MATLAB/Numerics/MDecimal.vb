Public Structure MDecimal
	Implements INumeric
	Private 数值 As Decimal

	Sub New(数值 As Decimal)
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

	Shared Widening Operator CType(数值 As Decimal) As MDecimal
		Return New MDecimal(数值)
	End Operator

	Shared Widening Operator CType(数值 As MDecimal) As Decimal
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MDouble
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MSingle
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MInt8
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MInt16
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MInt32
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MInt64
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MUInt8
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MUInt16
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MUInt32
		Return 数值.数值
	End Operator

	Shared Narrowing Operator CType(数值 As MDecimal) As MUInt64
		Return 数值.数值
	End Operator

	Shared Operator +(A As MDecimal, B As INumeric) As MDecimal
		Return A.数值 + CDec(B.RawData)
	End Operator

	Shared Operator -(A As MDecimal, B As INumeric) As MDecimal
		Return A.数值 - CDec(B.RawData)
	End Operator

	Shared Operator *(A As MDecimal, B As INumeric) As MDecimal
		Return A.数值 * CDec(B.RawData)
	End Operator

	Shared Operator /(A As MDecimal, B As INumeric) As MDecimal
		Return A.数值 / CDec(B.RawData)
	End Operator

	Shared Operator >(A As MDecimal, B As INumeric) As Boolean
		Return A.数值 > CDec(B.RawData)
	End Operator

	Shared Operator <(A As MDecimal, B As INumeric) As Boolean
		Return A.数值 < CDec(B.RawData)
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
		Return New MDecimal(Math.Min(数值, CDec(B.RawData)))
	End Function

	Public Function Max(B As INumeric) As INumeric Implements INumeric.Max
		Return New MDecimal(Math.Max(数值, CDec(B.RawData)))
	End Function

	Public Overrides Function Equals(obj As Object) As Boolean
		If obj.GetType.GetInterface("INumeric") Is Nothing Then
			Return 数值.Equals(CDec(obj))
		Else
			Return 数值.Equals(CDec(DirectCast(obj, INumeric).RawData))
		End If
	End Function

	Public Shared Operator =(left As MDecimal, right As INumeric) As Boolean
		Return left.数值.Equals(CDec(right.RawData))
	End Operator

	Public Shared Operator <>(left As MDecimal, right As INumeric) As Boolean
		Return left.数值.Equals(CDec(right.RawData))
	End Operator

	Public Sub SetValue(value As INumeric) Implements INumeric.SetValue
		数值 = value.RawData
	End Sub

	Public Sub SetValue(value As Object) Implements INumeric.SetValue
		数值 = value
	End Sub
	Shared Function CastFrom(ParamArray 数组 As Decimal()) As MDecimal()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As Decimal) New MDecimal(a)).ToArray
	End Function
	Shared Function CastBack(ParamArray 数组 As MDecimal()) As Decimal()
		Return 数组.AsParallel.AsOrdered.Select(Function(a As MDecimal) a.数值).ToArray
	End Function
	Public Overrides Function ToString() As String
		Return 数值
	End Function
End Structure
