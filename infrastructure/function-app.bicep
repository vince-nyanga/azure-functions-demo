@description('The location to create the function app in.')
param location string = resourceGroup().location

@description('The name of the function app that you wish to create.')
param functionAppName string = 'fnapp${uniqueString(resourceGroup().id)}'

@description('The name of the storage account to create.')
@minLength(3)
@maxLength(24)
param storageAccountName string = 'storage${uniqueString(resourceGroup().id)}'

@description('The storage account SKU name.')
@allowed([
  'Standard_LRS'
  'Standard_ZRS'
  'Standard_GRS'
])
param storageAccountSkuName string = 'Standard_LRS'

var functionAppRuntime = 'dotnet'
var hostingPlanName = functionAppName

resource storageAccount 'Microsoft.Storage/storageAccounts@2022-09-01' = {
  name: storageAccountName
  location: location
  sku: {
    name: storageAccountSkuName
  }
  kind: 'StorageV2'
  properties: {
    minimumTlsVersion: 'TLS1_2'
    supportsHttpsTrafficOnly: true
  }
}

resource hostingPlan 'Microsoft.Web/serverfarms@2022-09-01' = {
  name: hostingPlanName
  location: location
  sku: {
    name: 'Y1'
    tier: 'Dynamic'
  }
}

resource functionApp 'Microsoft.Web/sites@2022-09-01' = {
  name: functionAppName
  location: location
  kind: 'functionapp'
  properties: {
    serverFarmId: hostingPlan.id
    siteConfig: {
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: functionAppRuntime
        }
        {
          name: 'AzureWebJobsStorage'
          value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};AccountKey=${storageAccount.listKeys().keys[0].value};EndpointSuffix=${environment().suffixes.storage}'
        }
        {
          name: 'FUNCTIONS_EXTENSION_VERSION'
          value: '~4'
        }
      ]
    }
  }
}

output functionAppName string = functionAppName
