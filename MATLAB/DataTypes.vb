Imports System.Runtime.CompilerServices
Public Module DataTypes
	Public Delegate Function ArrayFun委托(Of In TIn, Out TOut)(输入 As TIn) As TOut
	Public Delegate Sub ArrayFunErrorHandler(Of TIn)(错误 As Exception, 索引 As Integer(), 输入 As TIn)
	Private Sub ArrayFun递归(Of TIn, TOut)(输入数组 As Array, 输出数组 As Array, 函数 As ArrayFun委托(Of TIn, TOut), 当前维度 As Byte, 当前索引 As Integer())
		If 当前维度 < 输出数组.Rank - 1 Then
			For a As Integer = 0 To 输出数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				ArrayFun递归(输入数组, 输出数组, 函数, 当前维度 + 1, 当前索引)
			Next
		Else
			For a As Integer = 0 To 输出数组.GetUpperBound(当前维度)
				当前索引(当前维度) = a
				输出数组.SetValue(函数.Invoke(输入数组.GetValue(当前索引)), 当前索引)
			Next
		End If
	End Sub
	''' <summary>
	''' 数组元素的类
	''' </summary>
	''' <param name="数组">数组</param>
	''' <returns>类</returns>
	<Extension> Public Function [Class](数组 As IEnumerable) As Type
		Dim a As IEnumerator = 数组.GetEnumerator
		a.MoveNext()
		Return a.Current.GetType
	End Function
	''' <summary>
	''' 将数组转换为不同的数据类型
	''' </summary>
	''' <typeparam name="TIn">原来类</typeparam>
	''' <typeparam name="newclass">目标类</typeparam>
	''' <param name="A">要转换的数组</param>
	''' <returns>转换后的数组</returns>
	Public Function Cast(Of TIn, newclass)(A As Array(Of TIn)) As Array(Of newclass)
		Return A.Cast(Of newclass)
	End Function
	''' <summary>
	''' 将函数应用于每个数组元素。
	''' </summary>
	''' <typeparam name="TIn">输入数组元素类型</typeparam>
	''' <typeparam name="TOut">输出数组元素类型</typeparam>
	''' <param name="func">要执行的函数</param>
	''' <param name="A">输入数组</param>
	''' <param name="ErrorHandler">错误处理方法</param>
	''' <returns>输出数组</returns>
	Public Function ArrayFun(Of TIn, TOut)(func As ArrayFun委托(Of TIn, TOut), A As Array(Of TIn), Optional ErrorHandler As ArrayFunErrorHandler(Of TIn) = Nothing) As Array(Of TOut)
		Dim b As New Array(Of TOut)(A.Size)
		If ErrorHandler Is Nothing Then
			ArrayFun递归(A, b, func, 0, Zeros(Of Integer)(b.NDims))
		Else
			Dim c As Integer() = Zeros(Of Integer)(b.NDims)
			Try
				ArrayFun递归(A, b, func, 0, c)
			Catch ex As Exception
				ErrorHandler.Invoke(ex, c, A(c))
			End Try
		End If
		Return b
	End Function
End Module
