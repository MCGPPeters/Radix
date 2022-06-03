param containerEnviromentName string
param location string = 'northeurope'

param logAnalysticsWorkspaceId string
@secure()
param logAnalysticsWorkspaceClientSecret string

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: containerEnviromentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalysticsWorkspaceId
        sharedKey: logAnalysticsWorkspaceClientSecret
      }
    }
  }
}

output containerAppEnvironmentId string = resourceId('Microsoft.App/managedEnvironments', containerAppEnvironment.name)
output location string = location
