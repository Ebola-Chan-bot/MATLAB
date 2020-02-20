Public Interface INumeric
	ReadOnly Property RawData As Object
	Function Plus(B As INumeric) As INumeric
	Function Minus(B As INumeric) As INumeric
	Function Times(B As INumeric) As INumeric
	Function RDivide(B As INumeric) As INumeric
	Function Gt(B As INumeric) As Boolean
	Function Lt(B As INumeric) As Boolean
	Function Min(B As INumeric) As INumeric
	Function Max(B As INumeric) As INumeric
	Sub SetValue(value As INumeric)
	Sub SetValue(value As Object)
End Interface
