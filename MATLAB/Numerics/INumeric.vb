''' <summary>
''' 所有支持数组运算符的元素类型必须实现此接口。对于内置数据类型，<see cref="MSingle"/>和<see cref="MDouble"/>转化为非浮点数类型时，NaN将转换为0。较大的数值转换为不包含此数值的数据类型时，将转为最接近此值的边界值。无论何种情况，都不会报错。
''' </summary>
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
