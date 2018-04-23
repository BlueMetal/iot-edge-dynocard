
param(
    [Parameter(ValueFromPipeline)]
    [string] $line,
    [hashtable] $tokens,
    [regex] $tokenFormat = '\${(?<name>\w+)\}'
)

begin {
    $output = @()
    $undefinedToken = $false
    $lineNum = 0
}

process {
    $matches = $tokenFormat.matches($line)

    if($matches.success) {
        foreach($token in $matches) {
            $name = $token.groups['name'].value
            
            if(-not $tokens.ContainsKey($name)) {
                $undefinedToken = $true
                write-error ("Line {0}: undefined token name '{1}'" -f $lineNum,$name)
            }

            $output += ($line -replace $tokenFormat,$tokens[$name])
        }
    } else {
        $output += $line
    }

    $lineNum++
}

end {
    if(-not $undefinedToken) {
        $output -join [Environment]::NewLine
    }
}