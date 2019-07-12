Public MustInherit Class RGRP
    Public MustOverride Function LoadGRP(bitmap As ByteBitmap, framedata As List(Of FrameData), grpfile As String) As Boolean
    Public MustOverride Function DrawGRP(frame As Integer) As BitmapSource
    Public MustOverride Sub Reset()
End Class
Public Structure FrameData
    'struct frame{
    ' unsigned short x; // Coordinates of the top-left pixel of the frame
    ' unsigned short y;
    ' unsigned short xoff; // X,Y offsets from the top left of the GRP frame -- value seems directly copied from each GRP
    ' unsigned short yoff;
    ' unsigned short width; // Dimensions, relative to the top-left pixel, of the frame
    ' unsigned short height;
    ' unsigned short unk1; // always 0? Or 1?
    ' unsigned short unk2; // always 0?
    '};
    Public x As UShort
    Public y As UShort
    Public xoff As UShort
    Public yoff As UShort
    Public width As UShort
    Public height As UShort
    Public unk1 As UShort
    Public unk2 As UShort
    Public Sub New(ByRef pos As UInteger, bytes As Byte())
        x = BReader.ReadUint16(pos, bytes)
        y = BReader.ReadUint16(pos, bytes)
        xoff = BReader.ReadUint16(pos, bytes)
        yoff = BReader.ReadUint16(pos, bytes)
        width = BReader.ReadUint16(pos, bytes)
        height = BReader.ReadUint16(pos, bytes)
        unk1 = BReader.ReadUint16(pos, bytes)
        unk2 = BReader.ReadUint16(pos, bytes)
    End Sub
End Structure