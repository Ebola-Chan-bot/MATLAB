Imports System.Runtime.InteropServices
Friend Enum WICDecodeOptions
	WICDecodeMetadataCacheOnDemand = 0
End Enum
Friend Enum StandardAccessTypes As Long
	GENERIC_READ = &H80000000
End Enum
Friend Enum TagCLSCTX
	CLSCTX_INPROC_SERVER = 1
End Enum
Friend Structure WICRect
	Property X As Integer
	Property Y As Integer
	Property Width As Integer
	Property Height As Integer
End Structure
Public Enum HRESULT As Long
	S_OK = 0
	WINCODEC_ERR_COMPONENTNOTFOUND = 2291674960
End Enum
Public Class ComException
	Inherits Exception
	ReadOnly Property 错误代码 As HRESULT
	Public Sub New(message As String)
		MyBase.New(message)
	End Sub

	Public Sub New(message As String, innerException As Exception)
		MyBase.New(message, innerException)
	End Sub

	Public Sub New()
	End Sub
	Friend Sub New(错误代码 As HRESULT, message As String)
		MyBase.New(message)
		Me.错误代码 = 错误代码
	End Sub
	Friend Shared Sub 检查(错误代码 As HRESULT, message As String)
		If 错误代码 <> MATLAB.HRESULT.S_OK Then
			Throw New ComException(错误代码, message)
		End If
	End Sub
