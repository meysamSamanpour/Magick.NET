#==================================================================================================
function GzipAssembly($inFile,$outFile)
{
	$inFile = FullPath $inFile
	$outFile = FullPath $outFile

	if (!(Test-Path $inFile))
	{
		Write-Error "Unable to find file: $inFile."
		Exit 1
	}
 
	Write-Host "Compressing $inFile to $outFile."
 
	$input = New-Object System.IO.FileStream $inFile, ([IO.FileMode]::Open), ([IO.FileAccess]::Read), ([IO.FileShare]::Read)
 
	$buffer = New-Object byte[]($input.Length)
	$byteCount = $input.Read($buffer, 0, $input.Length)
	$input.Close()
 	
	$output = New-Object System.IO.FileStream $outFile, ([IO.FileMode]::Create), ([IO.FileAccess]::Write), ([IO.FileShare]::None)
	$gzipStream = New-Object System.IO.Compression.GzipStream $output, ([IO.Compression.CompressionMode]::Compress)
 	
	$gzipStream.Write($buffer, 0, $buffer.Length)
	$gzipStream.Close()
 	
	$output.Close()
}
#==================================================================================================
function GzipAssemblies()
{
	GzipAssembly "Magick.NET\bin\ReleaseQ8\v4.0\Win32\Magick.NET-x86.dll" "Magick.NET.AnyCPU\Resources\Q8\Magick.NET-x86.gzip"
	GzipAssembly "Magick.NET\bin\ReleaseQ8\v4.0\x64\Magick.NET-x64.dll" "Magick.NET.AnyCPU\Resources\Q8\Magick.NET-x64.gzip"
	GzipAssembly "Magick.NET\bin\ReleaseQ16\v4.0\Win32\Magick.NET-x86.dll" "Magick.NET.AnyCPU\Resources\Q16\Magick.NET-x86.gzip"
	GzipAssembly "Magick.NET\bin\ReleaseQ16\v4.0\x64\Magick.NET-x64.dll" "Magick.NET.AnyCPU\Resources\Q16\Magick.NET-x64.gzip"
}
#==================================================================================================