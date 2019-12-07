Imports System.Runtime.CompilerServices
''' <summary>
''' ���͹淶����ֵһ��Long������һ��UInteger��ά��һ��Byte
''' </summary>
Public Module Elmat
	Public Delegate Function Funbsx(A, B)
#Region "Private"
	Private Sub Reshape�ݹ�(ԭ���� As IEnumerator, ά���� As Byte, ��ά���� As Integer(), ������ As Array, ��ǰά�� As Byte, ������ As Integer())
		If ��ǰά�� < ά���� - 1 Then
			For a As Integer = 0 To ��ά����(��ǰά��) - 1
				������(��ǰά��) = a
				Reshape�ݹ�(ԭ����, ά����, ��ά����, ������, ��ǰά�� + 1, ������)
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
	Private Sub Bsxfun�ݹ�(���� As Funbsx, ����1 As Array, ����2 As Array, ά���� As Byte, ��ά���� As Integer(), ������ As Array, ��ǰά�� As Byte, ��ǰ���� As Integer())
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
#End Region
#Region "Public"
	''' <summary>
	''' ����ȫ������
	''' </summary>
	''' <typeparam name="T">Ҫ�������������ͣ��ࣩ</typeparam>
	''' <param name="sz">ÿ��ά�ȵĴ�С</param>
	''' <returns>ȫ������</returns>
	Public Function Zeros(Of T)(ParamArray sz As Integer()) As Array
		Return Array.CreateInstance(GetType(T), sz)
	End Function
	''' <summary>
	''' ����ȫ������
	''' </summary>
	''' <typeparam name="T">Ҫ�������������ͣ��ࣩ</typeparam>
	''' <param name="sz">��������</param>
	''' <returns>ȫ������</returns>
	Public Function Zeros(Of T)(sz As Integer) As T()
		Dim a(sz - 1) As T
		Return a
	End Function
	''' <summary>
	''' ����������Ӧ�ð�Ԫ�����㣨������ʽ��չ��
	''' </summary>
	''' <param name="fun">ҪӦ�õĶ�Ԫ����</param>
	''' <param name="A">��������</param>
	''' <param name="B">��������</param>
	''' <returns>��������</returns>
	Public Function Bsxfun(fun As Funbsx, A As Array, B As Array) As Array
		Dim c As Integer() = ����(A, B), d As Array = Array.CreateInstance(A.Class, c)
		Bsxfun�ݹ�(fun, A, B, c.Length, c, d, 0, Zeros(Of Integer)(c.Length))
		Return d
	End Function
	''' <summary>
	''' �ع�����
	''' </summary>
	''' <param name="A">��������</param>
	''' <param name="sz">��ά���ȣ�����ʹ��һ��Nothing��ʾ�Զ��ƶϸ�ά����</param>
	''' <returns>�ع�������</returns>
	<Extension> Public Function Reshape(A As Array, ParamArray sz As UInteger?()) As Array
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
			Lengths(c) = A.Length / b
		End If
		Dim d As Array = Array.CreateInstance(A.Class, Lengths)
		Reshape�ݹ�(A.GetEnumerator, f, Lengths, d, 0, Zeros(Of Integer)(f))
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
#End Region
End Module