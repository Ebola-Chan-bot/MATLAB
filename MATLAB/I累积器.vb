''' <summary>
''' 为了使用累积降维功能，你需要自行定义一个累积器，实现此接口。
''' </summary>
''' <typeparam name="TIn">积入值类型</typeparam>
''' <typeparam name="TOut">计算结果类型</typeparam>
Public Interface I累积器(Of TIn, TOut)
	''' <summary>
	''' 对于参与累积的每一个值，都会调用一次此方法，向累积器输入。
	''' </summary>
	Sub 累积(积入值 As TIn)
	''' <summary>
	''' 累积完成后，通过这个函数取得累积结果。
	''' </summary>
	Function 结果() As TOut
End Interface
