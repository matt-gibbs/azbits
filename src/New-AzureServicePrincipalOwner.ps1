<#
.SYNOPSIS
    Creates a ServicePrincipal in Owner role for the current Azure subscription.

#>
param(
	[Parameter(Mandatory=$true)][string]$ServicePrincipalName,
	[Parameter(Mandatory=$true)][string]$Password
)

# Create the PSADPasswordCredential
$keyId = [guid]::NewGuid()
$startDate = Get-Date
$passwordCredential = New-Object Microsoft.Azure.Commands.Resources.Models.ActiveDirectory.PSADPasswordCredential
$passwordCredential.StartDate = $startDate
$passwordCredential.EndDate = $startDate.AddYears(1)
$passwordCredential.KeyId = $keyId
$passwordCredential.Value = $Password

# Create the AzureADApplication
$homePage = "http://" + $ServicePrincipalName + ".com"
$identifierUri = "http://" + $ServicePrincipalName
$application = New-AzureADApplication -DisplayName $ServicePrincipalName -HomePage $homePage -IdentifierUris $identifierUri -PasswordCredentials $passwordCredential

echo "Save output somewhere to reference the IDs for future tasks."
echo "Output from New-AzureADApplication."
echo "Save the ApplicationObjectId somewhere in case you want to delete it later."
$application

# Create the Service Principal.
$principal = New-AzureADServicePrincipal -ApplicationId $application.ApplicationId
echo "Output from New-AzureADServicePrincipal."
$principal

# Give AD a moment to make the Service Principal available.
Start-Sleep -s 10

# Create a Role Assignment granting "owner" permissions to the ServicePrincipal.
$roleAssignment = New-AzureRoleAssignment -ObjectId $principal.Id -RoleDefinitionName "Owner"
echo "Output from New-AzureRoleAssignment"
$roleAssignment

echo "You can now use applicationId and password for non-interactive login."
$application.ApplicationId

#echo "Removing AzureADServicePrincipal"
#Remove-AzureADServicePrincipal -ObjectId $principal.Id  -Force
#echo "Removing AzureADApplication"
#Remove-AzureADApplication -ApplicationObjectId $application.ApplicationObjectId –Force
#echo "Removing AzureADRoleAssignment assignment"
#Remove-AzureADRoleAssignment -ObjectId $roleAssignment.Id  -Force
