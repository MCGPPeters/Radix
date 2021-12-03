param location string = 'northeurope'
param containerAppEnvironmentId string
param containerAppName string

@secure()
@description('The connection string of the storage account for search terms')
param storageAccountPrimaryConnectionString string
@description('The name of the storage queue for search terms')
param jumboQueueuName string

@description('The fully qualitfied image name including tag')
param dockerContainerImage string

@secure()
@description('The api key of the congnitive search service')
param searchApiKey string
@description('The name of the search index to be created')
param searchIndexName string
@description('The name of the cognitive search service')
param searchServiceName string

param numberOfPagesToCrawl string

resource containerApp 'Microsoft.Web/containerApps@2021-03-01' = {
  name: containerAppName
  kind: 'containerapp'
  location: location
  properties: {
    kubeEnvironmentId: containerAppEnvironmentId
    resourceGroup: resourceGroup().name
    activeRevisionMode: 'single' 
    configuration: {
      secrets: [
        {
          name: 'search-api-key'
          value: searchApiKey
        }
        {
          name: 'queue-connection-string'
          value: storageAccountPrimaryConnectionString
        }
      ]   
    }
    template: {
      containers: [
        {
          image: dockerContainerImage
          name: containerAppName
          resources: {
              cpu: '2.0'
              memory: '4.0Gi'
          }
          env:[
            {
              name: 'JUMBO_CONNECTION_STRING'
              secretRef: 'queue-connection-string'
            }
            {
              name: 'JUMBO_QUEUE_NAME'
              value: jumboQueueuName
            }
            {
              name: 'SEARCH_API_KEY'
              secretRef: 'search-api-key'
            }
            {
              name: 'SEARCH_INDEX_NAME'
              value: searchIndexName
            }
            {
              name: 'SEARCH_SERVICE_NAME'
              value: searchServiceName
            }
            {
              name: 'NUMBER_OF_PAGES_TO_CRAWL'
              value: numberOfPagesToCrawl
            }        
          ]
        }
      ]
      scale: {
        minReplicas: 1
        maxReplicas: 10
        rules: [
          {
            name: 'jumbo-queue-depth'
            azureQueue: {
              queueName: jumboQueueuName
              queueLength: '10'
              auth: [
                {
                  secretRef: 'queue-connection-string'
                  triggerParameter: 'connection'
                }
              ]              
            }
          }
        ]
      }
    }
  }
}
