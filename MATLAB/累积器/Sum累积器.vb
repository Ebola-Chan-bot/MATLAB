Public Class Sum累积器(Of T)
	Implements I累积器(Of T, T)
	Private 总和 As INumeric, 未积入 As Boolean = True
	Public Sub 累积(积入值 As T) Implements I累积器(Of T, T).累积
		If 未积入 Then
			总和 = 积入值
			未积入 = False
		Else
			总和 = 总和.Plus(积入值)
		End If
	End Sub

	Public Function 结果() As T Implements I累积器(Of T, T).结果
		未积入 = True
		Return 总和
	End Function
End Class