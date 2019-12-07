Public Module Ops
#Region "Public"
	''' <summary>
	''' 创建等差数列，类似于MATLAB三元冒号运算符
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="i">公差</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As Decimal, i As Decimal, k As Decimal) As Decimal()
		Dim c As Long = Math.Floor((k - j) / i) + 1, a(c) As Decimal
		a(0) = j
		For b As Long = 1 To c - 1
			a(b) = a(b - 1) + i
		Next
		Return a
	End Function
	''' <summary>
	''' 创建等差数列，指定返回值类型，类似于MATLAB三元冒号运算符
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="i">公差</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(Of T)(j As Decimal, i As Decimal, k As Decimal) As T()
		Return Colon(j, i, k).Cast(Of T)
	End Function
	''' <summary>
	''' 创建公差为1的等差数列，类似于MATLAB二元冒号运算符
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As Decimal, k As Decimal) As Decimal()
		Return Colon(j, 1, k)
	End Function
	''' <summary>
	''' 创建公差为1的等差数列，指定返回值类型，类似于MATLAB二元冒号运算符
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(Of T)(j As Decimal, k As Decimal) As T()
		Return Colon(j, 1, k).Cast(Of T)
	End Function
	Public Function Plus(A As Array, B As Array) As Array
		Return Bsxfun(Function(c As Double, d As Double) c + d, A, B)
	End Function
#End Region
End Module
