<#
.SYNOPSIS
    Creates a ServicePrincipal in Owner role for the current Azure subscription.

#>
param(
  [Parameter(Mandatory=$true)][string]$principalName
)

import-module -Name "AzureRM.Resources"
$azContext = Get-AzureRmContext

write-verbose("CREATE THE PSADPasswordCredential.")
$psadCredential = New-Object Microsoft.Azure.Commands.Resources.Models.ActiveDirectory.PSADPasswordCredential
$startDate = Get-Date
$psadCredential.StartDate = $startDate
$psadCredential.EndDate = $startDate.AddYears(1)
$psadCredential.KeyId = [guid]::NewGuid()
$psadCredential.Value = [System.Guid]::NewGuid().ToString("N")
#write-verbose("PSADPasswordCredential")
write-verbose("StartDate: " + $psadCredential.StartDate)
write-verbose("EndDate: " + $psadCredential.EndDate)
write-verbose("KeyId: " + $psadCredential.KeyId)
write-verbose("Value: " + $psadCredential.Value)

write-verbose("CREATE THE AzureRmADApplication.")
$homePage = "http://" + $principalName + ".com"
$identifierUri = "http://" + $principalName
$application = New-AzureRmADApplication -DisplayName $principalName -HomePage $homePage -IdentifierUris $identifierUri -PasswordCredentials $psadCredential
write-output("Suggestion: save output for future reference.") 
write-verbose("AzureRmADApplication")
write-verbose("ApplicationId: " + $application.ApplicationId)
write-verbose("ApplicationObjectId: " + $application.ApplicationObjectId)
write-verbose("AvailableToOtherTenants: " + $application.AvailableToOtherTenants)
write-verbose("ApplicationId: " + $application.ApplicationId)


write-verbose("CREATE THE AzureRmAdServicePrincipal")
$principal = New-AzureRmADServicePrincipal -ApplicationId $application.ApplicationId
write-verbose("AzureRmADServicePrincipal")
write-verbose("DisplayName: " + $principal.DisplayName)
write-verbose("ObjectId: " + $principal.Id)

# Give AD a moment to make the Service Principal available.
for($i = 10; $i -le 101; $i=$i+10)
{
  write-progress -activity "propagating in AD" -status "$i Complete:" -percentComplete $i;
  Start-Sleep -s 1
}

#write-verbose("CREATE A ROLE ASSIGNMENT GRANTING 'Owner' TO THE SERVICEPRINCIPAL AT SUBSCRIPTION SCOPE.")
$scope = "/subscriptions/" + ($azContext.Subscription.SubscriptionId)
$roleAssignment = New-AzureRmRoleAssignment -ObjectId $principal.Id -RoleDefinitionName "Owner" -Scope $scope
write-verbose("AzureRmRoleAssignment")
write-verbose("RoleDefinitionName: " + $roleAssignment.RoleDefinitionName)
write-verbose("RoleDefinitionId: " + $roleAssignment.RoleDefinitionId)
write-verbose("ObjectId: " + $roleAssignment.ObjectId)

write-output("`r`nCommands for deleting the ServicePrincipal.")
write-output("Remove-AzureRmRoleAssignment -ObjectId " + $roleAssignment.ObjectId + " -RoleDefinitionName Owner -Force")
write-output("Remove-AzureRmADServicePrincipal -ObjectId " + $principal.Id + " -Force")
write-output("Remove-AzureRmADApplication -ApplicationObjectId " + $application.ApplicationObjectId + " -Force")

write-output("`r`nParameters needed for API calls.")
write-output("TenantId: " + $azContext.Tenant.TenantId)
write-output("SubscriptionId: " + $azContext.Subscription.SubscriptionId)
write-output("ApplicationId: " + $application.ApplicationId)
write-output("Secret: " + $psadCredential.Value)
