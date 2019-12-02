$uri = 'https://localhost:44335/api/v1/AtBash/Encrpyt'
$headers = New-Object "System.Collections.Generic.Dictionary[[String],[String]]"
$headers.Add("Content-Type", "application/json")
$headers.Add("Accept", "application/json")
$requestFile = "Encrypt-AtBash.json"
$responseFile = "Encrypt-AtBash-Response.json"
$requestBody = Get-Content -Path .\$requestFile
echo $requestBody
$response = Invoke-RestMethod -Uri $uri -body $requestBody -method 'POST' -headers $headers
$response = $response | ConvertTo-Json
$responseBody = Get-Content -Path .\$responseFile
$responseBody = $responseBody.Value.Trim()
echo $response
echo $responseBody
if ($response -eq $responseBody) {
	echo "yes"
} else {
	echo "no"
}