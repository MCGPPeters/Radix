param storageAccountname string
param ahQueueName string
param jumboQueueName string
param location string

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-06-01' = {
    name: storageAccountname
    location: location
    sku: {
        name: 'Standard_LRS'
    }
    kind: 'StorageV2'
    properties:{
        accessTier: 'Hot'
        publicNetworkAccess: 'Enabled'
        
    }
}

resource ahStorageQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2021-06-01' = {
    name: ahQueueName
    dependsOn:[
        storageAccount
    ]
}

resource jumboStorageQueue 'Microsoft.Storage/storageAccounts/queueServices/queues@2021-06-01' = {
    name: jumboQueueName
    dependsOn:[
        storageAccount
    ]
}

output storageAccountName string = storageAccount.name
output storageAccountPrimaryConnectionString string =  'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value};EndpointSuffix=core.windows.net'
output ahQueueName string = ahStorageQueue.name
output jumboQueueName string = jumboStorageQueue.name


