Public Class Mean累积器(Of T)
	Implements I累积器(Of T, T)
	Private 总和 As INumeric, 计数 As New MUInt32(0)
	Shared ReadOnly 计数1 As New MUInt32(1), 计数0 As New MUInt32(0)
	Public Sub 累积(积入值 As T) Implements I累积器(Of T, T).累积
		If 计数 = 计数0 Then
			总和 = 积入值
			计数 = 计数1
		Else
			总和 = 总和.Plus(积入值)
			计数 += 计数1
		End If
	End Sub

	Public Function 结果() As T Implements I累积器(Of T, T).结果
		结果 = 总和.RDivide(计数)
		计数 = 计数0
	End Function
End Class
