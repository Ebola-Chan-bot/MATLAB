Imports System.Runtime.CompilerServices
Public Module Elmat
	Public Delegate Function Funbsx(Of Out T)(A, B) As T
	Private Sub ����ݹ�(ԭ����1 As Array, ԭ����2 As Array, ��ά���� As Integer(), ��ά�� As Byte, ������1 As Array, ������2 As Array, ��ǰ���� As Integer(), ��ǰά�� As Byte, ԭ����1 As Integer(), ԭ����2 As Integer())
		Dim b As Integer = ԭ����1.Size(��ǰά��), c As Integer = ԭ����2.Size(��ǰά��)
		If ��ǰά�� < ��ά�� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.Rank Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.Rank Then ԭ����2(��ǰά��) = a Mod c
				����ݹ�(ԭ����1, ԭ����2, ��ά����, ��ά��, ������1, ������2, ��ǰ����, ��ǰά�� + 1, ԭ����1, ԭ����2)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				If ��ǰά�� < ԭ����1.Rank Then ԭ����1(��ǰά��) = a Mod b
				If ��ǰά�� < ԭ����2.Rank Then ԭ����2(��ǰά��) = a Mod c
				������1.SetValue(ԭ����1.GetValue(ԭ����1), ��ǰ����)
				������2.SetValue(ԭ����2.GetValue(ԭ����2), ��ǰ����)
			Next
		End If
	End Sub
	Private Function ����(ByRef ����1 As Array, ByRef ����2 As Array) As Integer()
		Dim b As Byte = Math.Max(����1.Rank, ����2.Rank), c(b - 1) As Integer, h As Integer, i As Integer
		For a As Byte = 0 To b - 1
			h = ����1.Size(a)
			i = ����2.Size(a)
			If h = i Then
				c(a) = h
			ElseIf h < i Then
				c(a) = i
			Else
				c(a) = h
			End If
		Next
		Dim d As Array = Array.CreateInstance(����1.Class, c), e As Array = Array.CreateInstance(����2.Class, c)
		����ݹ�(����1, ����2, c, b, d, e, Zeros(Of Integer)(b), 0, Zeros(Of Integer)(����1.Rank), Zeros(Of Integer)(����2.Rank))
		����1 = d
		����2 = e
		Return c
	End Function
