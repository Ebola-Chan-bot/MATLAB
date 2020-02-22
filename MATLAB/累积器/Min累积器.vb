Public Class Min累积器(Of T)
	Implements I累积器(Of T, T)
	Private 最小值 As INumeric, 未积入 As Boolean = True

	Public Sub 累积(积入值 As T) Implements I累积器(Of T, T).累积
		If 未积入 Then
			最小值 = 积入值
			未积入 = False
		Else
			最小值 = 最小值.Min(积入值)
		End If
	End Sub

	Public Function 结果() As T Implements I累积器(Of T, T).结果
		未积入 = True
		Return 最小值
	End Function
End Class
