Imports System.Runtime.CompilerServices
Public Module Ops
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
	''' <summary>
	''' 安全的下标索引赋值。如果下标数组的长度大于目标数组的维数，将忽略多余的下标值而不是报错。
	''' </summary>
	''' <param name="A">索引操作中使用的数组</param>
	''' <param name="B">所赋的值</param>
	''' <param name="S">下标数组</param>
	<Extension> Public Sub SubsAsgn(A As Array, B As Object, ParamArray S As Integer())
		If S.Length > A.Rank Then
			A.SetValue(B, S.Take(A.Rank).ToArray)
		Else
			A.SetValue(B, S)
		End If
	End Sub
	''' <summary>
	''' 安全的下标引用。如果下标数组的长度大于目标数组的维数，将忽略多余的下标值而不是报错。
	''' </summary>
	''' <param name="A">索引对象数组</param>
	''' <param name="S">下标数组</param>
	''' <returns>索引表达式的结果</returns>
	<Extension> Public Function SubsRef(A As Array, ParamArray S As Integer())
		If S.Length > A.Rank Then
			Return A.GetValue(S.Take(A.Rank).ToArray)
		Else
			Return A.GetValue(S)
		End If
	End Function
End Module