#Region "Private"
	Private Sub Reshape�ݹ�(ԭ���� As IEnumerator, ά���� As Byte, ��ά���� As Integer(), ������ As Array, ��ǰά�� As Byte, ������ As Integer())
		If ��ǰά�� > 0 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				������(��ǰά��) = a
				Reshape�ݹ�(ԭ����, ά����, ��ά����, ������, ��ǰά�� - 1, ������)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				������(��ǰά��) = a
				ԭ����.MoveNext()
				������.SetValue(ԭ����.Current, ������)
			Next
		End If
	End Sub
	Private Sub Permute�ݹ�(ԭ���� As Array, ά���� As Byte, ά��ӳ�� As Byte(), ��ά���� As Integer(), ������ As Array, ��ǰά�� As Byte, ԭ���� As Integer(), ������ As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				ԭ����(ά��ӳ��(��ǰά��)) = a
				������(��ǰά��) = a
				Permute�ݹ�(ԭ����, ά����, ά��ӳ��, ��ά����, ������, ��ǰά�� + 1, ԭ����, ������)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				ԭ����(ά��ӳ��(��ǰά��)) = a
				������(��ǰά��) = a
				������.SetValue(ԭ����.GetValue(ԭ����), ������)
			Next
		End If
	End Sub
	Private Sub Bsxfun�ݹ�(Of T)(���� As Funbsx(Of T), ����1 As Array, ����2 As Array, ά���� As Byte, ��ά���� As Integer(), ������ As Array(Of T), ��ǰά�� As Byte, ��ǰ���� As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				Bsxfun�ݹ�(����, ����1, ����2, ά����, ��ά����, ������, ��ǰά�� + 1, ��ǰ����)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				������.SetValue(����.Invoke(����1.GetValue(��ǰ����), ����2.GetValue(��ǰ����)), ��ǰ����)
			Next
		End If
	End Sub
	Private Sub Ones�ݹ�(Of T)(���� As Array, ����1 As T, ά���� As Byte, ��ά���� As Integer(), ��ǰά�� As Byte, ��ǰ���� As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				Ones�ݹ�(����, ����1, ά����, ��ά����, ��ǰά�� + 1, ��ǰ����)
			Next
		Else
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				��ǰ����(��ǰά��) = a
				����.SetValue(����1, ��ǰ����)
			Next
		End If
	End Sub
#End Region
#Region "Public"
	''' <summary>
	''' ����ȫ�����顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <typeparam name="T">Ҫ�������������ͣ��ࣩ</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ������</returns>
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Return New Array(Of T)(sz)
	End Function
	''' <summary>
	''' ����ȫ��Double���顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ��Double����</returns>
	Public Function Zeros(ParamArray sz As Integer()) As Array(Of Double)
		Return Zeros(Of Double)(sz)
	End Function
	''' <summary>
	''' ����ȫ��Ϊ 1 �����顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <typeparam name="T">�����</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1����</returns>
	Public Function Ones(Of T)(ParamArray sz As Integer()) As Array(Of T)
		Dim a As New Array(Of T)(sz)
		Ones�ݹ�(a, CType(DirectCast(1, Object), T), sz.Length, sz, 0, Zeros(Of Integer)(sz.Length))
		Return a
	End Function
	''' <summary>
	''' ����ȫ��Ϊ 1 ��Double���顣��ͬ��MATLAB�����ֻ��һ����С�����������ص�1ά��������������������
	''' </summary>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ1Double����</returns>
	Public Function Ones(ParamArray sz As Integer()) As Array(Of Double)
		Return Ones(Of Double)(sz)
	End Function
	''' <summary>
	''' ����������Ӧ�ð�Ԫ�����㣨������ʽ��չ������ͬ��MATLAB���������ʽ��չ���ӽ�׳��������ѭ����䷽ʽ��ʹ������Ones(2,2)+Ones(4,4)=Ones(4,4)+Ones(4,4)
	''' </summary>
	''' <param name="fun">ҪӦ�õĶ�Ԫ����</param>
	''' <param name="A">��������</param>
	''' <param name="B">��������</param>
	''' <returns>��������</returns>
	Public Function Bsxfun(Of T)(fun As Funbsx(Of T), A As Array, B As Array) As Array(Of T)
		Dim c As Integer() = ����(A, B)
		Dim d As New Array(Of T)(c)
		Bsxfun�ݹ�(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' �ع����顣
	''' </summary>
	''' <param name="A">�������飬�������ȷ���ĳ���</param>
	''' <param name="sz">��ά���ȣ�����ʹ��һ��Nothing��ʾ�Զ��ƶϸ�ά����</param>
	''' <returns>�ع�������</returns>
	<Extension> Public Function Reshape(A As ICollection, ParamArray sz As UInteger?()) As Array
		Dim f As Byte = sz.Length, b As UInteger = 1, c As UInteger? = Nothing, Lengths(f - 1) As Integer
		For e As Byte = 0 To f - 1
			If sz(e) Is Nothing Then
				c = e
			Else
				b *= sz(e)
				Lengths(e) = sz(e)
			End If
		Next
		If c IsNot Nothing Then
			Lengths(c) = A.Count / b
		End If
		Dim d As Array = Array.CreateInstance(A.Class, Lengths)
		Reshape�ݹ�(A.GetEnumerator, f, Lengths, d, f - 1, Zeros(Of Integer)(f))
		Return d
	End Function
	''' <summary>
	''' �û�����ά�ȣ�ά��˳����MATLAB���в�ͬ
	''' </summary>
	''' <param name="A">��������</param>
	''' <param name="dimorder">ά��˳�򡣲�ͬ��MATLAB����0��ʼ</param>
	''' <returns>�û�ά�ȵ�����</returns>
	<Extension> Public Function Permute(A As Array, ParamArray dimorder As Byte()) As Array
		Dim e As Byte = dimorder.Length, b(e - 1) As Integer
		For c As Byte = 0 To e - 1
			b(c) = A.GetLength(dimorder(c))
		Next
		Dim d As Array = Array.CreateInstance(A.Class, b)
		Permute�ݹ�(A, e, dimorder, b, d, 0, Zeros(Of Integer)(e), Zeros(Of Integer)(e))
		Return d
	End Function
	''' <summary>
	''' �����С
	''' </summary>
	''' <param name="A">��������</param>
	''' <returns>�����ά�ߴ繹�ɵ�����</returns>
	<Extension> Public Function Size(A As Array) As Integer()
		Dim d As Byte = A.Rank
		If d = 1 Then
			Return {A.Length}
		Else
			Dim c(d - 1) As Integer
			For b As Byte = 0 To d - 1
				c(b) = A.GetLength(b)
			Next
			Return c
		End If
	End Function
	''' <summary>
	''' �����С
	''' </summary>
	''' <param name="A">��������</param>
	''' <param name="[dim]">��ѯ��ά��</param>
	''' <returns>�����С</returns>
	<Extension> Public Function Size(A As Array, [dim] As Byte) As Integer
		If [dim] < A.Rank Then
			Return A.GetLength([dim])
		Else
			Return 1
		End If
	End Function
	''' <summary>
	''' �����С
	''' </summary>
	''' <param name="A">��������</param>
	''' <returns>�����ά�ߴ繹�ɵ�����</returns>
	Public Function Size(Of T)(A As Array(Of T)) As Integer()
		Return A.Size
	End Function
	''' <summary>
	''' �����С
	''' </summary>
	''' <param name="A">��������</param>
	''' <param name="[dim]">��ѯ��ά��</param>
	''' <returns>�����С</returns>
	Public Function Size(Of T)(A As Array(Of T), [dim] As Byte) As Integer
		Return A.Size([dim])
	End Function
#End Region
End Module