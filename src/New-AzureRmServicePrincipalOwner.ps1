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
$application = New-AzureRmADApplication -DisplayName $ServicePrincipalName -HomePage $homePage -IdentifierUris $identifierUri -PasswordCredentials $passwordCredential

echo "Save output somewhere to reference the IDs for future tasks."
echo "Output from New-AzureRmADApplication."
echo "Save the ApplicationObjectId somewhere in case you want to delete it later."
$application

# Create the Service Principal.
$principal = New-AzureRmADServicePrincipal -ApplicationId $application.ApplicationId
echo "Output from New-AzureRmADServicePrincipal."
$principal

# Give AD a moment to make the Service Principal available.
Start-Sleep -s 10

# Create a Role Assignment granting "owner" permissions to the ServicePrincipal.
$roleAssignment = New-AzureRmRoleAssignment -ObjectId $principal.Id -RoleDefinitionName "Owner"
echo "Output from New-AzureRmRoleAssignment"
$roleAssignment

echo "You can now use applicationId and password for non-interactive login."
$application.ApplicationId

#echo "Removing AzureRmADServicePrincipal"
#Remove-AzureRmADServicePrincipal -ObjectId $principal.Id  -Force
#echo "Removing AzureRmADApplication"
#Remove-AzureRmADApplication -ApplicationObjectId $application.ApplicationObjectId –Force
#echo "Removing AzureRmADRoleAssignment assignment"
#Remove-AzureRmADRoleAssignment -ObjectId $roleAssignment.Id  -Force
