# ---------------------------------------------------------
# The purpose of this Caller Function is to make a call to
# the Test Engine with specific inputs.

Param([string] $testContainer= "L:\Usage\_Source\C#\JoeWareTools\JoeWareTools_UT\bin\Debug\JoeWareTools_UT.dll", 
      [string] $specificArguments = "",
      [string] $outputFile = "L:\Usage\TestOutput\JoeWare_Tools.txt")

# ----------------------------------------
# Setup Variables pointing to test engines

. "L:\Usage\_Source\C#\JoeWareTools\JoeWareTools_UT\Test Script\ConfigureTestVars.ps1"
$testEngine = $VSTest

Clear-Host

# ---------------------------------------
# Add the Datetime to the output filename

$runDate = Get-Date -format "yyyy_MM_dd_HH_mm_sss"
$outputFile =  $outputFile.Insert($outputFile.IndexOf('.'), '_' + $runDate)

# ---
# Log

Write-Host "`nTest Engine:    $testEngine" -ForegroundColor Cyan
Write-Host "`nTest Container: $testContainer" 
Write-Host "`nTest Arguments: $specificArguments"
Write-Host "`nOutput File:    $outputFile`n`n" -ForegroundColor Yellow 

# ---------------
# Run Test Engine

. $testEngine $testContainer $specificArguments > $outputFile

# --------
# Clean up

if($runDate) { Clear-Variable runDate }
if($testEngine) { Clear-Variable testEngine }