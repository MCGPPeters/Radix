param containerEnviromentName string
param location string = 'northeurope'

param logAnalysticsWorkspaceId string
@secure()
param logAnalysticsWorkspaceClientSecret string

resource containerAppEnvironment 'Microsoft.Web/kubeEnvironments@2021-02-01' = {
  name: containerEnviromentName
  location: location
  kind: 'containerenvironment'
  properties: {
    type: 'managed'
    internalLoadBalancerEnabled: false
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalysticsWorkspaceId
        sharedKey: logAnalysticsWorkspaceClientSecret
      }
    }
  }
}

output containerAppEnvironmentId string = containerAppEnvironment.id
