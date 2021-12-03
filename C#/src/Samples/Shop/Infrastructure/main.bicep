param ahContainerImage string = 'mcgppeters/radix.shop.catalog.crawling.ah'
param jumboContainerImage string = 'mcgppeters/radix.shop.catalog.crawling.jumbo'
param numberOfPagesToCrawl string = '80'
param searchIndexName string = 'products'
param searchServiceName string = 'srch-radix-samples-shop${uniqueString(resourceGroup().id)}'
param location string = resourceGroup().location

module cognitiveSearchModule 'cognitiveSearch.bicep' = {
  name: 'cognitiveSearch'
  params: {
    location: location
    searchServiceName: searchServiceName
  }
}

var storageAccountName = 'stradixshop${uniqueString(resourceGroup().id)}'
var ahQueueName = 'stq-radix-samples-shop-catalog-ah-searchterms${uniqueString(resourceGroup().id)}'
var jumboQueueName = 'stq-radix-samples-shop-catalog-jumbo-searchterms${uniqueString(resourceGroup().id)}'

module storageModule 'storage.bicep' = {
  name: 'storage'
  params: {
    location: location
    storageAccountname: storageAccountName
    ahQueueName: '${storageAccountName}/default/${ahQueueName}'
    jumboQueueName: '${storageAccountName}/default/${jumboQueueName}'
  }
}

module logAnalyticsModule 'logAnalytics.bicep' = {
  name: 'logAnalytics'
  params: {
    location: location
    name: 'log-radix-samples-shop${uniqueString(resourceGroup().id)}'
  }
}

module containerEnvironment 'containerEnvironment.bicep' = {
  name: 'containerEnvironment'
  params: {
    location: location
    containerEnviromentName: 'ca-env-radix-samples-shop${uniqueString(resourceGroup().id)}'
    logAnalysticsWorkspaceClientSecret: logAnalyticsModule.outputs.clientSecret
    logAnalysticsWorkspaceId: logAnalyticsModule.outputs.clientId
  }
  dependsOn: [
    logAnalyticsModule
    storageModule
  ]
}

module ahContainerAppModule 'ahContainerApp.bicep' = {
  name: 'ca-catalog-crawling-ah${uniqueString(resourceGroup().id)}'
  params: {
    ahQueueuName: ahQueueName
    dockerContainerImage: ahContainerImage
    searchApiKey: cognitiveSearchModule.outputs.searchApiKey
    searchIndexName: searchIndexName
    searchServiceName: searchServiceName
    storageAccountPrimaryConnectionString: storageModule.outputs.storageAccountPrimaryConnectionString
    numberOfPagesToCrawl: numberOfPagesToCrawl
    containerAppEnvironmentId: containerEnvironment.outputs.containerAppEnvironmentId
    containerAppName: 'ca-catalog-crawling-ah'
  }
  dependsOn: [
    containerEnvironment
    cognitiveSearchModule
    logAnalyticsModule
    storageModule
  ]
}

module jumboContainerAppModule 'jumboContainerApp.bicep' = {
  name: 'ca-catalog-crawling-jumbo${uniqueString(resourceGroup().id)}'
  params: {
    jumboQueueuName: jumboQueueName
    dockerContainerImage: jumboContainerImage
    searchApiKey: cognitiveSearchModule.outputs.searchApiKey
    searchIndexName: searchIndexName
    searchServiceName: searchServiceName
    storageAccountPrimaryConnectionString: storageModule.outputs.storageAccountPrimaryConnectionString
    numberOfPagesToCrawl: numberOfPagesToCrawl
    containerAppEnvironmentId: containerEnvironment.outputs.containerAppEnvironmentId
    containerAppName: 'ca-catalog-crawling-jumbo'
  }
  dependsOn: [
    containerEnvironment
    cognitiveSearchModule
    logAnalyticsModule
    storageModule
  ]
}
