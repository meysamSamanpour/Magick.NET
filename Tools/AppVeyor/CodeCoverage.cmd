@echo off
call "..\VsDevCmd.cmd"

if not "%1" == "Q8" goto skip
if not "%2" == "x86" goto skip

..\Programs\NuGet.exe restore packages.config -PackagesDirectory .

OpenCover.4.6.519\tools\OpenCover.Console.exe -target:"%VSINSTALLDIR%Common7\IDE\CommonExtensions\Microsoft\TestWindow\vstest.console.exe" -targetargs:"/platform:x86 /inIsolation ..\..\Tests\Magick.NET.Tests\bin\TestQ8\x86\net45\Magick.NET.Tests.dll" -register:user -threshold:10 -excludebyattribute:*.ExcludeFromCodeCoverage* -excludebyfile:*\Generated\*.cs;*\MagickColors.cs -hideskipped:All -output:.\Magick.NET.Coverage.xml

SET PATH=C:\\Python34;C:\\Python34\\Scripts;%PATH%
pip install codecov
codecov -f "Magick.NET.Coverage.xml"

:skip