End Class
Public Module ImageSci
	Private Declare Function CoCreateInstance Lib "ole32.dll" (rclsid As Guid, pUnkOuter As IntPtr, dwClsContext As TagCLSCTX, riid As Guid, <Out> ByRef ppv As IntPtr) As HRESULT
	Private Declare Function DllGetClassObject Lib "WindowsCodecs.dll" (rclsid As Guid, riid As Guid, <Out> ByRef ppv As IntPtr) As HRESULT
	Private Declare Unicode Function IWICImagingFactory_CreateDecoderFromFilename_Proxy Lib "WindowsCodecs.dll" (pFactory As IntPtr, wzFilename As String, pguidVendor As IntPtr, dwDesiredAccess As StandardAccessTypes, metadataOptions As WICDecodeOptions, <Out> ByRef ppIDecoder As IntPtr) As HRESULT
	Private Declare Function IWICBitmapDecoder_GetFrameCount_Proxy Lib "WindowsCodecs.dll" (THIS_PTR As IntPtr, <Out> ByRef pCount As UInteger) As HRESULT
	Private Declare Function IWICBitmapDecoder_GetFrame_Proxy Lib "WindowsCodecs.dll" (THIS_PTR As IntPtr, index As UInteger, <Out> ByRef ppIBitmapFrame As IntPtr) As HRESULT
	Private Declare Function IWICBitmapSource_GetSize_Proxy Lib "WindowsCodecs.dll" (THIS_PTR As IntPtr, <Out> ByRef puiWidth As UInteger, <Out> ByRef puiHeight As UInteger) As HRESULT
	Private Declare Function WICConvertBitmapSource Lib "WindowsCodecs.dll" (dstFormat As Guid, pISrc As IntPtr, <Out> ByRef ppIDst As IntPtr) As HRESULT
	Private Declare Function IWICBitmapSource_CopyPixels_Proxy Lib "WindowsCodecs.dll" (THIS_PTR As IntPtr, ByRef prc As WICRect, cbStride As UInteger, cbBufferSize As UInteger, <MarshalAs(UnmanagedType.LPArray)> pbBuffer As Byte()) As HRESULT
	ReadOnly Property GUID_WICPixelFormatDontCare As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H0)
	ReadOnly Property GUID_WICPixelFormat1bppIndexed As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1)
	ReadOnly Property GUID_WICPixelFormat2bppIndexed As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2)
	ReadOnly Property GUID_WICPixelFormat4bppIndexed As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3)
	ReadOnly Property GUID_WICPixelFormat8bppIndexed As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H4)
	ReadOnly Property GUID_WICPixelFormatBlackWhite As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H5)
	ReadOnly Property GUID_WICPixelFormat2bppGray As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H6)
	ReadOnly Property GUID_WICPixelFormat4bppGray As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H7)
	ReadOnly Property GUID_WICPixelFormat8bppGray As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H8)
	ReadOnly Property GUID_WICPixelFormat8bppAlpha As New Guid(&HE6CD0116L, &HEEBA, &H4161, &HAA, &H85, &H27, &HDD, &H9F, &HB3, &HA8, &H95)
	ReadOnly Property GUID_WICPixelFormat16bppBGR555 As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H9)
	ReadOnly Property GUID_WICPixelFormat16bppBGR565 As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HA)
	ReadOnly Property GUID_WICPixelFormat16bppBGRA5551 As New Guid(&H5EC7C2B, &HF1E6, &H4961, &HAD, &H46, &HE1, &HCC, &H81, &HA, &H87, &HD2)
	ReadOnly Property GUID_WICPixelFormat16bppGray As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HB)
	ReadOnly Property GUID_WICPixelFormat24bppBGR As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HC)
	ReadOnly Property GUID_WICPixelFormat24bppRGB As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HD)
	ReadOnly Property GUID_WICPixelFormat32bppBGR As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HE)
	ReadOnly Property GUID_WICPixelFormat32bppBGRA As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &HF)
	ReadOnly Property GUID_WICPixelFormat32bppPBGRA As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H10)
	ReadOnly Property GUID_WICPixelFormat32bppGrayFloat As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H11)
	ReadOnly Property GUID_WICPixelFormat32bppRGB As New Guid(&HD98C6B95, &H3EFE, &H47D6, &HBB, &H25, &HEB, &H17, &H48, &HAB, &HC, &HF1)
	ReadOnly Property GUID_WICPixelFormat32bppRGBA As New Guid(&HF5C7AD2D, &H6A8D, &H43DD, &HA7, &HA8, &HA2, &H99, &H35, &H26, &H1A, &HE9)
	ReadOnly Property GUID_WICPixelFormat32bppPRGBA As New Guid(&H3CC4A650, &HA527, &H4D37, &HA9, &H16, &H31, &H42, &HC7, &HEB, &HED, &HBA)
	ReadOnly Property GUID_WICPixelFormat48bppRGB As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H15)
	ReadOnly Property GUID_WICPixelFormat48bppBGR As New Guid(&HE605A384L, &HB468, &H46CE, &HBB, &H2E, &H36, &HF1, &H80, &HE6, &H43, &H13)
	ReadOnly Property GUID_WICPixelFormat64bppRGB As New Guid(&HA1182111, &H186D, &H4D42, &HBC, &H6A, &H9C, &H83, &H3, &HA8, &HDF, &HF9)
	ReadOnly Property GUID_WICPixelFormat64bppRGBA As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H16)
	ReadOnly Property GUID_WICPixelFormat64bppBGRA As New Guid(&H1562FF7C, &HD352, &H46F9, &H97, &H9E, &H42, &H97, &H6B, &H79, &H22, &H46)
	ReadOnly Property GUID_WICPixelFormat64bppPRGBA As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H17)
	ReadOnly Property GUID_WICPixelFormat64bppPBGRA As New Guid(&H8C518E8EL, &HA4EC, &H468B, &HAE, &H70, &HC9, &HA3, &H5A, &H9C, &H55, &H30)
	ReadOnly Property GUID_WICPixelFormat16bppGrayFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H13)
	ReadOnly Property GUID_WICPixelFormat32bppBGR101010 As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H14)
	ReadOnly Property GUID_WICPixelFormat48bppRGBFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H12)
	ReadOnly Property GUID_WICPixelFormat48bppBGRFixedPoint As New Guid(&H49CA140E, &HCAB6, &H493B, &H9D, &HDF, &H60, &H18, &H7C, &H37, &H53, &H2A)
	ReadOnly Property GUID_WICPixelFormat96bppRGBFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H18)
	ReadOnly Property GUID_WICPixelFormat96bppRGBFloat As New Guid(&HE3FED78FL, &HE8DB, &H4ACF, &H84, &HC1, &HE9, &H7F, &H61, &H36, &HB3, &H27)
	ReadOnly Property GUID_WICPixelFormat128bppRGBAFloat As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H19)
	ReadOnly Property GUID_WICPixelFormat128bppPRGBAFloat As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1A)
	ReadOnly Property GUID_WICPixelFormat128bppRGBFloat As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1B)
	ReadOnly Property GUID_WICPixelFormat32bppCMYK As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1C)
	ReadOnly Property GUID_WICPixelFormat64bppRGBAFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1D)
	ReadOnly Property GUID_WICPixelFormat64bppBGRAFixedPoint As New Guid(&H356DE33C, &H54D2, &H4A23, &HBB, &H4, &H9B, &H7B, &HF9, &HB1, &HD4, &H2D)
	ReadOnly Property GUID_WICPixelFormat64bppRGBFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H40)
	ReadOnly Property GUID_WICPixelFormat128bppRGBAFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1E)
	ReadOnly Property GUID_WICPixelFormat128bppRGBFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H41)
	ReadOnly Property GUID_WICPixelFormat64bppRGBAHalf As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3A)
	ReadOnly Property GUID_WICPixelFormat64bppPRGBAHalf As New Guid(&H58AD26C2, &HC623, &H4D9D, &HB3, &H20, &H38, &H7E, &H49, &HF8, &HC4, &H42)
	ReadOnly Property GUID_WICPixelFormat64bppRGBHalf As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H42)
	ReadOnly Property GUID_WICPixelFormat48bppRGBHalf As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3B)
	ReadOnly Property GUID_WICPixelFormat32bppRGBE As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3D)
	ReadOnly Property GUID_WICPixelFormat16bppGrayHalf As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3E)
	ReadOnly Property GUID_WICPixelFormat32bppGrayFixedPoint As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H3F)
	ReadOnly Property GUID_WICPixelFormat32bppRGBA1010102 As New Guid(&H25238D72, &HFCF9, &H4522, &HB5, &H14, &H55, &H78, &HE5, &HAD, &H55, &HE0)
	ReadOnly Property GUID_WICPixelFormat32bppRGBA1010102XR As New Guid(&HDE6B9A, &HC101, &H434B, &HB5, &H2, &HD0, &H16, &H5E, &HE1, &H12, &H2C)
	ReadOnly Property GUID_WICPixelFormat32bppR10G10B10A2 As New Guid(&H604E1BB5, &H8A3C, &H4B65, &HB1, &H1C, &HBC, &HB, &H8D, &HD7, &H5B, &H7F)
	ReadOnly Property GUID_WICPixelFormat32bppR10G10B10A2HDR10 As New Guid(&H9C215C5D, &H1ACC, &H4F0E, &HA4, &HBC, &H70, &HFB, &H3A, &HE8, &HFD, &H28)
	ReadOnly Property GUID_WICPixelFormat64bppCMYK As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H1F)
	ReadOnly Property GUID_WICPixelFormat24bpp3Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H20)
	ReadOnly Property GUID_WICPixelFormat32bpp4Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H21)
	ReadOnly Property GUID_WICPixelFormat40bpp5Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H22)
	ReadOnly Property GUID_WICPixelFormat48bpp6Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H23)
	ReadOnly Property GUID_WICPixelFormat56bpp7Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H24)
	ReadOnly Property GUID_WICPixelFormat64bpp8Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H25)
	ReadOnly Property GUID_WICPixelFormat48bpp3Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H26)
	ReadOnly Property GUID_WICPixelFormat64bpp4Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H27)
	ReadOnly Property GUID_WICPixelFormat80bpp5Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H28)
	ReadOnly Property GUID_WICPixelFormat96bpp6Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H29)
	ReadOnly Property GUID_WICPixelFormat112bpp7Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2A)
	ReadOnly Property GUID_WICPixelFormat128bpp8Channels As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2B)
	ReadOnly Property GUID_WICPixelFormat40bppCMYKAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2C)
	ReadOnly Property GUID_WICPixelFormat80bppCMYKAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2D)
	ReadOnly Property GUID_WICPixelFormat32bpp3ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2E)
	ReadOnly Property GUID_WICPixelFormat40bpp4ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H2F)
	ReadOnly Property GUID_WICPixelFormat48bpp5ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H30)
	ReadOnly Property GUID_WICPixelFormat56bpp6ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H31)
	ReadOnly Property GUID_WICPixelFormat64bpp7ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H32)
	ReadOnly Property GUID_WICPixelFormat72bpp8ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H33)
	ReadOnly Property GUID_WICPixelFormat64bpp3ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H34)
	ReadOnly Property GUID_WICPixelFormat80bpp4ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H35)
	ReadOnly Property GUID_WICPixelFormat96bpp5ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H36)
	ReadOnly Property GUID_WICPixelFormat112bpp6ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H37)
	ReadOnly Property GUID_WICPixelFormat128bpp7ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H38)
	ReadOnly Property GUID_WICPixelFormat144bpp8ChannelsAlpha As New Guid(&H6FDDC324, &H4E03, &H4BFE, &HB1, &H85, &H3D, &H77, &H76, &H8D, &HC9, &H39)
	ReadOnly Property GUID_WICPixelFormat8bppY As New Guid(&H91B4DB54, &H2DF9, &H42F0, &HB4, &H49, &H29, &H9, &HBB, &H3D, &HF8, &H8E)
	ReadOnly Property GUID_WICPixelFormat8bppCb As New Guid(&H1339F224, &H6BFE, &H4C3E, &H93, &H2, &HE4, &HF3, &HA6, &HD0, &HCA, &H2A)
	ReadOnly Property GUID_WICPixelFormat8bppCr As New Guid(&HB8145053, &H2116, &H49F0, &H88, &H35, &HED, &H84, &H4B, &H20, &H5C, &H51)
	ReadOnly Property GUID_WICPixelFormat16bppCbCr As New Guid(&HFF95BA6E, &H11E0, &H4263, &HBB, &H45, &H1, &H72, &H1F, &H34, &H60, &HA4)
	ReadOnly Property GUID_WICPixelFormat16bppYQuantizedDctCoefficients As New Guid(&HA355F433, &H48E8, &H4A42, &H84, &HD8, &HE2, &HAA, &H26, &HCA, &H80, &HA4)
	ReadOnly Property GUID_WICPixelFormat16bppCbQuantizedDctCoefficients As New Guid(&HD2C4FF61, &H56A5, &H49C2, &H8B, &H5C, &H4C, &H19, &H25, &H96, &H48, &H37)
	ReadOnly Property GUID_WICPixelFormat16bppCrQuantizedDctCoefficients As New Guid(&H2FE354F0, &H1680, &H42D8, &H92, &H31, &HE7, &H3C, &H5, &H65, &HBF, &HC1)
	''' <summary>
	''' 不同于MATLAB，无论图片文件本身格式为何，此函数总是将返回高×宽×RGB[×帧]形式的Byte数组，即模块中列出的GUID_WICPixelFormat32bppRGBA格式。列出的其它格式只表示这些格式的图片文件可以被正确读取。
	''' </summary>
	''' <param name="filename">图片文件路径</param>
	''' <param name="transparency">可选返回Alpha通道。如果返回多帧，将安排在第3维。</param>
	''' <param name="Frames">对于多帧图像（如GIF），选择返回的帧序号。不同于MATLAB，从0开始。如果返回多帧，将安排在第4维。</param>
	''' <returns>高×宽×RGB[×帧]形式的Byte数组</returns>
	Public Function ImRead(filename As String, Optional ByRef transparency As Array(Of Byte) = Nothing, Optional Frames As UInteger() = Nothing) As Array(Of Byte)
		Static 图像工厂 As IntPtr
		ComException.检查(CoCreateInstance(New Guid("317D06E8-5F24-433D-BDF7-79CE68D8ABC2"), Nothing, TagCLSCTX.CLSCTX_INPROC_SERVER, New Guid("ec5ec8a9-c395-4314-9c77-54d7a935ff70"), 图像工厂), "Ole32：创建Com实例失败")
		Dim a As IntPtr
		ComException.检查(IWICImagingFactory_CreateDecoderFromFilename_Proxy(图像工厂, filename, Nothing, StandardAccessTypes.GENERIC_READ, WICDecodeOptions.WICDecodeMetadataCacheOnDemand, a), "WindowsCodecs：从文件名创建解码器失败")
		If Frames Is Nothing Then Frames = {0}
		If Frames.Length = 1 Then
			ComException.检查(IWICBitmapDecoder_GetFrame_Proxy(a, Frames(0), a), "WindowsCodecs：获取帧失败")
			ComException.检查(WICConvertBitmapSource(GUID_WICPixelFormat32bppRGBA, a, a), "WindowsCodecs：转换位图源失败")
			Dim b As UInteger, c As UInteger
			ComException.检查(IWICBitmapSource_GetSize_Proxy(a, b, c), "WindowsCodecs：获取尺寸失败")
			Dim e(b * c * 4 - 1) As Byte
			ComException.检查(IWICBitmapSource_CopyPixels_Proxy(a, New WICRect With {.X = 0, .Y = 0, .Height = c, .Width = b}, b * 4, e.Length, e), "WindowsCodecs：复制像素失败")
			Dim d As New Array(Of Byte)(c, b, 3), i As IEnumerator(Of Byte) = e.AsEnumerable.GetEnumerator
			transparency = New Array(Of Byte)(c, b)
			For f As Integer = 0 To c - 1
				For g As Integer = 0 To b - 1
					For h As Byte = 0 To 2
						i.MoveNext()
						d({f, g, h}) = i.Current
					Next
					i.MoveNext()
					transparency({f, g}) = i.Current
				Next
			Next
			Return d
		Else
			Dim l As IntPtr
			ComException.检查(IWICBitmapDecoder_GetFrame_Proxy(a, Frames(0), l), "WindowsCodecs：获取帧失败")
			ComException.检查(WICConvertBitmapSource(GUID_WICPixelFormat32bppRGBA, l, l), "WindowsCodecs：转换位图源失败")
			Dim b As UInteger, c As UInteger
			ComException.检查(IWICBitmapSource_GetSize_Proxy(l, b, c), "WindowsCodecs：获取尺寸失败")
			Dim e(b * c * 4 - 1) As Byte, k As New WICRect With {.X = 0, .Y = 0, .Height = c, .Width = b}
			ComException.检查(IWICBitmapSource_CopyPixels_Proxy(l, k, b * 4, e.Length, e), "WindowsCodecs：复制像素失败")
			Dim d As New Array(Of Byte)(c, b, 3, Frames.Length), i As IEnumerator(Of Byte)
			transparency = New Array(Of Byte)(c, b, Frames.Length)
			For j As Integer = 0 To Frames.GetUpperBound(0)
				IWICBitmapDecoder_GetFrame_Proxy(a, Frames(j), l)
				WICConvertBitmapSource(GUID_WICPixelFormat32bppRGBA, l, l)
				IWICBitmapSource_CopyPixels_Proxy(l, k, b * 4, e.Length, e)
				i = e.AsEnumerable.GetEnumerator
				For f As Integer = 0 To c - 1
					For g As Integer = 0 To b - 1
						For h As Byte = 0 To 2
							i.MoveNext()
							d({f, g, h, j}) = i.Current
						Next
						i.MoveNext()
						transparency({f, g, j}) = i.Current
					Next
				Next
			Next
			Return d
		End If
	End Function
End Module
