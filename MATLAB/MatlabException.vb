Public Enum 错误类型
	无效的索引Type
End Enum
''' <summary>
''' 用于包装本类库特定的错误类型
''' </summary>
Public Class MatlabException
	Inherits Exception
	ReadOnly Property 错误类型 As 错误类型
	Public Sub New(message As String)
		MyBase.New(message)
	End Sub

	Public Sub New(message As String, innerException As Exception)
		MyBase.New(message, innerException)
	End Sub

	Public Sub New()
	End Sub
	Sub New(错误类型 As 错误类型)
		Me.错误类型 = 错误类型
	End Sub
End Class
