Imports System.Runtime.CompilerServices
Public Module Ops
	''' <summary>
	''' 创建公差为1的等差数列
	''' </summary>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>等差数列</returns>
	Public Function Colon(j As Decimal, k As Decimal) As Decimal()
		Dim c As Long = Math.Floor(k - j + 1), a(c) As Decimal
		For b As Long = 0 To c - 1
			a(b) = j + b
		Next
		Return a
	End Function
	''' <summary>
	''' 创建公差为1的等差数列，指定返回值类型，类似于MATLAB二元冒号运算符
	''' </summary>
	''' <typeparam name="T">返回值类型</typeparam>
	''' <param name="j">起点</param>
	''' <param name="k">终点</param>
	''' <returns>指定类型的等差数列</returns>
	Public Function Colon(Of T)(j As Decimal, k As Decimal) As T()
		Return Colon(j, k).Cast(Of T)
	End Function
End Module
