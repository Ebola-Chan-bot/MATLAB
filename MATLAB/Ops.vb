Imports System.Runtime.CompilerServices
Public Interface IPlusable
	Function Plus(B As IPlusable) As IPlusable
End Interface
Public Interface IMinusable
	Function Minus(B As IMinusable) As IMinusable
End Interface
Public Interface ITimesable
	Function Times(B As ITimesable) As ITimesable
End Interface
Public Interface IRDividable
	Function RDivide(B As IRDividable) As IRDividable
End Interface
Public Module Ops
	''' <summary>
	''' 创建等差数列，类似于MATLAB三元冒号运算符
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="i">公差</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Private Function IColon(j As Double, i As Double, k As Double) As Array(Of Double)
		Dim c As Long = Math.Floor((k - j) / i) + 1, a(c - 1) As Double
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
	Public Function Colon(Of T)(j As T, i As T, k As T) As Array(Of T)
		Return IColon(DirectCast(j, Object), DirectCast(i, Object), DirectCast(k, Object)).Cast(Of T)
	End Function
	''' <summary>
	''' 创建公差为1的等差数列，指定返回值类型，类似于MATLAB二元冒号运算符
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(Of T)(j As T, k As T) As Array(Of T)
		Return IColon(DirectCast(j, Object), 1, DirectCast(k, Object)).Cast(Of T)
	End Function
	''' <summary>
	''' 数组加法，每个元素的位置对应相加产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型、可转换为<see cref="Double"/>或实现<see cref="IPlusable"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被加数组</param>
	''' <param name="B">加数组</param>
	''' <returns>和数组</returns>
	Public Function Plus(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A + B
	End Function
	''' <summary>
	''' 数组减法，每个元素的位置对应相减产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型、可转换为<see cref="Double"/>或实现<see cref="IMinusable"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被减数组</param>
	''' <param name="B">减数组</param>
	''' <returns>差数组</returns>
	Public Function Minus(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A - B
	End Function
	''' <summary>
	''' 数组点乘法，每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型、可转换为<see cref="Double"/>或实现<see cref="ITimesable"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">乘数组1</param>
	''' <param name="B">乘数组2</param>
	''' <returns>积数组</returns>
	Public Function Times(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A * B
	End Function
	''' <summary>
	''' 数组右除,每个元素的位置对应相乘产生新数组，如果尺寸不匹配则循环填充扩展。数组元素必须为基本数值类型、可转换为<see cref="Double"/>或实现<see cref="IRDividable"/>
	''' </summary>
	''' <typeparam name="T">返回元素类型</typeparam>
	''' <param name="A">被除数组</param>
	''' <param name="B">除数组</param>
	''' <returns>商数组</returns>
	Public Function RDivide(Of T)(A As Array(Of T), B As Array(Of T)) As Array(Of T)
		Return A / B
	End Function
End Module