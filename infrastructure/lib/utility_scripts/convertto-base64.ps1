param(
    [Parameter(ValueFromPipeline)]
    [string] $input,
    [string] $encoding = "utf-8"
)

## check encoding
$encodings = [System.Text.Encoding]::GetEncodings().name
if($encodings -notcontains $encoding) {
    write-error ("Invalid encoding '{0}', specified. Valid encodings: '{1}'" -f $encoding,($encodings -join "','"))
    exit
}

[System.Text.Encoding] $encoding = [System.Text.Encoding]::GetEncoding($encoding)

[System.Convert]::ToBase64String($encoding.getbytes($input))