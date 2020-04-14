Public Class BooleanArray
	Inherits TypedArray(Of Boolean)
	Sub New(各维长度 As Integer(), 原型 As Boolean())
		MyBase.New(各维长度, 原型)
	End Sub
	Public Overrides Function Clone() As BaseArray
		Return New BooleanArray(各维长度.Clone, 本体.Clone)
	End Function
End Class
