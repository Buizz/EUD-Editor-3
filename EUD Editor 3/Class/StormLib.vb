Imports System.Runtime.InteropServices

Module MPQMoudle
    Public Class SFmpq
        ' General error codes
        Const MPQ_ERROR_MPQ_INVALID As UInteger = 2233466981
        Const MPQ_ERROR_FILE_NOT_FOUND As UInteger = 2233466982
        Const MPQ_ERROR_DISK_FULL As UInteger = 2233466984
        'Physical write file to MPQ failed. Not sure of exact meaning
        Const MPQ_ERROR_HASH_TABLE_FULL As UInteger = 2233466985
        Const MPQ_ERROR_ALREADY_EXISTS As UInteger = 2233466986
        Const MPQ_ERROR_BAD_OPEN_MODE As UInteger = 2233466988
        'When MOAU_READ_ONLY is used without MOAU_OPEN_EXISTING
        Const MPQ_ERROR_COMPACT_ERROR As UInteger = 2234515457

        ' MpqOpenArchiveForUpdate flags
        Const MOAU_CREATE_NEW As UInteger = 0
        Const MOAU_CREATE_ALWAYS As UInteger = 8
        'Was wrongly named MOAU_CREATE_NEW
        Const MOAU_OPEN_EXISTING As UInteger = 4
        Const MOAU_OPEN_ALWAYS As UInteger = 32
        Const MOAU_READ_ONLY As UInteger = 16
        'Must be used with MOAU_OPEN_EXISTING
        Const MOAU_MAINTAIN_LISTFILE As UInteger = 1

        ' MpqAddFileToArchive flags
        Const MAFA_EXISTS As UInteger = 2147483648
        'Will be added if not present
        Const MAFA_UNKNOWN40000000 As UInteger = 1073741824
        Const MAFA_MODCRYPTKEY As UInteger = 131072
        Const MAFA_ENCRYPT As UInteger = 65536
        Const MAFA_COMPRESS As UInteger = 512
        Const MAFA_COMPRESS2 As UInteger = 256
        Const MAFA_REPLACE_EXISTING As UInteger = 1

        ' MpqAddFileToArchiveEx compression flags
        Const MAFA_COMPRESS_STANDARD As UInteger = 8
        'Standard PKWare DCL compression
        Const MAFA_COMPRESS_DEFLATE As UInteger = 2
        'ZLib's deflate compression
        Const MAFA_COMPRESS_WAVE As UInteger = 129
        'Standard wave compression
        Const MAFA_COMPRESS_WAVE2 As UInteger = 65
        'Unused wave compression
        ' Flags for individual compression types used for wave compression
        Const MAFA_COMPRESS_WAVECOMP1 As UInteger = 128
        'Main compressor for standard wave compression
        Const MAFA_COMPRESS_WAVECOMP2 As UInteger = 64
        'Main compressor for unused wave compression
        Const MAFA_COMPRESS_WAVECOMP3 As UInteger = 1
        'Secondary compressor for wave compression
        ' ZLib deflate compression level constants (used with MpqAddFileToArchiveEx and MpqAddFileFromBufferEx)
        Const Z_NO_COMPRESSION As UInteger = 0
        Const Z_BEST_SPEED As UInteger = 1
        Const Z_BEST_COMPRESSION As UInteger = 9
        Const Z_DEFAULT_COMPRESSION As Integer = (-1)

        ' MpqAddWaveToArchive quality flags
        Const MAWA_QUALITY_HIGH As UInteger = 1
        Const MAWA_QUALITY_MEDIUM As UInteger = 0
        Const MAWA_QUALITY_LOW As UInteger = 2

        ' SFileGetFileInfo flags
        Const SFILE_INFO_BLOCK_SIZE As UInteger = 1
        'Block size in MPQ
        Const SFILE_INFO_HASH_TABLE_SIZE As UInteger = 2
        'Hash table size in MPQ
        Const SFILE_INFO_NUM_FILES As UInteger = 3
        'Number of files in MPQ
        Const SFILE_INFO_TYPE As UInteger = 4
        'Is int a file or an MPQ?
        Const SFILE_INFO_SIZE As UInteger = 5
        'Size of MPQ or uncompressed file
        Const SFILE_INFO_COMPRESSED_SIZE As UInteger = 6
        'Size of compressed file
        Const SFILE_INFO_FLAGS As UInteger = 7
        'File flags (compressed, etc.), file attributes if a file not in an archive
        Const SFILE_INFO_PARENT As UInteger = 8
        'int of MPQ that file is in
        Const SFILE_INFO_POSITION As UInteger = 9
        'Position of file pointer in files
        Const SFILE_INFO_LOCALEID As UInteger = 10
        'Locale ID of file in MPQ
        Const SFILE_INFO_PRIORITY As UInteger = 11
        'Priority of open MPQ
        Const SFILE_INFO_HASH_INDEX As UInteger = 12
        'Hash index of file in MPQ
        ' SFileListFiles flags
        Const SFILE_LIST_MEMORY_LIST As UInteger = 1
        ' Specifies that lpFilelists is a file list from memory, rather than being a list of file lists
        Const SFILE_LIST_ONLY_KNOWN As UInteger = 2
        ' Only list files that the function finds a name for
        Const SFILE_LIST_ONLY_UNKNOWN As UInteger = 4
        ' Only list files that the function does not find a name for
        Const SFILE_TYPE_MPQ As UInteger = 1
        Const SFILE_TYPE_FILE As UInteger = 2

        Const SFILE_OPEN_HARD_DISK_FILE As UInteger = 0
        'Open archive without regard to the drive type it resides on
        Const SFILE_OPEN_CD_ROM_FILE As UInteger = 1
        'Open the archive only if it is on a CD-ROM
        Const SFILE_OPEN_ALLOW_WRITE As UInteger = 32768
        'Open file with write access
        Const SFILE_SEARCH_CURRENT_ONLY As UInteger = 0
        'Used with SFileOpenFileEx; only the archive with the int specified will be searched for the file
        Const SFILE_SEARCH_ALL_OPEN As UInteger = 1
        'SFileOpenFileEx will look through all open archives for the file
        <StructLayout(LayoutKind.Sequential)>
        Public Structure LCID
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=4)>
            Public lcLocale As Char()
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure SFMPQVERSION
            Public Major As UShort
            Public Minor As UShort
            Public Revision As UShort
            Public Subrevision As UShort
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure FILELISTENTRY
            Public dwFileExists As UInteger
            ' Nonzero if this entry is used
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=4)>
            Public lcLocale As Char()
            ' Locale ID of file
            Public dwCompressedSize As UInteger
            ' Compressed size of file
            Public dwFullSize As UInteger
            ' Uncompressed size of file
            Public dwFlags As UInteger
            ' Flags for file
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=260)>
            Public szFileName As Char()
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure MPQHEADER
            Public dwMPQID As UInteger
            '"MPQ\x1A" for mpq's, "BN3\x1A" for bncache.dat
            Public dwHeaderSize As UInteger
            ' Size of this header
            Public dwMPQSize As UInteger
            'The size of the mpq archive
            Public wUnused0C As UShort
            ' Seems to always be 0
            Public wBlockSize As UShort
            ' Size of blocks in files equals 512 << wBlockSize
            Public dwHashTableOffset As UInteger
            ' Offset to hash table
            Public dwBlockTableOffset As UInteger
            ' Offset to block table
            Public dwHashTableSize As UInteger
            ' Number of entries in hash table
            Public dwBlockTableSize As UInteger
            ' Number of entries in block table
        End Structure

        'Archive ints may be typecasted to this struct so you can access
        'some of the archive's properties and the decrypted hash table and
        'block table directly.
        <StructLayout(LayoutKind.Sequential)>
        Public Structure MPQARCHIVE
            ' Arranged according to priority with lowest priority first
            Public lpNextArc As IntPtr
            ' Pointer to the next ARCHIVEREC struct. Pointer to addresses of first and last archives if last archive
            Public lpPrevArc As IntPtr
            ' Pointer to the previous ARCHIVEREC struct. 0xEAFC5E23 if first archive
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=260)>
            Private szFileName As Char()
            ' Filename of the archive
            Public hFile As UInteger
            ' The archive's file int
            Public dwFlags1 As UInteger
            ' Some flags, bit 1 (0 based) seems to be set when opening an archive from a CD
            Public dwPriority As UInteger
            ' Priority of the archive set when calling SFileOpenArchive
            Public lpLastReadFile As IntPtr
            ' Pointer to the last read file's FILEREC struct. Only used for incomplete reads of blocks
            Public dwUnk As UInteger
            ' Seems to always be 0
            Public dwBlockSize As UInteger
            ' Size of file blocks in bytes
            Public lpLastReadBlock As IntPtr
            ' Pointer to the read buffer for archive. Only used for incomplete reads of blocks
            Public dwBufferSize As UInteger
            ' Size of the read buffer for archive. Only used for incomplete reads of blocks
            Public dwMPQStart As UInteger
            ' The starting offset of the archive
            Public lpMPQHeader As IntPtr
            ' Pointer to the archive header
            Public lpBlockTable As IntPtr
            ' Pointer to the start of the block table
            Public lpHashTable As IntPtr
            ' Pointer to the start of the hash table
            Public dwFileSize As UInteger
            ' The size of the file in which the archive is contained
            Public dwOpenFiles As UInteger
            ' Count of files open in archive + 1
            Public MpqHeader As MPQHEADER
            Public dwFlags As UInteger
            'The only flag that should be changed is MOAU_MAINTAIN_LISTFILE
            Public lpFileName As String
        End Structure

        'ints to files in the archive may be typecasted to this struct
        'so you can access some of the file's properties directly.
        <StructLayout(LayoutKind.Sequential)>
        Public Structure MPQFILE
            Public lpNextFile As IntPtr
            ' Pointer to the next FILEREC struct. Pointer to addresses of first and last files if last file
            Public lpPrevFile As IntPtr
            ' Pointer to the previous FILEREC struct. 0xEAFC5E13 if first file
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=260)>
            Private szFileName As Char()
            ' Filename of the archive
            Public hPlaceHolder As UInteger
            ' Always 0xFFFFFFFF
            Public lpParentArc As IntPtr
            ' Pointer to the ARCHIVEREC struct of the archive in which the file is contained
            Public lpBlockEntry As IntPtr
            ' Pointer to the file's block table entry
            Public dwCryptKey As UInteger
            ' Decryption key for the file
            Public dwFilePointer As UInteger
            ' Position of file pointer in the file
            Public dwUnk1 As UInteger
            ' Seems to always be 0
            Public dwBlockCount As UInteger
            ' Number of blocks in file
            Public lpdwBlockOffsets As IntPtr
            ' Offsets to blocks in file. There are 1 more of these than the number of blocks
            Public dwReadStarted As UInteger
            ' Set to 1 after first read
            Public dwUnk2 As UInteger
            ' Seems to always be 0
            Public lpLastReadBlock As IntPtr
            ' Pointer to the read buffer for file. Only used for incomplete reads of blocks
            Public dwBytesRead As UInteger
            ' Total bytes read from open file
            Public dwBufferSize As UInteger
            ' Size of the read buffer for file. Only used for incomplete reads of blocks
            Public dwConstant As UInteger
            ' Seems to always be 1
            Public lpHashEntry As IntPtr
            Public lpFileName As String
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure BLOCKTABLEENTRY
            Public dwFileOffset As UInteger
            ' Offset to file
            Public dwCompressedSize As UInteger
            ' Compressed size of file
            Public dwFullSize As UInteger
            ' Uncompressed size of file
            Public dwFlags As UInteger
            ' Flags for file
        End Structure

        <StructLayout(LayoutKind.Sequential)>
        Public Structure HASHTABLEENTRY
            Public dwNameHashA As UInteger
            ' First name hash of file
            Public dwNameHashB As UInteger
            ' Second name hash of file
            <MarshalAs(UnmanagedType.LPArray, SizeConst:=4)>
            Public lcLocale As Char()
            ' Locale ID of file
            Public dwBlockTableIndex As UInteger
            ' Index to the block table entry for the file
        End Structure

        <DllImport("SFmpq.dll")>
        Public Shared Function MpqGetVersionString() As String
        End Function

        <DllImport("SFmpq.dll")>
        Public Shared Function MpqGetVersion() As Single
        End Function

        <DllImport("SFmpq.dll")>
        Public Shared Function SFMpqGetVersionString() As String
        End Function

        ' SFMpqGetVersionString2's return value is the required length of the buffer plus
        ' the terminating null, so use SFMpqGetVersionString2(0, 0); to get the length.
        <DllImport("SFmpq.dll")>
        Public Shared Function SFMpqGetVersionString2(ByVal lpBuffer As IntPtr, ByVal dwBufferLength As UInteger) As UInteger
        End Function

        <DllImport("SFmpq.dll")>
        Public Shared Function SFMpqGetVersion() As SFMPQVERSION
        End Function



        ' Storm functions implemented by this library
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileOpenArchive(ByVal lpFileName As String, ByVal dwPriority As UInteger, ByVal dwFlags As UInteger, ByRef hMPQ As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileCloseArchive(ByVal hMPQ As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetArchiveName(ByVal hMPQ As Integer, ByVal lpBuffer As String, ByVal dwBufferLength As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileOpenFile(ByVal lpFileName As String, ByRef hFile As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileOpenFileEx(ByVal hMPQ As Integer, ByVal lpFileName As String, ByVal dwSearchScope As UInteger, ByRef hFile As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileCloseFile(ByVal hFile As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetFileSize(ByVal hFile As Integer, ByRef lpFileSizeHigh As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetFileArchive(ByVal hFile As Integer, ByRef hMPQ As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetFileName(ByVal hFile As Integer, ByVal lpBuffer As String, ByVal dwBufferLength As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileSetFilePointer(ByVal hFile As Integer, ByVal lDistanceToMove As Integer, ByRef lplDistanceToMoveHigh As Integer, ByVal dwMoveMethod As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileReadFile(ByVal hFile As Integer, ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToRead As UInteger, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As IntPtr) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileSetLocale(ByVal nNewLocale As LCID) As LCID
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetBasePath(ByVal lpBuffer As String, ByVal dwBufferLength As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileSetBasePath(ByVal lpNewBasePath As String) As Integer
        End Function

        ' Extra storm-related functions
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileGetFileInfo(ByVal hFile As Integer, ByVal dwInfoType As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileSetArchivePriority(ByVal hMPQ As Integer, ByVal dwPriority As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileFindMpqHeader(ByVal hFile As Integer) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function SFileListFiles(ByVal hMPQ As Integer, ByVal lpFileLists As String, ByRef lpListBuffer As FILELISTENTRY, ByVal dwFlags As UInteger) As Integer
        End Function

        ' Archive editing functions implemented by this library
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqOpenArchiveForUpdate(ByVal lpFileName As String, ByVal dwFlags As UInteger, ByVal dwMaximumFilesInArchive As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqCloseUpdatedArchive(ByVal hMPQ As Integer, ByVal dwUnknown2 As UInteger) As UInteger
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddFileToArchive(ByVal hMPQ As Integer, ByVal lpSourceFileName As String, ByVal lpDestFileName As String, ByVal dwFlags As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddWaveToArchive(ByVal hMPQ As Integer, ByVal lpSourceFileName As String, ByVal lpDestFileName As String, ByVal dwFlags As UInteger, ByVal dwQuality As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqRenameFile(ByVal hMPQ As Integer, ByVal lpcOldFileName As String, ByVal lpcNewFileName As String) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqDeleteFile(ByVal hMPQ As Integer, ByVal lpFileName As String) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqCompactArchive(ByVal hMPQ As Integer) As Integer
        End Function

        ' Extra archive editing functions
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddFileToArchiveEx(ByVal hMPQ As Integer, ByVal lpSourceFileName As String, ByVal lpDestFileName As String, ByVal dwFlags As UInteger, ByVal dwCompressionType As UInteger, ByVal dwCompressLevel As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddFileFromBufferEx(ByVal hMPQ As Integer, ByVal lpBuffer As Byte(), ByVal dwLength As UInteger, ByVal lpFileName As String, ByVal dwFlags As UInteger, ByVal dwCompressionType As UInteger,
            ByVal dwCompressLevel As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddFileFromBuffer(ByVal hMPQ As Integer, ByVal lpBuffer As Byte(), ByVal dwLength As UInteger, ByVal lpFileName As String, ByVal dwFlags As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqAddWaveFromBuffer(ByVal hMPQ As Integer, ByVal lpBuffer As Byte(), ByVal dwLength As UInteger, ByVal lpFileName As String, ByVal dwFlags As UInteger, ByVal dwQuality As UInteger) As Integer
        End Function
        <DllImport("SFmpq.dll")>
        Public Shared Function MpqSetFileLocale(ByVal hMPQ As Integer, ByVal lpFileName As String, ByVal nOldLocale As LCID, ByVal nNewLocale As LCID) As Integer
        End Function
    End Class

    Public Class Storm
        <DllImport("storm.dll", EntryPoint:="#272")>
        Public Shared Function SFileSetLocale(ByVal hMPQ As UInt16) As Integer
        End Function


        <DllImport("storm.dll", EntryPoint:="#252")>
        Public Shared Function SFileCloseArchive(ByVal hMPQ As Integer) As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#253")>
        Public Shared Function SFileCloseFile(ByVal hFile As Integer) As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#262")>
        Public Shared Function SFileDestroy() As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#266")>
        Public Shared Function SFileOpenArchive(ByVal lpFileName As String, ByVal dwPriority As UInteger, ByVal dwFlags As UInteger, ByRef hMPQ As Integer) As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#268")>
        Public Shared Function SFileOpenFileEx(ByVal hMPQ As Integer, ByVal lpFileName As String, ByVal dwSearchScope As UInteger, ByRef hFile As Integer) As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#269")>
        Public Shared Function SFileReadFile(ByVal hFile As Integer, ByVal lpBuffer As Byte(), ByVal nNumberOfBytesToRead As UInteger, ByRef lpNumberOfBytesRead As Integer, ByVal lpOverlapped As IntPtr) As Integer
        End Function

        <DllImport("storm.dll", EntryPoint:="#265")>
        Public Shared Function SFileGetFileSize(ByVal hFile As Integer, ByRef lpFileSizeHigh As Integer) As Integer
        End Function
    End Class

    Public Class StormLib
        ''' <summary>
        ''' Path to DLL file.
        ''' </summary>
        Public Const DLL As String = "StormLib.dll"

        Public Const STORMLIB_VERSION As UInteger = &H815
        Public Const STORMLIB_VERSION_STRING As String = "8.21"

        Public Const ID_MPQ As UInteger = &H1A51504D
        Public Const ID_MPQ_USERDATA As UInteger = &H1B51504D

        Public Const ERROR_AVI_FILE As UInteger = 10000
        Public Const ERROR_UNKNOWN_FILE_KEY As UInteger = 10001
        Public Const ERROR_CHECKSUM_ERROR As UInteger = 10002
        Public Const ERROR_INTERNAL_FILE As UInteger = 10003
        Public Const ERROR_BASE_FILE_MISSING As UInteger = 10004
        Public Const ERROR_MARKED_FOR_DELETE As UInteger = 10005

        Public Const HASH_TABLE_SIZE_MIN As UInteger = &H4
        Public Const HASH_TABLE_SIZE_DEFAULT As UInteger = &H1000
        Public Const HASH_TABLE_SIZE_MAX As UInteger = &H80000
        Public Const HASH_ENTRY_DELETED As UInteger = &HFFFFFFFEUI
        Public Const HASH_ENTRY_FREE As UInteger = &HFFFFFFFFUI
        Public Const HET_ENTRY_DELETED As UInteger = &H80
        Public Const HET_ENTRY_FREE As UInteger = &H0
        Public Const HASH_STATE_SIZE As UInteger = &H60

        Public Const SFILE_OPEN_HARD_DISK_FILE As UInteger = 2
        Public Const SFILE_OPEN_CDROM_FILE As UInteger = 3
        Public Const SFILE_OPEN_FROM_MPQ As UInteger = &H0
        Public Const SFILE_OPEN_BASE_FILE As UInteger = &HFFFFFFFDUI
        Public Const SFILE_OPEN_ANY_LANG As UInteger = &HFFFFFFFEUI
        Public Const SFILE_OPEN_LOCAL_FILE As UInteger = &HFFFFFFFFUI

        Public Const SFILE_INVALID_SIZE As UInteger = &HFFFFFFFFUI
        Public Const SFILE_INVALID_POS As UInteger = &HFFFFFFFFUI
        Public Const SFILE_INVALID_ATTRIBUTES As UInteger = &HFFFFFFFFUI

        Public Const SFILE_INFO_ARCHIVE_NAME As UInteger = 1
        Public Const SFILE_INFO_ARCHIVE_SIZE As UInteger = 2
        Public Const SFILE_INFO_MAX_FILE_COUNT As UInteger = 3
        Public Const SFILE_INFO_HASH_TABLE_SIZE As UInteger = 4
        Public Const SFILE_INFO_BLOCK_TABLE_SIZE As UInteger = 5
        Public Const SFILE_INFO_SECTOR_SIZE As UInteger = 6
        Public Const SFILE_INFO_HASH_TABLE As UInteger = 7
        Public Const SFILE_INFO_BLOCK_TABLE As UInteger = 8
        Public Const SFILE_INFO_NUM_FILES As UInteger = 9
        Public Const SFILE_INFO_STREAM_FLAGS As UInteger = 10
        Public Const SFILE_INFO_IS_READ_ONLY As UInteger = 11
        Public Const SFILE_INFO_HASH_INDEX As UInteger = 100
        Public Const SFILE_INFO_CODENAME1 As UInteger = 101
        Public Const SFILE_INFO_CODENAME2 As UInteger = 102
        Public Const SFILE_INFO_LANGID As UInteger = 103
        Public Const SFILE_INFO_BLOCKINDEX As UInteger = 104
        Public Const SFILE_INFO_FILE_SIZE As UInteger = 105
        Public Const SFILE_INFO_COMPRESSED_SIZE As UInteger = 106
        Public Const SFILE_INFO_FLAGS As UInteger = 107
        Public Const SFILE_INFO_POSITION As UInteger = 108
        Public Const SFILE_INFO_KEY As UInteger = 109
        Public Const SFILE_INFO_KEY_UNFIXED As UInteger = 110
        Public Const SFILE_INFO_FILETIME As UInteger = 111
        Public Const SFILE_INFO_PATCH_CHAIN As UInteger = 112

        Public Const SFILE_VERIFY_SECTOR_CRC As UInteger = &H1
        Public Const SFILE_VERIFY_FILE_CRC As UInteger = &H2
        Public Const SFILE_VERIFY_FILE_MD5 As UInteger = &H4
        Public Const SFILE_VERIFY_RAW_MD5 As UInteger = &H8
        Public Const SFILE_VERIFY_ALL As UInteger = &HF
        Public Const SFILE_VERIFY_MPQ_HEADER As UInteger = &H1
        Public Const SFILE_VERIFY_HET_TABLE As UInteger = &H2
        Public Const SFILE_VERIFY_BET_TABLE As UInteger = &H3
        Public Const SFILE_VERIFY_HASH_TABLE As UInteger = &H4
        Public Const SFILE_VERIFY_BLOCK_TABLE As UInteger = &H5
        Public Const SFILE_VERIFY_HIBLOCK_TABLE As UInteger = &H6
        Public Const SFILE_VERIFY_FILE As UInteger = &H7

        Public Const MPQ_PATCH_PREFIX_LEN As UInteger = &H20

        Public Const MPQ_FLAG_READ_ONLY As UInteger = &H1
        Public Const MPQ_FLAG_CHANGED As UInteger = &H2
        Public Const MPQ_FLAG_PROTECTED As UInteger = &H4
        Public Const MPQ_FLAG_CHECK_SECTOR_CRC As UInteger = &H8
        Public Const MPQ_FLAG_NEED_FIX_SIZE As UInteger = &H10
        Public Const MPQ_FLAG_INV_LISTFILE As UInteger = &H20
        Public Const MPQ_FLAG_INV_ATTRIBUTES As UInteger = &H40

        Public Const MPQ_FILE_IMPLODE As UInteger = &H100
        Public Const MPQ_FILE_COMPRESS As UInteger = &H200
        Public Const MPQ_FILE_COMPRESSED As UInteger = &HFF00
        Public Const MPQ_FILE_ENCRYPTED As UInteger = &H10000
        Public Const MPQ_FILE_FIX_KEY As UInteger = &H20000
        Public Const MPQ_FILE_PATCH_FILE As UInteger = &H100000
        Public Const MPQ_FILE_SINGLE_UNIT As UInteger = &H1000000
        Public Const MPQ_FILE_DELETE_MARKER As UInteger = &H2000000
        Public Const MPQ_FILE_SECTOR_CRC As UInteger = &H4000000
        Public Const MPQ_FILE_EXISTS As UInteger = &H80000000UI
        Public Const MPQ_FILE_REPLACEEXISTING As UInteger = &H80000000UI

        Public Const MPQ_COMPRESSION_HUFFMANN As UInteger = &H1
        Public Const MPQ_COMPRESSION_ZLIB As UInteger = &H2
        Public Const MPQ_COMPRESSION_PKWARE As UInteger = &H8
        Public Const MPQ_COMPRESSION_BZIP2 As UInteger = &H10
        Public Const MPQ_COMPRESSION_SPARSE As UInteger = &H20
        Public Const MPQ_COMPRESSION_ADPCM_MONO As UInteger = &H40
        Public Const MPQ_COMPRESSION_ADPCM_STEREO As UInteger = &H80
        Public Const MPQ_COMPRESSION_LZMA As UInteger = &H12
        Public Const MPQ_COMPRESSION_NEXT_SAME As UInteger = &HFFFFFFFFUI

        Public Const MPQ_WAVE_QUALITY_HIGH As UInteger = 0
        Public Const MPQ_WAVE_QUALITY_MEDIUM As UInteger = 1
        Public Const MPQ_WAVE_QUALITY_LOW As UInteger = 2

        Public Const HET_TABLE_SIGNATURE As UInteger = &H1A544548
        Public Const BET_TABLE_SIGNATURE As UInteger = &H1A544542

        Public Const MPQ_KEY_HASH_TABLE As UInteger = &HC3AF3770UI
        Public Const MPQ_KEY_BLOCK_TABLE As UInteger = &HEC83B3A3UI

        Public Const MPQ_DATA_BITMAP_SIGNATURE As UInteger = &H33767470

        Public Const LISTFILE_NAME As String = "(listfile)"
        Public Const SIGNATURE_NAME As String = "(signature)"
        Public Const ATTRIBUTES_NAME As String = "(attributes)"
        Public Const PATCH_METADATA_NAME As String = "(patch_metadata)"

        Public Const MPQ_FORMAT_VERSION_1 As UInteger = 0
        Public Const MPQ_FORMAT_VERSION_2 As UInteger = 1
        Public Const MPQ_FORMAT_VERSION_3 As UInteger = 2
        Public Const MPQ_FORMAT_VERSION_4 As UInteger = 3

        Public Const MPQ_ATTRIBUTE_CRC32 As UInteger = &H1
        Public Const MPQ_ATTRIBUTE_FILETIME As UInteger = &H2
        Public Const MPQ_ATTRIBUTE_MD5 As UInteger = &H4
        Public Const MPQ_ATTRIBUTE_PATCH_BIT As UInteger = &H8
        Public Const MPQ_ATTRIBUTE_ALL As UInteger = &HF
        Public Const MPQ_ATTRIBUTES_V1 As UInteger = 100

        Public Const BASE_PROVIDER_FILE As UInteger = &H0
        Public Const BASE_PROVIDER_MAP As UInteger = &H1
        Public Const BASE_PROVIDER_HTTP As UInteger = &H2
        Public Const BASE_PROVIDER_MASK As UInteger = &HF

        Public Const STREAM_PROVIDER_LINEAR As UInteger = &H0
        Public Const STREAM_PROVIDER_PARTIAL As UInteger = &H10
        Public Const STREAM_PROVIDER_ENCRYPTED As UInteger = &H20
        Public Const STREAM_PROVIDER_MASK As UInteger = &HF0
        Public Const STREAM_FLAG_READ_ONLY As UInteger = &H100
        Public Const STREAM_FLAG_WRITE_SHARE As UInteger = &H200
        Public Const STREAM_FLAG_MASK As UInteger = &HFF00
        Public Const STREAM_OPTIONS_MASK As UInteger = &HFFFF

        Public Const MPQ_OPEN_NO_LISTFILE As UInteger = &H10000
        Public Const MPQ_OPEN_NO_ATTRIBUTES As UInteger = &H20000
        Public Const MPQ_OPEN_FORCE_MPQ_V1 As UInteger = &H40000
        Public Const MPQ_OPEN_CHECK_SECTOR_CRC As UInteger = &H80000
        Public Const MPQ_OPEN_READ_ONLY As UInteger = STREAM_FLAG_READ_ONLY
        Public Const MPQ_OPEN_ENCRYPTED As UInteger = STREAM_PROVIDER_ENCRYPTED

        Public Const MPQ_CREATE_ATTRIBUTES As UInteger = &H100000
        Public Const MPQ_CREATE_ARCHIVE_V1 As UInteger = &H0
        Public Const MPQ_CREATE_ARCHIVE_V2 As UInteger = &H1000000
        Public Const MPQ_CREATE_ARCHIVE_V3 As UInteger = &H2000000
        Public Const MPQ_CREATE_ARCHIVE_V4 As UInteger = &H3000000
        Public Const MPQ_CREATE_ARCHIVE_VMASK As UInteger = &HF000000

        Public Const MPQ_HEADER_SIZE_V1 As UInteger = &H20
        Public Const MPQ_HEADER_SIZE_V2 As UInteger = &H2C
        Public Const MPQ_HEADER_SIZE_V3 As UInteger = &H44
        Public Const MPQ_HEADER_SIZE_V4 As UInteger = &HD0

        Public Const FLAGS_TO_FORMAT_SHIFT As UInteger = 24

        Public Const VERIFY_OPEN_ERROR As UInteger = &H1
        Public Const VERIFY_READ_ERROR As UInteger = &H2

        Public Const VERIFY_FILE_HAS_SECTOR_CRC As UInteger = &H4
        Public Const VERIFY_FILE_SECTOR_CRC_ERROR As UInteger = &H8
        Public Const VERIFY_FILE_HAS_CHECKSUM As UInteger = &H10
        Public Const VERIFY_FILE_CHECKSUM_ERROR As UInteger = &H20
        Public Const VERIFY_FILE_HAS_MD5 As UInteger = &H40
        Public Const VERIFY_FILE_MD5_ERROR As UInteger = &H80
        Public Const VERIFY_FILE_HAS_RAW_MD5 As UInteger = &H100
        Public Const VERIFY_FILE_RAW_MD5_ERROR As UInteger = &H200

        Public Const ERROR_NO_SIGNATURE As UInteger = 0
        Public Const ERROR_VERIFY_FAILED As UInteger = 1
        Public Const ERROR_WEAK_SIGNATURE_OK As UInteger = 2
        Public Const ERROR_WEAK_SIGNATURE_ERROR As UInteger = 3
        Public Const ERROR_STRONG_SIGNATURE_OK As UInteger = 4
        Public Const ERROR_STRONG_SIGNATURE_ERROR As UInteger = 5

        Public Const MD5_DIGEST_SIZE As UInteger = &H10
        Public Const SHA1_DIGEST_SIZE As UInteger = &H14

        Public Const LANG_NEUTRAL As UInteger = 0
        Public Const LANG_CHINESE As UInteger = &H404
        Public Const LANG_CZECH As UInteger = &H405
        Public Const LANG_GERMAN As UInteger = &H407
        Public Const LANG_ENGLISH As UInteger = &H409
        Public Const LANG_SPANISH As UInteger = &H40A
        Public Const LANG_FRENCH As UInteger = &H40C
        Public Const LANG_ITALIAN As UInteger = &H410
        Public Const LANG_JAPANESE As UInteger = &H411
        Public Const LANG_KOREAN As UInteger = &H412
        Public Const LANG_POLISH As UInteger = &H415
        Public Const LANG_PORTUGUESE As UInteger = &H416
        Public Const LANG_RUSSIAN As UInteger = &H419

        Public Const CCB_CHECKING_FILES As UInteger = 1
        Public Const CCB_CHECKING_HASH_TABLE As UInteger = 2
        Public Const CCB_COPYING_NON_MPQ_DATA As UInteger = 3
        Public Const CCB_COMPACTING_FILES As UInteger = 4
        Public Const CCB_CLOSING_ARCHIVE As UInteger = 5

        Public Const SIZE_OF_XFRM_HEADER As UInteger = &HC

        Public Const FILE_BEGIN As UInteger = 0
        Public Const FILE_CURRENT As UInteger = 1
        Public Const FILE_END As UInteger = 2

        'Manipulating MPQ archives
        ''' <summary>
        ''' Function SFileOpenArchive opens a MPQ archive. During the open operation, the archive is checked for corruptions, internal (listfile) and (attributes) are loaded, unless specified otherwise.
        ''' The archive is open for read and write operations, unless MPQ_OPEN_READ_ONLY is specified.
        ''' </summary>
        ''' <param name="szMpqName">Archive file name to open.</param>
        ''' <param name="dwPriority">Priority of the archive for later search. StormLib does not use this parameter, set it to zero.</param>
        ''' <param name="dwFlags">Flags that specify additional options about how to open the file. Several flags can be combined that can tell StormLib where to open the MPQ from, and what's the stream type of the MPQ.
        ''' Note that the BASE_PROVIDER_*, STREAM_PROVIDER_* and STREAM_FLAG_* are valid since StormLib v 8.10.</param>
        ''' <param name="phMpq">Pointer to a variable of HANDLE type, where the opened archive handle will be stored.</param>
        ''' <returns>When the function succeeds, it returns nonzero and phMPQ contains the handle of the opened archive. When the archive cannot be open, function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileOpenArchive")>
        Public Shared Function SFileOpenArchive(szMpqName As String, dwPriority As UInteger, dwFlags As UInteger, ByRef phMpq As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileCreateArchive opens or creates the MPQ archive. The function can also convert an existing file to MPQ archive. The MPQ archive is always open for write operations. The function internally verifies the file using SFileOpenArchive. If the file already exists and it is an MPQ archive, the function fails and GetLastError() returns ERROR_ALREADY_EXISTS.
        ''' </summary>
        ''' <param name="szMpqName">Archive file name to be created.</param>
        ''' <param name="dwFlags">Specifies additional flags for MPQ creation process.</param>
        ''' <param name="dwMaxFileCount">File count limit. The value must be in range of HASH_TABLE_SIZE_MIN (0x04) and HASH_TABLE_SIZE_MAX (0x80000). StormLib will automatically calculate size of hash tables and block tables from this value.</param>
        ''' <param name="phMpq">Pointer to a variable of HANDLE type, where the opened archive handle will be stored.</param>
        ''' <returns>When the function succeeds, it returns nonzero and phMPQ contains the handle of the new archive. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileCreateArchive")>
        Public Shared Function SFileCreateArchive(szMpqName As String, dwFlags As UInteger, dwMaxFileCount As UInteger, ByRef phMpq As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' SFileAddListFile adds an external listfile to the open MPQ. Note that the listfile is merely added to the memory structures of the open MPQ. On-disk structures of the MPQ are not changed. Use this function to specify an extra listfile to an opened MPQ, for example when there is no internal listfile, or if the internal listfile is not complete.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ.</param>
        ''' <param name="szListFile">Listfile name to add. If this parameter is NULL, the function adds the internal listfile from the MPQ, if present. Adding the same listfile multiple times has no effect.</param>
        ''' <returns>When the function succeeds, it returns ERROR_SUCCESS. On an error, the function returns error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileAddListFile")>
        Public Shared Function SFileAddListFile(hMpq As UInteger, szListFile As String) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetLocale sets a preferred locale for file functions, such as SFileOpenFileEx or SFileAddFileEx. The locale is stored as a global variable and thus affects every open or add operation.
        ''' Note that this function does not change locale ID of any existing file in the MPQ.
        ''' </summary>
        ''' <param name="lcNewLocale">Locale ID to be set.</param>
        ''' <returns>The function never fails and always returns lcNewLocale.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetLocale")>
        Public Shared Function SFileSetLocale(lcNewLocale As UInteger) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileGetLocale returns locale that is set as a preferred locale for files that will be open by SFileOpenFileEx and added by SFileAddFileEx. The locale is stored as a global variable and thus affects every open or add operation.
        ''' </summary>
        ''' <returns>The function never fails and always returns current locale ID.</returns>
        <DllImport(DLL, EntryPoint:="SFileGetLocale")>
        Public Shared Function SFileGetLocale() As UInteger
        End Function
        ''' <summary>
        ''' Function SFileFlushArchive saves any in-memory structures to the MPQ archive on disk. Due to performance reasons, StormLib caches several data structures in memory (e.g. block table or hash table). When a file is added to the MPQ, those structures are only updated in memory. Calling SFileFlushArchive forces saving in-memory MPQ tables to the file, preventing a MPQ corruption incase of power down or crash of the calling application. Note that this function is called internally when the archive is closed.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ.</param>
        <DllImport(DLL, EntryPoint:="SFileFlushArchive")>
        Public Shared Function SFileFlushArchive(hMpq As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileFlushArchive saves any in-memory structures to the MPQ archive on disk. Due to performance reasons, StormLib caches several data structures in memory (e.g. block table or hash table). When a file is added to the MPQ, those structures are only updated in memory. Calling SFileFlushArchive forces saving in-memory MPQ tables to the file, preventing a MPQ corruption incase of power down or crash of the calling application. Note that this function is called internally when the archive is closed.
        ''' </summary>
        ''' <param name="phMpq"> Handle to an open MPQ.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileCloseArchive")>
        Public Shared Function SFileCloseArchive(phMpq As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetMaxFileCount changes the limit for number of files that can be stored in the archive. No files are changed during this operation.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open archive. This handle must have been obtained by SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="dwMaxFileCount">New size of the hash table. This parameter must be in range of HASH_TABLE_SIZE_MIN and HASH_TABLE_SIZE_MAX.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetMaxFileCount")>
        Public Shared Function SFileSetMaxFileCount(hMpq As UInteger, dwMaxFileCount As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' SFileCompactArchive might take several minutes to complete, depending on size of the archive being rebuilt. If you want to use SFileCompactArchive in your application, you can utilize a compact callback, which can be set by SFileSetCompactCallback.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive.</param>
        ''' <param name="szListFile">Allows to specify an additional listfile, that will be used together with internal listfile. Can be NULL.</param>
        ''' <param name="bReserved">Not used, set to zero.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileCompactArchive")>
        Public Shared Function SFileCompactArchive(hMpq As UInteger, szListFile As String, bReserved As Boolean) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetCompactCallback sets a callback that will be called during operations performed by SFileCompactArchive. Registering a callback will help the calling application to show a progress about the operation, which will enhance user experience with the application.
        ''' </summary>
        ''' <param name="hMpq">Handle to the MPQ that will be compacted. Current version of StormLib ignores the parameter, but it is recommended to set it to the handle of the archive.</param>
        ''' <param name="pfnCompactCB">Pointer to the callback function. For the prototype and parameters, see below.</param>
        ''' <param name="pvUserData">User defined data that will be passed to the callback function.</param>
        ''' <returns>The function never fails and always sets the callback.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetCompactCallback")>
        Public Shared Function SFileSetCompactCallback(hMpq As UInteger, pfnCompactCB As SFILE_COMPACT_CALLBACK, pvUserData As IntPtr) As Boolean
        End Function

        'Using patched archives
        ''' <summary>
        ''' Function SFileOpenPatchArchive adds a patch archive to the existing open MPQ. The MPQ must have been open by SFileOpenArchive, and also with MPQ_OPEN_READ_ONLY specified. The patch archive is added to the list of patches that belong to the primary MPQ. No handle is returned, and the patch(es) is closed when the primary MPQ handle is closed. The patch MPQ opened during the process is maintained internally by StormLib and cannot be accessed directly.
        ''' </summary>
        ''' <param name="hMpq">Handle to a MPQ that serves as primary MPQ when patched.</param>
        ''' <param name="szMpqName">Name of the patch MPQ to be added.</param>
        ''' <param name="szPatchPathPrefix">Pointer to patch prefix for file names. This parameter can be NULL, which makes StormLib to determine patch prefix for the specific combination of names of the base MPQ and patch MPQ.</param>
        ''' <param name="dwFlags">Reserved for future use.</param>
        ''' <returns>When the function succeeds, it returns nonzero. When the archive cannot be added as patch archive, function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileOpenPatchArchive")>
        Public Shared Function SFileOpenPatchArchive(hMpq As UInteger, szMpqName As String, szPatchPathPrefix As String, dwFlags As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileIsPatchedArchive returns true, if the given MPQ has one or more patches added.
        ''' </summary>
        ''' <param name="hMpq">Handle to a MPQ in question.</param>
        ''' <returns>The function returns true, when there is at least one patch added to the MPQ. Otherwise, it returns false.</returns>
        <DllImport(DLL, EntryPoint:="SFileIsPatchedArchive")>
        Public Shared Function SFileIsPatchedArchive(hMpq As UInteger) As Boolean
        End Function

        'Reading files
        ''' <summary>
        ''' Function SFileOpenFileEx opens a file from MPQ archive. The file is only open for read. The file must be closed by calling SFileCloseFile. All files must be closed before the MPQ archive is closed.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open archive.</param>
        ''' <param name="szFileName">Name or index of the file to open.</param>
        ''' <param name="dwSearchScope">Value that specifies how exactly the file should be open.</param>
        ''' <param name="phFile">Pointer to a variable of HANDLE type, that will receive HANDLE to the open file.</param>
        ''' <returns>When the function succeeds, it returns nonzero and phFile contains the handle of the opened file. When the file cannot be open, function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileOpenFileEx")>
        Public Shared Function SFileOpenFileEx(hMpq As UInteger, szFileName As String, dwSearchScope As UInteger, ByRef phFile As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileGetFileSize retrieves the size of an open file.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open file. The file handle must have been created by SFileOpenFileEx.</param>
        ''' <param name="pdwFileSizeHigh">Receives high 32 bits of the a file size. This parameter can be NULL.</param>
        ''' <returns>When the function succeeds, it returns lower 32-bit of the file size. On an error, it returns SFILE_INVALID_SIZE and GetLastError returns an error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileGetFileSize")>
        Public Shared Function SFileGetFileSize(hMpq As UInteger, ByRef pdwFileSizeHigh As UInteger) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileSetFilePointer sets current position in an open file.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open file. The file handle must have been created by SFileOpenFileEx.</param>
        ''' <param name="lFilePos"> Low 32 bits of new position in the file.</param>
        ''' <param name="plFilePosHigh">Pointer to a high 32 bits of new position in the file.</param>
        ''' <param name="dwMoveMethod">The starting point for the file pointer move.</param>
        ''' <returns>When the function succeeds, it returns lower 32-bit of the file size. On an error, it returns SFILE_INVALID_SIZE and GetLastError returns an error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetFilePointer")>
        Public Shared Function SFileSetFilePointer(hMpq As UInteger, lFilePos As Integer, ByRef plFilePosHigh As Integer, dwMoveMethod As UInteger) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileReadFile reads data from an open file.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open file. The file handle must have been created by SFileOpenFileEx.</param>
        ''' <param name="lpBuffer">Pointer to buffer that will receive loaded data. The buffer size must be greater or equal to dwToRead.</param>
        ''' <param name="dwToRead">Number of bytes to be read.</param>
        ''' <param name="pdwRead">Pointer to DWORD that will receive number of bytes read.</param>
        ''' <param name="lpOverlapped">If hFile is handle to a local disk file, lpOverlapped is passed to ReadFile. Otherwise not used.</param>
        ''' <returns>When all requested bytes have been read, the function returns true. When less than requested bytes have been read, the function returns false and GetLastError returns ERROR_HANDLE_EOF. If an error occured, the function returns false and GetLastError returns an error code different from ERROR_HANDLE_EOF.</returns>
        <DllImport(DLL, EntryPoint:="SFileReadFile")>
        Public Shared Function SFileReadFile(hMpq As UInteger, lpBuffer As Byte(), dwToRead As UInteger, ByRef pdwRead As UInteger, lpOverlapped As IntPtr) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileCloseFile closes an open MPQ file. All in-memory data are freed. After this function finishes, the hFile handle is no longer valid and must not be used in any file operations.
        ''' </summary>
        ''' <param name="hFile">Handle to an open file.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileCloseFile")>
        Public Shared Function SFileCloseFile(hFile As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileHasFile performs a quick check if a file exists within the MPQ archive. The function does not perform file open, not even internally. It merely checks hash table if the file is present.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ.</param>
        ''' <param name="szFileName">Name of the file to check.</param>
        ''' <returns>When the file is present in the MPQ, function returns true. When the file is not present in the MPQ archive, the function returns false and GetLastError returns ERROR_FILE_NOT_FOUND. If an error occured, the function returns false and GetLastError returns an error code different than ERROR_FILE_NOT_FOUND.</returns>
        <DllImport(DLL, EntryPoint:="SFileHasFile")>
        Public Shared Function SFileHasFile(hMpq As UInteger, szFileName As String) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileGetFileName retrieves the name of an open file.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open file. The file handle must have been created by SFileOpenFileEx.</param>
        ''' <param name="szFileName">Receives the file name. The buffer must be at least MAX_PATH characters long.</param>
        ''' <returns>When the function succeeds, it returns true and buffer pointed by szFileName contains name of the file. On an error, the function returns false and GetLastError returns an error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileGetFileName")>
        Public Shared Function SFileGetFileName(hMpq As UInteger, ByRef szFileName As String) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileGetFileInfo retrieves an information about an open MPQ archive or a file.
        ''' </summary>
        ''' <param name="hMpqOrFile">Handle to an open file or to an open MPQ archive, depending on the value of dwInfoType.</param>
        ''' <param name="dwInfoType">Type of information to retrieve. See Return Value for more information.</param>
        ''' <param name="pvFileInfo">Pointer to buffer where to store the required information.</param>
        ''' <param name="cbFileInfo">Size of the buffer pointed by pvFileInfo.</param>
        ''' <param name="pcbLengthNeeded">Size, in bytes, needed to store the information into pvFileInfo.</param>
        ''' <returns>When the function succeeds, it returns true. On an error, the function returns false and GetLastError returns error code. Possible error codes may be ERROR_INVALID_PARAMETER (unknown file info type) or ERROR_INSUFFICIENT_BUFFER (not enough space in the supplied buffer).</returns>
        <DllImport(DLL, EntryPoint:="SFileGetFileInfo")>
        Public Shared Function SFileGetFileInfo(hMpqOrFile As UInteger, dwInfoType As UInteger, pvFileInfo As IntPtr, cbFileInfo As UInteger, ByRef pcbLengthNeeded As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileVerifyFile verifies the file by its CRC and MD5. The (attributes) file must exist in the MPQ and must have been open by SFileOpenArchive or created by SFileCreateArchive. The entire file is always checked for readability. Additional flags in dwFlags turn on extra checks on the file.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ archive.</param>
        ''' <param name="szFileName">Name of a file to verify.</param>
        ''' <param name="dwFlags">Specifies what to verify.</param>
        ''' <returns>Return value is zero when no problerms were found.</returns>
        <DllImport(DLL, EntryPoint:="SFileVerifyFile")>
        Public Shared Function SFileVerifyFile(hMpq As UInteger, szFileName As String, dwFlags As UInteger) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileVerifyArchive verifies digital signature of the archive, is a digital signature is present. in the MPQ and must have been open by SFileOpenArchive. Note that MPQ archives created by StormLib are never signed.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ archive to be verified.</param>
        ''' <returns>Return value can be one of the following values:</returns>
        <DllImport(DLL, EntryPoint:="SFileVerifyArchive")>
        Public Shared Function SFileVerifyArchive(hMpq As UInteger) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileExtractFile extracts one file from an MPQ archive.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ archive.</param>
        ''' <param name="szToExtract">Name of a file within the MPQ that is to be extracted.</param>
        ''' <param name="szExtracted">Specifies the name of a local file that will be created and will contain data from the extracted MPQ file.</param>
        ''' <param name="dwSearchScope">This parameter refines the definition of what to extract. If you want ot extract an unpatched file, use SFILE_OPEN_FROM_MPQ (this is the default parameter). If you want to extract patched version of the file, use SFILE_OPEN_PATCHED_FILE.</param>
        ''' <returns>If the MPQ file has been successfully extracted into the target file, the function returns true. On an error, the function returns false and GetLastError returns an error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileExtractFile")>
        Public Shared Function SFileExtractFile(hMpq As UInteger, szToExtract As String, szExtracted As String, dwSearchScope As UInteger) As Boolean
        End Function

        'File searching
        ''' <summary>
        ''' Function SFileFindFirstFile searches an MPQ archive and returns name of the first file that matches the given search mask and exists in the MPQ archive. When the caller finishes searching, the returned handle must be freed by calling SFileFindClose.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open archive.</param>
        ''' <param name="szMask">Name of the search mask. "*" will return all files.</param>
        ''' <param name="lpFindFileData">Pointer to SFILE_FIND_DATA structure that will receive information about the found file.</param>
        ''' <param name="szListFile">Name of an extra list file that will be used for searching. Note that SFileAddListFile is called internally. The internal listfile in the MPQ is always used (if exists). This parameter can be NULL.</param>
        ''' <returns>When the function succeeds, it returns handle to the MPQ search object and the SFILE_FIND_DATA structure is filled with information about the file. On an error, the function returns NULL and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileFindFirstFile")>
        Public Shared Function SFileFindFirstFile(hMpq As UInteger, szMask As String, <MarshalAs(UnmanagedType.LPStruct)> lpFindFileData As SFILE_FIND_DATA, szListFile As String) As UInteger
        End Function
        ''' <summary>
        ''' Function SFileFindNextFile continues search that has been initiated by SFileFindFirstFile. When the caller finishes searching, the returned handle must be freed by calling SFileFindClose.
        ''' </summary>
        ''' <param name="hFind">Search handle. Must have been obtained by call to SFileFindFirstFile.</param>
        ''' <param name="lpFindFileData">Pointer to SFILE_FIND_DATA structure that will receive information about the found file. For layout of the structure, see SFileFindFirstFile.</param>
        ''' <returns>When the function succeeds, it returns nonzero and the SFILE_FIND_DATA structure is filled with information about the file. On an error, the function returns zero and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileFindNextFile")>
        Public Shared Function SFileFindNextFile(hFind As UInteger, <MarshalAs(UnmanagedType.LPStruct)> lpFindFileData As SFILE_FIND_DATA) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileFindClose closes a find handle that has been created by SFileFindFirstFile.
        ''' </summary>
        ''' <param name="hFind">Search handle. Must have been obtained by call to SFileFindFirstFile.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns zero and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileFindClose")>
        Public Shared Function SFileFindClose(hFind As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SListFileFindFirstFile searches a listfile and returns name of the first file that matches the given search mask. When the caller finishes searching, the returned handle must be freed by calling SListFileFindClose. Note that unlike SFileFindFirstFile, this function does not check if the file exists within the archive and doesn't call SFileAddListFile.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open archive. This parameter must only be valid if szListFile is NULL.</param>
        ''' <param name="szListFile">Name of the listfile that will be used for searching. If this parameter is NULL, the function searches the MPQ internal listfile (if any).</param>
        ''' <param name="szMask">Name of the search mask. "*" will return all files.</param>
        ''' <param name="lpFindFileData">Pointer to SFILE_FIND_DATA structure that will receive name of the found file. For layout of this structure, see SFileFindFirstFile.</param>
        ''' <returns>When the function succeeds, it returns handle to the MPQ search object and the cFileName member of SFILE_FIND_DATA structure is filled with name of the file. On an error, the function returns NULL and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SListFileFindFirstFile")>
        Public Shared Function SListFileFindFirstFile(hMpq As UInteger, szListFile As String, szMask As String, <MarshalAs(UnmanagedType.LPStruct)> lpFindFileData As SFILE_FIND_DATA) As UInteger
        End Function
        ''' <summary>
        ''' Function SListFileFindNextFile continues listfile searching initiated by SListFileFindFirstFile.
        ''' </summary>
        ''' <param name="hFind">Search handle. Must have been obtained by call to SListFileFindFirstFile.</param>
        ''' <param name="lpFindFileData">Pointer to SFILE_FIND_DATA structure that will receive name of the found file. For layout of the structure, see SFileFindFirstFile.</param>
        ''' <returns>When the function succeeds, it returns nonzero and the cFileName member of SFILE_FIND_DATA structure is filled with name of the file. On an error, the function returns zero and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SListFileFindNextFile")>
        Public Shared Function SListFileFindNextFile(hFind As UInteger, <MarshalAs(UnmanagedType.LPStruct)> lpFindFileData As SFILE_FIND_DATA) As UInteger
        End Function
        ''' <summary>
        ''' Function SListFileFindClose closes a find handle that has been created by SListFileFindFirstFile.
        ''' </summary>
        ''' <param name="hFind">Search handle. Must have been obtained by call to SListFileFindFirstFile.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns zero and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SListFileFindClose")>
        Public Shared Function SListFileFindClose(hFind As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileEnumLocales enumerates all locales for the given file that are present in the MPQ.
        ''' </summary>
        ''' <param name="hMpq">Handle to a MPQ.</param>
        ''' <param name="szFileName">Name of a file to enumerate the locales.</param>
        ''' <param name="plcLocales">An array of LCIDs that will receive locales. This parameter can be NULL if pdwMaxLocales points to zero.</param>
        ''' <param name="pdwMaxLocales"> On input, this argument must point to a variable that contains maximum number of entries in plcLocales array. On output, this variable receives number of locales that are for the file. This argument cannot be NULL.</param>
        ''' <param name="dwSearchScope">This parameter is ignored.</param>
        ''' <returns>When the function succeeds, it returns ERROR_SUCCESS. On an error, the function returns an error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileEnumLocales")>
        Public Shared Function SListFileFindClose(hMpq As UInteger, szFileName As String, ByRef plcLocales As UInteger, ByRef pdwMaxLocales As UInteger, dwSearchScope As UInteger) As Boolean
        End Function

        'Adding files to MPQ
        ''' <summary>
        ''' Function SFileCreateFile creates a new file within archive and prepares it for storing the data.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szArchivedName">A name under which the file will be stored into the MPQ.</param>
        ''' <param name="FileTime">Specifies the file date-time that will be stored into "(attributes)" file in MPQ. This parameter is optional and can be zero.</param>
        ''' <param name="dwFileSize">Specifies the size of the data that will be written to the file. This size of the file is set by the call and cannot be changed. The subsequent amount of data written must exactly match the size given by this parameter.</param>
        ''' <param name="lcLocale">Specifies the locale for the new file.</param>
        ''' <param name="dwFlags">Specifies additional options about how to add the file to the MPQ. For more information about these flags, see SFileAddFileEx.</param>
        ''' <param name="phFile">Pointer to a variable of HANDLE type that receives a valid handle. Note that this handle can only be used in call to SFileWriteFile and SFileFinishFile. This handle must never be passed to another file function. Moreover, this handle must always be freed by SFileFinishFile, if not NULL.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileCreateFile")>
        Public Shared Function SFileCreateFile(hMpq As UInteger, szArchivedName As String, FileTime As ULong, dwFileSize As UInteger, lcLocale As UInteger, dwFlags As UInteger,
        ByRef phFile As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileWriteFile writes data to the archive. The file must have been created by SFileCreateFile.
        ''' </summary>
        ''' <param name="hMpq">Handle to a new file within MPQ. This handle must have been obtained by calling SFileCreateFile.</param>
        ''' <param name="pvData">Pointer to data to be written to the file.</param>
        ''' <param name="dwSize">Size of the data that are to be written to the MPQ.</param>
        ''' <param name="dwCompression">Specifies the type of data compression that is to be applied to the data, in case the amount of the data will reach size of one file sector. For more information about the available compressions, see SFileAddFileEx.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileWriteFile")>
        Public Shared Function SFileWriteFile(hMpq As UInteger, pvData As IntPtr, dwSize As UInteger, dwCompression As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileFinishFile finalized creation of the archived file. The file must have been created by SFileCreateFile.
        ''' </summary>
        ''' <param name="hFile">Handle to a new file within MPQ. This handle must have been obtained by calling SFileCreateFile.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileFinishFile")>
        Public Shared Function SFileFinishFile(hFile As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileAddFileEx adds a file to the MPQ archive. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation might cause MPQ fragmentation. To reduce size of the MPQ, use SFileCompactArchive.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szfileName">Name of a file to be added to the MPQ.</param>
        ''' <param name="szArchivedName">A name under which the file will be stored into the MPQ. This does not have to be the same like the original file name.</param>
        ''' <param name="dwFlags">Specifies additional options about how to add the file to the MPQ.</param>
        ''' <param name="dwCompression">Compression method of the first file block. This parameter is ignored if MPQ_FILE_COMPRESS is not specified in dwFlags. </param>
        ''' <param name="dwCompressionNext">Compression method of rest of the file. This parameter optional and is ignored if MPQ_FILE_COMPRESS is not specified in dwFlags.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileAddFileEx")>
        Public Shared Function SFileAddFileEx(hMpq As UInteger, szfileName As String, szArchivedName As String, dwFlags As UInteger, dwCompression As UInteger, dwCompressionNext As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileAddFile adds a file to the MPQ archive. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation might cause MPQ fragmentation. To reduce size of the MPQ, use SFileCompactArchive.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szfileName">Name of a file to be added to the MPQ.</param>
        ''' <param name="szArchivedName">A name under which the file will be stored into the MPQ. This does not have to be the same like the original file name.</param>
        ''' <param name="dwFlags">Specifies additional options about how to add the file to the MPQ. For more information about these flags, see SFileAddFileEx.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileAddFile")>
        Public Shared Function SFileAddFile(hMpq As UInteger, szfileName As String, szArchivedName As String, dwFlags As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileAddWave adds a WAVE file to the MPQ archive. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation might cause MPQ fragmentation. To reduce size of the MPQ, use SFileCompactArchive. This function is obsolete. WAVE files should be stored the same way like normal data files.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szfileName">Name of a file to be added to the MPQ.</param>
        ''' <param name="szArchivedName">A name under which the file will be stored into the MPQ. This does not have to be the same like the original file name.</param>
        ''' <param name="dwFlags">Specifies additional options about how to add the file to the MPQ. For more information about these flags, see SFileAddFileEx.</param>
        ''' <param name="dwQuality">Specifies quality of WAVE compression. This parameter is ignored if MPQ_FILE_COMPRESS is not specified in dwFlags.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileAddWave")>
        Public Shared Function SFileAddWave(hMpq As UInteger, szfileName As String, szArchivedName As String, dwFlags As UInteger, dwQuality As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileRemoveFile removes a file from MPQ. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation leaves a gap in the MPQ file. To reduce size of the MPQ, use SFileCompactArchive.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szFileName">Name of a file to be removed.</param>
        ''' <param name="dwSearchScope">This parameter is ignored in the current version of StormLib.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileRemoveFile")>
        Public Shared Function SFileRemoveFile(hMpq As UInteger, szFileName As String, dwSearchScope As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileRenameFile renames a file within MPQ. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation does not cause MPQ fragmentation and thus it is not necessary to compact the archive.
        ''' </summary>
        ''' <param name="hMpq">Handle to an open MPQ. This handle must have been obtained by calling SFileOpenArchive or SFileCreateArchive.</param>
        ''' <param name="szOldFileName">Name of a file to be renamed.</param>
        ''' <param name="szNewFileName">New name of the file.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileRenameFile")>
        Public Shared Function SFileRenameFile(hMpq As UInteger, szOldFileName As String, szNewFileName As String) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetFileLocale sets new locale ID for an open file. The locale ID is changed in the block table of the MPQ. The MPQ must have been open by SFileOpenArchive or created by SFileCreateArchive. Note that this operation does not cause MPQ fragmentation and thus it is not necessary to compact the archive.
        ''' </summary>
        ''' <param name="hFile">Handle to the file in the MPQ. This handle must have been obtained by calling SFileOpenFileEx.</param>
        ''' <param name="lcNewLocale">New locale ID for the file. For more onformation about locales, see SFileSetLocale.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetFileLocale")>
        Public Shared Function SFileSetFileLocale(hFile As UInteger, lcNewLocale As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetDataCompression configures compression mask for subsequent calls to SFileAddFile. The compression mask is remembered until changed.
        ''' </summary>
        ''' <param name="DataCompression">Bit mask of data compression.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns false and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetDataCompression")>
        Public Shared Function SFileSetDataCompression(DataCompression As UInteger) As Boolean
        End Function
        ''' <summary>
        ''' Function SFileSetAddFileCallback sets a callback that will be called during operations performed by SFileAddFileEx. Registering a callback will help the calling application to show a progress about the operation, which enhances user experience with the application.
        ''' </summary>
        ''' <param name="hMpq">Handle to the MPQ that will be compacted. Current version of StormLib ignores the parameter, but it is recommended to set it to the handle of the archive.</param>
        ''' <param name="pfnAddFileCB">Pointer to the callback function. For the prototype and parameters, see below.</param>
        ''' <param name="pvUserData">User defined data that will be passed to the callback function.</param>
        ''' <returns>The function never fails and always sets the callback.</returns>
        <DllImport(DLL, EntryPoint:="SFileSetAddFileCallback")>
        Public Shared Function SFileSetAddFileCallback(hMpq As UInteger, pfnAddFileCB As SFILE_ADDFILE_CALLBACK, pvUserData As IntPtr) As Boolean
        End Function

        'Compression functions
        ''' <summary>
        ''' SCompImplode compresses a data buffer, using Pkware Data Compression library's IMPLODE method.
        ''' </summary>
        ''' <param name="pbOutBuffer">Pointer to buffer where the compressed data will be stored.</param>
        ''' <param name="pcbOutBuffer">On call, pointer to the length of the buffer in pbOutBuffer. When finished, this variable receives length of the compressed data.</param>
        ''' <param name="pbInBuffer">Pointer to data that are to be imploded.</param>
        ''' <param name="cbInBuffer">Length of the data pointed by pbInBuffer.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns FALSE and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SCompImplode")>
        Public Shared Function SCompImplode(pbOutBuffer As Byte(), ByRef pcbOutBuffer As Integer, pbInBuffer As Byte(), cbInBuffer As Integer) As Integer
        End Function
        ''' <summary>
        ''' SCompExplode decompresses a data block compressed by SCompImplode.
        ''' </summary>
        ''' <param name="pbOutBuffer">Pointer to buffer where the decompressed data will be stored.</param>
        ''' <param name="pcbOutBuffer">On call, pointer to the length of the buffer in pbOutBuffer. When finished, this variable receives length of the decompressed data.</param>
        ''' <param name="pbInBuffer">Pointer to data that are to be exploded.</param>
        ''' <param name="cbInBuffer">Length of the data pointed by pbInBuffer.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns FALSE and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SCompExplode")>
        Public Shared Function SCompExplode(pbOutBuffer As Byte(), ByRef pcbOutBuffer As Integer, pbInBuffer As Byte(), cbInBuffer As Integer) As Integer
        End Function
        ''' <summary>
        ''' SCompCompress compresses a data buffer, using various compression methods.
        ''' </summary>
        ''' <param name="pvOutBuffer">Pointer to buffer where the compressed data will be stored.</param>
        ''' <param name="pcbOutBuffer">On call, pointer to the length of the buffer in pbOutBuffer. When finished, this variable receives length of the compressed data.</param>
        ''' <param name="pbInBuffer">Pointer to data that are to be imploded.</param>
        ''' <param name="cbInBuffer">Length of the data pointed by pbInBuffer.</param>
        ''' <param name="uCompressionMask">Bit mask that specifies compression methods to use. For possible values of this parameter, see SFileAddFileEx.</param>
        ''' <param name="nCmpType">An extra parameter, specific to compression type. This parameter is only used internally by Huffmann compression when applied after an ADPCM compression.</param>
        ''' <param name="nCmpLevel">An extra parameter, specific to compression type. This parameter is used by ADPCM compression and is related to WAVE quality. See Remarks section for additional information.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns FALSE and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SCompCompress")>
        Public Shared Function SCompCompress(pvOutBuffer As IntPtr, ByRef pcbOutBuffer As Integer, pbInBuffer As IntPtr, cbInBuffer As Integer, uCompressionMask As UInteger, nCmpType As Integer,
        nCmpLevel As Integer) As Integer
        End Function
        ''' <summary>
        ''' SCompDecompress decompresses a data block compressed by SCompCompress.
        ''' </summary>
        ''' <param name="pbOutBuffer">Pointer to buffer where the decompressed data will be stored.</param>
        ''' <param name="pcbOutBuffer">On call, pointer to the length of the buffer in pbOutBuffer. When finished, this variable receives length of the decompressed data.</param>
        ''' <param name="pbInBuffer">Pointer to data that are to be exploded.</param>
        ''' <param name="cbInBuffer">Length of the data pointed by pbInBuffer.</param>
        ''' <returns>When the function succeeds, it returns nonzero. On an error, the function returns FALSE and GetLastError gives the error code.</returns>
        <DllImport(DLL, EntryPoint:="SCompDecompress")>
        Public Shared Function SCompDecompress(pbOutBuffer As IntPtr, ByRef pcbOutBuffer As Integer, pbInBuffer As IntPtr, cbInBuffer As Integer) As Integer
        End Function

        'Common delegates and struct
        ''' <summary>
        ''' Callback function used by SFileSetCompactCallback.
        ''' </summary>
        ''' <param name="pvUserData">User data pointer that has been passed to SFileSetCompactCallback.</param>
        ''' <param name="dwWorkType">Contains identifier of the work that is currently being done.</param>
        ''' <param name="pBytesProcessed">Pointer to Int64, indicating what part of the archive has already been compacted.</param>
        ''' <param name="pTotalBytes">Pointer to Int64, containing total number of bytes that has to be compacted.</param>
        Public Delegate Sub SFILE_COMPACT_CALLBACK(pvUserData As IntPtr, dwWorkType As UInteger, ByRef pBytesProcessed As Int64, ByRef pTotalBytes As Int64)

        ''' <summary>
        ''' Callback function used by SFileSetAddFileCallback.
        ''' </summary>
        ''' <param name="pvUserData">User data pointer that has been passed to SFileSetAddFileCallback.</param>
        ''' <param name="dwBytesWritten"> Contains number of bytes that has already been written to the MPQ.</param>
        ''' <param name="dwTotalBytes">Contains total number of bytes to be written to the MPQ. It is the size of the file being added.</param>
        ''' <param name="bFinalCall">If this parameter is true, it means that the operation is complete and succeeded. It also means that this is the last call to the callback function.</param>
        Public Delegate Sub SFILE_ADDFILE_CALLBACK(pvUserData As IntPtr, dwBytesWritten As UInteger, dwTotalBytes As UInteger, bFinalCall As Boolean)

        Public Structure SFILE_FIND_DATA
            ''' <summary>
            ''' Name of the found file
            ''' </summary>
            Private cFileName As String
            ''' <summary>
            ''' Plain name of the found file
            ''' </summary>
            Public szPlainName As String
            ''' <summary>
            ''' Hash table index for the file
            ''' </summary>
            Public dwHashIndex As UInteger
            ''' <summary>
            ''' Block table index for the file
            ''' </summary>
            Public dwBlockIndex As UInteger
            ''' <summary>
            ''' Uncompressed size of the file, in bytes
            ''' </summary>
            Public dwFileSize As UInteger
            ''' <summary>
            ''' MPQ file flags
            ''' </summary>
            Public dwFileFlags As UInteger
            ''' <summary>
            ''' Compressed file size
            ''' </summary>
            Public dwCompSize As UInteger
            ''' <summary>
            ''' Low 32-bits of the file time (0 if not present)
            ''' </summary>
            Public dwFileTimeLo As UInteger
            ''' <summary>
            ''' High 32-bits of the file time (0 if not present)
            ''' </summary>
            Public dwFileTimeHi As UInteger
            ''' <summary>
            ''' Locale version
            ''' </summary>
            Public lcLocale As UInteger
        End Structure

    End Class

    '=======================================================
    'Service provided by Telerik (www.telerik.com)
    'Conversion powered by NRefactory.
    'Twitter: @telerik
    'Facebook: facebook.com/telerik
    '=======================================================

End Module